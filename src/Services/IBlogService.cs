using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Miniblog.Core.Entities;

namespace Miniblog.Core.Services
{
    public interface IBlogService
    {
        Task<List<Post>> GetPostsAsync(int count, int skip = 0);

        Task<List<Post>> GetPostsByCategoryAsync(string category);

        Task<Post>  GetPostBySlugAsync(string slug);

        Task<Post> GetPostByIdAsync(string id);

        Task<List<Category>> GetCategoriesAsync();

        Task SavePostAsync(Post post);

        Task DeletePostAsync(Post post, CancellationToken token);
    }
}