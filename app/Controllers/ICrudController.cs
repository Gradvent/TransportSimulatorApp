
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace transport_sim_app.Controllers
{
    internal interface ICrudController<TKey, TValue>
    {
        Task<ActionResult<IEnumerable<TValue>>> GetAll();
        Task<ActionResult<TValue>> Get(TKey Id);
        Task<ActionResult<TValue>> Create();
        Task<ActionResult<TValue>> Update(TKey Id, [FromBody] Dictionary<string, object> item);
        Task<ActionResult<TValue>> Delete(TKey Id);
    }
}