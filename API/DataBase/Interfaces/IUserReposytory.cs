using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using DataBase.Entities;
using DataBase.DTO;

namespace DataBase.Interfaces
{
    public interface IUserReposytory
    {
        Task<CreateUserResponce> Create(string firstName, string secondName, string email, string userName, string password);
        Task<User> FindByName(string userName);

    }
}
