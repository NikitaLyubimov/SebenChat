using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core.Domain.Entities;

namespace Core.Interfaces.Gateways.Reposytories
{
    public interface IEmailTokenReposytory : IReposytory<EmailConfirmToken>
    {
        Task<EmailConfirmToken> GetToken(string token);
    }
}
