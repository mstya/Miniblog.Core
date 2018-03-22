namespace Miniblog.Core.Db.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }

        public virtual Post Post { get; set; }

        public string PostId { get; set; }
    }
}