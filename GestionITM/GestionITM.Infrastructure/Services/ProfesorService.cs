using AutoMapper;
using GestionITM.Domain.Dtos;
using GestionITM.Domain.Entities;
using GestionITM.Domain.Interfaces;

namespace GestionITM.Infrastructure.Services
{
    public class ProfesorService : IProfesorService
    {
        private readonly IProfesorRepository _repository;
        private readonly IMapper _mapper;

        public ProfesorService(IProfesorRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProfesorDto>> ObtenerTodosAsync()
        {
            var profesores = await _repository.ObtenerTodosAsync();
            return _mapper.Map<IEnumerable<ProfesorDto>>(profesores);
        }

        public async Task<bool> RegistrarProfesorAsync(ProfesorCreateDto profesorCreateDto)
        {
            // Regla de negocio: la Especialidad no puede ser vacía
            if (string.IsNullOrWhiteSpace(profesorCreateDto.Especialidad))
            {
                return false;
            }

            // Regla de negocio adicional: si la especialidad es "Arquitectura",
            // se imprime un log en consola indicando perfil senior.
            if (profesorCreateDto.Especialidad.Equals("Arquitectura", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Perfil Senior Detectado");
            }

            // Reto de robustez: lanzar una excepción controlada cuando el nombre sea "Error"
            if (profesorCreateDto.Nombre.Equals("Error", StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("Error de prueba");
            }

            var profesor = _mapper.Map<Profesor>(profesorCreateDto);
            profesor.FechaContratacion = DateTime.UtcNow;

            await _repository.AgregarAsync(profesor);
            return true;
        }
    }
}
