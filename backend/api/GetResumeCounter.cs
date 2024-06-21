using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using System.Net.Http;

namespace Company.Function
{
    public static class GetResumeCounter
    {
        [FunctionName("GetResumeCounter")]
        public static HttpResponseMessage Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
        [CosmosDBInput(
            databaseName: "AzureResume",
            containerName: "Counter", 
            Connection = "AzureResumeConnectionString",
            Id = "1" , 
            PartitionKey = "1")] Counter counter,
        // This binding allows us to retrieve an item that has the ID 1
        [CosmosDBOutput(
            databaseName: "AzureResume", 
            containerName: "Counter", 
            Connection = "AzureResumeConnectionString", 
            Id = "1", 
            PartitionKey = "1")] out Counter updatedCounter,
        // Output binding to be returned
        ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            updatedCounter = counter;
            updatedCounter.Count += 1;

            var jsonToReturn = JsonConvert.SerializeObject(counter);

            return new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(jsonToReturn, Encoding.UTF8, "application/json")
            };
        }
    }
}


