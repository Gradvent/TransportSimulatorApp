using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using transport_sim_app.Models;

namespace transport_sim_app
{
    internal class SimulationWorker : BackgroundService
    {
        private readonly ILogger<SimulationWorker> _logger;
        private readonly ISimulationRepository _repository;

        public SimulationWorker(ILogger<SimulationWorker> logger, ISimulationRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Wait simulation running 
                if (!_repository.IsRunning) {
                    // _logger.LogInformation("Simulation worker is waiting");
                    await Task.Delay(1000, stoppingToken);
                    continue;
                }
                // _logger.LogInformation("Simulation worker update simulation");
                await _repository.Update();
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}