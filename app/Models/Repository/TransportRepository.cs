using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using transport_sim_app.Data;
using transport_sim_app.Models.Transports;

namespace transport_sim_app.Models.Repository
{
    public class TransportRepository : ITransportRepository
    {
        TransportCollection _transports;

        public TransportRepository(TransportCollection transports)
        {
            _transports = transports;
        }

        public Task<ITransport> Add(ITransport item) => Task.Run(()=>{
            _transports.Add(item);
            return item;
        });

        public Task<ITransport> Delete(string id) => Task.Run(()=>{
            var item = _transports[id];
            _transports.Remove(id);
            return item;
        });

        public Task<ITransport> Get(string id) => 
            Task.Run(()=>_transports[id]);

        public Task<IEnumerable<ITransport>> GetAll() =>
            Task.Run(()=>_transports.AsEnumerable());

        public Task<ITransport> Update(string id, ITransport item) => Task.Run(()=>{
            _transports.Remove(id);
            _transports.Add(item);
            return item;
        });
    }
}