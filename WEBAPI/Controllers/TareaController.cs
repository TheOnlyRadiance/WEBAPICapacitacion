using Microsoft.AspNetCore.Mvc;
using WEBAPI.Data.Interfaces;
using WEBAPI.DTOs.Tarea;
using WEBAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TareaController : ControllerBase
    {
        ITareaService _service;

        public TareaController (ITareaService service) => _service = service;

        // GET: api/<TareaController>
        [HttpGet]
        public async Task<IActionResult> Findall([FromQuery] int userId)
        {
            IEnumerable<TareaModel> task = await _service.Findall(userId);
            if (task.Count() == 0) 
            {
                return NotFound();
            }

            return Ok(task);
        }

        // GET api/<TareaController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<TareaController>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTareaDto
            createTareaDto) {
            TareaModel? task = await _service.Create(createTareaDto);

            if (task == null) return NotFound();

            return Ok(task);
        }

        // PUT api/<TareaController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTareaDto updateTareaDto, int iduser)
        {
            TareaModel? Task = await _service.Update(updateTareaDto, iduser);
            return Ok(Task); 
        }

        // DELETE api/<TareaController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
