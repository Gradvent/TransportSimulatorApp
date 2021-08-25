using System;
using System.Collections;
using System.Collections.Generic;

namespace transport_sim_app.Models
{
    public class Transport : ITransport, ITransportState
    {
        public float Speed { get; set; }
        public float WheelPunctureProbability { get; set; }
        public TimeSpan RepairTime { get; set; }

        public string Type { get; protected set; }  = nameof(Transport);

        public string Name { get; set; }
        public TimeSpan? StartedAt { get; set; }
        public bool Started => StartedAt != null;
        public TimeSpan? FinishedAt { get; set; }
        public bool Finished => FinishedAt != null;
        public float? DistanceTraveled { get; set; }
        public bool WheelPunctured => WheelPuncturedAt != null;
        public TimeSpan? WheelPuncturedAt { get; set; }

        public virtual IDictionary<string, object> getCharacteristics()
        {
            return new Dictionary<string, object>{
                {nameof(Speed), Speed},
                {nameof(WheelPunctureProbability), WheelPunctureProbability},
                {nameof(RepairTime), RepairTime}
            };
        }
    }
}