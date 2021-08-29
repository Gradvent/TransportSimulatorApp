using System.Collections.Generic;
using transport_sim_app.Data;

namespace transport_sim_app.Models.Simulation
{
    public class SimulationEventArgs : SimulationEventArgs<ITransport>
    {
    }

    public class SimulationEventArgs<T>
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public float TrackDistance { get; set; }

        public IEnumerable<T> Transports { get; set; }
    }
}