using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;

using Moq;
using NUnit.Framework;

using Core.Interfaces.Services;
using Core.DTO.UseCaseResponses;
using Core.UseCases;
using Core.Specifications;
using Core.Domain.Entities;
using Core.DTO.UseCaseRequests;

namespace Tests.Core.UnitTests.UseCases
{
    [TestFixture]
    public class RefreshTokenUseCaseUnitTest : UseCaseUnitTestBase<RefreshTokenResponce>
    {
        [Test]
        public async Task Handle_ValidaCredentials_ShouldSucceed()
        {
            var mockTokenValidator = new Mock<IJwtTokenValidator>();
            mockTokenValidator.Setup(validator => validator.GetPrincipalsFromToken(It.IsAny<string>(), It.IsAny<string>())).Returns(new ClaimsPrincipal(new[] 
            { 
                new ClaimsIdentity(new [] {new Claim("id", "112233")})    
            }));

            string refreshToken = "1234";
            var user = new User("", "", "", "", "");
            user.AddRefreshToken(refreshToken, 0, "");

            _mockUserReposytory.Setup(repo => repo.FindOneBySpec(It.IsAny<UserSpecification>())).ReturnsAsync(user);

            _mockTokenFactory.Setup(factory => factory.GenerateToken(32)).Returns("");


            var useCase = new RefreshTokenUseCase(_mockJwtFactory.Object, _mockTokenFactory.Object, mockTokenValidator.Object, _mockUserReposytory.Object);
            var responce = await useCase.Handle(new RefreshTokenRequest("", refreshToken, ""), _mockOutputPort.Object);

            Assert.True(responce);

        }

        [Test]
        public async Task Handle_InvalidToken_ShouldFail()
        {
            var mockTokenValidator = new Mock<IJwtTokenValidator>();
            mockTokenValidator.Setup(validator => validator.GetPrincipalsFromToken(It.IsAny<string>(), It.IsAny<string>())).Returns((ClaimsPrincipal)null);

            var useCase = new RefreshTokenUseCase(null, null, mockTokenValidator.Object, null);

            var responce = await useCase.Handle(new RefreshTokenRequest("", "", ""), _mockOutputPort.Object);

            Assert.False(responce);
        }
    }
}
