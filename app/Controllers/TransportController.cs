using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using transport_sim_app.Configuration;
using transport_sim_app.Data;
using transport_sim_app.Models.Factories;
using transport_sim_app.Models.Repository;

namespace transport_sim_app.Controllers
{
    [Route("[controller]")]
    public class TransportController : AppController//, ICrudController<string, object>
    {
        readonly ITransportRepository _repository;
        readonly ILogger<SimulationController> _logger;
        readonly IOptions<TransportOptions> _options;

        public TransportController(
            ITransportRepository repository,
            ILogger<SimulationController> logger,
            IOptions<TransportOptions> options)
        {
            _repository = repository;
            _logger = logger;
            _options = options;
        }

        [HttpPost("{type}")]
        public async Task<ActionResult> Create(string type) 
        {
            TransportFactory factory = type switch {
                nameof(Automobile) => new AutomobileFactory(_options),
                nameof(Truck) => new TruckFactory(_options),
                nameof(Motorbike) => new MotorbikeFactory(_options),
                _ => null
            };
            if (factory != null) 
            {
                var item = factory.Create();
                await _repository.Add(item);
                return CreatedAtAction(
                    nameof(Get),new {Id = item.Id}, (object)item);
            }
            return BadRequest();
        }
        
        [HttpPost]
        public async Task<ActionResult> Create() =>
            await Create(nameof(Automobile));

        [HttpDelete("{Id}")]
        public async Task<ActionResult> Delete(string Id)
        {
            var deleted = await _repository.Delete(Id);
            return Ok((object) deleted);
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult> Get(string Id) 
        {
            var item = await _repository.Get(Id);
            return Ok((object)item);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAll()
        {
            var items = await _repository.GetAll();
            return Ok(items.ToArray<object>());
        }

        [HttpPut("{Id}")]
        public async Task<ActionResult> Update(string Id, [FromBody] JsonElement item)
        {
            var type = item.GetProperty("type").GetString();
            var options = new JsonSerializerOptions {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var source = item.ToString();
            Transport newItem = type switch {
                nameof(Motorbike) => JsonSerializer.Deserialize<Motorbike>(source, options),
                nameof(Automobile) => JsonSerializer.Deserialize<Automobile>(source, options),
                nameof(Truck) => JsonSerializer.Deserialize<Truck>(source, options),
                _ => null
            };
            if (newItem == null) return BadRequest();
            var oldItem = await _repository.Get(Id);
            var updatedItem = await _repository.Update(Id, newItem);
            return CreatedAtAction(nameof(Get), new { Id = Id }, (object)updatedItem);
        }
    }
}
