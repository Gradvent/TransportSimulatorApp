
using System;
using System.Threading.Tasks;
using transport_sim_app.Configuration;

namespace transport_sim_app.Models.Simulation
{
    public interface ISimulation : ISimulationEvents
    {
        SimulationOptions Options { get; }
        SimulationStatus Status { get; }
        bool IsSimulating { get; }
        Task Start();
        Task Stop();
        Task StopForce();
        Task Pause();
        Task Update();
        Task Finish();
    }
}