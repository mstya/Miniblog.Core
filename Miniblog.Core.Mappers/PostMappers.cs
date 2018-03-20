using System.Collections.Generic;
using System.Linq;
using Miniblog.Core.Db.Entities;

using Miniblog.Core.Models;

namespace Miniblog.Core.Mappers
{
    public static class PostMappers
    {
        public static Post ToPost(this PostViewModel postViewModel)
        {
            return new Post
            {
                Id = postViewModel.Id,
                Content = postViewModel.Content,
                Excerpt = postViewModel.Excerpt,
                IsPublished = postViewModel.IsPublished,
                LastModified = postViewModel.LastModified,
                PubDate = postViewModel.PubDate,
                Slug = postViewModel.Slug,
                Title = postViewModel.Title
            };
        }

        public static PostViewModel ToPostViewModel(this Post post)
        {
            return new PostViewModel
            {
                Id = post.Id,
                Categories = post.Categories.Select(x => x.Name).ToList(),
                Content = post.Content,
                Excerpt = post.Excerpt,
                IsPublished = post.IsPublished,
                LastModified = post.LastModified,
                PubDate = post.PubDate,
                Slug = post.Slug,
                Title = post.Title,
                Comments = post.Comments.ToCommentViewModel()
            };
        }

        public static List<PostViewModel> ToPostViewModel(this List<Post> posts)
        {
            return posts.Select(x => x.ToPostViewModel()).ToList();
        }
    }
}
