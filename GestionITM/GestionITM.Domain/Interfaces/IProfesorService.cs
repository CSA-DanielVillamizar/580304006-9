using GestionITM.Domain.Dtos;

namespace GestionITM.Domain.Interfaces
{
    public interface IProfesorService
    {
        // Operaciones de negocio para el ciclo de vida de Profesor
        Task<IEnumerable<ProfesorDto>> ObtenerTodosAsync();
        Task<bool> RegistrarProfesorAsync(ProfesorCreateDto profesorCreateDto);
    }
}
