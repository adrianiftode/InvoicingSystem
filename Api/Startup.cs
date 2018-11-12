using System.IO;
using System.Net;
using Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Database;

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
            services
                .AddRepositories(Configuration)
                .AddCoreServices()
                //.AddAuthenticationCore(options => { options.DefaultChallengeScheme = "Test"; })
                .AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //app.UseAuthentication();


            if (!env.IsDevelopment())
            {
                app.UseHsts();
            }

            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        var error = new
                        {
                            value = new[] { "Internal Server Error." },
                            error = contextFeature.Error
                        };

                        using (var writer = new StreamWriter(context.Response.Body))
                        {
                            var serializer = new JsonSerializer
                            {
                                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                                
                            };
                            serializer.Serialize(writer, error);
                            await writer.FlushAsync().ConfigureAwait(false);
                        }
                    }
                });
            });

            app.UseMvc();
        }
    }
}
