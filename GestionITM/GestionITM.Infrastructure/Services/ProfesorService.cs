using AutoMapper;
using GestionITM.Domain.Dtos;
using GestionITM.Domain.Entities;
using GestionITM.Domain.Interfaces;
using GestionITM.Domain.Models; // Para el PagedResult
using Microsoft.EntityFrameworkCore; // Crucial para TolistAsync y CountAsync


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

        // 1. OBTENER TODO (Sin paginación - para uso administrativo interno)
        public async Task<IEnumerable<ProfesorDto>> ObtenerTodosAsync()
        {
            var profesores = await _repository.ObtenerTodosAsync();
            return _mapper.Map<IEnumerable<ProfesorDto>>(profesores);
        }

        // 2. Obtener el paginado y filtrado (Nivel 5: IQueryable + Skip/Take en SQL Server)
        public async Task<PagedResult<ProfesorDto>> ObtenerPaginadosAsync(ProfesorFilterDto filtro)
        {
            // FASE A : Preparar la consulta (IQueryable) con los filtros aplicados.
            // Todavía no se ha ejecutado nada en SQL; solo construimos la expresión.
            var consulta = _repository.ConsultarTodo();

            // FASE B: Aplicar filtros dinámicos
            if (!string.IsNullOrEmpty(filtro.BusquedaNombre))
            {
                consulta = consulta.Where(p => p.Nombre.Contains(filtro.BusquedaNombre));
            }

            if (!string.IsNullOrEmpty(filtro.Especialidad))
            {
                consulta = consulta.Where(p => p.Especialidad == filtro.Especialidad);
            }

            // FASE C: Conteo total de registros para paginación (se traduce a COUNT(*) en SQL)
            var totalRegistros = await consulta.CountAsync();

            // FASE D: Aplicar paginación (Skip/Take) - sólo traemos lo que cabe en la página actual
            var items = await consulta
                .Skip((filtro.Pagina - 1) * filtro.RegistrosPorPagina)
                .Take(filtro.RegistrosPorPagina)
                .ToListAsync();

            // FASE E: Empaquetado final en un PagedResult
            return new PagedResult<ProfesorDto>
            {
                Items = _mapper.Map<List<ProfesorDto>>(items),
                TotalRegistros = totalRegistros,
                PaginaActual = filtro.Pagina,
                RegistrosPorPagina = filtro.RegistrosPorPagina,
                TotalPaginas = (int)Math.Ceiling(totalRegistros / (double)filtro.RegistrosPorPagina)
            };
        }

        // 3. Registrar profesor (reglas de negocio)
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
