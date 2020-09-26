using System.Threading.Tasks;
using System.Linq;


using Core.DTO.UseCaseRequests;
using Core.DTO.UseCaseResponses;
using Core.Helpers;
using Core.Interfaces;
using Core.Interfaces.UseCases;
using Core.Interfaces.Gateways.Reposytories;
using Core.Interfaces.Helpers;

namespace Core.UseCases
{
    public class RegisterUserUsecase : IRegisterUserUseCase
    {
        IUserReposytory _userReposytory;
        IEmailActions _email;

        public RegisterUserUsecase(IUserReposytory userReposytory, IEmailActions email)
        {
            _userReposytory = userReposytory;
            _email = email;
        }

        public async Task<bool> Handle(RegisterUserRequest message, IOutputPort<RegisterUserResponce> outputPort)
        {
            var responce = await _userReposytory.Create(message.FirstName, message.SecondName, message.Email, message.UserName, message.Password);
            outputPort.Handle(responce.Success ? new RegisterUserResponce(responce.Id, true) : new RegisterUserResponce(responce.Errors.Select(e => e.Description)));

            if (responce.Success)
            {
                var user = await _userReposytory.GetByIdentityId(responce.Id);
                await _email.SendMessage(message.Email, user.Id);
                
            }
                

            return responce.Success;
        }
    }
}
