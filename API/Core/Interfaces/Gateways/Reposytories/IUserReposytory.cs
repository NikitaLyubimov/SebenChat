using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Core.Domain.Entities;
using Core.DTO.GatewayResponces.Repositories;

namespace Core.Interfaces.Gateways.Reposytories
{
    public interface IUserReposytory : IReposytory<User>
    {
        Task<CreateUserResponce> Create(string firstName, string secondName, string email, string userName, string password);
        Task<User> FindByName(string userName);

        Task<bool> CheckPassword(User user, string password);
    }
}
