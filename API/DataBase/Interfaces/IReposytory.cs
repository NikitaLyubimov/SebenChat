using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


using DataBase.Entities;
using DataBase.DTO;

namespace DataBase.Interfaces
{
    public interface IReposytory<TEntity> 
        where TEntity : class
    {
        Task<TEntity> Add(TEntity ent);
        Task<List<TEntity>> GetAll();
        Task<TEntity> Delete(TEntity ent);
        Task<TEntity> GetById(long id);
        Task<TEntity> Update(TEntity ent);
    }
}
