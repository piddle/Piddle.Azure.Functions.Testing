using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Piddle.Azure.Functions.Testing.ExampleHttpTrigger;

namespace Piddle.Azure.Functions.Testing.ExampleTests
{
    [TestFixture]
    public class Function1Tests
    {
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
            AND the Value of it is 'Please pass a name on the query string or in the request body'
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
            var result = await Function1.Run(mockHttpRequest.Object, Mock.Of<ILogger>());
            
            // THEN BadRequestObjectResult is returned
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            // AND the Value of it is 'Please pass a name on the query string or in the request body'
            Assert.That(((BadRequestObjectResult)result).Value,
                Is.EqualTo("Please pass a name on the query string or in the request body"));
        }
    }
}
