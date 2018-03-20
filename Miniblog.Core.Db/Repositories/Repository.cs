using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Miniblog.Core.Db.Entities;
using Miniblog.Core.Db.Interfaces;

namespace Miniblog.Core.Db.Repositories
{
    public class Repository : IRepository
    {
        private BlogContext context;

        public Repository(BlogContext context)
        {
            this.context = context;
        }

        public async Task<string> AddOrUpdateAsync<T>(T entity, CancellationToken token) where T : BaseEntity
        {
            if(string.IsNullOrEmpty(entity.Id))
            {
                await this.context.Set<T>().AddAsync(entity, token);
            }
            else 
            {
                var local = this.context.Set<T>()
                                    .Local
                                    .FirstOrDefault(entry => entry.Id.Equals(entity.Id));
                this.context.Entry(local).CurrentValues.SetValues(entity);
            }

            await this.SaveChangesAsync(token);
            return entity.Id;
        }

        public Task DeleteAsync<T>(T entity, CancellationToken token) where T : BaseEntity
        {
            this.context.Set<T>().Attach(entity);
            this.context.Set<T>().Remove(entity);
            return this.SaveChangesAsync(token);
        }

        public Task DeleteCollectionAsync<T>(ICollection<T> collection, CancellationToken token) where T : BaseEntity
        {
            this.context.Set<T>().RemoveRange(collection);
            return this.SaveChangesAsync(token);
        }

        public Task<T> GetByIdAsync<T>(string id, CancellationToken token) where T : BaseEntity
        {
            return this.context.Set<T>().FindAsync(id);
        }

        public IQueryable<T> GetByIdsAsync<T>(List<string> ids) where T : BaseEntity
        {
            return this.context.Set<T>().Where(x => ids.Contains(x.Id));
        }

        public Task SaveChangesAsync(CancellationToken token)
        {
            return this.context.SaveChangesAsync(token);
        }

        public IQueryable<T> GetAllAsync<T>() where T : BaseEntity
        {
            return this.context.Set<T>().AsQueryable();
        }
    }
}
