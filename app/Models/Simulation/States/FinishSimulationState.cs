using System.Threading.Tasks;

namespace transport_sim_app.Models.Simulation.States
{
    public class FinishSimulationState : ISimulationState
    {
        private bool completed;

        public ISimulationContext Context { get; }

        public FinishSimulationState(ISimulationContext context)
        {
            Context = context;
            completed = false;
        }

        public Task Cancel() => Task.CompletedTask;

        public async Task Doing()
        {
            ISimulationState start = new StartSimulationState(Context);
            Context.ChangeState(start);
            await start.Init();
            await start.Doing();
        }

        public async Task Init()
        {
            Context.SimulationEventArgs = new SimulationEventArgs
            {
                Message = "Simulation finished",
                Status = SimulationStatus.Finished.ToString()
            };
            await Task.CompletedTask;
        }
    }
}