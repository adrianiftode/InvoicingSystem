using Core;
using Core.Pipeline;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Core.Pipeline
{
    public class AuthorizationTests
    {
        [Fact]
        public async Task When_user_is_not_authorized_a_warning_containing_the_user_identity_is_logged()
        {
            // Arrange
            var authorizeMock = new Mock<IAuthorize<TestRequest>>();
            authorizeMock.Setup(c => c.Authorize(It.IsAny<TestRequest>())).ReturnsAsync(false);
            var loggerMock = new Mock<ILogger<Authorization<TestRequest, Result>>>();
            var sut = new Authorization<TestRequest, Result>(authorizeMock.Object, loggerMock.Object);

            // Act
            await sut.Handle(new TestRequest(), default, null);

            // Assert
            loggerMock.VerifyLog(logger => logger.LogWarning("User is not authorized*"));
        }
    }
}
