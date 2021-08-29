
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using transport_sim_app.Configuration;
using transport_sim_app.Data;
using transport_sim_app.Models.Transports;

namespace transport_sim_app.Models.Simulation
{
    public class Simulator : ISimulationScope, ISimulation
    {
        private readonly Random _rand = new Random();
        private readonly ILogger<Simulator> _logger;
        private readonly ITransportCollection _transports;
        private SimulationStatus _status;

        public event EventHandler<SimulationEventArgs> StartEvent;
        public event EventHandler<SimulationEventArgs> UpdateEvent;
        public event EventHandler<SimulationEventArgs> FinishEvent;
        public event EventHandler<SimulationEventArgs> StopEvent;
        public event EventHandler<SimulationEventArgs> ScopeUpdateEvent;

        public bool AllFinished => _transports.All(t => t.Finished);

        public float TrackDistance => Options.Distance;
        public SimulationStatus Status
        {
            get => _status;
            set
            {
                if (_status != value)
                    _logger.LogInformation($"Simulation status setted {value.ToString()}");
                _status = value;
            }
        }

        public SimulationEventArgs SimulationEventArgs { get; private set; }
        public bool IsSimulating => _status switch
        {
            SimulationStatus.Started or
            SimulationStatus.Running
            => true,
            _ => false
        };

        public SimulationOptions Options { get; protected set; }

        public Simulator(ILogger<Simulator> logger, IOptions<SimulationOptions> options, ITransportCollection transports)
        {
            _logger = logger;
            Options = options.Value;
            _transports = transports;
            if (Options.SimulationSeed != null)
                _rand = new Random(
                    int.TryParse(Options.SimulationSeed, out var seed) ?
                    seed : Options.SimulationSeed.GetHashCode()
                ); 
            else {
                var seed = _rand.Next();
                Options.SimulationSeed = seed.ToString();
                _rand = new Random(seed);
            }

            Task.Run(Start);
        }

        void SimulateRepairing(ITransport _transport)
        {
            bool repaired = (DateTime.Now - _transport.WheelPuncturedAt)?.TotalSeconds > _transport.RepairTimeSeconds;
            if (repaired) _transport.WheelPuncturedAt = null;
        }

        void SimulatePuncturing(ITransport _transport)
        {
            if (_rand.NextDouble() <= _transport.WheelPunctureProbability)
                _transport.WheelPuncturedAt = DateTime.Now;
        }

        void SimulateMoving(ITransport _transport)
        {
            _transport.DistanceTraveled += _transport.Speed;
        }
        public void Simulate(ITransport _transport)
        {
            if (_transport.Finished) return;
            if (_transport.DistanceTraveled >= TrackDistance)
            {
                _transport.FinishedAt = DateTime.Now;
                return;
            }
            if (!_transport.WheelPunctured)
            {
                SimulateMoving(_transport);
                SimulatePuncturing(_transport);
            }
            else
                SimulateRepairing(_transport);
        }

        public Task Start()
        {
            if (IsSimulating) return Task.CompletedTask;
            if (_transports.Count == 0) return Stop();

            var time = DateTime.Now;
            foreach (var t in _transports)
            {
                t.DistanceTraveled = .0f;
                t.StartedAt = time;
                t.FinishedAt = null;
            }
            SimulationEventArgs = new SimulationEventArgs
            {
                Message = "Simulation started",
                Status = Status.ToString(),
                TrackDistance = TrackDistance,
                Transports = _transports
            };
            Status = SimulationStatus.Started;
            OnSimulationEvent(StartEvent, SimulationEventArgs);
            return Task.CompletedTask;
        }

        public Task Pause()
        {
            Status = SimulationStatus.Paused;
            return Task.CompletedTask;
        }

        public Task Update()
        {
            if (!IsSimulating) return Task.CompletedTask;
            Status = SimulationStatus.Running;
            if (AllFinished)
            {
                Finish();
                return Task.CompletedTask;
            }
            // Обновление состояния каждого транспорта
            foreach (var t in _transports)
                Simulate(t);
            SimulationEventArgs = new SimulationEventArgs
            {
                Message = "Simulation is running",
                Status = Status.ToString(),
                TrackDistance = TrackDistance,
                Transports = _transports
            };
            OnSimulationEvent(UpdateEvent, SimulationEventArgs);
            return Task.CompletedTask;
        }

        public Task Finish()
        {
            if (Status == SimulationStatus.Finished) return Task.CompletedTask;
            Status = SimulationStatus.Finished;
            SimulationEventArgs = new SimulationEventArgs
            {
                Message = "Simulation finished",
                Status = Status.ToString(),
                TrackDistance = TrackDistance,
                Transports = _transports,
            };
            OnSimulationEvent(FinishEvent, SimulationEventArgs);
            return Task.CompletedTask;
        }

        public Task Stop()
        {
            if (!IsSimulating) return Task.CompletedTask;
            Status = SimulationStatus.Stopped;
            SimulationEventArgs = new SimulationEventArgs
            {
                Message = "Simulation stopped",
                Status = Status.ToString(),
                TrackDistance = TrackDistance,
                Transports = _transports
            };
            OnSimulationEvent(StopEvent, SimulationEventArgs);
            return Task.CompletedTask;
        }

        // Reserv for future
        public Task StopForce()
        {
            return Stop();
        }

        void OnSimulationEvent<TArgs>(EventHandler<TArgs> eventHandler, TArgs args)
        {
            if (eventHandler != null)
                eventHandler(this, args);
        }
    }
}