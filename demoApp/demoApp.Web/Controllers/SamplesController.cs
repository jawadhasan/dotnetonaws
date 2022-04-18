using Microsoft.AspNetCore.Mvc;
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
        private string BooksAPIEndpoint = "https://o6p76ra4e4xubup5vhmwwhg2v40agvms.lambda-url.eu-central-1.on.aws/"; //Get from Env
       

        public SamplesController(ILogger<SamplesController> logger)
        {
            _logger = logger;
            _client = new HttpClient();
        }

        [HttpGet("getHttp")]
        public async Task<dynamic> GetHttp()
        {
            var httpResponse = await _client.GetAsync(BooksAPIEndpoint);

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot retrieve tasks");
            }

            var content = await httpResponse.Content.ReadAsStringAsync();   
            return Ok(content);
        }


        [HttpGet("getEnv")]
        public dynamic GetEnv()
        {
            dynamic payload = new
            {
                Notes = "Environment.GetEnvironmentVariable('KEYNAME')",
                BooksAPIEndpoint = Environment.GetEnvironmentVariable("BooksAPIEndpoint"),
                Authority = Environment.GetEnvironmentVariable("Authority"),
                DefaultConnection = Environment.GetEnvironmentVariable("DefaultConnection")
            };

            return Ok(payload);
        }


        [HttpGet("logSamples")]
        public dynamic LogSamples()
        {
            _logger.LogInformation("LogInformation");
            _logger.LogWarning("LogWarning");
            _logger.LogError("LogError");
            _logger.LogCritical("LogCritical");
            _logger.LogDebug("LogDebug");
            _logger.LogTrace("LogTrace");


            return Ok();
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
