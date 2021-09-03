using System.Threading.Tasks;

namespace transport_sim_app.Models.Simulation.States
{
    internal class StopSimulationState : ISimulationState
    {
        public ISimulationContext Context { get; }

        public StopSimulationState(ISimulationContext context)
        {
            Context = context;
        }

        public async Task Doing()
        {
            var start = new StartSimulationState(Context);
            Context.ChangeState(start);
            await start.Init();
            await Context.Start();
        }

        public async Task Cancel() => await Task.CompletedTask;

        public async Task Init()
        {
            Context.SimulationEventArgs = new SimulationEventArgs
            {
                Message = "Simulation stopped",
                Status = SimulationStatus.Stopped.ToString()
            };
            await Task.CompletedTask;
        }
    }
}