using GestionITM.Domain.Dtos;
using GestionITM.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace GestionITM.API.Controllers
{
    [Authorize] // Pide token para usar este controlador
    [Route("api/[controller]")]
    [ApiController]
    public class ProfesorController : ControllerBase
    {
        // Aquí usamos el servicio de profesor
        private readonly IProfesorService _service;

        // Constructor
        public ProfesorController(IProfesorService service)
        {
            _service = service;
        }

        // GET: api/profesor
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProfesorDto>>> Get()
        {
            var profesoresDto = await _service.ObtenerTodosLosProfesoresAsync();
            return Ok(profesoresDto);
        }

        // POST: api/profesor
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProfesorCreateDto profesorCreateDto)
        {
            var resultado = await _service.RegistrarProfesorAsync(profesorCreateDto);

            if (!resultado)
            {
                return BadRequest("No se pudo registrar. La especialidad no puede estar vacía.");
            }

            return Ok(new { message = "Profesor registrado con éxito en el sistema del ITM." });
        }
    }
}
