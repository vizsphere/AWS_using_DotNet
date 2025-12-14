using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using CustomerApi_using_DynamoDb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading.Tasks;

namespace CustomerApi_using_DynamoDb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IAmazonDynamoDB _dynamoDB;
        private readonly ILogger<CustomerController> _logger;
        private readonly string _tableName = "t_customers";
        
        public CustomerController(IAmazonDynamoDB amazonDynamoDB, ILogger<CustomerController> logger)
        {
            _dynamoDB = amazonDynamoDB;
            _logger = logger;
        }
        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var scanRequest = new ScanRequest
            {
                TableName = _tableName,
            };

            var scanResponse = await _dynamoDB.ScanAsync(scanRequest);

            if(scanResponse.HttpStatusCode != System.Net.HttpStatusCode.OK || scanResponse.Count == 0)
            {
                _logger.LogError("Error scanning DynamoDB table {TableName}. Status code: {StatusCode}", _tableName, scanResponse.HttpStatusCode);
                return Ok(Enumerable.Empty<Customer?>());
            }

            return Ok(scanResponse.Items.Select(x =>
            {
                var json = Document.FromAttributeMap(x).ToJson();
                return JsonSerializer.Deserialize<Customer>(json);
            }));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var getItemById = new GetItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    { "Id", new AttributeValue { S =  id.ToString() } }
                }
            };

            var response = await _dynamoDB.GetItemAsync(getItemById);

            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK || response.Item.Count == 0)
            {
                _logger.LogError("Error getting item with Id {Id} from DynamoDB table {TableName}. Status code: {StatusCode}", id, _tableName, response.HttpStatusCode);
                return BadRequest();
            }

            var itemAsDocument = Document.FromAttributeMap(response.Item);

            return Ok(JsonSerializer.Deserialize<Customer>(itemAsDocument.ToJson()));
        }

        [HttpPost]
        public async Task<IActionResult> Post(Customer customer)
        {
            var newCustomerRequest = new PutItemRequest
            {
                TableName = _tableName,
                Item = Document.FromJson(JsonSerializer.Serialize(customer)).ToAttributeMap()
            };

            var response = await _dynamoDB.PutItemAsync(newCustomerRequest);

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Put(Customer customer)
        {
            var updateRequest = new PutItemRequest
            {
                TableName = _tableName,
                Item = Document.FromJson(JsonSerializer.Serialize(customer)).ToAttributeMap()
            };

            var response = await _dynamoDB.PutItemAsync(updateRequest);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleteRequest = new DeleteItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    { "Id", new AttributeValue { S = id.ToString() } }
                }
            };
            var response = await _dynamoDB.DeleteItemAsync(deleteRequest);
            return Ok(response);
        }
    }
}
