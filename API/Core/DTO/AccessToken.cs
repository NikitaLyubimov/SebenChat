using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.DTO
{
    public class AccessToken
    {
        public string Token { get; set; }
        public int ExpiredIn { get; set; }

        public AccessToken(string token, int expiredIn)
        {
            Token = token;
            ExpiredIn = expiredIn;
        }
    }
}
