using Core;
using Core.Repositories;
using FluentAssertions;
using Moq;
using System.Threading.Tasks;
using Core.Handlers;
using Xunit;

namespace Tests.Core
{
    public class CreateInvoiceTests
    {
        private readonly Mock<IInvoicesRepository> _repository;
        private readonly CreateInvoiceHandler _sut;

        public CreateInvoiceTests()
        {
            _repository = new Mock<IInvoicesRepository>();
            _sut = new CreateInvoiceHandler(_repository.Object);
        }

        [Fact]
        public async Task Create_ReturnsInvoice()
        {
            //Arrange
            var request = new CreateInvoiceRequest
            {
                Identifier = "INV-001",
                Amount = 150.05m,
                User = TestsHelpers.CreateUser("1", Roles.Admin)
            };

            //Act
            var result = await _sut.Handle(request);

            //Assert
            var invoice = result.Item;
            result.ShouldBeSuccess();
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
                User = TestsHelpers.CreateUser("1", Roles.Admin)
            };

            //Act
            await _sut.Handle(request);

            //Assert
            _repository.Verify(c => c.Create(It.IsAny<Invoice>()), Times.Once);
        }

        [Fact]
        public async Task Create_WithAdminType_CreatesAnInvoice()
        {
            //Arrange
            var request = new CreateInvoiceRequest
            {
                Identifier = "INV-001",
                Amount = 150.05m,
                User = TestsHelpers.CreateUser("1", Roles.Admin)
            };

            //Act
            var result = await _sut.Handle(request);

            //Assert
            result.ShouldBeSuccess();
            result.Item.Should().NotBeNull();
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
                User = TestsHelpers.CreateUser("1", Roles.Admin)
            };

            //Act
            var result = await _sut.Handle(request);

            //Assert
            _repository.Verify(c => c.Create(It.IsAny<Invoice>()), Times.Never);
            result.ShouldFail();
            result.Errors.Should().Contain("The invoice cannot be created because another invoice with the same Identifier already exists.");
        }
    }
}
