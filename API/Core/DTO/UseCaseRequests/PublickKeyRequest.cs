using System;
using System.Collections.Generic;
using System.Text;

using Core.Interfaces;
using Core.DTO.UseCaseResponses;

namespace Core.DTO.UseCaseRequests
{
    public class PublickKeyRequest : IUseCaseRequest<PublickKeyResponce>
    {
        public string UserName { get; }
        public string KeyValue { get; }

        public PublickKeyRequest(string userName, string keyValue)
        {
            UserName = userName;
            KeyValue = keyValue;
        }
    }
}
