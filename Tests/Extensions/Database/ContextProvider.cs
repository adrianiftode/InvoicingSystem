using Database;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Tests.Extensions.Database
{
    internal class ContextProvider
    {
        public static IEnumerable<object[]> Contexts =>
            new[]
            {
                new object[] { (Func<string, InvoicingContext>) CreateInMemoryContext },
                new object[] { (Func<string, InvoicingContext>) CreateSqliteContext },
            };

        public static InvoicingContext CreateInMemoryContext(string databaseName)
        {
            var options = new DbContextOptionsBuilder<InvoicingContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
            var context = new InvoicingContext(options);
            context.Database.EnsureCreated(); // this will also call HasData
            return context;
        }

        public static InvoicingContext CreateSqliteContext(string databaseName)
        {
            var options = new DbContextOptionsBuilder<InvoicingContext>()
                .UseSqlite(GetSqliteConnection(databaseName))
                .Options;
            var context = new InvoicingContext(options);
            context.Database.EnsureCreated(); // this will also call HasData
            return context;
        }

        private static ConcurrentDictionary<string, SqliteConnection> Connections = new ConcurrentDictionary<string, SqliteConnection>();

        private static SqliteConnection GetSqliteConnection(string databaseName)
            =>
            Connections.GetOrAdd(databaseName, _ =>
            {
                var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();
                return connection;
            });
    }
}