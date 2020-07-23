using System;
using System.Collections.Generic;
using System.Text;

using Core.DTO.UseCaseRequests;
using Core.DTO.UseCaseResponses;

namespace Core.Interfaces.UseCases
{
    public interface ILoginUserUseCase : IUseCaseRequestHandler<LoginRequest, LoginResponce>
    {
    }
}
