using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionITM.Domain.Dtos;

namespace GestionITM.Domain.Interfaces
{
        public interface IProfesorService
        {
            // Método para obtener la lista de profesores (usando el DTO de lectura)
            Task<IEnumerable<ProfesorDto>> GetAllProfesoresAsync();

            // Método para registrar un nuevo profesor (recibe el DTO de creación)
            Task RegisterProfesorAsync(ProfesorCreateDto profesorDto);
        }
    }

