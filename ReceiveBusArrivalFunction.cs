using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace ServiceBusFunctionsApp
{
    public class ReceiveBusArrivalFunction
    {
        private readonly ILogger<ReceiveBusArrivalFunction> _logger;
        private IConfigurationRoot _config;

        private string _topicName => _config["TopicName"];
        private string _subscriptionName => _config["SubscriptionName"];

        public ReceiveBusArrivalFunction(ILogger<ReceiveBusArrivalFunction> log)
        {
            _logger = log;
            _config = new ConfigurationBuilder()
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
                
        }

        [FunctionName("ReceiveBusArrivalFunction")]
        // how do I get this config from the local.settings.json?
        public void Run([ServiceBusTrigger("busarrivaltopic", "bussubscription", Connection = "ServiceBusConnectionString")]string mySbMsg)
        {
            // not sure why the message is not appearing now
            _logger.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
            
            // var subscriptionClient = new SubscriptionClient(ServiceBusConnectionString, TopicName, SubscriptionName);
            // var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            // {
            // // Handling our message
            // }
            // subscriptionClient.RegisterMessageHandler(ProcessMessageAsync, messageHandlerOptions);
            // subscriptionClient.CloseAsync();
        }
    }
}
