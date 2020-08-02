using Core;
using FluentAssertions;
using System.Threading.Tasks;
using Tests.Extensions;
using Xunit;

namespace Tests.Core
{
    public class OnlyAdminsCreateCreateInvoicesIssue
    {
        private readonly CreateInvoiceAuthorization _sut;

        public OnlyAdminsCreateCreateInvoicesIssue() => _sut = new CreateInvoiceAuthorization();

        [Fact]
        public async Task Create_WithUserType_ShouldNotCreatesAnInvoice()
        {
            //Arrange
            var request = new CreateInvoiceRequest
            {
                Identifier = "INV-001",
                Amount = 150.05m,
                User = TestsHelpers.CreateUser("1", Roles.User)
            };

            //Act
            var result = await _sut.Authorize(request);

            //Assert
            result.Should().BeFalse();
        }
    }
}
