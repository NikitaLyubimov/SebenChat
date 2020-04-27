using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataBase.Entities
{
    public class Message : BaseEntity
    {
        [Required]
        public string EncMessage { get; set; }
        [Required]
        public User Sender { get; set; }
        [Required]
        public User Receiver { get; set; }
    }
}
