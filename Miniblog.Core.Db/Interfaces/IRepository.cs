using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Miniblog.Core.Db.Entities;

namespace Miniblog.Core.Db.Interfaces
{
    public interface IRepository
    {
        Task<string> AddOrUpdateAsync<T>(T entity, CancellationToken token) where T : BaseEntity;

        Task DeleteAsync<T>(T entity, CancellationToken token) where T : BaseEntity;

        Task DeleteCollectionAsync<T>(ICollection<T> collection, CancellationToken token) where T : BaseEntity;

        Task<T> GetByIdAsync<T>(string id, CancellationToken token) where T : BaseEntity;

        IQueryable<T> GetByIdsAsync<T>(List<string> ids) where T : BaseEntity;

        IQueryable<T> GetAllAsync<T>() where T : BaseEntity;

        Task SaveChangesAsync(CancellationToken token);
    }
}