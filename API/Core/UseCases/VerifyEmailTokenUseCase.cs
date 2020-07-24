using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;


using Core.DTO.UseCaseRequests;
using Core.DTO.UseCaseResponses;
using Core.Interfaces;
using Core.Interfaces.Gateways.Reposytories;
using Core.Interfaces.UseCases;

namespace Core.UseCases
{
    public class VerifyEmailTokenUseCase : IVerifyEmailTokenUseCase
    {
        private IEmailTokenReposytory _emailTokenReposytory;
        private IUserReposytory _userReposytory;
        public VerifyEmailTokenUseCase(IEmailTokenReposytory emailTokenReposytory, IUserReposytory userReposytory)
        {
            _emailTokenReposytory = emailTokenReposytory;
            _userReposytory = userReposytory;
        }
        public async Task<bool> Handle(VerifyEmailTokenRequest message, IOutputPort<VerifyEmailTokenResponce> outputPort)
        {
            var decToken = HttpUtility.UrlDecode(message.Token);
            var newToken = Regex.Replace(decToken, "%2b", "+", RegexOptions.IgnoreCase);

            var confToken = await _emailTokenReposytory.GetToken(newToken);

            if (confToken == null)
            {
                outputPort.Handle(new VerifyEmailTokenResponce(false, "Wrong Token"));
                return false;
            }
                
            var user = await _userReposytory.GetById(confToken.UserId);
            await _userReposytory.ConfirmEmail(user);
            outputPort.Handle(new VerifyEmailTokenResponce(true, "Email Successfuly verified"));

            return true;
        }
    }
}
