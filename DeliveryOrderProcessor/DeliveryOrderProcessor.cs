using DeliveryOrderProcessor.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace DeliveryOrderProcessor
{
    public readonly record struct Person(int age, string name);

    public static class DeliveryOrderProcessor
    {
        [FunctionName("DeliveryOrderProcessor")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("DeliveryOrderProcessor was called.");
            DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(DeliveryInfo));
            DeliveryInfo info = (DeliveryInfo)deserializer.ReadObject(req.Body);
            CosmosDbHandler dbHandler = new CosmosDbHandler();
            await dbHandler.AddItemToDbAsync(info);
            log.LogInformation("New order was created.");
            return new OkResult();
        }
    }
}
