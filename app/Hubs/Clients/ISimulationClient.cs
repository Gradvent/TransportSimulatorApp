
using System.Threading.Tasks;

namespace transport_sim_app.Hubs.Clients
{
    interface ISimulationClient
    {
        Task StartSimulationNotification(string simId);
        Task StopSimulationNotification(string simId);
        Task PauseSimulationNotification(string simId);
    }
}