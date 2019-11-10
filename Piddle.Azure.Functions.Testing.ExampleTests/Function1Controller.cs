using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Piddle.Azure.Functions.Testing.ExampleHttpTrigger;

namespace Piddle.Azure.Functions.Testing.ExampleTests
{
    [ApiController]
    public class Function1Controller : ControllerBase
    {
        // At time of writing, my general thoughts about this are that it has shortcomings, but...
        // 1) The URL binding is similar enough for me to get away with this most of the time.
        // 2) If I wanted the ILogger or another injected dependency to be injected all the way,
        //    then I could set it up as a Service in TestStartup.ConfigureServices.
        // 3) I could maybe write some code to inspect the HTTP Methods accepted by the HttpTrigger
        //    binding on the Run method and check this method is allowed.
        // 4) There is no substitute for a real API test on the route in a deployment instance and
        //    this isn't meant to replace that either.
        // 5) There are other uses for this crude technique, for example one Azure Function calls
        //    another via HttpClient, if I wanted to test that interaction from end-to-end and
        //    in-process within an "Integration" Unit Test then this might help.
        [HttpGet]
        [Route(Function1.CustomRoute)]
        public async Task<IActionResult> GetFunction1(string custom)
        {
            return await Function1.Run(HttpContext.Request, custom, Mock.Of<ILogger>());
        }
    }
}
