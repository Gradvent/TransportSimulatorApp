
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using transport_sim_app.Configuration;
using transport_sim_app.Data;
using transport_sim_app.Models.Simulation.States;
using transport_sim_app.Models.Transports;

namespace transport_sim_app.Models.Simulation
{
    public class Simulator : ISimulation
    {
        public ITransportCollection Transports => _transports;
        private readonly ILogger<Simulator> _logger;
        private readonly ITransportCollection _transports;
        private SimulationEventArgs _simEventArgs;

        public SimulationEventArgs SimulationEventArgs 
        { 
            get => new SimulationEventArgs
            {
                Message = _simEventArgs.Message ?? "",
                Status = _simEventArgs.Status ?? "none",
                TrackDistance = Options.Distance,
                Transports = Transports
            };
            set => _simEventArgs = value; 
        }

        public SimulationOptions Options { get; protected set; }

        public ISimulationState State { get; private set; }
        public Simulator(ILogger<Simulator> logger,
                         IOptions<SimulationOptions> options,
                         ITransportCollection transports)
        {
            Options = options.Value;
            _logger = logger;
            _transports = transports;
            SimulationEventArgs = new SimulationEventArgs
            {
                Message = "",
                Status = SimulationStatus.Stopped.ToString(),
                TrackDistance = Options.Distance,
                Transports = _transports
            };
            var stop = new StopSimulationState(this);
            ChangeState(stop);
            Task.Run(stop.Init);
        }

        public void ChangeState(ISimulationState state) => State = state;

        public async Task Start() => await State.Doing();
        public async Task Stop() => await State.Cancel();
        public async Task Pause() => await Task.CompletedTask;
    }
}