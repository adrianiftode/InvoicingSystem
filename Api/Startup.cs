using Api.Authentication;
using Core;
using Database;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = UserApiKeyDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = UserApiKeyDefaults.AuthenticationScheme;
            }).AddScheme<AuthenticationSchemeOptions, CustomAuthenticationHandler>(UserApiKeyDefaults.AuthenticationScheme, _ => { });
            ;
            services
                .AddRepositories(Configuration)
                .AddCoreServices()
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication(); // use the authentication middleware 

            if (!env.IsDevelopment())
            {
                app.UseHsts();
            }

            app.UseCustomJsonErrors();
            app.UseMvc();


            app.UseMigrations(Configuration);
        }
    }
}
