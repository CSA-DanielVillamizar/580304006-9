using AutoMapper;
using GestionITM.Domain.Dtos;
using GestionITM.Domain.Entities;
using GestionITM.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<ProfesorDto>> ObtenerTodosLosProfesoresAsync()
        {
            var profesores = await _repository.ObtenerTodosLosProfesoresAsync();
            return _mapper.Map<IEnumerable<ProfesorDto>>(profesores);
        }

        public async Task<bool> RegistrarProfesorAsync(ProfesorCreateDto profesorDto)
        {
            // Reto de robustez: lanza excepción si el Nombre es "Error"
            if (profesorDto.Nombre.Equals("Error", StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("Error de prueba para el middleware");
            }

            // Regla de negocio: Especialidad no puede estar vacía
            if (string.IsNullOrWhiteSpace(profesorDto.Especialidad))
            {
                return false;
            }

            // Regla de negocio: log si especialidad es "Arquitectura"
            if (profesorDto.Especialidad.Trim().Equals("Arquitectura", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Perfil Senior Detectado");
            }

            var profesor = _mapper.Map<Profesor>(profesorDto);
            await _repository.AgregarAsync(profesor);
            return true;
        }
    }
}