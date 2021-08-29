using System;

namespace transport_sim_app.Data
{
    public class Transport : ITransport
    {
        public string Id { get; set; }
        public float Speed { get; set; }
        public float WheelPunctureProbability { get; set; }
        public float RepairTimeSeconds { get; set; }
        public string Type { get; set; } = nameof(Transport);
        public string Name { get; set; }

        public DateTime? StartedAt { get; set; }
        public bool Started => StartedAt != null;
        public DateTime? FinishedAt { get; set; }
        public bool Finished => FinishedAt != null;
        public float? DistanceTraveled { get; set; }
        public bool WheelPunctured => WheelPuncturedAt != null;
        public DateTime? WheelPuncturedAt { get; set; }

    }
}
