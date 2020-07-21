using System;
using System.Collections.Generic;
using System.Text;

using Core.Interfaces;
using Core.DTO.UseCaseResponses;

namespace Core.DTO.UseCaseRequests
{
    public class RefreshTokenRequest : IUseCaseRequest<RefreshTokenResponce>
    {
        public string AccessToken { get; }
        public string RefreshToken { get; }
        
        public RefreshTokenRequest(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
