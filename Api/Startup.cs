using Api.Authentication;
using Core;
using Core.Pipeline;
using Database;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = UserApiKeyDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = UserApiKeyDefaults.AuthenticationScheme;
            }).AddScheme<AuthenticationSchemeOptions, CustomAuthenticationHandler>(UserApiKeyDefaults.AuthenticationScheme, _ => { });

            services
                .AddRepositories(Configuration)
                .AddMvc()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<UpdateNoteValidator>());

            services.AddMediatR();
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(AttachUser<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(Validation<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(Authorization<,>));
            services.AddScoped(typeof(IAuthorize<>), typeof(DefaultAuthorize<>));
            services.AddScoped(typeof(IAuthorize<CreateInvoiceRequest>), typeof(CreateInvoiceAuthorization));
            services.AddScoped(typeof(IAuthorize<UpdateNoteRequest>), typeof(UpdateNoteAuthorization));
            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseAuthentication(); // use the authentication middleware 

            if (!env.IsDevelopment())
            {
                app.UseHsts();
            }

            app.UseCustomJsonErrors();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
            app.UseMigrations(Configuration);
        }
    }
}
