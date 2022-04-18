using demoApp.Web.Dynamo;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace demoApp.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly TruckSensorRepo _truckSensorRepo;

        //ctor
        public VehiclesController(TruckSensorRepo truckSensorRepo)
        {
            _truckSensorRepo = truckSensorRepo;
        }

        [HttpGet]      
        public async Task<dynamic> Get()
        {
            var result = await _truckSensorRepo.GetAllItems();
            var count = result.Count();

            return new
            {
                Result = result,
                Count = count,
                GeneratedAt = DateTime.Now
            };
        }
    }
}
