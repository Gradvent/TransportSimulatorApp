using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using transport_sim_app.Models;

namespace transport_sim_app.Controllers
{
    [Route("[controller]")]
    public class TransportController : AppController//, ICrudController<string, object>
    {
        readonly ISimulationRepository _repository;
        readonly ILogger<SimulationController> _logger;
        readonly IConfiguration _config;
        private readonly TransportFactory _transportFactory;

        public TransportController(
            ISimulationRepository repository,
            ILogger<SimulationController> logger, IConfiguration config)
        {
            _repository = repository;
            _logger = logger;
            _config = config;
            _transportFactory = new TransportFactory(_config);
        }

        [HttpPost("{type}")]
        public async Task<ActionResult<object>> Create(string type)
        {
            var Id = await Task.Run(() =>
            {
                Transport transport = _transportFactory.CreateByType(type);
                _repository.AddTransport(transport);
                return transport.Id;             
            });
            return await Get(Id);
        }
        
        [HttpPost]
        public async Task<ActionResult<object>> Create()
        {
            return await Create(nameof(Automobile));
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult<object>> Delete(string Id)
        {
            return await Task.Run(() => Ok((object)_repository.DeleteTransport(Id)));
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<object>> Get(string Id)
        {
            return await Task.Run(() => Ok((object)_repository.Transports.First(t => t.Id == Id)));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAll()
        {
            return await Task.Run(() => Ok(_repository.Transports.ToArray<object>()));
        }

        [HttpPut("{Id}")]
        public ActionResult<object> Update(string Id, [FromBody] JsonElement item)
        {
            var type = item.GetProperty("type").GetString();
            Utf8JsonReader utf8JsonReader = new Utf8JsonReader(Encoding.UTF8.GetBytes(item.ToString()));
            Transport newItem = type switch {
                nameof(Motorbike) => JsonSerializer.Deserialize<Motorbike>(ref utf8JsonReader),
                nameof(Automobile) => JsonSerializer.Deserialize<Automobile>(ref utf8JsonReader),
                nameof(Truck) => JsonSerializer.Deserialize<Truck>(ref utf8JsonReader),
                _ => null
            };
            if (newItem == null) return BadRequest();
            var oldItem = _repository.Transports.First(t => t.Id == Id);
            _repository.UpdateTransport(oldItem, newItem);
            return CreatedAtAction(nameof(Get), new { Id = Id }, item);
        }
    }
}
