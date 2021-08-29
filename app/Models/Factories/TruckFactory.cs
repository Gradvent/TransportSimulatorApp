using Microsoft.Extensions.Options;
using transport_sim_app.Configuration;
using transport_sim_app.Data;
using transport_sim_app.Models.Transports;

namespace transport_sim_app.Models.Factories
{
    internal class TruckFactory : TransportFactory
    {
        static int count;

        public TruckFactory(IOptions<TransportOptions> transportOptions) : base(transportOptions)
        {
        }

        public override ITransport Create()
        {
            var t = new Truck {
                CargoWeight = _transportOptions.TruckCargo,
                Name = $"Truck #{count++}"
            };
            Init(t);
            return t;
        }

        public override void CreateAndAddRange(ITransportCollection transports, int count)
        {
            base.CreateAndAddRange(transports, count);
        }
    }
}

