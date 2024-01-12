using System;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using demoApp.Web.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace demoApp.Web.Services
{
    public class SQSEventPublisher : IEventPublisher
    {
        private readonly ILogger<SQSEventPublisher> _logger;
        private readonly AmazonSQSClient _sqsClient;
        private readonly string _queueUrl;

        public SQSEventPublisher(IOptions<SQSSettings> settings, ILogger<SQSEventPublisher> logger)
        {
            _logger = logger;

            try
            {
                AmazonSQSConfig amazonSQSConfig = new AmazonSQSConfig();
                amazonSQSConfig.RegionEndpoint = RegionEndpoint.GetBySystemName(settings.Value.AWSRegion);
                amazonSQSConfig.Validate();

                _sqsClient = new AmazonSQSClient(amazonSQSConfig);
                _queueUrl = settings.Value.QueueUrl;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Couldn't create an instance of SQSEventPublisher");

                throw;
            }
        }

        public async void PublishEvent(Event eventData)
        {
            try
            {
                SendMessageRequest sendMessageRequest = new SendMessageRequest(
                    _queueUrl,
                    JsonConvert.SerializeObject(eventData)
                );

                sendMessageRequest.MessageAttributes.Add(
                    "Publisher",
                    new MessageAttributeValue()
                    {
                        DataType = "String", //String, Number, Binary 
                        StringValue = "fleet"
                    }
                );

                SendMessageResponse sendMessageResponse = await _sqsClient.SendMessageAsync(sendMessageRequest);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Couldn't publish an event to SQS");
                throw;
            }
        }
    }
}
