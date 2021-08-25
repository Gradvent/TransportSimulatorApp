using System.Collections.Generic;

namespace transport_sim_app.Models
{
    class Motorbike : Transport
    {
        public bool HasSidecar { get; set;}
        public override IDictionary<string, object> getCharacteristics()
        {
            var character = base.getCharacteristics();
            character.Add(nameof(HasSidecar), HasSidecar);
            return character;
        }
    }
}