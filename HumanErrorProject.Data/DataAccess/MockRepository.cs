using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HumanErrorProject.Data.Models;

namespace HumanErrorProject.Data.DataAccess
{
    public class MockRepository<TEntity, T> : IRepository<TEntity, T>
        where TEntity : IdentityModel<T>
    {
        protected ICollection<TEntity> Entities;

        public MockRepository()
        {
            Entities = new List<TEntity>();
        }

        public MockRepository(IList<TEntity> entities)
        {
            Entities = entities;
        }

        public Task<TEntity> Get(T id)
        {
            return Task.FromResult(Entities
                .Single(entity => entity.Id.Equals(id)));
        }

        public Task<IEnumerable<TEntity>> GetAll()
        {
            return Task.FromResult<IEnumerable<TEntity>>(Entities);
        }

        public Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(Entities.Where(predicate.Compile()));
        }

        public Task<TEntity> SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(Entities.SingleOrDefault(predicate.Compile()));
        }

        public Task Add(TEntity entity)
        {
            entity.Id = (T) Convert.ChangeType(Entities.Count + 1, typeof(T));
            Entities.Add(entity);
            return Task.CompletedTask;
        }

        public Task AddRange(IEnumerable<TEntity> entities)
        {
            entities.ToList().ForEach(async e => await Add(e));
            return Task.CompletedTask;
        }

        public async Task Remove(TEntity entity)
        {
            var original = await Get(entity.Id);
            Entities.Remove(original);
        }

        public Task RemoveRange(IEnumerable<TEntity> entities)
        {
            entities.ToList().ForEach(async e => await Remove(e));
            return Task.CompletedTask;
        }

        public async Task Update(TEntity entity)
        {
            var original = await Get(entity.Id);
            Entities.Remove(original);
            Entities.Add(entity);
        }
    }
}
