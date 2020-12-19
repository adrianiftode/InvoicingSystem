using Amazon.QLDB.Driver;
using Audit.Core;
using Database;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Audit.EntityFramework.Configuration.Setup()
                .ForContext<InvoicingContext>(config => config
                    .IncludeEntityObjects()
                    .AuditEventType("{context}:{database}"));

            Audit.Core.Configuration.Setup()
                .UseAmazonQldb(config => config.WithQldbDriver(QldbDriver.Builder()
                .WithLedger("InvoicingSystem")
                .Build())
                .Table("AuditEvents"));

            var host = CreateHostBuilder(args).Build();
            await host.RunAsync();

            Audit.Core.Configuration
                .AddCustomAction(ActionType.OnEventSaving, scope =>
                {
                    var httpContextAccessor = (IHttpContextAccessor)host.Services.GetService(typeof(IHttpContextAccessor));
                    scope.SetCustomField("TraceId", httpContextAccessor.HttpContext.TraceIdentifier);
                });
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
            => Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}
