
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace transport_sim_app.Models.Repository
{
    public interface ICrudRepository<TKey, TValuer> where TValuer : class
    {
        Task<IEnumerable<TValuer>> GetAll();
        Task<TValuer> Get(TKey id);
        Task<TValuer> Add(TValuer item);
        Task<TValuer> Update(TKey id, TValuer item);
        Task<TValuer> Delete(TKey id);
    }
}