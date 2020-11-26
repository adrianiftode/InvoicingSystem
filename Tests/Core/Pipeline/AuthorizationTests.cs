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
        private readonly Mock<IAuthorize<TestRequest>> _authorizeMock;
        private readonly Mock<ILogger<Authorization<TestRequest, Result>>> _loggerMock;
        private readonly Authorization<TestRequest, Result> _sut;

        public AuthorizationTests()
        {
            _authorizeMock = new Mock<IAuthorize<TestRequest>>();
            _loggerMock = new Mock<ILogger<Authorization<TestRequest, Result>>>();
            _sut = new Authorization<TestRequest, Result>(_authorizeMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task When_user_is_not_authorized_a_warning_containing_the_user_identity_is_logged()
        {
            // Arrange
            _authorizeMock.Setup(c => c.Authorize(It.IsAny<TestRequest>())).ReturnsAsync(false);

            // Act
            await _sut.Handle(new TestRequest(), default, null);

            // Assert
            _loggerMock.VerifyLog(logger => logger.LogWarning("User is not authorized*"));
        }

        [Fact]
        public async Task When_user_is_authorized_a_warning_containing_the_user_identity_is_not_logged()
        {
            // Arrange
            _authorizeMock.Setup(c => c.Authorize(It.IsAny<TestRequest>())).ReturnsAsync(true);

            // Act
            await _sut.Handle(new TestRequest(), default, () => Task.FromResult(Result.Success));

            // Assert
            _loggerMock.VerifyLog(logger => logger.LogWarning("User is not authorized*"), Times.Never);
        }
    }
}
