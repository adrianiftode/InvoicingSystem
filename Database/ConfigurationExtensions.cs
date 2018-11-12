using Core.Repositories;
using Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Database
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration) 
            => services
            .AddDbContext<InvoicingContext>(opts => opts.UseSqlServer(configuration.GetConnectionString("Invoicing")))
            .AddTransient<IInvoicesRepository, InvoicesRepository>();
    }
}
