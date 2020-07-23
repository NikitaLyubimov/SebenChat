using System.Collections.Generic;
using System.Threading.Tasks;

using Core.Domain.Entities;

namespace Core.Interfaces.Gateways.Reposytories
{
    public interface IReposytory<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity> Add(TEntity ent);
        Task<List<TEntity>> GetAll();
        Task<TEntity> Delete(TEntity ent);
        Task<TEntity> GetById(long id);
        Task<TEntity> Update(TEntity ent);
        Task<TEntity> FindOneBySpec(ISpecification<TEntity> spec);
    }
}
