﻿using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
//using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using WilderMinds.MetaWeblog;
//using Miniblog.Core.Mappers;
//using WilderMinds.MetaWeblog;

namespace Miniblog.Core.Services
{
    public class MetaWeblogService : IMetaWeblogProvider
    {
        private readonly IBlogService _blog;
        private readonly IConfiguration _config;
        private readonly IUserServices _userServices;
        private readonly IHttpContextAccessor _context;

        public MetaWeblogService(IBlogService blog, IConfiguration config, IHttpContextAccessor context, IUserServices userServices)
        {
            _blog = blog;
            _config = config;
            _userServices = userServices;
            _context = context;
        }

        public string AddPost(string blogid, string username, string password, WilderMinds.MetaWeblog.Post post, bool publish)
        {
            throw new NotImplementedException();
            //ValidateUser(username, password);

            //var newPost = new Models.PostViewModel
            //{
            //    Title = post.title,
            //    Slug = !string.IsNullOrWhiteSpace(post.wp_slug) ? post.wp_slug : Models.PostViewModel.CreateSlug(post.title),
            //    Content = post.description,
            //    IsPublished = publish,
            //    Categories = post.categories.ToList()
            //};

            //if (post.dateCreated != DateTime.MinValue)
            //{
            //    newPost.PubDate = post.dateCreated;
            //}

            //_blog.SavePostAsync(newPost.ToPost()).GetAwaiter().GetResult();

            return null;//newPost.Id;
        }

        public bool DeletePost(string key, string postid, string username, string password, bool publish)
        {
            throw new NotImplementedException();
            //ValidateUser(username, password);

            //var post = _blog.GetPostByIdAsync(postid).GetAwaiter().GetResult();

            //if (post != null)
            //{
            //    _blog.DeletePostAsync(post, CancellationToken.None).GetAwaiter().GetResult();
            //    return true;
            //}

            //return false;
        }

        public bool EditPost(string postid, string username, string password, WilderMinds.MetaWeblog.Post post, bool publish)
        {
            throw new NotImplementedException();
            //ValidateUser(username, password);

            //var existing = _blog.GetPostByIdAsync(postid).GetAwaiter().GetResult();

            //if (existing != null)
            //{
            //    existing.Title = post.title;
            //    existing.Slug = post.wp_slug;
            //    existing.Content = post.description;
            //    existing.IsPublished = publish;
            //    //existing.Categories = post.categories.ToList();

            //    if (post.dateCreated != DateTime.MinValue)
            //    {
            //        existing.PubDate = post.dateCreated;
            //    }

            //    _blog.SavePostAsync(existing).GetAwaiter().GetResult();

            //    return true;
            //}

            //return false;
        }

        public CategoryInfo[] GetCategories(string blogid, string username, string password)
        {
            throw new NotImplementedException();
            //ValidateUser(username, password);

            //return null;
            //_blog.GetCategoriesAsync().GetAwaiter().GetResult()
                           //.Select(cat =>
                           //    new CategoryInfo
                           //    {
                           //        categoryid = cat,
                           //        title = cat
                           //    })
                           //.ToArray();
        }

        public WilderMinds.MetaWeblog.Post GetPost(string postid, string username, string password)
        {
            throw new NotImplementedException();
            //ValidateUser(username, password);

            //var post = _blog.GetPostByIdAsync(postid).GetAwaiter().GetResult();

            //if (post != null)
            //{
            //    return null;//ToMetaWebLogPost(post);
            //}

            //return null;
        }

        public WilderMinds.MetaWeblog.Post[] GetRecentPosts(string blogid, string username, string password, int numberOfPosts)
        {
            throw new NotImplementedException();
            //ValidateUser(username, password);

            //return null;//_blog.GetPostsAsync(numberOfPosts).GetAwaiter().GetResult().Select(ToMetaWebLogPost).ToArray();
        }

        public BlogInfo[] GetUsersBlogs(string key, string username, string password)
        {
            throw new NotImplementedException();
            //ValidateUser(username, password);

            //var request = _context.HttpContext.Request;
            //string url = request.Scheme + "://" + request.Host;

            //return new[] { new BlogInfo {
            //    blogid ="1",
            //    blogName = _config["blog:name"],
            //    url = url
            //}};
        }

        public MediaObjectInfo NewMediaObject(string blogid, string username, string password, MediaObject mediaObject)
        {
            throw new NotImplementedException();
            //ValidateUser(username, password);
            //byte[] bytes = Convert.FromBase64String(mediaObject.bits);
            ////string path = _blog.SaveFileAsync(bytes, mediaObject.name).GetAwaiter().GetResult();

            //return null;//new MediaObjectInfo { url = path };
        }

        public UserInfo GetUserInfo(string key, string username, string password)
        {
            //ValidateUser(username, password);
            throw new NotImplementedException();
        }

        public int AddCategory(string key, string username, string password, NewCategory category)
        {
            //ValidateUser(username, password);
            throw new NotImplementedException();
        }

        private void ValidateUser(string username, string password)
        {
            throw new NotImplementedException();
            //if (_userServices.ValidateUser(username, password))
            //{
            //    throw new MetaWeblogException("Unauthorized");
            //}

            //var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            //identity.AddClaim(new Claim(ClaimTypes.Name, username));

            //_context.HttpContext.User = new ClaimsPrincipal(identity);
        }

        //private WilderMinds.MetaWeblog.Post ToMetaWebLogPost(Models.PostViewModel post)
        //{
        //    var request = _context.HttpContext.Request;
        //    string url = request.Scheme + "://" + request.Host;

        //    return new WilderMinds.MetaWeblog.Post
        //    {
        //        postid = post.Id,
        //        title = post.Title,
        //        wp_slug = post.Slug,
        //        permalink = url + post.GetLink(),
        //        dateCreated = post.PubDate,
        //        description = post.Content,
        //        categories = post.Categories.ToArray()
        //    };
        //}
    }
}
