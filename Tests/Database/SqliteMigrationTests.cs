using System.Threading.Tasks;
using Database;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests.Database
{
    public class SqliteMigrationTests
    {
        [Fact(Skip = "Reached to the sqlite limitations (SQLite does not support this migration operation ('AlterColumnOperation'))")]
        public async Task Migrate_WithSqlite_ShouldComplete()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<InvoicingContext>()
                .UseSqlite(connection)
                .Options;

            using (var context = new InvoicingContext(options))
            {
                await context.Database.MigrateAsync();
            }
        }
    }
}
