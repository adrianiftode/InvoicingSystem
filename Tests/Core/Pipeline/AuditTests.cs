using Audit.Core;
using Core;
using Core.Pipeline;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Core.Pipeline
{
    public class AuditTests
    {
        [Fact]
        public async Task Request_Is_Audited()
        {
            // Arrange
            var auditScopeFactoryMock = new Mock<IAuditScopeFactory>();
            var sut = new Audit<TestRequest, Result>(auditScopeFactoryMock.Object);

            // Act
            await sut.Handle(new TestRequest(), default, () => Task.FromResult(Result.Success));

            // Arrange
            auditScopeFactoryMock.Verify(c => c.CreateAsync(It.Is<AuditScopeOptions>(opts => opts.EventType == nameof(TestRequest))));
        }
    }
}
