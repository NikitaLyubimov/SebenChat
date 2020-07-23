
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Core.DTO;

namespace Core.Interfaces.Services
{
    public interface IJwtFactory
    {
        Task<AccessToken> GenerateEncodedToken(string id, string userName);
    }
}
