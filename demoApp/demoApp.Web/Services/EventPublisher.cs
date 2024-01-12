using Microsoft.Extensions.Logging;

namespace demoApp.Web.Services
{
    public enum EventType
    {

        VehicleSaved = 10,
        VehicleDeleted = 20,
        VehicleRegistered = 30,
        VehicleUnRegistered = 40,
        DataChanged = 50,

        StartVehicle = 100,
        StopVehicle = 110,
        Shutdown = 500
    }
    public class Event
    {
        public EventType EventType { get; set; }
        public int VehicleId { get; set; }
        public string LicensePlate { get; set; }
    }

    public interface IEventPublisher
    {
        void PublishEvent(Event eventData);
    }

    public class SampleEventPublisher : IEventPublisher
    {
        private readonly ILogger<SampleEventPublisher> _logger;
        public SampleEventPublisher(ILogger<SampleEventPublisher> logger)
        {
            _logger = logger;
        }
        public void PublishEvent(Event eventData)
        {
            _logger.LogInformation($"SampleEventPublisher: publishing {eventData.EventType} for {eventData.LicensePlate}");
        }
    }


}
