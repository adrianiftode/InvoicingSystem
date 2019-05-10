using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Tests.Extensions.FluentAssertions
{

    public static class HttpResponseMessageExtensions
    {
        public static async Task<string> GetStringContent(this HttpResponseMessage response)
        {
            string beautified;
            try
            {
                var content = await response.Content.ReadAsStringAsync();
                beautified = JToken.Parse(content).ToString();
            }
            catch
            {
                beautified = "The response content could not be parsed.";
            }

            return beautified;
        }
    }
    public static class HttpResponseMessageFluentAssertionsExtensions
    {
        public static HttpResponseMessageExtendedAssertions Should(this HttpResponseMessage actual)
        {
            return new HttpResponseMessageExtendedAssertions(actual);
        }
    }

    public class HttpResponseMessageExtendedAssertions : ReferenceTypeAssertions<HttpResponseMessage,
            HttpResponseMessageExtendedAssertions>
    {
        public HttpResponseMessageExtendedAssertions(HttpResponseMessage value)
        {
            Subject = value;
        }

        protected override string Identifier => $"{nameof(HttpResponseMessageExtendedAssertions)}";

        public async Task BeWithStatusCode(HttpStatusCode expectedStatusCode,
            string because = "", params object[] becauseArgs)
        {
            var content = await Subject.GetStringContent();
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(expectedStatusCode == Subject.StatusCode)
                .FailWith("Expected {0} {because}, but found {1}. The content was {2}"
                    , expectedStatusCode, Subject.StatusCode, content);
        }

        public async Task<AndConstraint<ObjectAssertions>> BeOk<TModel>(string because = "",
            params object[] becauseArgs)
        {
            var subjectModel = await Subject.Content.ReadAsAsync<TModel>();
            var content = await Subject.GetStringContent();
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(HttpStatusCode.OK == Subject.StatusCode)
                .FailWith("Expected OK {because}, but found {0}. The content was {1}"
                    , Subject.StatusCode, content);

            return new AndConstraint<ObjectAssertions>(new ObjectAssertions(subjectModel));
        }

        public async Task<AndConstraint<ObjectAssertions>> BeCreated<TModel>(string because = "",
            params object[] becauseArgs)
        {
            var subjectModel = await Subject.Content.ReadAsAsync<TModel>();
            var content = await Subject.GetStringContent();
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(HttpStatusCode.Created == Subject.StatusCode)
                .FailWith("Expected Created {because}, but found {0}. The content was {1}"
                    , Subject.StatusCode, content);

            return new AndConstraint<ObjectAssertions>(new ObjectAssertions(subjectModel));
        }

        public async Task<BadRequestAssertions> BeBadRequest(string because = "", params object[] becauseArgs)
        {
            var content = await Subject.GetStringContent();
            var expandoContent = await Subject.Content.ReadAsAsync<ExpandoObject>();
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(HttpStatusCode.BadRequest == Subject.StatusCode)
                .FailWith("Expected OK {because}, but found {0}. The content was {1}"
                    , Subject.StatusCode, content);

            return new BadRequestAssertions(Subject, content, expandoContent);
        }

        public async Task BeNotFound(string because = "", params object[] becauseArgs)
        {
            var content = await Subject.GetStringContent();
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(HttpStatusCode.NotFound == Subject.StatusCode)
                .FailWith("Expected NotFound {because}, but found {0}. The content was {1}"
                    , Subject.StatusCode, content);
        }

        public async Task BeForbidden(string because = "", params object[] becauseArgs)
        {
            var content = await Subject.GetStringContent();
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(HttpStatusCode.Forbidden == Subject.StatusCode)
                .FailWith("Expected Forbidden {because}, but found {0}. The content was {1}"
                    , Subject.StatusCode, content);
        }
    }

    public class ContentAssertions : ReferenceTypeAssertions<string, ContentAssertions>
    {
        public ContentAssertions(string content)
        {
            Subject = content;
        }

        protected override string Identifier => "content";
    }

    public class CreatedAssertions<TModel> : ReferenceTypeAssertions<HttpResponseMessage, CreatedAssertions<TModel>>
    {
        private readonly TModel _model;
        private readonly string _responseContent;
        private readonly ExpandoObject _responseContentExpando;
        public CreatedAssertions(HttpResponseMessage value, TModel model, string responseContent, ExpandoObject responseContentExpando)
        {
            Subject = value;
            _model = model;
            _responseContent = responseContent;
            _responseContentExpando = responseContentExpando;
        }

        protected override string Identifier => "Created";
    }

    public class BadRequestAssertions : ReferenceTypeAssertions<HttpResponseMessage, BadRequestAssertions>
    {
        private readonly string _responseContent;
        private readonly ExpandoObject _responseContentExpando;
        public BadRequestAssertions(HttpResponseMessage value, string responseContent, ExpandoObject responseContentExpando)
        {
            Subject = value;
            _responseContent = responseContent;
            _responseContentExpando = responseContentExpando;
        }

        protected override string Identifier => "BadRequest";

        public AndConstraint<BadRequestAssertions> WithError(string expectedField, string expectedErrorMessage, string because = "", params object[] becauseArgs)
        {
            IDictionary<string, object> properties = _responseContentExpando;
            List<string> errors = properties != null && properties.ContainsKey(expectedField)
                ? ((List<object>)properties[expectedField]).Select(c => c.ToString()).ToList()
                : null;

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(_responseContent != null)
                .FailWith("Expected {context:BadRequest} to have a response body, but the response was not provided")

                .Then
                .ForCondition(properties.ContainsKey(expectedField))
                .FailWith("Expected {context:BadRequest} to contain a response with a field named {0}, but found {1}. " +
                          $"The response content was {Environment.NewLine} {{3}}",
                    expectedField, properties.Keys, _responseContent)

                .Then
                .ForCondition(errors.Any(c => c.Contains(expectedErrorMessage)))
                .FailWith("Expected {context:BadRequest} to contain " +
                          "the error message {0} associated with the {1} field, " +
                          $"but no such message was found in the actual source: {Environment.NewLine}{{2}}{Environment.NewLine}" +
                          $"{Environment.NewLine}{Environment.NewLine}The response content was {Environment.NewLine}{{3}}",
                    expectedErrorMessage,
                    expectedField,
                    errors,
                    _responseContent)
                ;
            return new AndConstraint<BadRequestAssertions>(new BadRequestAssertions(Subject, _responseContent, _responseContentExpando));
        }
    }
}