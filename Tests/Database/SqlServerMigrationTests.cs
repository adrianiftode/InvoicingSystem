using Database;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Database
{
    [Trait(Traits.Category, Traits.IntegrationTests)]

    public sealed class SqlServerMigrationTests : IDisposable
    {
        private const string IntegrationDatabase = "invoicing-integration";
        private const string IntegrationTransientDatabase = "invoicing-transient";

        [Theory]
        [InlineData(IntegrationDatabase)]
        [InlineData(IntegrationTransientDatabase)]
        public async Task InvoicesQuery_AfterMigrations_Success(string databaseName)
        {
            //Act
            using (var context = CreateContextForSqlServer(databaseName))
            {
                await context.Database.MigrateAsync();
            }

            using (var context = CreateContextForSqlServer(databaseName))
            {
                var invoices = await context.Invoices.ToListAsync();

                //Assert
                invoices.Should().NotBeNull();
            }
        }

        private static InvoicingContext CreateContextForSqlServer(string databaseName)
        {
            //On a CI machine this should come from some configuration value and the db needs to be accessible
            var options = new DbContextOptionsBuilder<InvoicingContext>()
                .UseSqlServer($@"Server=.;Database={databaseName};Trusted_Connection=True;")
                .Options;
            var context = new InvoicingContext(options);
            return context;
        }

        public void Dispose()
        {
            using (var context = CreateContextForSqlServer(IntegrationTransientDatabase))
            {
                context.Database.EnsureDeleted();
            }
        }
    }
}
