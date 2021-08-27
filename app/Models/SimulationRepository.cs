
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace transport_sim_app.Models
{
    class SimulationRepository : ISimulationRepository
    {
        readonly IConfiguration _config;
        // static readonly IEnumerable<ITransport> transports = new List<ITransport>();
        static readonly IList<Transport> _transports = new List<Transport>();
        // static readonly float _trackDistance = 20000;
        static readonly Random _rnd = new Random();
        static readonly Simulator _simulator = new Simulator() { AllFinished = false };

        public IEnumerable<Transport> Transports => _transports;

        readonly ILogger<SimulationRepository> _logger;
        private readonly TransportFactory _transportFactory;

        public SimulationRepository(IConfiguration config, ILogger<SimulationRepository> logger)
        {
            _config = config;
            if (_transports.Count == 0)
            {
                var initRand = new Random((int)(Math.PI * 1000));
                _simulator.TrackDistance = 
                    _config.GetValue<float>("SIMULATION_DISTANCE", 500);
                var randomTransport = 
                    _config.GetValue<bool>("SIMULATION_RAND_TRANSPORT", true);
                _transportFactory = new TransportFactory(_config);
                var transports = SimulationRepository._transports;
                if (randomTransport) {
                    var count = 
                        _config.GetValue<int>("SIMULATION_TRANSPORT_COUNT", 10);
                    _transportFactory.RandomPush(transports, count);
                } else _transportFactory.AddRangeTo(transports);
            }
            _logger = logger;
            Task.Run(Start);
        }

        private bool disposedValue;

        public bool IsRunning { get; protected set; } = false;
        public SimulationStatus Status { get => _status; set {
            if (_status != value)
                _logger.LogInformation($"Simulation status setted {value.ToString()}");
            _status = value;
        } }

        public SimulationEventArgs SimulationArgs { 
            get; 
            private set; 
        }

        static SimulationStatus _status = SimulationStatus.Stopped;

        // public event EventHandler StartEvent;
        public event EventHandler<SimulationEventArgs> UpdateEvent;
        public event EventHandler<SimulationEventArgs> StartEvent;
        public event EventHandler<SimulationEventArgs> StopEvent;
        public event EventHandler<SimulationEventArgs> ScopeUpdateEvent;
        public event EventHandler<SimulationEventArgs> FinishEvent;

        void OnSimulationEvent<TArgs>(EventHandler<TArgs> eventHandler, TArgs args)
        {
            if (eventHandler != null)
                eventHandler(this, args);
        }

        public Task Pause()
        {
            Status = SimulationStatus.Paused;
            return Task.CompletedTask;
        }

        public Task Start()
        {
            if (IsRunning) return Task.CompletedTask;
            IsRunning = true;
            Status = SimulationStatus.Started;
            if (_transports.Count == 0) return Stop();
            _simulator.AllFinished = false;

            var time = DateTime.Now;
            foreach (var t in _transports)
            {
                t.DistanceTraveled = .0f;
                t.StartedAt = time;
                t.FinishedAt = null;
            }
            SimulationArgs = new SimulationEventArgs
            {
                Message = "Simulation started",
                Status = Status.ToString(),
                TrackDistance = _simulator.TrackDistance,
                Transports = _transports
            };
            OnSimulationEvent(StartEvent, SimulationArgs);
            return Task.CompletedTask;
        }

        public Task Stop()
        {
            if (!IsRunning) return Task.CompletedTask;
            IsRunning = false;
            Status = SimulationStatus.Stopped;
            SimulationArgs = new SimulationEventArgs
            {
                Message = "Simulation stopped",
                Status = Status.ToString(),
                TrackDistance = _simulator.TrackDistance,
                Transports = _transports
            };
            OnSimulationEvent(StopEvent, SimulationArgs); 
            return Task.CompletedTask;
        }

        // Reserv for future
        public Task StopForce()
        {
            return Stop();
        }

        public Task Update()
        {
            if (!IsRunning) return Task.CompletedTask;
            Status = SimulationStatus.Running;
            if (_simulator.AllFinished) 
            {
                Finish();
                return Task.CompletedTask;
            }
            _simulator.AllFinished = true; // Флаг финиша всех транспорта для окончания гонки
            var rnd = new Random();
            // Обновление состояния каждого транспорта
            foreach (var t in _transports) 
                _simulator.Simulate(t);
            SimulationArgs = new SimulationEventArgs
            {
                Message = "Simulation is running",
                Status = Status.ToString(),
                TrackDistance = _simulator.TrackDistance,
                Transports = _transports
            };
            OnSimulationEvent(UpdateEvent, SimulationArgs);
            return Task.CompletedTask;
        }


        public Task Finish()
        {
            if (Status == SimulationStatus.Finished) return Task.CompletedTask;
            IsRunning = false;
            Status = SimulationStatus.Finished;
            SimulationArgs = new SimulationEventArgs {
                Message = "Simulation finished",
                Status = Status.ToString(),
                TrackDistance = _simulator.TrackDistance,
                Transports = _transports,
            };
            OnSimulationEvent(FinishEvent, SimulationArgs);
            return Task.CompletedTask;
        }

        void ScopeUpdate()
        {
            SimulationArgs = new SimulationEventArgs
            {
                Message = "Scope updated",
                Status = Status.ToString(),
                TrackDistance = _simulator.TrackDistance,
                Transports = _transports
            };
            OnSimulationEvent(ScopeUpdateEvent, SimulationArgs);
        }
        public void AddTransport(Transport transport)
        {
            _transports.Add(transport);
            ScopeUpdate();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~SimulationRepository()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public void UpdateTransport(Transport oldItem, Transport newItem)
        {
            _transports.Remove(oldItem);
            _transports.Add(newItem);
        }

        public Transport DeleteTransport(string id)
        {
            var item = _transports.First(t=>t.Id == id);
            _transports.Remove(item);
            return item;
        }

        public void SetDistance(int distance)
        {
            _simulator.TrackDistance = distance;
        }
    }
}