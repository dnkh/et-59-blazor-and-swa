using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.Cosmos;
using static System.Environment;
using MyPOIs.Models;
using PartitionKey = Microsoft.Azure.Cosmos.PartitionKey;

namespace Api
{
    public static class mypois
    {
        [FunctionName("GetPOIs")]
        public static async Task<IActionResult> GetPOIs(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "poi")] HttpRequest req,ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            var connectionString = GetEnvironmentVariable("CosmosDbConnectionString");
            var client = new CosmosClient(connectionString);

            var container = client.GetContainer("MyPOIs", "POIs2");
            var query = container.GetItemQueryIterator<POIData>();
            var results = new List<POIData>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }

            return new OkObjectResult(results);
        }
    

        [FunctionName("GetPOI")]
        public static async Task<IActionResult> GetPOI(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "poi/{poiid:guid}")] HttpRequest req, Guid poiid,ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            var connectionString = GetEnvironmentVariable("CosmosDbConnectionString");
            var client = new CosmosClient(connectionString);

            var container = client.GetContainer("MyPOIs", "POIs2");
            var query = new QueryDefinition("SELECT * FROM c WHERE c.id = @POIId").WithParameter("@POIId", poiid.ToString());
            var iterator = container.GetItemQueryIterator<POIData>(query);
            var results = new List<POIData>();
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                results.AddRange(response.ToList());
            }

            return new OkObjectResult(results);
        }


        [FunctionName("AddPOI")]
        public static async Task<IActionResult> AddPOI(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "poi")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<POIData>(requestBody);
            
            var connectionString = GetEnvironmentVariable("CosmosDbConnectionString");
            var client = new CosmosClient(connectionString);

            var container = client.GetContainer("MyPOIs", "POIs2");
            var query = new QueryDefinition("SELECT * FROM c WHERE c.id = @POIId").WithParameter("@POIId", data.id.ToString());
            var iterator = container.GetItemQueryIterator<POIData>(query);
            //get the first or default item from the iterator
            var item = (await iterator.ReadNextAsync()).FirstOrDefault();

            if (item == null)
            {   //create new item
                var createResponse = await container.CreateItemAsync<POIData>(data, new PartitionKey(data.id.ToString()));
                return createResponse.StatusCode == System.Net.HttpStatusCode.Created
                    ? (ActionResult)new OkObjectResult(createResponse.Resource)
                    : new BadRequestObjectResult("Could not create the item.");
            }
            else
            {
                //update the item
                var updateResponse = await container.ReplaceItemAsync<POIData>(data, data.id.ToString(), new PartitionKey(data.id.ToString()));
                return updateResponse.StatusCode == System.Net.HttpStatusCode.OK
                    ? (ActionResult)new OkObjectResult(updateResponse.Resource)
                    : new BadRequestObjectResult("Could not update the item.");
            }
        }

        [FunctionName("DeletePOI")]
        public static async Task<IActionResult> DeletePOI(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "poi")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            POIData data = JsonConvert.DeserializeObject<POIData>(requestBody);
            
            var connectionString = GetEnvironmentVariable("CosmosDbConnectionString");
            var client = new CosmosClient(connectionString);

            var container = client.GetContainer("MyPOIs", "POIs2");
            var response = await container.DeleteItemAsync<POIData>(data.id.ToString(), new PartitionKey(data.id.ToString()));

            return new OkResult();
        }
        [FunctionName("DeletePOIById")]
        public static async Task<IActionResult> DeletePOIById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "poi/{poiid:guid}")] HttpRequest req, Guid poiid, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            POIData data = JsonConvert.DeserializeObject<POIData>(requestBody);

            var connectionString = GetEnvironmentVariable("CosmosDbConnectionString");
            var client = new CosmosClient(connectionString);

            var container = client.GetContainer("MyPOIs", "POIs2");
            var response = await container.DeleteItemAsync<POIData>(poiid.ToString(), new PartitionKey(poiid.ToString()));

            return new OkResult();
        }

    }
}
