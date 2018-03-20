using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Miniblog.Core.Entities;
using Miniblog.Core.Mappers;
using Miniblog.Core.Models;
using Miniblog.Core.Services;
using WebEssentials.AspNetCore.Pwa;

namespace Miniblog.Core.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogService _blog;
        private readonly IOptionsSnapshot<BlogSettings> _settings;
        private readonly WebManifest _manifest;
        private readonly ICategoryService categoryService;

        public BlogController(IBlogService blog, IOptionsSnapshot<BlogSettings> settings, WebManifest manifest, ICategoryService categoryService)
        {
            _blog = blog;
            _settings = settings;
            _manifest = manifest;
            this.categoryService = categoryService;
        }

        [Route("/{page:int?}")]
        [OutputCache(Profile = "default")]
        public async Task<IActionResult> Index([FromRoute]int page = 0)
        {
            var posts = await _blog.GetPostsAsync(_settings.Value.PostsPerPage, _settings.Value.PostsPerPage * page);
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
            var posts = (await _blog.GetPostsByCategoryAsync(category)).Skip(_settings.Value.PostsPerPage * page).Take(_settings.Value.PostsPerPage).ToList();
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
            var post = await _blog.GetPostBySlugAsync(slug);

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

            var post = await _blog.GetPostByIdAsync(id);

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

            var post = postViewModel.ToPost();
            var existing = await _blog.GetPostByIdAsync(postViewModel.Id);
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

            await categoryService.RemoveForPost(existing.Id);
            existing.Categories = categories.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(c => new Category
            {
                Name = c.Trim().ToLowerInvariant()
            }).ToList();

            await this._blog.SavePostAsync(existing);

            return Redirect(postViewModel.GetLink());
        }

        [Route("/blog/deletepost/{id}")]
        [HttpPost, Authorize, AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeletePost(string id)
        {
            var existing = await _blog.GetPostByIdAsync(id);
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
            var post = await _blog.GetPostByIdAsync(postId);

            if (!ModelState.IsValid)
            {
                return View("Post", post);
            }

            if (post == null || !post.AreCommentsOpen(_settings.Value.CommentsCloseAfterDays))
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
                var comment = commentViewModel.ToComment();
                post.Comments.Add(comment);
                await _blog.SavePostAsync(post);
            }

            return Redirect(post.GetLink() + "#" + commentViewModel.Id);
        }

        [Route("/blog/comment/{postId}/{commentId}")]
        [Authorize]
        public async Task<IActionResult> DeleteComment(string postId, string commentId)
        {
            var post = await _blog.GetPostByIdAsync(postId);

            if (post == null)
            {
                return NotFound();
            }

            var comment = post.Comments.FirstOrDefault(c => c.Id.Equals(commentId, StringComparison.OrdinalIgnoreCase));

            if (comment == null)
            {
                return NotFound();
            }

            post.Comments.Remove(comment);
            await _blog.SavePostAsync(post);

            return Redirect(post.GetLink() + "#comments");
        }
    }
}
