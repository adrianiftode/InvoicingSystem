using System.Net.Http;

namespace Tests.Functional.Extensions
{
    internal static class HttpClientExtensions
    {
        public static HttpClient WithApiKey(this HttpClient client, string password)
        {
            client.DefaultRequestHeaders.Add("X-Api-Key", password);
            return client;
        }
    }
}
