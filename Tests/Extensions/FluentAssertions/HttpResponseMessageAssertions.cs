using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using Newtonsoft.Json.Linq;

namespace Tests.Extensions.FluentAssertions
{

    public static class HttpResponseMessageExtensions
    {
        public static HttpResponseMessageExtendedAssertions Should(this HttpResponseMessage actual)
        {
            return new HttpResponseMessageExtendedAssertions(actual);
        }
    }

    public class HttpResponseMessageExtendedAssertions : ReferenceTypeAssertions<HttpResponseMessage, HttpResponseMessageExtendedAssertions>
    {
        public HttpResponseMessageExtendedAssertions(HttpResponseMessage value)
        {
            Subject = value;
        }
        protected override string Identifier => $"{nameof(HttpResponseMessage)}";

        [CustomAssertion]
        public HttpResponseMessageExtendedAssertions BeWithStatusCode(HttpStatusCode expectedStatusCode, string because = "", params object[] becauseArgs)
        {
            ExecuteAssertion(expectedStatusCode, because, becauseArgs);
            return this;
        }

        [CustomAssertion]
        public HttpResponseMessageExtendedAssertions BeOk(string because = "", params object[] becauseArgs)
        {
            ExecuteAssertion(HttpStatusCode.OK, because, becauseArgs);
            return this;
        }

        [CustomAssertion]
        public async Task<HttpResponseMessageExtendedAssertions> WithModel<TModel>(object expectedModel, 
            string because = "", params object[] becauseArgs)
        {
            var actualModel = await Subject.Content.ReadAsAsync<TModel>();

            string beautified;
            try
            {
                var content = await Subject.Content.ReadAsStringAsync();
                beautified = JToken.Parse(content).ToString();
            }
            catch
            {
                beautified = "The response content could not be parsed.";
            }

            actualModel.Should().BeEquivalentTo(expectedModel, "The response had the following content {0}", beautified);
            return this;
        }

        private void ExecuteAssertion(HttpStatusCode expectedStatusCode, string because, object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(expectedStatusCode == Subject.StatusCode)
                .FailWith("Expected HttpStatusCode to be {0}{because}, but found {1}. Content was {2}"
                    , expectedStatusCode, Subject.StatusCode, Subject.Content.ReadAsStringAsync().Result);
        }
    }
}
