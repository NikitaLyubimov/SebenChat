using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataBase.Entities
{
    public class User : BaseEntity
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string SecondName { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string IdentityId { get; set; }

        public ICollection<RefreshToken> RefreshTokens { get; set; }

        public ICollection<Message> MessagesSender { get; set; }
        public ICollection<Message> MessagesReceiver { get; set; }

        public User(string firstName, string secondName, string userName, string email, string identityId)
        {
            FirstName = firstName;
            SecondName = secondName;
            UserName = userName;
            Email = email;
            IdentityId = identityId;
        }

    }
}
