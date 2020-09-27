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
        private Mock<IUserReposytory> _mockUserReposytory;
        private Mock<IEmailActions> _mockEmailActions;
        private Mock<IOutputPort<RegisterUserResponce>> _mockOutputPort;

        [SetUp]
        public void Setup()
        {
            _mockUserReposytory = new Mock<IUserReposytory>();
            _mockUserReposytory.Setup(repo => repo.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new CreateUserResponce("", true));
            _mockUserReposytory.Setup(repo => repo.GetByIdentityId(It.IsAny<string>())).ReturnsAsync(new User("", "", "", "", ""));

            _mockEmailActions = new Mock<IEmailActions>();
            _mockEmailActions.Setup(email => email.SendMessage(It.IsAny<string>(), It.IsAny<long>())).Returns(Task.CompletedTask);

            _mockOutputPort = new Mock<IOutputPort<RegisterUserResponce>>();
            _mockOutputPort.Setup(presenter => presenter.Handle(It.IsAny<RegisterUserResponce>()));
        }

        [Test]
        public async Task Handle_ValidCredentials_ShouldSucceed()
        {

            var registerUseCase = new RegisterUserUsecase(_mockUserReposytory.Object, _mockEmailActions.Object);

            var responce = await registerUseCase.Handle(new RegisterUserRequest("name", "secName", "username", "email@email.ru", "password"), _mockOutputPort.Object);

            Assert.True(responce);
        }
    }
}
