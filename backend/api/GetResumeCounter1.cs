using System.Text;
using Microsoft.Azure.WebJobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace api
{
    public static class GetResumeCounter1
    {
        [FunctionName("GetResumeCounter1")]
        public static async Task<HttpResponseMessage> Run(
            [Microsoft.Azure.Functions.Worker.HttpTrigger(Microsoft.Azure.Functions.Worker.AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [CosmosDB(
                databaseName: "azcloudresume100",
                containerName: "Counter",
                Connection = "AzureResumeConnectionString",
                Id = "1",
                PartitionKey = "1")] Counter counter,
            [CosmosDB(
                databaseName: "azcloudresume100",
                containerName: "Counter",
                Connection = "AzureResumeConnectionString")] IAsyncCollector<Counter> counterOut,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            counter.Count += 1;
            await counterOut.AddAsync(counter);

            var jsonToReturn = JsonConvert.SerializeObject(counter);

            return new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(jsonToReturn, Encoding.UTF8, "application/json")
            };
        }
    }

    internal class FunctionNameAttribute : Attribute
    {
        private string v;

        public FunctionNameAttribute(string v)
        {
            this.v = v;
        }
    }

    public class Counter
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }
    }
}


