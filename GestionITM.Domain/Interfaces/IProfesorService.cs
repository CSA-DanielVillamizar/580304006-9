using GestionITM.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GestionITM.Domain.Interfaces
{
    public interface IProfesorService
    {
        Task<IEnumerable<ProfesorDto>> ObtenerTodosLosProfesoresAsync();
        Task<bool> RegistrarProfesorAsync(ProfesorCreateDto profesorDto);
    }
}