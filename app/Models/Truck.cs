using System.Collections.Generic;

namespace transport_sim_app.Models
{
    internal class Truck: Transport
    {
        public Truck()
        {
            Type = nameof(Truck);
        }

        public float CargoWeight { get; set; }
        public override IDictionary<string, object> getCharacteristics()
        {
            var character = base.getCharacteristics();
            character.Add(nameof(CargoWeight),CargoWeight);
            return character;
        }
    }
}