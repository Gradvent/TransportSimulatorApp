using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using transport_sim_app.Models.Repository;
using transport_sim_app.Configuration;
using Microsoft.Extensions.Options;

namespace transport_sim_app.Controllers
{
    [Route("[controller]")]
    public class SimulationController : AppController
    {
        readonly ISimulationRepository _repository;
        readonly ILogger<SimulationController> _logger;
        private readonly SimulationOptions _options;

        public SimulationController(
            ISimulationRepository repository,
            ILogger<SimulationController> logger, 
            IOptions<SimulationOptions> options)
        {
            _repository = repository;
            _logger = logger;
            _options = options.Value;
        }

        [HttpGet("start")]
        public async Task<ActionResult> GetStart() {
            await _repository.Simulation.Start();
            return Ok();
        } 
        
        [HttpPost("distance/{distance}")]
        public ActionResult SetDistance(int distance) {
            
            _repository.Simulation.Options.Distance = distance;
            return Ok();
        } 

        [HttpGet("stop")]
        public async Task<ActionResult> GetStop() {
            await _repository.Simulation.Stop();
            return Ok();
        }

        [HttpGet("config")]
        public ActionResult GetConfig() {

            return Ok((object)_options);
        }
    }
}
