using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels.Responce
{
    public class RefreshResponce
    {
        public AccessToken AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }

        public RefreshResponce(AccessToken accessToken, string refreshToken, bool success = false, string error = "")
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            Success = success;
            Error = error;
        }
    }
}
