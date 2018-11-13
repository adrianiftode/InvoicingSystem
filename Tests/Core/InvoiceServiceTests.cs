using Core;
using Core.Repositories;
using Core.Services;
using FluentAssertions;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Core
{
    public class InvoiceServiceTests
    {
        private readonly Mock<IInvoicesRepository> _repository;
        private readonly InvoicesService _sut;

        public InvoiceServiceTests()
        {
            _repository = new Mock<IInvoicesRepository>();
            _sut = new InvoicesService(_repository.Object);
        }

        [Fact]
        public async Task Create_ReturnsInvoice()
        {
            //Arrange
            var request = new CreateInvoiceRequest
            {
                Identifier = "INV-001",
                Amount = 150.05m,
                User = TestsHelpers.CreateUser("1", "Admin")
            };

            //Act
            var invoice = await _sut.Create(request);

            //Assert
            invoice.UpdatedBy.Should().Be("1");
            invoice.Amount.Should().Be(request.Amount);
            invoice.Identifier.Should().Be(request.Identifier);
        }

        [Fact]
        public async Task Create_UsesRepositoryCreate()
        {
            //Arrange
            var request = new CreateInvoiceRequest
            {
                Identifier = "INV-001",
                Amount = 150.05m,
                User = TestsHelpers.CreateUser("1", "Admin")
            };

            //Act
            await _sut.Create(request);

            //Assert
            _repository.Verify(c => c.Create(It.IsAny<Invoice>()), Times.Once);
        }

        [Theory]
        [InlineData("Admin")]
        [InlineData("User")]
        public async Task Create_WithAnyUserType_CreatesAnInvoice(string role)
        {
            //Arrange
            var request = new CreateInvoiceRequest
            {
                Identifier = "INV-001",
                Amount = 150.05m,
                User = TestsHelpers.CreateUser("1", role)
            };

            //Act
            var invoice = await _sut.Create(request);

            //Assert
            invoice.Should().NotBeNull();
        }

        [Fact]
        public async Task Create_WhenInvoiceAlreadyExists_DoesNotCreate()
        {
            //Arrange
            _repository
                .Setup(c => c.GetByIdentifier("INV-001"))
                .ReturnsAsync(new Invoice { Identifier = "INV-001" });
            var request = new CreateInvoiceRequest
            {
                Identifier = "INV-001",
                Amount = 150.05m,
                User = TestsHelpers.CreateUser("1", "Admin")
            };

            //Act
            var invoice = await _sut.Create(request);

            //Assert
            _repository.Verify(c => c.Create(It.IsAny<Invoice>()), Times.Never);
            invoice.Should().BeNull();
        }
    }
}
