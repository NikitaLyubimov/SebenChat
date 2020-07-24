using Core.Interfaces;
using Core.DTO.UseCaseResponses;

namespace Core.DTO.UseCaseRequests
{
    public class VerifyEmailTokenRequest : IUseCaseRequest<VerifyEmailTokenResponce>
    {
        public string Token { get; }


        public VerifyEmailTokenRequest(string token)
        {
            Token = token;
        }
    }
}
