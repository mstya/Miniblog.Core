using System.Collections.Generic;
using System.Linq;
using Miniblog.Core.Db.Entities;
using Miniblog.Core.Models;

namespace Miniblog.Core.Mappers
{
    public static  class CommentMappers
    {
        public static Comment ToComment(this CommentViewModel commentViewModel)
        {
            return new Comment
            {
                Author = commentViewModel.Author,
                Content = commentViewModel.Content,
                Id = commentViewModel.Id,
                Email = commentViewModel.Email,
                IsAdmin = commentViewModel.IsAdmin,
                PubDate = commentViewModel.PubDate
            };
        }

        public static CommentViewModel ToCommentViewModel(this Comment comment)
        {
            return new CommentViewModel
            {
                Author = comment.Author,
                Content = comment.Content,
                Id = comment.Id,
                Email = comment.Email,
                IsAdmin = comment.IsAdmin,
                PubDate = comment.PubDate
            };
        }

        public static List<CommentViewModel> ToCommentViewModel(this List<Comment> comments)
        {
            return comments.Select(x => x.ToCommentViewModel()).ToList();
        }
    }
}