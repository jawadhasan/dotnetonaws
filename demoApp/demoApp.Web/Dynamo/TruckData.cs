using Amazon.DynamoDBv2.DataModel;

namespace demoApp.Web.Dynamo
{
    [DynamoDBTable("truck_sensor")]
    public class TruckData
    {
        [DynamoDBHashKey]
        [DynamoDBProperty("rideId")]
        public double RideId { get; set; }

        [DynamoDBProperty("temperature")]
        public double Temperature { get; set; }

        [DynamoDBRangeKey]
        [DynamoDBProperty("ts")]
        public long Ts { get; set; }

        [DynamoDBProperty("lat")]
        public double Lat { get; set; }

        [DynamoDBProperty("lon")]
        public double Lon { get; set; }
    }
}
