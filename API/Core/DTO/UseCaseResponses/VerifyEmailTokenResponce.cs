using System;
using System.Collections.Generic;
using System.Text;

using Core.Interfaces;

namespace Core.DTO.UseCaseResponses
{
    public class VerifyEmailTokenResponce : UseCaseResponceMessage
    {
        public VerifyEmailTokenResponce(bool success, string message) : base(success, message) { }
    }
}
