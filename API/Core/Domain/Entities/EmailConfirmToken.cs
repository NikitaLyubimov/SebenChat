using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Domain.Entities
{
    public class EmailConfirmToken : BaseEntity
    {
        [Required]
        public string Token { get; set; }

        public User User { get; set; }
        [Required]
        public long UserId { get; set; }

        public EmailConfirmToken(string token, long userId)
        {
            Token = token;
            UserId = userId;
        }

        
    }
}
