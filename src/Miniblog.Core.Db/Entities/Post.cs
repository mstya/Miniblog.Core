using System;
using System.Collections.Generic;

namespace Miniblog.Core.Db.Entities
{
    public class Post : BaseEntity
    {
        public string Title { get; set; }

        public string Slug { get; set; }

        public string Excerpt { get; set; }

        public string Content { get; set; }

        public DateTime PubDate { get; set; }

        public DateTime LastModified { get; set; }

        public bool IsPublished { get; set; } = true;

        public virtual List<Category> Categories { get; set; } = new List<Category>();

        public virtual List<Comment> Comments { get; set; } = new List<Comment>();
    }
}