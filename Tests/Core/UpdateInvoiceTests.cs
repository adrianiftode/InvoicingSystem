using Core;
using Core.Repositories;
using FluentAssertions;
using Moq;
using System.Threading.Tasks;
using Core.Handlers;
using Tests.Extensions;
using Xunit;

namespace Tests.Core
{
    public class UpdateInvoiceTests
    {
        private readonly Mock<IInvoicesRepository> _repository;
        private readonly UpdateInvoiceHandler _sut;

        public UpdateInvoiceTests()
        {
            _repository = new Mock<IInvoicesRepository>();
            _repository.Setup(c => c.Get(1))
                .ReturnsAsync(new Invoice
                {
                    InvoiceId = 1,
                    Identifier = "INV-001",
                    Amount = 150.05m,
                    UpdatedBy = TestsHelpers.CreateUser("1", Roles.Admin).GetIdentity()
                });
            _repository.Setup(c => c.GetByIdentifier("INV-002"))
                .ReturnsAsync(new Invoice
                {
                    InvoiceId = 2,
                    Identifier = "INV-002"
                });
            _sut = new UpdateInvoiceHandler(_repository.Object);
        }

        [Fact]
        public async Task Update_ReturnsUpdatedInvoice()
        {
            //Arrange
            var request = new UpdateInvoiceRequest
            {
                InvoiceId = 1,
                Identifier = "INV-001-A",
                Amount = 160.05m,
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
        public async Task Update_UsesRepositoryUpdate()
        {
            //Arrange
            var request = new UpdateInvoiceRequest
            {
                InvoiceId = 1,
                Identifier = "INV-001-A",
                Amount = 160.05m,
                User = TestsHelpers.CreateUser("1", Roles.Admin)
            };

            //Act
            await _sut.Handle(request);

            //Assert
            _repository.Verify(c => c.Update(), Times.Once);
        }

        [Fact]
        public async Task Update_WhenUpdatingUserIsNotTheCreator_ShouldNotUpdate()
        {
            //Arrange
            var request = new UpdateInvoiceRequest
            {
                InvoiceId = 1,
                Identifier = "INV-001-A",
                Amount = 160.05m,
                User = TestsHelpers.CreateUser("2", Roles.Admin)
            };

            //Act
            var result = await _sut.Handle(request);

            //Assert
            result.ShouldFail();
            result.Item.Should().BeNull();
            _repository.Verify(c => c.Update(), Times.Never);
        }

        [Fact]
        public async Task Update_WhenTheInvoiceDoesNotExists_ShouldNotUpdate()
        {
            //Arrange
            var request = new UpdateInvoiceRequest
            {
                InvoiceId = 2,
                User = TestsHelpers.CreateUser("2", Roles.Admin)
            };

            //Act
            var result = await _sut.Handle(request);

            //Assert
            result.ShouldFail();
            result.Item.Should().BeNull();
            _repository.Verify(c => c.Update(), Times.Never);
        }

        [Fact]
        public async Task Update_WhenIdentifierIsUsedByADifferentInvoice_ShouldNotUpdate()
        {
            //Arrange
            var request = new UpdateInvoiceRequest
            {
                InvoiceId = 1,
                Identifier = "INV-002",
                User = TestsHelpers.CreateUser("1", Roles.Admin)
            };

            //Act
            var result = await _sut.Handle(request);

            //Assert
            result.ShouldFail();
            result.Item.Should().BeNull();
            _repository.Verify(c => c.Update(), Times.Never);
        }
    }
}
