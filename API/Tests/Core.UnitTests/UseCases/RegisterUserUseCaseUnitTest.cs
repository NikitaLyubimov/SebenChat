using System.Threading.Tasks;


using Moq;
using NUnit.Framework;

using Core.Interfaces.Gateways.Reposytories;
using Core.Interfaces.Helpers;
using Core.DTO.GatewayResponces.Repositories;
using Core.UseCases;
using Core.Interfaces;
using Core.DTO.UseCaseResponses;
using Core.DTO.UseCaseRequests;
using Core.Domain.Entities;

namespace Tests.Core.UnitTests.UseCases
{
    [TestFixture]
    public class RegisterUserUseCaseUnitTest
    {
        [Test]
        public async Task Handle_ValidCredentials_ShouldSucceed()
        {
            var mockUserReposytory = new Mock<IUserReposytory>();
            mockUserReposytory.Setup(repo => repo.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new CreateUserResponce("", true));
            mockUserReposytory.Setup(repo => repo.GetByIdentityId(It.IsAny<string>())).ReturnsAsync(new User("", "", "", "", ""));

            var mockEmailActions = new Mock<IEmailActions>();
            mockEmailActions.Setup(email => email.SendMessage(It.IsAny<string>(), It.IsAny<long>())).Returns(Task.CompletedTask);

            var registerUseCase = new RegisterUserUsecase(mockUserReposytory.Object, mockEmailActions.Object);

            var mockOutputPort = new Mock<IOutputPort<RegisterUserResponce>>();
            mockOutputPort.Setup(presenter => presenter.Handle(It.IsAny<RegisterUserResponce>()));

            var responce = await registerUseCase.Handle(new RegisterUserRequest("name", "secName", "username", "email@email.ru", "password"), mockOutputPort.Object);

            Assert.True(responce);
        }
    }
}
