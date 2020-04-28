using System;
using System.Collections.Generic;
using System.Text;

namespace DataBase.DTO
{
    public class BaseResponce
    {
        public bool Success { get; set; }
        public IEnumerable<Error> Errors { get; set; }

        public BaseResponce(bool success = false, IEnumerable<Error> errors = null)
        {
            Success = success;
            Errors = errors;
        }
    }
}
