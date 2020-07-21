using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Core.Domain.Entities
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

        private readonly List<RefreshToken> _refreshTokens = new List<RefreshToken>();

        public ICollection<Message> MessagesSender { get; set; }
        public ICollection<Message> MessagesReceiver { get; set; }
        public ICollection<EmailConfirmToken> EmailConfirmTokens { get; set; }

        public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

        internal User() { }

        internal User(string firstName, string secondName, string userName, string email, string identityId)
        {
            FirstName = firstName;
            SecondName = secondName;
            UserName = userName;
            Email = email;
            IdentityId = identityId;

            MessagesSender = new List<Message>();
            MessagesReceiver = new List<Message>();
           
        }

        public bool HasValidRefreshTokens(string refreshToken)
        {
            return _refreshTokens.Any(rt => rt.Token == refreshToken);
        }

        public void AddRefreshToken(string token, long userId, string remoteIpAddress, double dayToExpire = 5)
        {
            _refreshTokens.Add(new RefreshToken(token, DateTime.UtcNow.AddDays(dayToExpire), remoteIpAddress, userId));  
        }

        public void RemoveRefreshToken(string token)
        {
            _refreshTokens.Remove(_refreshTokens.First(rt => rt.Token == token));
        }

    }
}
