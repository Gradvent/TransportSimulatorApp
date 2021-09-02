using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using transport_sim_app.Models.Repository;

namespace transport_sim_app.Models.Simulation
{
    internal class SimulationWorker : BackgroundService
    {
        private readonly ILogger<SimulationWorker> _logger;
        readonly ISimulation _simulation;

        public SimulationWorker(
            ILogger<SimulationWorker> logger,
            ISimulation simulation)
        {
            _logger = logger;
            _simulation = simulation;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Wait simulation running 
                if (!_simulation.IsSimulating)
                {
                    // _logger.LogInformation("Simulation worker is waiting");
                    await Task.Delay(1000, stoppingToken);
                    continue;
                }
                // _logger.LogInformation("Simulation worker update simulation");
                await _simulation.Update();
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}