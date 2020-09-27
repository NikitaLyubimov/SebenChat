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
    public class RegisterUserUseCaseUnitTest : UseCaseUnitTestBase<RegisterUserResponce>
    {
        [Test]
        public async Task Handle_ValidCredentials_ShouldSucceed()
        {
            

            var mockEmailActions = new Mock<IEmailActions>();
            mockEmailActions.Setup(email => email.SendMessage(It.IsAny<string>(), It.IsAny<long>())).Returns(Task.CompletedTask);

            var registerUseCase = new RegisterUserUsecase(_mockUserReposytory.Object, mockEmailActions.Object);

            var responce = await registerUseCase.Handle(new RegisterUserRequest("name", "secName", "username", "email@email.ru", "password"), _mockOutputPort.Object);

            Assert.True(responce);
        }
    }
}
