using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace demoApp.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SamplesController : ControllerBase
    {
        private readonly ILogger<SamplesController> _logger;
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        private readonly string _externalAPIEndPoint = ""; //Get from configs
        private readonly IHostingEnvironment _hostingEnv;

        public SamplesController(ILogger<SamplesController> logger, IConfiguration configuration, IHostingEnvironment hostingEnv)
        {
            _logger = logger;
            _client = new HttpClient();
            _configuration = configuration;
            _externalAPIEndPoint = _configuration["ExternalAPIEndPoint"];
            _hostingEnv = hostingEnv;


        }

        [HttpGet("getHttp")]
        public async Task<dynamic> GetHttp()
        {
            var httpResponse = await _client.GetAsync(_externalAPIEndPoint);

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot retrieve tasks");
            }

            var content = await httpResponse.Content.ReadAsStringAsync();   
            return Ok(content);
        }

        [HttpGet("getSecure")]
        public ActionResult GetSecure()
        {
            var payload = Guid.NewGuid().ToString("N");
            return Ok(payload);
        }

        [HttpGet("getEnv")]
        public dynamic GetEnv()
        {
            dynamic payload = new
            {
                Notes = "Environment.GetEnvironmentVariable('KEYNAME')",
                Environment = _hostingEnv.EnvironmentName,
                ExternalAPIEndPoint = _configuration["ExternalAPIEndPoint"],
                Authority = Environment.GetEnvironmentVariable("Authority"),              
                DefaultConnection = _configuration["DefaultConnection"]//_configuration["ConnectionStrings:DefaultConnection"]
            };

            return Ok(payload);
        }

        [HttpGet("logSamples")]
        public dynamic LogSamples()
        {
            _logger.LogInformation("LogInformation");
            _logger.LogWarning("LogWarning");            
            _logger.LogCritical("LogCritical");
            _logger.LogDebug("LogDebug");
            _logger.LogTrace("LogTrace");
            return Ok();
        }

        [HttpGet("logError")]
        public dynamic LogError()
        {           
            _logger.LogError("LogError");          
            return Ok("_logger.LogError");
        }

        [HttpGet("produceError")]
        public dynamic ProduceError()
        {
            var firstNum = 10;
            var secondNum = 0;

            var divideByZero = firstNum / secondNum;

            _logger.LogError("LogError");

            return Ok(divideByZero);
        }

        [HttpGet("throwExceptionDemo")]
        public dynamic ThrowExceptionDemo()
        {
            throw new Exception("ThrowExceptionDemo");
        }

    }
}
