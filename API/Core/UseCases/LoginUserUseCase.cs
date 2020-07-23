using System.Threading.Tasks;

using Core.DTO.UseCaseRequests;
using Core.DTO.UseCaseResponses;
using Core.Interfaces;
using Core.Interfaces.Gateways.Reposytories;
using Core.Interfaces.UseCases;
using Core.Interfaces.Services;
using Core.DTO;

namespace Core.UseCases
{
    public class LoginUserUseCase : ILoginUserUseCase
    {
        private IUserReposytory _userReposytory;
        private IJwtFactory _jwtFactory;
        private ITokenFactory _tokenFactory;

        public LoginUserUseCase(IUserReposytory userReposytory, IJwtFactory jwtFactory, ITokenFactory tokenFactory)
        {
            _userReposytory = userReposytory;
            _jwtFactory = jwtFactory;
            _tokenFactory = tokenFactory;
        }
        public async Task<bool> Handle(LoginRequest message, IOutputPort<LoginResponce> outputPort)
        {
            if(!string.IsNullOrEmpty(message.UserName) || !string.IsNullOrEmpty(message.Password))
            {
                var user = await _userReposytory.FindByName(message.UserName);

                if(user != null)
                {
                    if(await _userReposytory.CheckPassword(user, message.Password))
                    {
                        var jwtToken = await _jwtFactory.GenerateEncodedToken(user.IdentityId, message.UserName);
                        var refreshToken = _tokenFactory.GenerateToken();
                        user.AddRefreshToken(refreshToken, user.Id, message.RemoteIpAddress);
                        await _userReposytory.Update(user);
                            
                        outputPort.Handle(new LoginResponce(jwtToken, refreshToken, true));
                        return true;
                    }
                }
            }

            outputPort.Handle(new LoginResponce(new[] { new Error("login_failure", "Wrong Username of Password") }));
            return false;
        }
    }
}
