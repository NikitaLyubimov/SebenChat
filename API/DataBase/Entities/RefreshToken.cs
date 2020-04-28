using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataBase.Entities
{
    public class RefreshToken
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public string Token { get; set; }
        [Required]
        public DateTime Expires { get; set; }
        [Required]
        public string RemoteIpAddress { get; set; }
        [Required]
        public long UserId { get; set; }

        public RefreshToken(string token, DateTime expires, string remoteIp, long userId)
        {
            Token = token;
            Expires = expires;
            RemoteIpAddress = remoteIp;
            UserId = userId;
        }

        internal RefreshToken() { }

    }
}
