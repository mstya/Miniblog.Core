using System.Collections.Generic;
using System.Linq;
using Miniblog.Core.Db.Entities;
using Miniblog.Core.DTO;
using Miniblog.Core.Web.Models;

namespace Miniblog.Core.Mappers
{
    public static class PostMappers
    {
        public static PostDto ToPostDto(this PostViewModel postViewModel)
        {
            return new PostDto
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

        public static Post ToPost(this PostDto postDto)
        {
            return new Post
            {
                Id = postDto.Id,
                Content = postDto.Content,
                Excerpt = postDto.Excerpt,
                IsPublished = postDto.IsPublished,
                LastModified = postDto.LastModified,
                PubDate = postDto.PubDate,
                Slug = postDto.Slug,
                Title = postDto.Title,
                Categories = postDto.Categories.Select(x => new Category() { Name = x, PostId = postDto.Id }).ToList()
            };
        }

        public static PostViewModel ToPostViewModel(this PostDto post)
        {
            return new PostViewModel
            {
                Id = post.Id,
                Categories = post.Categories,
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

        public static PostDto ToPostDto(this Post post)
        {
            if (post == null)
            {
                return null;
            }

            return new PostDto
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
                Comments = post.Comments.ToCommentDto()
            };
        }

        public static List<PostViewModel> ToPostViewModel(this List<PostDto> posts)
        {
            return posts.Select(x => x.ToPostViewModel()).ToList();
        }

        public static List<PostDto> ToPostDto(this List<Post> posts)
        {
            return posts.Select(x => x.ToPostDto()).ToList();
        }
    }
}
