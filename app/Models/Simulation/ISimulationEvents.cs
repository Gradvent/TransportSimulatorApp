using System;

namespace transport_sim_app.Models.Simulation
{
    public interface ISimulationEvents
    {
        SimulationEventArgs SimulationEventArgs { get; }

        event EventHandler<SimulationEventArgs> UpdateEvent;
        event EventHandler<SimulationEventArgs> ScopeUpdateEvent;
        event EventHandler<SimulationEventArgs> StartEvent;
        event EventHandler<SimulationEventArgs> StopEvent;
        event EventHandler<SimulationEventArgs> FinishEvent;
    }
}