
using Amazon.SQS;
using Amazon.SQS.Model;
using System.Net;

namespace Consumer
{
    public class SqsConsumerService : BackgroundService
    {
        private readonly IAmazonSQS _sqsClient;
        private const string QueueName = "Viz";
        private readonly List<string> _messageAttributeNames = new() { "All" };
        public SqsConsumerService(IAmazonSQS sqsClient)
        {
            _sqsClient = sqsClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var queryUrl = await _sqsClient.GetQueueUrlAsync(QueueName, stoppingToken);
            var receiveMessageRequest = new ReceiveMessageRequest
            {
                QueueUrl = queryUrl.QueueUrl,
                MaxNumberOfMessages = 10,
                WaitTimeSeconds = 20,
                MessageAttributeNames = _messageAttributeNames,
            };

            while (!stoppingToken.IsCancellationRequested)
            {
                var receiveMessageResponse = await _sqsClient.ReceiveMessageAsync(receiveMessageRequest, stoppingToken);
                
                if (receiveMessageResponse.HttpStatusCode != HttpStatusCode.OK || receiveMessageResponse.Messages == null)
                    continue;

                foreach (var message in receiveMessageResponse.Messages)
                {
                    Console.WriteLine($"Received message: {message.Body}");
                    // Process the message here
                    // After processing, delete the message from the queue
                    await _sqsClient.DeleteMessageAsync(queryUrl.QueueUrl, message.ReceiptHandle, stoppingToken);
                }
            }
        }
    }
}
