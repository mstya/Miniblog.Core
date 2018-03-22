using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Miniblog.Core.DTO;

using Miniblog.Core.Mappers;
using Miniblog.Core.Web.Models;
using Miniblog.Core.Services;
using WebEssentials.AspNetCore.Pwa;
using Miniblog.Core.Services.Interfaces;

namespace Miniblog.Core.Ui.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogService _blog;
        private readonly IOptionsSnapshot<BlogSettings> settings;
        private readonly WebManifest _manifest;
        private readonly ICategoryService categoryService;
        private readonly ICommentService commentService;

        public BlogController(IBlogService blog, 
                              IOptionsSnapshot<BlogSettings> settings, 
                              WebManifest manifest, 
                              ICategoryService categoryService,
                              ICommentService commentService)
        {
            _blog = blog;
            this.settings = settings;
            _manifest = manifest;
            this.categoryService = categoryService;
            this.commentService = commentService;
        }

        [Route("/{page:int?}")]
        [OutputCache(Profile = "default")]
        public async Task<IActionResult> Index([FromRoute]int page = 0)
        {
            CancellationToken token = this.Request.HttpContext.RequestAborted;
            var posts = await _blog.GetPostsAsync(settings.Value.PostsPerPage, token, settings.Value.PostsPerPage * page);
            ViewData["Title"] = _manifest.Name;
            ViewData["Description"] = _manifest.Description;
            ViewData["prev"] = $"/{page + 1}/";
            ViewData["next"] = $"/{(page <= 1 ? null : page - 1 + "/")}";
            return View("~/Views/Blog/Index.cshtml", posts.ToPostViewModel());
        }

        [Route("/blog/category/{category}/{page:int?}")]
        [OutputCache(Profile = "default")]
        public async Task<IActionResult> Category(string category, int page = 0)
        {
            CancellationToken token = this.Request.HttpContext.RequestAborted;
            var posts = await _blog.GetPostsByCategoryAsync(category,
                                                             take: settings.Value.PostsPerPage * page,
                                                             token: token,
                                                             skip: settings.Value.PostsPerPage);
            ViewData["Title"] = _manifest.Name + " " + category;
            ViewData["Description"] = $"Articles posted in the {category} category";
            ViewData["prev"] = $"/blog/category/{category}/{page + 1}/";
            ViewData["next"] = $"/blog/category/{category}/{(page <= 1 ? null : page - 1 + "/")}";
            return View("~/Views/Blog/Index.cshtml", posts.ToPostViewModel());
        }

        // This is for redirecting potential existing URLs from the old Miniblog URL format
        [Route("/post/{slug}")]
        [HttpGet]
        public IActionResult Redirects(string slug)
        {
            return LocalRedirectPermanent($"/blog/{slug}");
        }

        [Route("/blog/{slug?}")]
        [OutputCache(Profile = "default")]
        public async Task<IActionResult> Post(string slug)
        {
            CancellationToken token = this.Request.HttpContext.RequestAborted;
            PostDto post = await _blog.GetPostBySlugAsync(slug, token);

            if (post != null)
            {
                return View(post.ToPostViewModel());
            }

            return NotFound();
        }

        [Route("/blog/edit/{id?}")]
        [HttpGet, Authorize]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return View(new PostViewModel());
            }

            CancellationToken token = this.Request.HttpContext.RequestAborted;
            PostDto post = await _blog.GetPostByIdAsync(id, token);

            if (post != null)
            {
                return View(post.ToPostViewModel());
            }

            return NotFound();
        }

        [Route("/blog/{slug?}")]
        [HttpPost, Authorize, AutoValidateAntiforgeryToken]
        public async Task<IActionResult> UpdatePost(PostViewModel postViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", postViewModel);
            }

            CancellationToken token = this.Request.HttpContext.RequestAborted;
            PostDto post = postViewModel.ToPostDto();
            PostDto existing = await _blog.GetPostByIdAsync(postViewModel.Id, token);
            string categories = Request.Form["categories"];

            if(existing == null)
            {
                existing = post;
            }
            else 
            {
                existing.Title = post.Title.Trim();
                existing.Slug = !string.IsNullOrWhiteSpace(postViewModel.Slug) ? postViewModel.Slug.Trim() : PostViewModel.CreateSlug(postViewModel.Title);
                existing.IsPublished = postViewModel.IsPublished;
                existing.Content = postViewModel.Content.Trim();
                existing.Excerpt = postViewModel.Excerpt.Trim();
            }

            existing.Categories = categories.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(c => c.Trim().ToLowerInvariant()).ToList();

            await this._blog.SavePostAsync(existing, token);

            return Redirect(postViewModel.GetLink());
        }

        [Route("/blog/deletepost/{id}")]
        [HttpPost, Authorize, AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeletePost(string id)
        {
            CancellationToken token = this.Request.HttpContext.RequestAborted;
            var existing = await _blog.GetPostByIdAsync(id, token);
            var cancellationToken = this.Response.HttpContext.RequestAborted;
            if (existing != null)
            {
                await _blog.DeletePostAsync(existing, cancellationToken);
                return Redirect("/");
            }

            return NotFound();
        }

        [Route("/blog/comment/{postId}")]
        [HttpPost]
        public async Task<IActionResult> AddComment(string postId, CommentViewModel commentViewModel)
        {
            CancellationToken token = this.Request.HttpContext.RequestAborted;
            PostDto post = await _blog.GetPostByIdAsync(postId, token);

            if (!ModelState.IsValid)
            {
                return View("Post", post.ToPostViewModel());
            }

            if (post == null || !post.AreCommentsOpen(settings.Value.CommentsCloseAfterDays))
            {
                return NotFound();
            }

            commentViewModel.IsAdmin = User.Identity.IsAuthenticated;
            commentViewModel.Content = commentViewModel.Content.Trim();
            commentViewModel.Author = commentViewModel.Author.Trim();
            commentViewModel.Email = commentViewModel.Email.Trim();

            // the website form key should have been removed by javascript
            // unless the comment was posted by a spam robot
            if (!Request.Form.ContainsKey("website"))
            {
                CommentDto comment = commentViewModel.ToCommentDto(postId);
                await this.commentService.SaveCommentAsync(comment, token);
            }

            return Redirect(post.GetLink() + "#" + commentViewModel.Id);
        }

        [Route("/blog/comment/{postId}/{commentId}")]
        [Authorize]
        public async Task<IActionResult> DeleteComment(string postId, string commentId)
        {
            CancellationToken token = this.Request.HttpContext.RequestAborted;
            var post = await _blog.GetPostByIdAsync(postId, token);

            await this.commentService.DeleteByIdAsync(commentId, token);

            return Redirect(post.GetLink() + "#comments");
        }
    }
}
