using System.Collections.Generic;

using Core.Interfaces;

namespace Core.DTO.UseCaseResponses
{
    public class LoginResponce : UseCaseResponceMessage
    {
        public AccessToken AccessToken { get; }
        public string RefreshToken { get; }
        public IEnumerable<Error> Errors { get; }

        public LoginResponce(IEnumerable<Error> errors, bool success = false, string message = null) : base(success, message)
        {
            Errors = errors;
        }

        public LoginResponce(AccessToken accessToken, string refreshToken, bool success = false, string message = null) : base(success, message)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
