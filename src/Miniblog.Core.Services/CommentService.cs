using System.Threading;
using System.Threading.Tasks;
using Miniblog.Core.Db.Interfaces;
using Miniblog.Core.DTO;
using Miniblog.Core.Services.Interfaces;
using Miniblog.Core.Db.Entities;
using Miniblog.Core.Mappers;

namespace Miniblog.Core.Services
{
    public class CommentService : ICommentService
    {
        private readonly IRepository repository;

        public CommentService(IRepository repository)
        {
            this.repository = repository;
        }

        public Task<string> SaveCommentAsync(CommentDto dto, CancellationToken token)
        {
            return this.repository.AddOrUpdateAsync(dto.ToComment(), token);
        }

        public async Task DeleteByIdAsync(string commentId, CancellationToken token)
        {
            Comment comment = await this.repository.GetByIdAsync<Comment>(commentId, token);
            await this.repository.DeleteAsync(comment, token);
        }
    }
}
