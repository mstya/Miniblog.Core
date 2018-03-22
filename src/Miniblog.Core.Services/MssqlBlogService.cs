using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Miniblog.Core.Db.Entities;
using Miniblog.Core.Db.Interfaces;
using Miniblog.Core.DTO;
using Miniblog.Core.Mappers;

namespace Miniblog.Core.Services
{
    public class MssqlBlogService : IBlogService
    {
        private readonly IHttpContextAccessor contextAccessor;
        private readonly IRepository repository;
        private readonly ICategoryService categoryService;

        public MssqlBlogService(IHttpContextAccessor contextAccessor, IRepository repository, ICategoryService categoryService)
        {
            this.contextAccessor = contextAccessor;
            this.repository = repository;
            this.categoryService = categoryService;
        }

        public Task DeletePostAsync(PostDto postDto, CancellationToken token)
        {
            return this.repository.DeleteAsync(postDto.ToPost(), token);
        }

        public async Task<List<CategoryDto>> GetCategoriesAsync(CancellationToken token)
        {
            bool isAdmin = IsAdmin();

            return (await this.repository.GetAllAsync<Post>()
                       .Where(p => p.IsPublished || isAdmin)
                       .SelectMany(post => post.Categories)
                       .Select(cat => cat)
                       .Distinct()
                    .ToListAsync(token)).ToCategoryDto();
        }

        public async Task<PostDto> GetPostByIdAsync(string id, CancellationToken token)
        {
            return (await this.repository
                       .GetAllAsync<Post>()
                       .Include(c => c.Categories)
                       .Include(x => x.Comments)
                    .FirstOrDefaultAsync(x => x.Id == id, token)).ToPostDto();
        }

        public async Task<PostDto> GetPostBySlugAsync(string slug, CancellationToken token)
        {
            return (await this.repository
                       .GetAllAsync<Post>()
                       .Include(c => c.Categories)
                       .Include(x => x.Comments)
                       .FirstOrDefaultAsync(x => x.Slug == slug, token)).ToPostDto();
        }

        public async Task<List<PostDto>> GetPostsAsync(int count, CancellationToken token, int skip = 0)
        {
            return (await this.repository
                       .GetAllAsync<Post>()
                       .Skip(skip)
                       .Take(count)
                       .Include(x => x.Comments)
                       .ToListAsync(token))
                .ToPostDto();
        }

        public async Task<List<PostDto>> GetPostsByCategoryAsync(string category, CancellationToken token)
        {
            return (await this.repository
                       .GetAllAsync<Post>()
                       .Where(x => x.Categories.Any(c => c.Name == category))
                       .ToListAsync(token))
                .ToPostDto();
        }

        public async Task<List<PostDto>> GetPostsByCategoryAsync(string category, int take, CancellationToken token, int skip = 0)
        {
            return (await this.repository
                       .GetAllAsync<Post>()
                       .Where(x => x.Categories.Any(c => c.Name == category))
                       .Skip(skip)
                       .Take(take)
                    .ToListAsync(token)).ToPostDto();
        }

        public async Task SavePostAsync(PostDto postDto, CancellationToken token)
        {
            Post post = postDto.ToPost();

            await this.categoryService.RemoveForPost(post.Id, token);
            await this.repository.AddOrUpdateAsync(post, token);

            foreach (var category in post.Categories)
            {
                category.Post = post;
                await this.repository.AddOrUpdateAsync(category, token);
            }
        }

        protected bool IsAdmin()
        {
            return this.contextAccessor.HttpContext?.User?.Identity.IsAuthenticated == true;
        }
    }
}