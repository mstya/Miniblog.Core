using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Miniblog.Core.Db.Entities;
using Miniblog.Core.Db.Interfaces;

namespace Miniblog.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository repository;

        public CategoryService(BlogContext db, IRepository repository)
        {
            this.repository = repository;
        }

        public async Task RemoveForPost(string postId, CancellationToken token)
        {
            var categoriesForRemove = await this.repository.GetAllAsync<Category>().Where(x => x.Post.Id == postId).ToListAsync();
            await repository.DeleteCollectionAsync(categoriesForRemove, token);
        }
    }
}