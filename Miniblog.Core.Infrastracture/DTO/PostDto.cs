using System;
using System.Collections.Generic;

namespace Miniblog.Core.Services.DTO
{
    public class PostDto
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Slug { get; set; }

        public string Excerpt { get; set; }

        public string Content { get; set; }

        public DateTime PubDate { get; set; }

        public DateTime LastModified { get; set; }

        public bool IsPublished { get; set; } = true;

        public List<string> Categories { get; set; } = new List<string>();

        public List<CommentDto> Comments { get; set; } = new List<CommentDto>();
    }
}
