using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using transport_sim_app.Configuration;
using transport_sim_app.Data;
using transport_sim_app.Models.Transports;

namespace transport_sim_app.Models.Factories
{

    public class TransportFactory
    {
        protected TransportOptions _transportOptions;

        public TransportFactory(IOptions<TransportOptions> transportOptions)
        {
            _transportOptions = transportOptions.Value;
        }

        public virtual void CreateAndAddRange(ITransportCollection transports, int count)
        {
            for (int i = 0; i < count; i++)
            {
                transports.Add(Create());
            }
        }
        protected virtual void Init(ITransport transport)
        {
            var options = _transportOptions;
            transport.Id = Guid.NewGuid().ToString();
            transport.DistanceTraveled = 0;
            transport.Speed = options.Speed;
            transport.RepairTimeSeconds = options.RepairSeconds;
            transport.WheelPunctureProbability = options.PunctureProbability / 100.0f;
        }
        public virtual ITransport Create()
        {
            var transport = new Transport();
            Init(transport);
            return transport;
        }
    }
}

