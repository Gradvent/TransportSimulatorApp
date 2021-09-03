using System.Collections.Generic;
using transport_sim_app.Data;

namespace transport_sim_app.Models.Transports
{
    public interface ITransportCollection: ICollection<ITransport>, IEnumerable<ITransport>, IAsyncEnumerable<ITransport>
    {
        
    }
}