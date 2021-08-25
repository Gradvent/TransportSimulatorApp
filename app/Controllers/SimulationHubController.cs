using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using transport_sim_app.Models;

namespace transport_sim_app.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SimulationController : ControllerBase
    {
        readonly ISimulationRepository _repository;
        readonly ILogger<SimulationController> _logger;

        public SimulationController(
            ISimulationRepository repository,
            ILogger<SimulationController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("start")]
        public ActionResult GetStart() {
            _logger.LogInformation("Simulation start from controller");
            _repository.Start();
            return Ok();
        } 

        [HttpGet("stop")]
        public ActionResult GetStop() {
            _repository.Stop();
            return Ok();
        }
    }
}
