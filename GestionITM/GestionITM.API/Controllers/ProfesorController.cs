using GestionITM.Domain.Dtos;
using GestionITM.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionITM.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProfesorController : ControllerBase
    {
        private readonly IProfesorService _service;

        public ProfesorController(IProfesorService service)
        {
            _service = service;
        }

        // GET: api/profesor
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProfesorDto>>> Get()
        {
            var profesores = await _service.ObtenerTodosAsync();
            return Ok(profesores);
        }

        // POST: api/profesor
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProfesorCreateDto profesorCreateDto)
        {
            var resultado = await _service.RegistrarProfesorAsync(profesorCreateDto);

            if (!resultado)
            {
                return BadRequest("No se pudo registrar el profesor. Verifique que la especialidad no sea vacía.");
            }

            return Ok(new { message = "Profesor registrado con éxito en el sistema del ITM." });
        }
    }
}
