using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using API.ViewModels;

namespace API.ViewModels.Responce
{
    public class LoginResponce
    {
        public AccessToken AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public LoginResponce(AccessToken accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
