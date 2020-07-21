using System;
using System.Collections.Generic;
using System.Text;

using Core.Interfaces;
using Core.DTO.UseCaseResponses;

namespace Core.DTO.UseCaseRequests
{
    public class RegisterUserRequest : IUseCaseRequest<RegisterUserResponce>
    {
        public string FirstName { get; }
        public string SecondName { get; }
        public string UserName { get; }
        public string Email { get; }
        public string Password { get; }

        public RegisterUserRequest(string firstName, string secondName, string userName, string email, string password)
        {
            FirstName = firstName;
            SecondName = secondName;
            UserName = userName;
            Email = email;
            Password = password;
        }
    }
}
