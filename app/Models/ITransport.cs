using System;
using System.Collections.Generic;

namespace transport_sim_app.Models
{
    internal interface ITransport
    {
        string Id { get; }
        string Type { get; }
        string Name { get; }
        float Speed { get; }
        float WheelPunctureProbability { get; }
        float RepairTimeSeconds { get; }
        
        /// <summary>
        /// Get out characteristics of transport
        /// </summary>
        IDictionary<string, object> getCharacteristics();
    }
}