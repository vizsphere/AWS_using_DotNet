using Amazon.Lambda.Core;
using System;
using Newtonsoft.Json;
using Amazon.Lambda.APIGatewayEvents;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AWSLambdaFun
{

    public class Function
    {
        public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
        {
            var customer = JsonConvert.DeserializeObject<Customer>(request.Body);
            var response = new APIGatewayProxyResponse
            {
                StatusCode = 200,
                Body = JsonConvert.SerializeObject(new
                {
                    Message = "Customer received",
                    Customer = customer
                }),
                Headers = new System.Collections.Generic.Dictionary<string, string>
                {
                    { "Content-Type", "application/json" }
                }
            };

            return response;
        }
    }


    public class Customer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}