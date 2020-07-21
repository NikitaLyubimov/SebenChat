﻿using System;
using System.Collections.Generic;
using System.Text;

using Core.Interfaces;
using Core.DTO.UseCaseResponses;

namespace Core.DTO.UseCaseRequests
{
    public class LoginRequest : IUseCaseRequest<LoginResponce>
    {
        public string UserName { get; }
        public string Password { get; }

        public LoginRequest(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
    }
}
