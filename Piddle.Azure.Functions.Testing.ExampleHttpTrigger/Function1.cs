using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Piddle.Azure.Functions.Testing.ExampleHttpTrigger
{
    /// <summary>
    /// This was created by the Visual Studio 2019 template.
    /// I added a custom route to it.
    /// </summary>
    public static class Function1
    {
        public const string CustomRoute = "a/{custom}/route";

        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = CustomRoute)] HttpRequest req,
            string custom,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            return name != null
                ? (ActionResult)new OkObjectResult($"{custom}: Hello, {name}")
                : new BadRequestObjectResult($"{custom}: Please pass a name on the query string or in the request body");
        }
    }
}
