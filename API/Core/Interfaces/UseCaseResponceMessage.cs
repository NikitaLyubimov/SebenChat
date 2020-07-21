using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces
{
    public abstract class UseCaseResponceMessage
    {
        public bool Success { get; }
        public string Message { get; }

        public UseCaseResponceMessage(bool success = false, string message = null)
        {
            Success = success;
            Message = message;
        }
    }
}
