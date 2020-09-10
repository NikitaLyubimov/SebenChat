using System;
using System.Collections.Generic;
using System.Threading.Tasks;


using Core.DTO.UseCaseRequests;
using Core.DTO.UseCaseResponses;
using Core.Interfaces;
using Core.Interfaces.UseCases;
using Core.Interfaces.Gateways.Reposytories;

namespace Core.UseCases
{
    public class PublickKeyUseCase : IPublickKeyUseCase
    {

        private IPublickKeyReposytory _publickKeyReposytory;
        private IUserReposytory _userReposytory;
        public PublickKeyUseCase(IPublickKeyReposytory publickKeyReposytory, IUserReposytory userReposytory)
        {
            _publickKeyReposytory = publickKeyReposytory;
            _userReposytory = userReposytory;
        }
        public async Task<bool> Handle(PublickKeyRequest message, IOutputPort<PublickKeyResponce> outputPort)
        {
            var userId = _userReposytory.FindByName(message.UserName).Id;
            bool result = await _publickKeyReposytory.Create(userId, message.KeyValue);

            if (result)
            {
                outputPort.Handle(new PublickKeyResponce(true, "Publick key added succesfuly"));
                return true;
            }

            outputPort.Handle(new PublickKeyResponce(false, "Error whilr adding publick key"));
            return false;
                
            

        }
    }
}
