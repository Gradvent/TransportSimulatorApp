using System.Collections.Generic;

namespace transport_sim_app.Models
{
    public class SimulationEventArgs : SimulationEventArgs<Transport>
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