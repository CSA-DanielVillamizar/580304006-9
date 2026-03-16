using GestionITM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionITM.Domain.Dtos;    
using System.Collections.Generic;   

namespace GestionITM.Domain.Interfaces
{
    public interface IProfesorRepository
    {
        Task<IEnumerable<Profesor>> GetAllAsync();
        Task AddAsync(Profesor profesor);
        Task<bool> EmailExistsAsync(string email); 
    }
    }

