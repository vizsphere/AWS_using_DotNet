using Amazon.SQS;
using Amazon.SQS.Model;
using Messages;

namespace Publisher
{
    public class SqsPublisher
    {
        private readonly IAmazonSQS _sqs;

        public SqsPublisher(IAmazonSQS sqs)
        {
            _sqs = sqs;
        }


        public async Task PublishAsync<TMessage>(TMessage message, string queueName) where  TMessage : IMessage
        {
            var json = System.Text.Json.JsonSerializer.Serialize(message);
            var queueUrlResponse = await _sqs.GetQueueUrlAsync(queueName);
            var sendMessageRequest = new SendMessageRequest
            {
                QueueUrl = queueUrlResponse.QueueUrl,
                MessageBody = json,

                MessageAttributes = new Dictionary<string, MessageAttributeValue>
                {
                    {
                        nameof(IMessage.MessageTypeName), new MessageAttributeValue
                        {
                            DataType = "String",
                            StringValue = message.MessageTypeName
                        }
                    }
                }
            };

            await _sqs.SendMessageAsync(sendMessageRequest);
        }
    }
}
