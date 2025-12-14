using Amazon;
using Amazon.SQS;


var awsCredentials = new Amazon.Runtime.BasicAWSCredentials(
    Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID", EnvironmentVariableTarget.User),
    Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY", EnvironmentVariableTarget.User));

var sqsClient = new AmazonSQSClient(awsCredentials, RegionEndpoint.EUWest2);

var publisher = new Publisher.SqsPublisher(sqsClient);

Console.WriteLine("Publishing message..."); 
await publisher.PublishAsync(
    new Messages.CustomerCreated
    {
        Id = 1,
        Name = "John Doe"
    },
    "Viz"
);
Console.WriteLine("message published.");