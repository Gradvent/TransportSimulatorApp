using System;
using System.Threading;
using System.Threading.Tasks;
using transport_sim_app.Models.Transports;

namespace transport_sim_app.Models.Simulation.States
{
    internal class StartSimulationState : ISimulationState
    {
        public ISimulationContext Context { get; }
        public ITransportCollection Transports => Context.Transports;

        readonly CancellationTokenSource cancellation;

        public StartSimulationState(ISimulationContext context)
        {
            Context = context;
            cancellation = new CancellationTokenSource();
        }

        public async Task Doing()
        {
            // if (IsSimulating) return Task.CompletedTask;
            if (Transports.Count == 0)
            {
                await Cancel();
                return;
            }

            var time = DateTime.Now;
            await Task.Run(async () =>
            {
                await foreach (var t in Transports)
                {
                    t.DistanceTraveled = .0f;
                    t.StartedAt = time;
                    t.FinishedAt = null;
                }
            }, cancellation.Token);

            if (Context.State != this) return;
            ISimulationState running = new RunningSimulationState(Context);
            Context.ChangeState(running);
            await running.Init();
            await Context.Start();
        }

        public async Task Cancel()
        {
            ISimulationState stop = new StopSimulationState(Context);
            Context.ChangeState(stop);
            cancellation.Cancel();
            await Task.CompletedTask;
        }

        public async Task Init() 
        {
            Context.SimulationEventArgs = new SimulationEventArgs
            {
                Message = "Simulation starting",
                Status = SimulationStatus.Started.ToString()
            };
            await Task.CompletedTask;
        }
    }
}