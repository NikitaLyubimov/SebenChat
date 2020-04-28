using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


using DataBase.Entities;
using DataBase.DTO;

namespace DataBase.Interfaces
{
    public interface IReposytory<T, Y> 
        where T : BaseEntity 
        where Y : BaseResponce
    {
        Task<Y> Add(T ent);
        Task<Y> Delete(T ent);
        Task<T> GetById(long id);
        Task<Y> Update(long id);
    }
}
