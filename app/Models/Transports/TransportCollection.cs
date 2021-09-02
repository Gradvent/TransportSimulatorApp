using System.Collections.ObjectModel;
using transport_sim_app.Data;

namespace transport_sim_app.Models.Transports
{
    public class TransportCollection : KeyedCollection<string, ITransport>, ITransportCollection
    {
        protected override string GetKeyForItem(ITransport item)
        {
            return item.Id;
        }
    }
}