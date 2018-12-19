using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;

namespace Tests.Functional
{
    public static class HttpClientExtensions
    {
        public static HttpClient WithApiKey(this HttpClient client, string password)
        {
            client.DefaultRequestHeaders.Add("X-Api-Key", password);
            return client;
        }

        public static HttpClient CreateClient<TStartup>(this WebApplicationFactory<TStartup> factory, string password)
            where TStartup : class
            => factory.CreateClient().WithApiKey(password);
    }
}