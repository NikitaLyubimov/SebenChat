using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core.DTO.UseCaseRequests;
using Core.DTO.UseCaseResponses;
using Core.Interfaces;

namespace Core.Interfaces.UseCases
{
    public interface IVerifyEmailTokenUseCase : IUseCaseRequestHandler<VerifyEmailTokenRequest, VerifyEmailTokenResponce>
    {
    }
}
