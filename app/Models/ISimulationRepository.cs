
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace transport_sim_app.Models
{
    public interface ISimulationRepository : IDisposable
    {
        bool IsRunning { get; }

        IEnumerable<Transport> Transports { get; }
        Task Start();
        Task StopForce();
        Task Stop();
        Task Pause();
        Task Update();
        Task Finish();

        event EventHandler<SimulationEventArgs> UpdateEvent;
        event EventHandler<SimulationEventArgs> ScopeUpdateEvent;
        event EventHandler<SimulationEventArgs> StartEvent;
        event EventHandler<SimulationEventArgs> StopEvent;
        event EventHandler<SimulationEventArgs> FinishEvent;

        void AddTransport(Transport transport);
        SimulationEventArgs SimulationArgs { get; }

        void UpdateTransport(Transport oldItem, Transport newItem);
        Transport DeleteTransport(string id);
        void SetDistance(int distance);
        // IEnumerable<ITransport> Transports { get; }
    }
}