using Microsoft.Extensions.Options;
using transport_sim_app.Configuration;
using transport_sim_app.Data;
using transport_sim_app.Models.Transports;

namespace transport_sim_app.Models.Factories
{
    public class MotorbikeFactory : TransportFactory
    {
        static int count;

        public MotorbikeFactory(IOptions<TransportOptions> transportOptions) : base(transportOptions)
        {
        }

        public override ITransport Create()
        {
            var t = new Motorbike {
                HasSidecar = _transportOptions.MotorbikeHasSidecar,
                Name = $"Motorbike #{count++}"
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

