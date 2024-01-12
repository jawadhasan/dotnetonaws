namespace demoApp.Web.Dtos
{
    public class VehicleDto
    {
        public int Id { get; set; } //set from server
        public string LicensePlate { get; set; }
        public double Temperature { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
    }
}
