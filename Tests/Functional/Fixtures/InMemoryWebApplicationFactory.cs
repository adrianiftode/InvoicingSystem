using Api;
using Database;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Tests.Functional.Extensions;

namespace Tests.Functional.Fixtures
{
    public class InMemoryWebApplicationFactory
        : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "Migrate" , "False" }
                });
            });
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
                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<InvoicingContext>();

                    db.Database.EnsureCreated(); // this will fire the calls on HasData
                }
            });
        }

        public WebApplicationFactory<Startup> WithResponse<TRequest, TResponse>
            (TResponse response)
            where TRequest : IRequest<TResponse>
            => this.WithResponse<Startup, TRequest, TResponse>(response);
    }
}
