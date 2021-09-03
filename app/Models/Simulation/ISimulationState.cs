using System.Threading.Tasks;

namespace transport_sim_app.Models.Simulation
{
    public interface ISimulationState 
    {
        ISimulationContext Context { get; }
        Task Init();
        Task Doing();
        Task Cancel();
    }
}