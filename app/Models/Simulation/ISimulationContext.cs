using System.Threading.Tasks;
using transport_sim_app.Configuration;
using transport_sim_app.Models.Transports;

namespace transport_sim_app.Models.Simulation
{
    public interface ISimulationContext
    {
        ISimulationState State { get; }
        ITransportCollection Transports { get; }
        SimulationOptions Options { get; }
        SimulationEventArgs SimulationEventArgs { get; set; }

        void ChangeState(ISimulationState state);
        Task Start() => State.Doing();
        Task Stop() => State.Cancel();
    }
}