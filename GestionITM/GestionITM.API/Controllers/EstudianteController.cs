using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using GestionITM.Domain.Interfaces;
using GestionITM.Domain.Entities;
using GestionITM.Domain.Dtos;

namespace GestionITM.API.Controllers
{
    [Route("api/[controller]")] // La ruta sera: api/estudiante
    [ApiController]
    public class EstudianteController : ControllerBase
    {
        private readonly IEstudianteRepository _repository;
        private readonly IMapper _mapper;

        public EstudianteController(IEstudianteRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET: api/estudiante
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstudianteDto>>> Get()
        {
            var estudiantes = await _repository.ObtenerTodoAsync();
            // Transformación automática de Lista de Entidades a Lista de DTOs
            var estudiantesDto = _mapper.Map<IEnumerable<EstudianteDto>>(estudiantes);
            return Ok(estudiantesDto);
        }

        // GET: api/estudiante/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<EstudianteDto>> Get(int id)
        {
            var estudiante = await _repository.ObtenerPorIdAsync(id);
            if (estudiante == null)
            {
                return NotFound(new { message = $"Estudiante con ID {id} no encontrado." });
            }

            var estudianteDto = _mapper.Map<EstudianteDto>(estudiante);
            return Ok(estudianteDto);
        }

        // POST: api/estudiante
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] EstudianteCreateDto estudianteCreateDto)
        {
            var estudiante = _mapper.Map<Estudiante>(estudianteCreateDto);
            await _repository.AgregarAsync(estudiante);

            var estudianteDto = _mapper.Map<EstudianteDto>(estudiante);

            return CreatedAtAction(
                nameof(Get),
                new { id = estudiante.Id },
                estudianteDto);
        }
    }
}