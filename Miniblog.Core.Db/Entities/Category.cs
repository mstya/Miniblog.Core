using System;

namespace Miniblog.Core.Db.Entities
{
    public class Category
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; }

        public virtual Post Post { get; set; }
    }
}