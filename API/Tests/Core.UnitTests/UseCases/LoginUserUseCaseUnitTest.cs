using System.Threading.Tasks;

using Moq;
using NUnit.Framework;

using Core.Interfaces.Gateways.Reposytories;
using Core.Domain.Entities;
using Core.Interfaces.Services;
using Core.DTO;
using Core.UseCases;
using Core.Interfaces;
using Core.DTO.UseCaseResponses;
using Core.DTO.UseCaseRequests;

namespace Tests.Core.UnitTests.UseCases
{
    [TestFixture]
    public class LoginUserUseCaseUnitTest
    {
        private Mock<IUserReposytory> _mockUserReposytory;
        private Mock<IJwtFactory> _mockJwtFactory;
        private Mock<ITokenFactory> _mockTokenFactory;
        private Mock<IOutputPort<LoginResponce>> _mockOutputPort;

        [SetUp]
        public void Setup()
        {
            _mockUserReposytory = new Mock<IUserReposytory>();
            _mockUserReposytory.Setup(repo => repo.FindByName(It.IsAny<string>())).ReturnsAsync(new User("", "", "", "", ""));

            _mockJwtFactory = new Mock<IJwtFactory>();
            _mockJwtFactory.Setup(factory => factory.GenerateEncodedToken(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new AccessToken("", 0));

            _mockTokenFactory = new Mock<ITokenFactory>();

            _mockOutputPort = new Mock<IOutputPort<LoginResponce>>();
            _mockOutputPort.Setup(presenter => presenter.Handle(It.IsAny<LoginResponce>()));


        }



        [Test]
        public async Task Handle_ValidCredentials_ShouldSucceed()
        {
           _mockUserReposytory.Setup(repo => repo.CheckPassword(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(true);

            var useCase = new LoginUserUseCase(_mockUserReposytory.Object, _mockJwtFactory.Object, _mockTokenFactory.Object);
            var responce = await useCase.Handle(new LoginRequest("username", "password", "127.0.0.1"), _mockOutputPort.Object);

            Assert.True(responce);

        }

        [Test]
        public async Task Handle_IncompleteCredentials_ShouldFailed()
        {
            _mockUserReposytory.Setup(repo => repo.CheckPassword(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(false);

            var useCase = new LoginUserUseCase(_mockUserReposytory.Object, _mockJwtFactory.Object, _mockTokenFactory.Object);
            var responce = await useCase.Handle(new LoginRequest("", "password", "127.0.0.1"), _mockOutputPort.Object);

            Assert.False(responce);
            _mockTokenFactory.Verify(factory => factory.GenerateToken(32), Times.Never);

        }

        [Test]
        public async Task Handler_IncorrectPassword_ShouldFail()
        {
            _mockUserReposytory.Setup(repo => repo.CheckPassword(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(false);

            var useCase = new LoginUserUseCase(_mockUserReposytory.Object, _mockJwtFactory.Object, _mockTokenFactory.Object);

            var result = await useCase.Handle(new LoginRequest("username", "wrongpassword", "127.0.01"), _mockOutputPort.Object);

            Assert.False(result);
            _mockTokenFactory.Verify(factory => factory.GenerateToken(32), Times.Never);

        }
    }
}
