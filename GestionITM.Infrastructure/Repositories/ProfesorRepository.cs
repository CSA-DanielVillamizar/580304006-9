using GestionITM.Domain.Entities;
using GestionITM.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionITM.Infrastructure.Repositories
{
    public class ProfesorRepository : IProfesorRepository
    {

        // Guardamos el contexto de la base de datos para poder acceder a la tabla Profesores
        private readonly ApplicationDbContext _context;

        // Constructor: aquí inyectamos el ApplicationDbContext
        public ProfesorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Profesor>> ObtenerTodosLosProfesoresAsync()
        {
            // Trae todos los profesores de la base de datos en forma de lista
            return await _context.Profesores.ToListAsync();
        }

        public async Task AgregarAsync(Profesor profesor)
        {
            // Agrega el nuevo profesor al DbSet Profesores
            await _context.Profesores.AddAsync(profesor);

            // Guarda los cambios en la base de datos
            await _context.SaveChangesAsync();
        }
    }
}
