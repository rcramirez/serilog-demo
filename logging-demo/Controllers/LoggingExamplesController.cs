using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace logging_demo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Consumes("application/json", "text/json")]
    public class LoggingExamplesController : ControllerBase
    {

        private readonly ILogger<LoggingExamplesController> _logger;

        public LoggingExamplesController(ILogger<LoggingExamplesController> logger)
        {
            _logger = logger;
        }

        [HttpGet("exampleStringInterpolation")]
        public async Task<ActionResult> Get1()
        {
            Action<int> logInformationAction = (input) => _logger.LogInformation($"The value of i is {input}");

            DoLogSituation(logInformationAction);

            return Ok();
        }

        [HttpGet("exampleWithArgs")]
        public async Task<ActionResult> Get2()
        {
            Action<int> logInformationAction = (input) => _logger.LogInformation("The value of i is {iVariable}", input);

            DoLogSituation(logInformationAction);

            return Ok();
        }

        private void DoLogSituation(Action<int> createLogInformation)
        {
            try
            {
                for (int i = 0; i < 3; i++)
                {
                    if (i == 2)
                    {
                        throw new Exception("Demo Exception");
                    }
                    else
                    {
                        createLogInformation(i);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Demo exception was caught.");
            }
        }
    }
}
