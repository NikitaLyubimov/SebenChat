using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


using DataBase.Entities;

namespace DataBase.Interfaces
{
    public interface IReposytory<T> where T : class, BaseEntity
    {
        Task<T> Create(T entity);
    }
}
