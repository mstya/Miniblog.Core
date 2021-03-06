﻿using System;
using System.Collections.Generic;
using System.Linq;
using Miniblog.Core.Db.Entities;
using Miniblog.Core.DTO;
using Miniblog.Core.Web.Models;

namespace Miniblog.Core.Mappers
{
    public static class CommentMappers
    {
        public static CommentViewModel ToCommentViewModel(this CommentDto commentViewModel)
        {
            return new CommentViewModel
            {
                Author = commentViewModel.Author,
                Content = commentViewModel.Content,
                Id = commentViewModel.Id,
                Email = commentViewModel.Email,
                IsAdmin = commentViewModel.IsAdmin,
                PubDate = commentViewModel.PubDate
            };
        }

        public static Comment ToComment(this CommentDto commentViewModel)
        {
            return new Comment
            {
                Author = commentViewModel.Author,
                Content = commentViewModel.Content,
                Id = commentViewModel.Id,
                Email = commentViewModel.Email,
                IsAdmin = commentViewModel.IsAdmin,
                PubDate = DateTime.Now,
                PostId = commentViewModel.PostId
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

        public static CommentDto ToCommentDto(this Comment comment)
        {
            return new CommentDto
            {
                Author = comment.Author,
                Content = comment.Content,
                Id = comment.Id,
                Email = comment.Email,
                IsAdmin = comment.IsAdmin,
                PubDate = comment.PubDate
            };
        }

        public static CommentDto ToCommentDto(this CommentViewModel comment)
        {
            return new CommentDto
            {
                Author = comment.Author,
                Content = comment.Content,
                Id = comment.Id,
                Email = comment.Email,
                IsAdmin = comment.IsAdmin,
                PubDate = comment.PubDate
            };
        }

        public static CommentDto ToCommentDto(this CommentViewModel comment, string postId)
        {
            return new CommentDto
            {
                Author = comment.Author,
                Content = comment.Content,
                Id = comment.Id,
                Email = comment.Email,
                IsAdmin = comment.IsAdmin,
                PubDate = comment.PubDate,
                PostId = postId
            };
        }

        public static List<CommentViewModel> ToCommentViewModel(this List<CommentDto> comments)
        {
            return comments.Select(x => x.ToCommentViewModel()).ToList();
        }

        public static List<CommentDto> ToCommentDto(this List<Comment> comments)
        {
            return comments.Select(x => x.ToCommentDto()).ToList();
        }
    }
}
