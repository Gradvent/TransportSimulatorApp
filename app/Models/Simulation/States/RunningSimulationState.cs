using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using transport_sim_app.Data;
using transport_sim_app.Models.Transports;

namespace transport_sim_app.Models.Simulation.States
{
    public class RunningSimulationState : ISimulationState
    {
        public ISimulationContext Context { get; }
        float TrackDistance => Context.Options.Distance;
        ITransportCollection Transports => Context.Transports;
        bool AllFinished => Transports.All(t => t.Finished);
        readonly CancellationTokenSource _cancellation;
        readonly Random _rand = new Random();

        public RunningSimulationState(ISimulationContext context)
        {
            Context = context;
            _cancellation = new CancellationTokenSource();
            if (Context.Options.SimulationSeed != null)
                _rand = new Random(
                    int.TryParse(Context.Options.SimulationSeed, out var seed) ?
                    seed : Context.Options.SimulationSeed.GetHashCode()
                );
            else
            {
                var seed = _rand.Next();
                Context.Options.SimulationSeed = seed.ToString();
                _rand = new Random(seed);
            }
        }

        public async Task Doing()
        {
            while (!(AllFinished || _cancellation.IsCancellationRequested))
            {
                await foreach (var t in Transports)
                    Simulate(t);
                await Task.Delay(1000);
            }
            if (_cancellation.IsCancellationRequested)
            {
                ISimulationState stop = new StopSimulationState(Context);
                Context.ChangeState(stop);
                await stop.Init();
                return;
            }
            ISimulationState finish = new FinishSimulationState(Context);
            Context.ChangeState(finish);
            await finish.Init();
        }

        public async Task Cancel()
        {
            _cancellation.Cancel();
            await Task.CompletedTask;
        }

        public async Task Init()
        {
            Context.SimulationEventArgs = new SimulationEventArgs
            {
                Message = "Simulation running",
                Status = SimulationStatus.Running.ToString()
            };
            await Task.CompletedTask;
        }
        void Simulate(ITransport _transport)
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

        void SimulateMoving(ITransport _transport)
        {
            _transport.DistanceTraveled += _transport.Speed;
        }

        void SimulatePuncturing(ITransport _transport)
        {
            if (_rand.NextDouble() <= _transport.WheelPunctureProbability)
                _transport.WheelPuncturedAt = DateTime.Now;
        }

        void SimulateRepairing(ITransport _transport)
        {
            bool repaired = (DateTime.Now - _transport.WheelPuncturedAt)?.TotalSeconds > _transport.RepairTimeSeconds;
            if (repaired) _transport.WheelPuncturedAt = null;
        }
    }
}