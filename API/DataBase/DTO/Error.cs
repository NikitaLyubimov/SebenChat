using System;
using System.Collections.Generic;
using System.Text;

namespace DataBase.DTO
{
    public class Error
    {
        public string Code { get; set; }
        public string Description { get; set; }

        public Error(string code, string description)
        {
            Code = code;
            Description = description;
        }
    }
}
