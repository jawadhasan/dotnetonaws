using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace demoApp.Web.Dynamo
{
    public class TruckSensorRepo
    {
        private readonly DynamoDBContext _context;

        //ctor
        public TruckSensorRepo(IAmazonDynamoDB client)
        {
            _context = new DynamoDBContext(client);
        }
        public async Task<IEnumerable<dynamic>> GetAllItems()
        {
            //we need to pass scan condition, even empty in this case
            var scanCondition = new List<ScanCondition>();

            return await _context
                .ScanAsync<TruckData>(scanCondition)//Scan all items. It is an expensive operation
                .GetRemainingAsync();
        }
    }
}
