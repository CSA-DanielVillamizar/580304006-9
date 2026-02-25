using Microsoft.AspNetCore.Mvc;
using GestionITM.Domain.Interfaces;
using GestionITM.Domain.Entities;

namespace GestionITM.API.Controllers
{

    [Route("api/[controller]")] // La ruta sera: api/estudiante
    [ApiController]
    public class EstudianteController : ControllerBase
    { 
        private readonly IEstudianteRepository _repository;
        public EstudianteController(IEstudianteRepository repository)
        
        {
            _repository = repository;
        }

        // GET: api/estudiante
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Estudiante>>> GetEstudiantes()
        {
            var estudiantes = await _repository.ObtenerTodoAsync();
            return Ok(estudiantes); // Devuelve la lista de estudiantes con un código 200 OK
        }

        // GET: api/estudiante/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Estudiante>> GetEstudiante(int id)
        {
            var estudiante = await _repository.ObtenerPorIdAsync(id);
            if (estudiante == null)
            {
                return NotFound(new { message = $"Estudiante con ID {id} no encontrado." });
                               
            }
            return Ok(estudiante); // Devuelve el estudiante encontrado con un código 200 OK
        }

        // POST: api/estudiante
        [HttpPost]
        public async Task<ActionResult> PostEstudiante(Estudiante estudiante)
        {
            await _repository.AgregarAsync(estudiante);
            // Devuelve un código 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetEstudiante), new { id = estudiante.Id }, estudiante);
        }
    }
}

