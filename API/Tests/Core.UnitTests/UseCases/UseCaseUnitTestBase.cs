using NUnit.Framework;
using Moq;

using Core.DTO;
using Core.Interfaces;
using Core.Interfaces.Gateways.Reposytories;
using Core.Interfaces.Services;
using Core.Domain.Entities;
using Core.DTO.GatewayResponces.Repositories;

namespace Tests.Core.UnitTests.UseCases
{
    public class UseCaseUnitTestBase<TOutputResponce>
        where TOutputResponce : UseCaseResponceMessage
    {
        protected Mock<IUserReposytory> _mockUserReposytory;
        protected Mock<IJwtFactory> _mockJwtFactory;
        protected Mock<ITokenFactory> _mockTokenFactory;
        protected Mock<IOutputPort<TOutputResponce>> _mockOutputPort;

        [SetUp]
        public void Setup()
        {
            _mockUserReposytory = new Mock<IUserReposytory>();
            _mockUserReposytory.Setup(repo => repo.FindByName(It.IsAny<string>())).ReturnsAsync(new User("", "", "", "", ""));
            _mockUserReposytory.Setup(repo => repo.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new CreateUserResponce("", true));
            _mockUserReposytory.Setup(repo => repo.GetByIdentityId(It.IsAny<string>())).ReturnsAsync(new User("", "", "", "", ""));

            _mockJwtFactory = new Mock<IJwtFactory>();
            _mockJwtFactory.Setup(factory => factory.GenerateEncodedToken(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new AccessToken("", 0));

            _mockTokenFactory = new Mock<ITokenFactory>();

            _mockOutputPort = new Mock<IOutputPort<TOutputResponce>>();
            _mockOutputPort.Setup(presenter => presenter.Handle(It.IsAny<TOutputResponce>()));
        }

    }
}
