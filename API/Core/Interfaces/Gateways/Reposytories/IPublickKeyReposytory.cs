using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core.Domain.Entities;

namespace Core.Interfaces.Gateways.Reposytories
{
    public interface IPublickKeyReposytory : IReposytory<PublicKey>
    {
        Task<bool> Create(long userId, string keyValue);
    }
}
