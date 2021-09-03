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

using transport_sim_app.Models.Simulation;

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
        public ActionResult GetStart()
        {
            _repository.Simulation.Start();
            return Ok();
        }

        [HttpPost("distance/{distance}")]
        public ActionResult SetDistance(int distance)
        {

            _repository.Simulation.Options.Distance = distance;
            return Ok();
        }

        [HttpGet("stop")]
        public ActionResult GetStop()
        {
            _repository.Simulation.Stop();
            return Ok();
        }

        [HttpGet("config")]
        public ActionResult GetConfig()
        {

            return Ok((object)_options);
        }

        [HttpGet("status")]
        public ActionResult GetStatus()
        {
            var source = _repository.Simulation.SimulationEventArgs;
            var args = new SimulationEventArgs<object>
            {
                Message = source.Message,
                Status = source.Status,
                TrackDistance = source.TrackDistance,
                Transports = source.Transports.ToArray<object>()
            };
            return Ok((object) args);
        }
    }
}
