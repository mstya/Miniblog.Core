using System;
using System.Threading;
using System.Threading.Tasks;
using Miniblog.Core.DTO;

namespace Miniblog.Core.Services.Interfaces
{
    public interface ICommentService
    {
        Task<string> SaveCommentAsync(CommentDto dto, CancellationToken token);

        Task DeleteByIdAsync(string commentId, CancellationToken token);
    }
}
