using System;
using System.Collections.Generic;
using System.Text;

using Core.Interfaces;

namespace Core.DTO.UseCaseResponses
{
    public class RefreshTokenResponce : UseCaseResponceMessage
    {
        public AccessToken AccessToken { get;}
        public string RefreshToken { get; }

        public RefreshTokenResponce(bool success = false, string message = null) : base(success, message) { }

        public RefreshTokenResponce(AccessToken accessToken, string refreshToken, bool success = false, string message = null) : base(success, message)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
