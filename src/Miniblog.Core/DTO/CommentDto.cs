﻿using System;
namespace Miniblog.Core.DTO
{
    public class CommentDto
    {
        public string Id { get; set; }

        public string Author { get; set; }

        public string Email { get; set; }

        public string Content { get; set; }

        public DateTime PubDate { get; set; }

        public bool IsAdmin { get; set; }

        public string PostId { get; set; }
    }
}