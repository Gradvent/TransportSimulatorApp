
using System;
using transport_sim_app.Models.Simulation;
using transport_sim_app.Models.Transports;

namespace transport_sim_app.Models.Repository
{
    public interface ISimulationRepository
    {
        ISimulation Simulation { get; }
    }
}