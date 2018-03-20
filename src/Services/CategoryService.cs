using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Miniblog.Core.Entities;

namespace Miniblog.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly BlogContext db;

        public CategoryService(BlogContext db)
        {
            this.db = db;
        }

        public async Task RemoveForPost(string postId)
        {
            var categoriesForRemove = await this.db.Categories.Where(x => x.Post.Id == postId).ToListAsync();
            categoriesForRemove.ForEach(x => db.Categories.Remove(x));
            await db.SaveChangesAsync();
        }
    }
}