using System.Threading.Tasks;
using System.Linq;


using Core.DTO.UseCaseRequests;
using Core.DTO.UseCaseResponses;
using Core.Interfaces;
using Core.Interfaces.UseCases;
using Core.Interfaces.Gateways.Reposytories;


namespace Core.UseCases
{
    public class RegisterUserUsecase : IRegisterUserUseCase
    {
        IUserReposytory _userReposytory;

        public RegisterUserUsecase(IUserReposytory userReposytory)
        {
            _userReposytory = userReposytory;
        }

        public async Task<bool> Handle(RegisterUserRequest message, IOutputPort<RegisterUserResponce> outputPort)
        {
            var responce = await _userReposytory.Create(message.FirstName, message.SecondName, message.Email, message.UserName, message.Password);
            outputPort.Handle(responce.Success ? new RegisterUserResponce(responce.Id, true) : new RegisterUserResponce(responce.Errors.Select(e => e.Description)));
            return responce.Success;
        }
    }
}
