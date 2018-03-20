using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Miniblog.Core.Db.Entities;

namespace Miniblog.Core.Services
{
    public class MssqlBlogService : IBlogService
    {
        private BlogContext db;
        private readonly IHttpContextAccessor contextAccessor;

        public MssqlBlogService(BlogContext db, IHttpContextAccessor contextAccessor)
        {
            this.db = db;
            this.contextAccessor = contextAccessor;
        }

        public Task DeletePostAsync(Post post, CancellationToken token)
        {
            this.db.Posts.Remove(post);
            return this.db.SaveChangesAsync(token);
        }

        public Task<List<Category>> GetCategoriesAsync()
        {
            bool isAdmin = IsAdmin();

            return this.db.Posts.Where(p => p.IsPublished || isAdmin)
                .SelectMany(post => post.Categories)
                .Select(cat => cat)
                .Distinct()
                       .ToListAsync();
        }

        public Task<Post> GetPostByIdAsync(string id)
        {
            return this.db.Posts.Include(c => c.Categories).Include(x => x.Comments).FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<Post> GetPostBySlugAsync(string slug)
        {
            return this.db.Posts.Include(c => c.Categories).Include(x => x.Comments).FirstOrDefaultAsync(x => x.Slug == slug);
        }

        public Task<List<Post>> GetPostsAsync(int count, int skip = 0)
        {
            return this.db.Posts.Skip(skip).Take(count).Include(x => x.Comments).ToListAsync();
        }

        public Task<List<Post>> GetPostsByCategoryAsync(string category)
        {
            return this.db.Posts.Where(x => x.Categories.Any(c => c.Name == category)).ToListAsync();
        }

        public async Task SavePostAsync(Post post)
        {
            if (string.IsNullOrEmpty(post.Id))
            {
                post.Id = Guid.NewGuid().ToString();
                await this.db.Posts.AddAsync(post);
                await this.db.SaveChangesAsync();
                return;
            }

            this.db.Posts.Update(post);
            await this.db.SaveChangesAsync();
        }

        protected bool IsAdmin()
        {
            return this.contextAccessor.HttpContext?.User?.Identity.IsAuthenticated == true;
        }
    }
}
