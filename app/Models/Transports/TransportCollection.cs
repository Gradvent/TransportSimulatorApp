using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using transport_sim_app.Data;

namespace transport_sim_app.Models.Transports
{
    public class TransportCollection : KeyedCollection<string, ITransport>, ITransportCollection
    {
        public async IAsyncEnumerator<ITransport> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            foreach (var item in this)
            {
                await Task.CompletedTask;
                yield return item;
            }
        }

        protected override string GetKeyForItem(ITransport item)
        {
            return item.Id;
        }
    }
}