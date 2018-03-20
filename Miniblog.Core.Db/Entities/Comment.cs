using System;

namespace Miniblog.Core.Db.Entities
{
    public class Comment
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Author { get; set; }

        public string Email { get; set; }

        public string Content { get; set; }

        public DateTime PubDate { get; set; }

        public bool IsAdmin { get; set; }
    }
}