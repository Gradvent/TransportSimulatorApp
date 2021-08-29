using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using transport_sim_app.Configuration;
using transport_sim_app.Data;
using transport_sim_app.Models.Transports;

namespace transport_sim_app.Models.Factories
{
    public class RandomTransportFactory : TransportFactory
    {
        TransportFactoryOptions _options;
        Random _rand;
        MotorbikeFactory motorbikeWorkshop;
        TruckFactory truckWorkshop;
        AutomobileFactory automobileWorkshop;
        public RandomTransportFactory(IOptions<TransportOptions> transportOptions, TransportFactoryOptions options) : base(transportOptions)
        {
            _options = options;
            if (_options.Seed != null)
                if (int.TryParse(_options.Seed, out var seed))
                    _rand = new Random(seed);
                else
                    _rand = new Random(_options.Seed.GetHashCode());
            else
                _rand = new Random();
            motorbikeWorkshop = new MotorbikeFactory(transportOptions);
            truckWorkshop = new TruckFactory(transportOptions);
            automobileWorkshop = new AutomobileFactory(transportOptions);
        }

        public override ITransport Create()
        {
            var transport = _rand.Next(3) switch 
            {
                1 => motorbikeWorkshop.Create(),
                2 => automobileWorkshop.Create(),
                3 => truckWorkshop.Create(), 
                _ => base.Create() 
            };
            return transport;
        }

        public override void CreateAndAddRange(ITransportCollection transports, int count)
        {
            base.CreateAndAddRange(transports, count);
        }

        public void CreateAndAddRange(ITransportCollection transports, 
            IDictionary<string, int> counts)
        {
            foreach (var (type, count) in counts)
            {
                for (var i = 0; i < count; i++) {
                    var newTransport = type switch {
                        nameof(Truck) => truckWorkshop.Create(),
                        nameof(Automobile) => automobileWorkshop.Create(),
                        nameof(Motorbike) => motorbikeWorkshop.Create(),
                        _ => null
                    };
                    if (newTransport != null) transports.Add(newTransport);
                }
            }
            if (transports.Count < _options.RandTransportCount)
                CreateAndAddRange(
                    transports, 
                    _options.RandTransportCount - transports.Count);
        }
    }
}

