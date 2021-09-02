using System.Collections.Generic;
using transport_sim_app.Models.Transports;

namespace transport_sim_app.Configuration
{
    public class TransportOptions
    {
        public const string Transport = nameof(Transport);
        public const float TRUCK_CARGO_DEFAULT = 2000;
        public const int AUTOMOBILE_PERSONS_DEFAULT = 1;
        public const bool MOTORBIKE_SIDECAR_DEFAULT = false;
        public const float SPEED_DEFAULT = 70.0f;
        public const float REPAIR_SECONDS_DEFAULT = 2.0f;
        public const float PUNCTURE_PROBABILITY_DEFAULT = 20.0f;

        public float TruckCargo { get; set; } = TRUCK_CARGO_DEFAULT;
        public int AutomobilePersons { get; set; } = AUTOMOBILE_PERSONS_DEFAULT;
        public bool MotorbikeHasSidecar { get; set; } = MOTORBIKE_SIDECAR_DEFAULT;
        public float Speed { get; set; } = SPEED_DEFAULT;
        public float RepairSeconds { get; set; } = REPAIR_SECONDS_DEFAULT;
        public float PunctureProbability { get; set; } = PUNCTURE_PROBABILITY_DEFAULT;
    }
}