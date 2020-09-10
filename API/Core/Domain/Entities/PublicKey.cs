using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Domain.Entities
{
    public class PublicKey : BaseEntity
    {
        [Required]
        public long UserId { get; set; }
        [Required]
        public string KeyValue { get; set; }

        public User User { get; set; }

        public PublicKey(long userId, string keyValue)
        {
            UserId = userId;
            KeyValue = keyValue;
        }

        internal PublicKey() { }
    }
}
