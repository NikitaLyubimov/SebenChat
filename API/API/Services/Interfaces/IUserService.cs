using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using API.Presenters;

namespace API.Services.Interfaces
{
    public interface IUserService
    {
        Task<JsonContentResult> LoginUser(string username, string password);
        Task<JsonContentResult> RegisterUser(string firstName, string secondName, string username, string email, string password);
        Task<JsonContentResult> Refresh(string accessToken, string refreshToken);
    }
}
