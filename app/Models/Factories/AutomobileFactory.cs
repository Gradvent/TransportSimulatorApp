using Microsoft.Extensions.Options;
using transport_sim_app.Configuration;
using transport_sim_app.Data;
using transport_sim_app.Models.Transports;

namespace transport_sim_app.Models.Factories
{
    internal class AutomobileFactory : TransportFactory
    {
        public AutomobileFactory(IOptions<TransportOptions> transportOptions) : base(transportOptions)
        {
        }

        public override ITransport Create()
        {
            var t = new Automobile {
                PersonCount = _transportOptions.AutomobilePersons
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

