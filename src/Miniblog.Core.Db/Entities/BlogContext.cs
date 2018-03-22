using Microsoft.EntityFrameworkCore;

namespace Miniblog.Core.Db.Entities
{
    public class BlogContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Category> Categories { get; set; }

        public BlogContext(DbContextOptions<BlogContext> options) : base(options)
        {
        }
    }
}