using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using Moq;

using Core.DTO.UseCaseResponses;
using Core.Interfaces.Gateways.Reposytories;
using Core.Domain.Entities;
using Core.UseCases;
using Core.DTO.UseCaseRequests;

namespace Tests.Core.UnitTests.UseCases
{
    [TestFixture]
    public class VerifyEmailUseCaseUnitTest : UseCaseUnitTestBase<VerifyEmailTokenResponce>
    {
        [Test]
        public async Task Handle_ValidToken_ShouldSucceed()
        {
            var mockEmailTokenReposytory = new Mock<IEmailTokenReposytory>();
            mockEmailTokenReposytory.Setup(repo => repo.GetToken(It.IsAny<string>())).ReturnsAsync(new EmailConfirmToken("", 0));

            _mockUserReposytory.Setup(repo => repo.GetById(It.IsAny<long>())).ReturnsAsync(new User("", "", "", "", ""));
            _mockUserReposytory.Setup(repo => repo.ConfirmEmail(It.IsAny<User>())).ReturnsAsync(true);

            var useCase = new VerifyEmailTokenUseCase(mockEmailTokenReposytory.Object, _mockUserReposytory.Object);
            var result = await useCase.Handle(new VerifyEmailTokenRequest(""), _mockOutputPort.Object);

            Assert.True(result);
        }


        [Test]
        public async Task Handle_InvalidToken_ShouldFailed()
        {
            var mockEmailTokenReposytory = new Mock<IEmailTokenReposytory>();
            mockEmailTokenReposytory.Setup(repo => repo.GetToken(It.IsAny<string>())).ReturnsAsync((EmailConfirmToken)null);

            var useCase = new VerifyEmailTokenUseCase(mockEmailTokenReposytory.Object, null);
            var result = await useCase.Handle(new VerifyEmailTokenRequest(""), _mockOutputPort.Object);

            Assert.False(result);
        }
    }
}
