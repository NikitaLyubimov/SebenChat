using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels.Responce
{
    public class RegisterResponce
    {
        public bool Success { get; set; }
        public string Id { get; set; }
        public string Message { get; set; }
        public IEnumerable<string> Errors { get; set; }

        public RegisterResponce(IEnumerable<string> errors, bool success = false, string message = null)
        {
            Errors = errors;
            Success = success;
            Message = message;
        }

        public RegisterResponce(string id, bool success = true, IEnumerable<string> errors = null, string message = null)
        {
            Id = id;
            Success = success;
            Errors = errors;
            Message = message;
        }
    }
}
