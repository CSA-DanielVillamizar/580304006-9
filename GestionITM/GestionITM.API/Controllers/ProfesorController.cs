using Microsoft.AspNetCore.Mvc;
using GestionITM.Domain.Interfaces;
using GestionITM.Domain.Dtos;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GestionITM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
            var profesores = await _service.GetAllProfesoresAsync();
            return Ok(profesores);
        }

        // POST: api/profesor
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProfesorCreateDto profesorDto)
        {

            await _service.RegisterProfesorAsync(profesorDto);

            return Ok(new { message = "Profesor registrado exitosamente" });
        }
    }
}