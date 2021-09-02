using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using transport_sim_app.Configuration;
using transport_sim_app.Models.Factories;
using transport_sim_app.Models.Simulation;

namespace transport_sim_app.Models.Repository
{
    class SimulationRepository : ISimulationRepository
    {
        readonly ILogger<SimulationRepository> _logger;

        public ISimulation Simulation { get; protected set; }

        public SimulationRepository(ISimulation simulation, ILogger<SimulationRepository> logger)
        {
            Simulation = simulation;
            _logger = logger;
        }
    }
}