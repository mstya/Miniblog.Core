using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Miniblog.Core.DTO;

namespace Miniblog.Core.Services
{
    public interface IBlogService
    {
        Task<List<PostDto>> GetPostsAsync(int count, CancellationToken token, int skip = 0);

        Task<List<PostDto>> GetPostsByCategoryAsync(string category, CancellationToken token);

        Task<List<PostDto>> GetPostsByCategoryAsync(string category, int take, CancellationToken token, int skip = 0);

        Task<PostDto>  GetPostBySlugAsync(string slug, CancellationToken token);

        Task<PostDto> GetPostByIdAsync(string id, CancellationToken token);

        Task<List<CategoryDto>> GetCategoriesAsync(CancellationToken token);

        Task SavePostAsync(PostDto post, CancellationToken token);

        Task DeletePostAsync(PostDto post, CancellationToken token);
    }
}