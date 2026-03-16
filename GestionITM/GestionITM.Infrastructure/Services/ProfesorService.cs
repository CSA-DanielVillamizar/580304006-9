using GestionITM.Domain.Dtos;
using GestionITM.Domain.Entities;
using System;
using AutoMapper;
using GestionITM.Domain.Interfaces;
using System;
using System.Collections.Generic;
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

        public async Task<IEnumerable<ProfesorDto>> GetAllProfesoresAsync()
        {
            var profesores = await _repository.GetAllAsync();
            // Mapeo automático de Entidad a DTO
            return _mapper.Map<IEnumerable<ProfesorDto>>(profesores);
        }

        public async Task RegisterProfesorAsync(ProfesorCreateDto dto)
        {
            //Valida que la especiadlidad no esté vacía
            if (string.IsNullOrWhiteSpace(dto.Especialidad))
            {
                throw new Exception("La especialidad es obligatoria para registrar un profesor.");
            }

            if (dto.Especialidad.Equals("Arquitectura", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("----------------------------------");
                Console.WriteLine("Perfil Senior Detectado");
                Console.WriteLine("----------------------------------");
            }

            //RETO: Provocar error a propósito para el Middleware
            if (dto.Nombre.Equals("Error", StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("Error de prueba");
            }

            // Validar Email único
            var emailExiste = await _repository.EmailExistsAsync(dto.Email);
            if (emailExiste)
            {
                throw new Exception($"El email {dto.Email} ya está registrado en el sistema.");
            }

            //MAPEO Y PERSISTENCIA
            var entidad = _mapper.Map<Profesor>(dto);
            await _repository.AddAsync(entidad);
        }
    }
}
