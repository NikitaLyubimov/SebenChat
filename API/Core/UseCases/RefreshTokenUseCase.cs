using System.Linq;
using System.Threading.Tasks;


using Core.Specifications;
using Core.DTO.UseCaseRequests;
using Core.DTO.UseCaseResponses;
using Core.Interfaces;
using Core.Interfaces.Gateways.Reposytories;
using Core.Interfaces.UseCases;
using Core.Interfaces.Services;


namespace Core.UseCases
{
    public class RefreshTokenUseCase : IRefreshTokenUseCase
    {
        private IJwtFactory _jwtFactory;
        private ITokenFactory _tokenFactory;
        private IJwtTokenValidator _jwtValidator;
        private IUserReposytory _userReposytory;

        public RefreshTokenUseCase(IJwtFactory jwtFactory, ITokenFactory tokenFactory, IJwtTokenValidator tokenValidator, IUserReposytory userReposytory)
        {
            _jwtFactory = jwtFactory;
            _tokenFactory = tokenFactory;
            _jwtValidator = tokenValidator;
            _userReposytory = userReposytory;
            
        }

        public async Task<bool> Handle(RefreshTokenRequest message, IOutputPort<RefreshTokenResponce> outputPort)
        {
            var principals = _jwtValidator.GetPrincipalsFromToken(message.AccessToken, message.SigningKey);

            if(principals != null)
            {
                var id = principals.Claims.First(c => c.Type == "id");
                var user = await _userReposytory.FindOneBySpec(new UserSpecification(id.Value));

                if (user.HasValidRefreshTokens(message.RefreshToken))
                {
                    var jwtToken = await _jwtFactory.GenerateEncodedToken(user.IdentityId, user.UserName);
                    var refreshToken = _tokenFactory.GenerateToken();
                    user.RemoveRefreshToken(message.RefreshToken);
                    user.AddRefreshToken(refreshToken, user.Id, "");

                    await _userReposytory.Update(user);
                    outputPort.Handle(new RefreshTokenResponce(jwtToken, refreshToken, true));
                    return true;
                }
            }

            outputPort.Handle(new RefreshTokenResponce(false, "Invalid Refresh token"));
            return false;
        }
    }
}
