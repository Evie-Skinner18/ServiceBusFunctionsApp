namespace ServiceBusFunctionsApp
{
    public static class BusArrivalFunction
    {
        [FunctionName("BusArrivalFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("BusArrivalFunction processed a request.");

            string busNumber = req.Query["busNumber"];
            string busDestination = req.Query["busDestination"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            busNumber = busNumber ?? data?.busNumber;
            busDestination = busDestination ?? data?.busDestination;

            string responseMessage = $"Sending a message about the {busNumber} bus to {busDestination}!";

            var config = new ConfigurationBuilder()
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            string serviceBusConnectionString = config["ServiceBusConnectionString"];
            string topicName = config["TopicName"];
            string subscriptionName = config["SubscriptionName"];
            StorageCredentials storageAccountCredentials = new StorageCredentials(config["StorageAccountName"], config["StorageAccountKey"]);

            TopicClient busArrivalTopic = new TopicClient(serviceBusConnectionString, topicName, null);
            SubscriptionClient subscriptionClient = new SubscriptionClient(serviceBusConnectionString, topicName, subscriptionName);
            CloudStorageAccount storageAccount = new CloudStorageAccount(storageAccountCredentials, true);
            CloudQueueClient storageQueueClient = storageAccount.CreateCloudQueueClient();

            BusArrival stonehouseBusArrival = new BusArrival(Int32.Parse(busNumber.Trim()), busDestination, DateTime.Now);
            string busArrivalJsonPayload = JsonConvert.SerializeObject(stonehouseBusArrival);

            log.LogInformation($"Sending message: {busArrivalJsonPayload}");
            Message busArrivalMessage = new Message(Encoding.UTF8.GetBytes(busArrivalJsonPayload));
            await busArrivalTopic.SendAsync(busArrivalMessage);

            return new OkObjectResult(responseMessage);
        }
    }
}
