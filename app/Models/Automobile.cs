using System.Collections.Generic;

namespace transport_sim_app.Models
{
    internal class Automobile: Transport
    {
        public int PersonCount { get; set; }
        public override IDictionary<string, object> getCharacteristics()
        {
            var character = base.getCharacteristics();
            character.Add(nameof(PersonCount),PersonCount);
            return character;
        }
    }
}