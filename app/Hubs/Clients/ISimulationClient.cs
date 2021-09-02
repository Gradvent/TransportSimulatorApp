
using System.Collections.Generic;
using System.Threading.Tasks;
using transport_sim_app.Data;
using transport_sim_app.Models.Simulation;

namespace transport_sim_app.Hubs.Clients
{
    class ScopeUpdatedNotificationArgs : ISimulationScope
    {
        public IEnumerable<ITransport> Transports { get; set; }
        public bool AllFinished { get; set; }
        public float TrackDistance { get; set; }
    }
    interface ISimulationClient
    {
        Task StartSimulationNotification();
        Task StopSimulationNotification();
        Task PauseSimulationNotification();
        Task ScopeUpdatedNotification(ScopeUpdatedNotificationArgs scopeArgs);
        Task StateUpdatedNotification(SimulationEventArgs stateArgs);
    }
}