using transport_sim_app.Data;

namespace transport_sim_app.Models.Repository
{
    public interface ITransportRepository : ICrudRepository<string, ITransport> {}
}