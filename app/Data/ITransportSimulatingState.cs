
using System;

namespace transport_sim_app.Data
{
    public interface ITransportSimulatingState
    {
        DateTime? StartedAt { get; set; }
        bool Started { get => StartedAt != null; }
        DateTime? FinishedAt { get; set; }
        bool Finished { get => FinishedAt != null; }
        float? DistanceTraveled { get; set; }
        DateTime? WheelPuncturedAt { get; set; }
        bool WheelPunctured { get => WheelPuncturedAt != null; }

    }
}