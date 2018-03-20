using System.Threading.Tasks;

namespace Miniblog.Core.Services
{
    public interface ICategoryService
    {
        Task RemoveForPost(string postId);
    }
}