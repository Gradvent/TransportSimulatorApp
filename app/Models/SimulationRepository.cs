
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace transport_sim_app.Models
{

    class SimulationRepository : ISimulationRepository
    {
        readonly IConfiguration _config;
        // static readonly IEnumerable<ITransport> transports = new List<ITransport>();
        static readonly IList<Transport> _transports = new List<Transport>();
        // static readonly float _trackDistance = 20000;
        static readonly Random _rnd = new Random();
        static readonly Simulator _simulator = new Simulator();

        public SimulationRepository(IConfiguration config)
        {
            _config = config;
            if (_transports.Count == 0)
            {
                var initRand = new Random((int)(Math.PI * 1000));
                _simulator.TrackDistance = 200;
                var transports = SimulationRepository._transports;
                Transport t = null;
                for (int i = 0; i < 10; i++)
                {
                    t = initRand.Next(100) switch
                    {
                        > 50 => new Motorbike
                        {
                            HasSidecar = initRand.NextDouble() < .2
                        },
                        > 20 => new Automobile
                        {
                            PersonCount = initRand.Next(1, 6)
                        },
                        _ => new Truck
                        {
                            CargoWeight = initRand.Next(10000)
                        }
                    };
                    t.Name = $"{t.Type} {initRand.Next(10000)}";
                    t.RepairTime = TimeSpan.FromSeconds(initRand.Next(30));
                    t.Speed = initRand.Next(100);
                    t.WheelPunctureProbability = (float)initRand.NextDouble();
                    transports.Add(t);
                }

            }
        }

        private bool disposedValue;

        public bool IsRunning { get; protected set; } = false;
        static SimulationStatus _status = SimulationStatus.Stopped;
        static bool _allFinished = false;

        // public event EventHandler StartEvent;
        public event EventHandler<SimulationEventArgs> UpdateEvent;
        public event EventHandler<SimulationEventArgs> StartEvent;
        public event EventHandler<SimulationEventArgs> StopEvent;

        public Task Pause()
        {
            _status = SimulationStatus.Paused;
            return Task.CompletedTask;
        }

        public Task Start()
        {
            IsRunning = true;
            _status = SimulationStatus.Started;

            var time = DateTime.Now.TimeOfDay;
            foreach (var t in _transports)
            {
                t.DistanceTraveled = .0f;
                t.StartedAt = time;
                t.FinishedAt = null;
            }

            if (StartEvent != null)
                return Task.Run(() => StartEvent(this, new SimulationEventArgs
                {
                    Message = "Simulation started"
                }));
            return Task.CompletedTask;
        }

        public Task Stop()
        {
            IsRunning = false;
            _status = SimulationStatus.Stopped;
            if (StopEvent != null)
                return Task.Run(() => StopEvent(this, new SimulationEventArgs
                {
                    Message = "Simulation stopped"
                }));
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
            _status = SimulationStatus.Running;

            _simulator.AllFinished = true; // Флаг финиша всех транспорта для окончания гонки
            var rnd = new Random();
            // Обновление состояния каждого транспорта
            foreach (var t in _transports) 
                _simulator.Simulate(t);

            if (UpdateEvent != null)
                return Task.Run(() => UpdateEvent(this, 
                    new SimulationEventArgs {
                        Message = "Simulation is running",
                        Status = _status.ToString(),
                        TrackDistance = _simulator.TrackDistance,
                        Transports = _transports
                }));

            if (_simulator.AllFinished) Stop();
            return Task.CompletedTask;
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
    }

    public interface ISimulationRepository : IDisposable
    {
        bool IsRunning { get; }
        Task Start();
        Task StopForce();
        Task Stop();
        Task Pause();
        Task Update();

        event EventHandler<SimulationEventArgs> UpdateEvent;
        event EventHandler<SimulationEventArgs> StartEvent;
        event EventHandler<SimulationEventArgs> StopEvent;
        // IEnumerable<ITransport> Transports { get; }
    }

    public enum SimulationStatus
    {
        Stopped, Started, Running, Paused
    }

    public class SimulationEventArgs
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public float TrackDistance { get; set; }

        public IEnumerable<Transport> Transports { get; set; }
    }
}