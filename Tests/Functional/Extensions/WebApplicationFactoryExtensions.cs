using System.Net.Http;
using System.Threading;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Tests.Functional.Extensions
{
    internal static class WebApplicationFactoryExtensions
    {
        public static WebApplicationFactory<TStartup> WithResponse<TStartup, TRequest, TResponse>
            (this WebApplicationFactory<TStartup> factory, TResponse response)
            where TStartup : class
            where TRequest : IRequest<TResponse> => factory.WithWebHostBuilder(c =>
        {
            c.ConfigureTestServices(srv =>
            {
                var mediatorMock = new Mock<IMediator>();
                mediatorMock
                    .Setup(m => m.Send(It.IsAny<TRequest>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(response);
                ServiceCollectionServiceExtensions.AddTransient(srv, _ => mediatorMock.Object);
            });
        });

        public static HttpClient CreateClient<TStartup>(this WebApplicationFactory<TStartup> factory, string password)
            where TStartup : class
            => factory.CreateClient().WithApiKey(password);
    }
}