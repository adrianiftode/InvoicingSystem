using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;
using System.Net;

namespace Api
{
    public static class ConfigurationExtensions
    {
        public static IApplicationBuilder UseCustomJsonErrors(this IApplicationBuilder app)
            => app.UseExceptionHandler(appError =>
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
    }
}
