using System;

namespace Miniblog.Core.Db.Entities
{
    public class Comment : BaseEntity
    {
        public string Author { get; set; }

        public string Email { get; set; }

        public string Content { get; set; }

        public DateTime PubDate { get; set; }

        public bool IsAdmin { get; set; }

        public Post Post { get; set; }

        public string PostId { get; set; }
    }
}