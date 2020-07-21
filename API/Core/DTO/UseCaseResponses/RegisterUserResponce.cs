using System;
using System.Collections.Generic;
using System.Text;

using Core.Interfaces;

namespace Core.DTO.UseCaseResponses
{
    public class RegisterUserResponce : UseCaseResponceMessage
    {
        public string Id { get; }
        public IEnumerable<string> Errors { get; }

        public RegisterUserResponce(IEnumerable<string> errors, bool success = false, string message = null) : base(success, message)
        {
            Errors = errors;
        }

        public RegisterUserResponce(string id, bool success = false, string message = null) : base(success, message)
        {
            Id = id;
        }
    }
}
