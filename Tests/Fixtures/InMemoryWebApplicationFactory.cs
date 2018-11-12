using Api;
using Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Fixtures
{
    public class InMemoryWebApplicationFactory
        : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {         
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





            //builder.ConfigureTestServices(x =>
            //{
            //    x.AddAuthentication(options =>
            //        {
            //            options.DefaultAuthenticateScheme = "Test Scheme";
            //            options.DefaultChallengeScheme = "Test Scheme";
            //        })
            //        .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>("Test Scheme", _ => { });
            //});


        }
    }
}
