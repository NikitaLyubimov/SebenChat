using System;
using System.Collections.Generic;
using System.Text;

using Core.Domain.Entities;

namespace Core.Interfaces.Gateways.Reposytories
{
    public interface IEmailTokenReposytory : IReposytory<EmailConfirmToken>
    {
    }
}
