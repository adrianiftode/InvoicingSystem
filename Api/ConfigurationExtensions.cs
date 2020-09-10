using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;

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
                        type = contextFeature.Error.GetType().ToString(),
                        message = contextFeature.Error.Message,
                        body = contextFeature.Error.ToString()
                    };

                    await using var writer = new StreamWriter(context.Response.Body, Encoding.UTF8, 1024, true);
                    await JsonSerializer.SerializeAsync(context.Response.Body, error);
                    await writer.FlushAsync();
                }
            });
        });
    }
}
