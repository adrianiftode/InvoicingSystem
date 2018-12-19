using System.Threading.Tasks;
using Core;
using Core.Handlers;
using Core.Repositories;
using Moq;
using Xunit;

namespace Tests.Core
{
    public class OnlyAdminsCreateCreateInvoicesIssue
    {
        private readonly CreateInvoiceHandler _sut;

        public OnlyAdminsCreateCreateInvoicesIssue()
        {
            var repository = new Mock<IInvoicesRepository>();
            _sut = new CreateInvoiceHandler(repository.Object);
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
            var result = await _sut.Handle(request);

            //Assert
            result.ShouldFail();
        }
    }
}
