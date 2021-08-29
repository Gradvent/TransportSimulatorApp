using System.Collections.Generic;
using transport_sim_app.Data;
namespace transport_sim_app.Configuration
{
    public class TransportFactoryOptions
    {
        public const string Factory = nameof(Factory);
        private const bool RAND_TRANSPORT_DEFAULT = true;
        private const int RAND_TRANSPORT_COUNT_DEFAULT = 5;

        public bool RandTransport { get; set; } = RAND_TRANSPORT_DEFAULT;
        public int RandTransportCount { get; set; } = RAND_TRANSPORT_COUNT_DEFAULT;
        public string Seed { get; set; }
        public IDictionary<string, int> Counts { get; set; } = 
            new Dictionary<string, int> {
                {nameof(Truck), 2},
                {nameof(Automobile), 5},
                {nameof(Motorbike), 3},
            };
    }
}