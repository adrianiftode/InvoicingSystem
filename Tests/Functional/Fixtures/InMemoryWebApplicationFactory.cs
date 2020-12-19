using System;
using Api;
using Audit.Core.Providers;
using Database;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Microsoft.Extensions.Hosting;
using Tests.Functional.Extensions;

namespace Tests.Functional.Fixtures
{
    public class InMemoryWebApplicationFactory
        : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            Audit.Core.Configuration.ResetCustomActions();
            Audit.Core.Configuration.DataProvider = new NullDataProvider();

            builder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "Migrate" , "False" }
                });
            });

            // this duplication doesn't happen, we hope at least on of them is called after Startup
            builder.ConfigureServices(services =>
            {
                var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();
                services.AddDbContext<InvoicingContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                    options.UseInternalServiceProvider(serviceProvider);
                });
            });

            builder.ConfigureTestServices(services =>
            {
                var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();
                services.AddDbContext<InvoicingContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                    options.UseInternalServiceProvider(serviceProvider);
                });
            });
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            var host = builder.Build();

            using (var scope = host.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<InvoicingContext>();
                db.Database.EnsureCreated();
            }

            host.Start();
            return host;
        }

        public WebApplicationFactory<Startup> WithResponse<TRequest, TResponse>
            (TResponse response)
            where TRequest : IRequest<TResponse>
            => this.WithResponse<Startup, TRequest, TResponse>(response);
    }
}
