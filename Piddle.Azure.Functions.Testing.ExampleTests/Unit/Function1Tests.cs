using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Piddle.Azure.Functions.Testing.ExampleHttpTrigger;

namespace Piddle.Azure.Functions.Testing.ExampleTests.Unit
{
    [TestFixture]
    [Category("Unit")]
    public class Function1Tests
    {
        public TestWebApplicationFactory TestWebApplicationFactory { get; set; }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            TestWebApplicationFactory = new TestWebApplicationFactory();
        }

        [Test]
        [Description(@"
            Scenario: I'm a developer and I want to Unit Test the HttpTrigger-ed Function code without caring about
                routing or anything specific like that. Fast tests please.

            GIVEN a Mock HttpRequest
            AND it has an empty Query
            AND it has an empty Body
            WHEN Function1.Run is called
            AND the Mock HttpRequest is passed in
            THEN BadRequestObjectResult is returned
            AND the Value of it is 'Direct: Please pass a name on the query string or in the request body'
        ")]
        public async Task TestFunction1ViaDirectCall()
        {
            // GIVEN a Mock HttpRequest
            var mockHttpRequest = new Mock<HttpRequest> { CallBase = true };
            // AND it has an empty Query
            mockHttpRequest.SetupGet(request => request.Query).Returns(new QueryCollection());
            // AND it has an empty Body
            mockHttpRequest.SetupGet(request => request.Body).Returns(new MemoryStream());

            // WHEN Function1.Run is called
            // AND the Mock HttpRequest is passed in
            var result = await Function1.Run(mockHttpRequest.Object, "Direct", Mock.Of<ILogger>());
            
            // THEN BadRequestObjectResult is returned
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            // AND the Value of it is 'Direct: Please pass a name on the query string or in the request body'
            Assert.That(((BadRequestObjectResult)result).Value,
                Is.EqualTo("Direct: Please pass a name on the query string or in the request body"));
        }
        
        [Test]
        [Description(@"
            Scenario: I'm a developer and I want to Unit Test the HttpTrigger-ed Function and I really do care about
                routing or something specific like that. In-process and as fast as you can please.

            GIVEN a HttpClient
            WHEN GET a/HttpClient/route is called
            THEN a 400 BadRequest response is returned
            AND it has a Content of 'HttpClient: Please pass a name on the query string or in the request body'
        ")]
        public async Task TestFunction1ViaInMemoryServiceWrapper()
        {
            // GIVEN a HttpClient
            var httpClient = TestWebApplicationFactory.CreateClient();
            
            // WHEN GET a/HttpClient/route is called
            var response = await httpClient.GetAsync("a/HttpClient/route");
            
            // THEN a 400 BadRequest response is returned
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            // AND it has a Content of 'HttpClient: Please pass a name on the query string or in the request body'
            Assert.That(await response.Content.ReadAsStringAsync(),
                Is.EqualTo("HttpClient: Please pass a name on the query string or in the request body"));
        }
    }
}
