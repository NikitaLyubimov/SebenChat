using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataBase.Entities
{
    public class EmailConfirmToken
    {
        [Required]
        public int Id { get; set; }
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
