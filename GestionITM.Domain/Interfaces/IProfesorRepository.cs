using GestionITM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GestionITM.Domain.Interfaces
{
    public interface IProfesorRepository
    {
        Task<IEnumerable<Profesor>> ObtenerTodosLosProfesoresAsync();
        Task<Profesor?> ObtenerPorIdAsync(int id);
        Task AgregarAsync(Profesor profesor);
    }
}
