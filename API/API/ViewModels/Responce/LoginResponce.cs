using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using API.ViewModels;
using DataBase.DTO;

namespace API.ViewModels.Responce
{
    public class LoginResponce
    {
        public AccessToken AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public Error Error { get; set; }
        public bool Success { get; set; }
        public LoginResponce(Error error, bool success,AccessToken accessToken = null, string refreshToken = "")
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            Error = error;
            Success = success;
        }
    }
}
