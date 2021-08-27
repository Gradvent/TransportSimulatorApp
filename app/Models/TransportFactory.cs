using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Microsoft.Extensions.Configuration;
using transport_sim_app.Models;

namespace transport_sim_app.Models
{
    public class TransportFactory
    {
        static int _count = 1;
        IConfiguration _config;
        string[] _types = { nameof(Truck), nameof(Automobile), nameof(Motorbike) };
        private readonly Random _rand;

        public TransportFactory(IConfiguration config)
        {
            _config = config;
            var preSeed = _config.GetValue<string>("SIMULATION_SEED", "NAN");
            if (preSeed != "NAN")
                _rand = new Random();
            else
            {
                if (int.TryParse(preSeed, out int seed))
                    _rand = new Random(seed);
                else
                    _rand = new Random(preSeed.GetHashCode());
            }
        }

        public void RandomPush(IList<Transport> transports, int Count)
        {
            string type;
            for (int i = 0; i < Count; i++)
            {
                type = _types[_rand.Next(_types.Length)];
                transports.Add(CreateByType(type));
            }
        }
        public Transport CreateByType(string type)
        {
            var sb = new StringBuilder(type);
            for (int i = 0; i < type.Length; i++)
                if (type[i] == '-' && i + 1 < type.Length) sb[i + 1] = char.ToUpper(type[i + 1]);
            sb[0] = char.ToUpper(sb[0]);
            sb = sb.Replace("-", "");
            var pCType = sb.ToString();
            Transport transport;
            switch (pCType)
            {
                case nameof(Truck):
                    transport = new Truck
                    {
                        CargoWeight = _config.GetValue<int>("TRANSPORT_TRUCK_CARGO", 2000)
                    };
                    break;
                case nameof(Automobile):
                    transport = new Automobile
                    {

                        PersonCount = _config.GetValue<int>("TRANSPORT_AUTOMOBILE_PERSONS", 1)
                    };
                    break;
                case nameof(Motorbike):
                    transport = new Motorbike
                    {
                        HasSidecar = _config.GetValue<bool>("TRANSPORT_MOTORBIKE_HAS_SIDECAR", false)
                    };
                    break;
                default: throw new ArgumentException($"Invalide transport type: {type}", nameof(type));
            }
            transport.Id = Guid.NewGuid().ToString();
            transport.DistanceTraveled = 0;
            transport.Name = $"New {transport.Type.ToLower()} {_count++}";
            transport.Speed = _config.GetValue<float>("TRANSPORT_SPEED", 70);
            transport.RepairTimeSeconds = _config.GetValue<float>("TRANSPORT_REPAIR_SECONDS", 3);
            transport.WheelPunctureProbability = _config.GetValue<float>("TRANSPORT_PUNCTURE_PROBABILITY", 10.0f) / 100;
            return transport;
        }

        internal Transport CreateFromDictionary(Dictionary<string, object> item)
        {
            var typeString = (string)item["type"];
            Type type = typeString switch
            {
                nameof(Truck) => typeof(Truck),
                nameof(Motorbike) => typeof(Motorbike),
                _ => typeof(Automobile)
            };
            return (Transport) GetObject(item, type);
        }

        internal Transport CreateFromObject(object item)
        {
            try
            {
                switch (((ITransport)item).Type)
                {
                    case nameof(Truck): return (Truck)item;
                    case nameof(Automobile): return (Automobile)item;
                    case nameof(Motorbike): return (Motorbike)item;
                    default: return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal void AddRangeTo(IList<Transport> transports)
        {
            foreach (var type in _types)
            {
                var typeUpper = type.ToUpper();
                for (int i = 0; i < _config.GetValue<int>($"TRANSPORT_{typeUpper}_COUNT", 3); i++)
                {
                    transports.Add(CreateByType(type));
                }
            }
        }
        private object GetObject(Dictionary<string, object> item, Type type)
        {
            var obj = Activator.CreateInstance(type);
            foreach (var kv in item)
            {
                var prop = type.GetProperty(kv.Key);
                if (prop == null) continue;
                object value = kv.Value;
                if (value is Dictionary<string, object>)
                {
                    value = GetObject((Dictionary<string, object>)value, prop.PropertyType); // <= This line
                }            
                prop.SetValue(obj, value, null);        
            }
            return obj;
        }
    }
}

