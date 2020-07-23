using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.EntityFrameworkCore;


using Core.Interfaces.Gateways.Reposytories;
using Core.Domain.Entities;
using System.Linq;

namespace Infrustructure.Data.Repositories
{
    public abstract class BaseReposytory<TEntity, TContext> : IReposytory<TEntity>
        where TEntity : BaseEntity
        where TContext : DbContext
    {
        protected readonly TContext _db;

        public BaseReposytory(TContext db)
        {
            _db = db;
        }
        public async Task<TEntity> Add(TEntity ent)
        {
            _db.Set<TEntity>().Add(ent);
            await _db.SaveChangesAsync();
            return ent;

        }

        public async Task<TEntity> Delete(TEntity ent)
        {
            _db.Set<TEntity>().Remove(ent);
            await _db.SaveChangesAsync();
            return ent;
        }

        public async Task<TEntity> FindOneBySpec(ISpecification<TEntity> spec)
        {
            var result = await ListBySpec(spec);

            return result.FirstOrDefault();

        }

        public async Task<List<TEntity>> GetAll()
        {
            return await _db.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetById(long id)
        {
            return await _db.Set<TEntity>().FindAsync(id);
        }



        public async Task<List<TEntity>> ListBySpec(ISpecification<TEntity> spec)
        {
            var quarableWithIncludes = spec.Includes
                .Aggregate(_db.Set<TEntity>().AsQueryable(),
                (current, include) => current.Include(include));

            var resultWithIncludeString = spec.IncludeStrings
                .Aggregate(quarableWithIncludes, (current, include) => current.Include(include));

            return await resultWithIncludeString.Where(spec.Criteria).ToListAsync();
        }

        public async Task<TEntity> Update(TEntity ent)
        {
            _db.Entry(ent).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return ent;
        }
    }
}
