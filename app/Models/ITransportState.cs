
using System;

namespace transport_sim_app.Models
{
    internal interface ITransportState
    {
        TimeSpan? StartedAt { get; set; }
        bool Started { get => StartedAt != null; }
        TimeSpan? FinishedAt { get; set; }
        bool Finished { get => FinishedAt != null; }
        float? DistanceTraveled { get; set; }
        TimeSpan? WheelPuncturedAt { get; set; }
        bool WheelPunctured { get => WheelPuncturedAt != null; }

    }
}