using demoApp.Core;
using demoApp.Data;
using demoApp.Web.Dtos;
using demoApp.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace demoApp.Web.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class FleetController : ControllerBase
    {
        private readonly VehicleRepository _vehicleRepository;
        private readonly IEventPublisher _eventPublisher;

        public FleetController(VehicleRepository vehicleRepository, IEventPublisher evetPublisher)
        {
            _vehicleRepository = vehicleRepository;
            _eventPublisher = evetPublisher;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var users = _vehicleRepository.GetAll();
            return Ok(users);
        }

        [HttpPost("add")]
        public IActionResult Add([FromBody] VehicleDto vehicleDto)
        {
            var vehicle = new Vehicle();
            vehicle.LicensePlate = vehicleDto.LicensePlate;
            var insertedVehicle = _vehicleRepository.Insert(vehicle);

            //SQS: send message to Fleet for register.
            _eventPublisher.PublishEvent(new Event
            {
                VehicleId = insertedVehicle.Id,
                EventType = EventType.VehicleSaved,
                LicensePlate = insertedVehicle.LicensePlate

            });

            return Ok(insertedVehicle);
        }


        [HttpPost("start/{id}")]
        public IActionResult Start(int id)
        {
            var vehicle = _vehicleRepository.GetById(id);

            if (vehicle != null)
            {
                //SQS: send message to Fleet for start.
                _eventPublisher.PublishEvent(new Event
                {
                    VehicleId = vehicle.Id,
                    EventType = EventType.StartVehicle,
                    LicensePlate = vehicle.LicensePlate
                });

                return Ok($"Vehicle {vehicle.LicensePlate} start initated!");
            }

            return BadRequest("Vehicle does not exist");
        }

        [HttpPost("stop/{id}")]
        public IActionResult Stop(int id)
        {
            var vehicle = _vehicleRepository.GetById(id);

            if (vehicle != null)
            {
                //SQS: send message to Fleet for stop.
                _eventPublisher.PublishEvent(new Event
                {
                    VehicleId = vehicle.Id,
                    EventType = EventType.StopVehicle,
                    LicensePlate = vehicle.LicensePlate
                });

                return Ok($"Vehicle {vehicle.LicensePlate} stop initated!");
            }

            return BadRequest("Vehicle does not exist");
        }

        [HttpPost("shutdown")]
        public IActionResult Shutdown()
        {
            //SQS: send message to Fleet for shutdown.
            _eventPublisher.PublishEvent(new Event
            {
                VehicleId = 0,
                EventType = EventType.Shutdown,
                LicensePlate = string.Empty
            });

            return Ok($"Fleet Shutdown initated!");
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var vehicle = _vehicleRepository.GetById(id);
            _vehicleRepository.RemoveById(id);

            //SQS: send message to Fleet for un-register.
            _eventPublisher.PublishEvent(new Event
            {
                VehicleId = id,
                EventType = EventType.VehicleDeleted,
                LicensePlate = vehicle.LicensePlate

            });
            return Ok();
        }
    }
}
