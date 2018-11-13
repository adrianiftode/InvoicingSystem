using Core.Repositories;
using Database.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Database
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
            => services
                .AddDbContext<InvoicingContext>(opts =>
                    opts.UseSqlServer(configuration.GetConnectionString("Invoicing")))
                .AddTransient<IInvoicesRepository, InvoicesRepository>()
                .AddTransient<INotesRepository, NotesRepository>()
        ;

        public static void UseMigrations(this IApplicationBuilder app, IConfiguration configuration)
        {
            if (configuration["Migrate"] == "true")
            {
                using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
                {
                    var context = serviceScope.ServiceProvider.GetRequiredService<InvoicingContext>();

                    context.Database.Migrate();
                }
            }
        }
    }
}
