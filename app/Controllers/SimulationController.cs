using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using transport_sim_app.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace transport_sim_app.Controllers
{
    [Route("[controller]")]
    public class SimulationController : AppController
    {
        readonly ISimulationRepository _repository;
        readonly ILogger<SimulationController> _logger;
        private readonly IConfiguration _config;

        public SimulationController(
            ISimulationRepository repository,
            ILogger<SimulationController> logger, IConfiguration config)
        {
            _repository = repository;
            _logger = logger;
            _config = config;
        }

        [HttpGet("start")]
        public ActionResult GetStart() {
            _repository.Start();
            return Ok();
        } 
        
        [HttpPost("distance/{distance}")]
        public ActionResult GetStart(int distance) {
            
            _repository.SetDistance(distance);
            return Ok();
        } 

        [HttpGet("stop")]
        public ActionResult GetStop() {
            _repository.Stop();
            return Ok();
        }

        [HttpGet("config")]
        public ActionResult GetConfig() {

            return Ok();
        }
    }
}
