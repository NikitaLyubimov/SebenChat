using System;
using System.Collections.Generic;
using System.Text;

using Core.Interfaces;

namespace Core.DTO.UseCaseResponses
{
    public class PublickKeyResponce : UseCaseResponceMessage 
    {
        public PublickKeyResponce(bool success = false, string message = null) : base(success, message) { }
    }
}
