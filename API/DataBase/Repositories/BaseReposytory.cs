using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

using DataBase.Interfaces;
using DataBase.Entities;
using DataBase.DTO;
using System.Threading.Tasks;

namespace DataBase.Repositories
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

        public async Task<List<TEntity>> GetAll()
        {
            return await _db.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetById(long id)
        {
            return await _db.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity> Update(TEntity ent)
        {
            _db.Entry(ent).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return ent;
        }
    }
}
