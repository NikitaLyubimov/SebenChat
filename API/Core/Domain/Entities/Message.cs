using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Domain.Entities
{
    public class Message : BaseEntity
    {
        [Required]
        public string EncMessage { get; set; }
        [Required]
        public long SenderId { get; set; }
        [Required]
        public long ReceiverId { get; set; }
        [Required]
        public bool Read { get; set; }

        public User Sender { get; set; }
        public User Receiver { get; set; }
        public Message(string encMessage, long senderId, long receiverId)
        {
            EncMessage = encMessage;
            SenderId = senderId;
            ReceiverId = receiverId;
        }

        internal Message() { }

        
    }
}
