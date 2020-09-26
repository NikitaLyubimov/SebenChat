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
        [Test]
        public async Task Handle_ValidCredentials_ShouldSucceed()
        {
            var mockUserReposytory = new Mock<IUserReposytory>();
            mockUserReposytory.Setup(repo => repo.FindByName(It.IsAny<string>())).ReturnsAsync(new User("", "", "", "", ""));
            mockUserReposytory.Setup(repo => repo.CheckPassword(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(true);

            var mockJwtFactory = new Mock<IJwtFactory>();
            mockJwtFactory.Setup(factory => factory.GenerateEncodedToken(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new AccessToken("", 0));

            var mockTokenFactory = new Mock<ITokenFactory>();

            var useCase = new LoginUserUseCase(mockUserReposytory.Object, mockJwtFactory.Object, mockTokenFactory.Object);

            var mockOutPutPort = new Mock<IOutputPort<LoginResponce>>();
            mockOutPutPort.Setup(presenter => presenter.Handle(It.IsAny<LoginResponce>()));

            var responce = await useCase.Handle(new LoginRequest("username", "password", "127.0.0.1"), mockOutPutPort.Object);

            Assert.True(responce);

        }

        [Test]
        public async Task Handle_IncompleteCredentials_ShouldFailed()
        {
            var mockUserReposytory = new Mock<IUserReposytory>();
            mockUserReposytory.Setup(repo => repo.FindByName(It.IsAny<string>())).ReturnsAsync(new User("", "", "", "", ""));
            mockUserReposytory.Setup(repo => repo.CheckPassword(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(false);

            var mockJwtFactory = new Mock<IJwtFactory>();
            mockJwtFactory.Setup(factory => factory.GenerateEncodedToken(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new AccessToken("", 0));

            var mockTokenFactory = new Mock<ITokenFactory>();

            var useCase = new LoginUserUseCase(mockUserReposytory.Object, mockJwtFactory.Object, mockTokenFactory.Object);

            var mockOutputPort = new Mock<IOutputPort<LoginResponce>>();
            mockOutputPort.Setup(presenter => presenter.Handle(It.IsAny<LoginResponce>()));

            var responce = await useCase.Handle(new LoginRequest("", "password", "127.0.0.1"), mockOutputPort.Object);

            Assert.False(responce);
            mockTokenFactory.Verify(factory => factory.GenerateToken(32), Times.Never);

        }

        [Test]
        public async Task Handler_IncorrectPassword_ShouldFail()
        {
            var mockUserReposytory = new Mock<IUserReposytory>();
            mockUserReposytory.Setup(repo => repo.FindByName(It.IsAny<string>())).ReturnsAsync(new User("", "", "", "", ""));
            mockUserReposytory.Setup(repo => repo.CheckPassword(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(false);

            var mockJwtFactory = new Mock<IJwtFactory>();
            mockJwtFactory.Setup(factory => factory.GenerateEncodedToken(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new AccessToken("", 0));

            var mockTokenFactory = new Mock<ITokenFactory>();

            var useCase = new LoginUserUseCase(mockUserReposytory.Object, mockJwtFactory.Object, mockTokenFactory.Object);

            var mockOutputPort = new Mock<IOutputPort<LoginResponce>>();
            mockOutputPort.Setup(presenter => presenter.Handle(It.IsAny<LoginResponce>()));

            var result = await useCase.Handle(new LoginRequest("username", "wrongpassword", "127.0.01"), mockOutputPort.Object);

            Assert.False(result);
            mockTokenFactory.Verify(factory => factory.GenerateToken(32), Times.Never);

        }
    }
}
