using System.Threading.Tasks;
using Core;
using Core.Repositories;
using Core.Services;
using Moq;
using Xunit;

namespace Tests.Core
{
    public class OnlyAdminsCreateCreateInvoicesIssue
    {
        private readonly InvoicesService _sut;

        public OnlyAdminsCreateCreateInvoicesIssue()
        {
            var repository = new Mock<IInvoicesRepository>();
            _sut = new InvoicesService(repository.Object);
        }

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
            var result = await _sut.Create(request);

            //Assert
            result.ShouldFail();
        }
    }
}
