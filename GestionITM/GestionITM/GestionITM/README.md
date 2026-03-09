# GestionITM - API de Gestión Académica (ASP.NET Core / .NET 8)

Proyecto educativo que modela la gestión académica básica de un instituto (ITM): estudiantes, cursos, matrículas y productos de ejemplo.

El objetivo es que el estudiante aprenda a construir paso a paso una API REST moderna usando:

- ASP.NET Core Web API (.NET 8)
- Entity Framework Core (EF Core) con SQL Server
- Patrón Repositorio
- AutoMapper y DTOs
- Migraciones de base de datos
- Swagger / OpenAPI para probar la API

---

## 1. Estructura de la solución

Dentro de la carpeta `GestionITM/` hay una solución con tres proyectos principales:

- `GestionITM.API/`
  - Proyecto ASP.NET Core Web API (capa de presentación).
  - Contiene:
    - Controladores (por ejemplo, `EstudianteController`, `CursoController`) que dependen de servicios de dominio (`IEstudianteService`).
    - Middleware global de excepciones (`ExceptionMiddleware`) que unifica el formato de errores.
    - Configuración de Swagger, pipeline HTTP y enrutamiento.
    - Registro de dependencias (DbContext, repositorios, servicios, AutoMapper) en `Program.cs`.

- `GestionITM.Domain/`
  - Núcleo de dominio (modelo y contratos):
    - Entidades de dominio (modelo de datos):
      - `Estudiante`, `Curso`, `Matricula`, `Product`.
    - Interfaces de repositorio:
      - `IEstudianteRepository`, `ICursoRepository`.
    - Interfaces de servicio:
      - `IEstudianteService` (reglas de negocio para estudiantes).
    - DTOs y modelos compartidos:
      - `EstudianteDto`, `EstudianteCreateDto`.
      - `ErrorResponse` (formato estándar de respuesta de error para la API).

- `GestionITM.Infrastructure/`
  - Infraestructura de acceso a datos:
    - `ApplicationDbContext`: DbContext de EF Core.
    - Repositorios concretos que implementan las interfaces de `Domain` (por ejemplo, `EstudianteRepository`, `CursoRepository`).
    - Implementaciones de servicios que usan repositorios y AutoMapper (por ejemplo, `EstudianteService`).
    - Migraciones de EF Core (carpeta `Migrations`).

Arquitectura por capas clásica:

`API (Controllers + Middleware) -> Domain (Entidades + Interfaces + DTOs) -> Infrastructure (EF Core + Repositorios + Services)`

---

## 2. Requisitos previos

- .NET SDK 8.0 o superior.
- Visual Studio 2022/2026 (recomendado) o VS Code.
- SQL Server instalado y accesible (por ejemplo, `P-DVILLAMIZARA`).
- Git (para clonar el repositorio, si trabajas desde cero).

---

## 3. Clonar y abrir el proyecto

1. Clonar el repo (si aún no lo tienes):

   ```bash
   git clone https://github.com/CSA-DanielVillamizar/580304006-9.git
   cd 580304006-9/GestionITM
   ```

2. Abrir la solución `GestionITM.sln` en Visual Studio.

3. Establecer `GestionITM.API` como **Startup Project**.

---

## 4. Configuración de la base de datos (SQL Server)

La API usa SQL Server. La cadena de conexión se define en `GestionITM.API/appsettings.json` bajo la clave `DefaultConnection`.

Ejemplo (NO subas la contraseña real a GitHub):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=P-DVILLAMIZARA;Database=GestionITM;User Id=sa;Password=TU_CONTRASENA_AQUI;Encrypt=False;TrustServerCertificate=True;"
  }
}
```

Ajusta:

- `Server=` con el nombre de tu servidor SQL.
- `Database=` con el nombre que quieras usar (por defecto `GestionITM`).
- `User Id` y `Password` con tus credenciales.

En `Program.cs` se registra el DbContext con esta cadena de conexión:

```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
```

---

## 5. Migraciones de Entity Framework Core

EF Core se usa para generar y actualizar el esquema de base de datos a partir de las entidades.

### 5.1. Crear una nueva migración (ejemplo)

Comando general desde la carpeta `GestionITM`:

```powershell
dotnet ef migrations add NombreDeLaMigracion --project GestionITM.Infrastructure --startup-project GestionITM.API
```

En este proyecto ya se crearon, por ejemplo:

- `AddCursosAndEstudiantes`
- `CreateCursosTable`
- `AgregarTelefonoEstudiante`
- `AgregardatenowTelefonoEstudiante`

Estas migraciones crearon las tablas y agregaron, por ejemplo, el campo `Telefono` a la entidad `Estudiante`.

### 5.2. Aplicar migraciones a la base de datos

Para actualizar la base de datos con todas las migraciones pendientes:

```powershell
dotnet ef database update --project GestionITM.Infrastructure --startup-project GestionITM.API
```

Este comando:

- Usa `GestionITM.API` como proyecto de arranque (para leer configuración y DI).
- Aplica las migraciones definidas en `GestionITM.Infrastructure`.

---

## 6. Entidades principales

### 6.1. Estudiante

Archivo: `GestionITM.Domain/Entities/Estudiante.cs`

Representa a un estudiante.

Campos clave:

- `Id` – clave primaria.
- `Nombre` – requerido, máximo 100 caracteres.
- `Correo` – requerido, máximo 200, con validación de email.
- `FechaInscripcion` – fecha de inscripción.
- `Telefono` – máximo 20 caracteres (agregado mediante migración EF).

### 6.2. Curso

Archivo: `GestionITM.Domain/Entities/Curso.cs`

Representa un curso académico.

- `Id` – clave primaria.
- `Codigo` – requerido, máximo 50.
- `Nombre` – requerido, máximo 200.
- `Creditos` – entero (0-30).

### 6.3. Matricula

Archivo: `GestionITM.Domain/Entities/Matricula.cs`

Relaciona estudiantes con cursos.

- `Id`
- `EstudianteId`
- `CursoId`
- `Periodo` – string (ej. `"2024-1"`).
- `Estado` – string (ej. `"Activo"`).

---

## 7. Repositorios e inyección de dependencias

Interfaces de repositorio (`GestionITM.Domain/Interfaces`):

```csharp
public interface IEstudianteRepository
{
    Task<IEnumerable<Estudiante>> ObtenerTodoAsync();
    Task<Estudiante?> ObtenerPorIdAsync(int id);
    Task AgregarAsync(Estudiante estudiante);
}

public interface ICursoRepository
{
    Task<IEnumerable<Curso>> ObtenerTodoAsync();
    Task<Curso?> ObtenerPorIdAsync(int id);
    Task AgregarAsync(Curso curso);
}
```

Implementaciones en `GestionITM.Infrastructure/Repositories` (por ejemplo `EstudianteRepository` y `CursoRepository`) usan `ApplicationDbContext` para acceder a SQL Server.

En `GestionITM.API/Program.cs` se registran en el contenedor de DI:

```csharp
builder.Services.AddScoped<IEstudianteRepository, EstudianteRepository>();
builder.Services.AddScoped<ICursoRepository, CursoRepository>();
```

De esta manera los controladores reciben las interfaces por constructor.

---

## 8. DTOs y AutoMapper

### 8.1. DTOs de Estudiante

En `GestionITM.Domain/Dtos/`:

- `EstudianteDto`: DTO de salida (lo que la API devuelve).
- `EstudianteCreateDto`: DTO de entrada (lo que la API recibe en el POST).

Estos DTOs evitan exponer directamente la entidad de dominio y permiten controlar qué campos se entregan al cliente.

### 8.2. Configuración de AutoMapper

Archivo: `GestionITM.API/Mappings/MappingProfile.cs`

```csharp
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Estudiante, EstudianteDto>();
        CreateMap<EstudianteCreateDto, Estudiante>();
    }
}
```

Registro en `Program.cs`:

```csharp
builder.Services.AddAutoMapper(typeof(MappingProfile));
```

Ahora los controladores pueden inyectar `IMapper` y hacer conversiones automáticas entre Entidades y DTOs.

---

## 9. Controladores y endpoints

### 9.1. EstudianteController

Archivo: `GestionITM.API/Controllers/EstudianteController.cs`

Ruta base: `/api/estudiante`

Dependencias inyectadas:

- `IEstudianteRepository _repository`
- `IMapper _mapper`

#### GET /api/estudiante

Devuelve todos los estudiantes en forma de `EstudianteDto`.

Flujo simplificado:

1. `_repository.ObtenerTodoAsync()` obtiene la lista de `Estudiante` desde la BD.
2. `_mapper.Map<IEnumerable<EstudianteDto>>(estudiantes)` convierte la lista de entidades a lista de DTOs.
3. `return Ok(estudiantesDto);` devuelve `200 OK` con la lista.

#### GET /api/estudiante/{id}

Devuelve un estudiante específico.

1. `_repository.ObtenerPorIdAsync(id)` busca por ID.
2. Si es `null`, se devuelve `404 NotFound` con un mensaje amigable.
3. Si existe, se mapea con AutoMapper a `EstudianteDto` y se devuelve con `200 OK`.

#### POST /api/estudiante

Crea un nuevo estudiante a partir de un `EstudianteCreateDto`.

1. El body de la petición se deserializa a `EstudianteCreateDto`.
2. `_mapper.Map<Estudiante>(estudianteCreateDto)` crea la entidad.
3. `_repository.AgregarAsync(estudiante)` guarda en la base de datos.
4. Se mapea la entidad guardada a `EstudianteDto`.
5. Se devuelve `CreatedAtAction(...)` con código `201 Created` y el recurso creado.

### 9.2. CursoController

Archivo: `GestionITM.API/Controllers/CursoController.cs`

Ruta base: `/api/curso`

Endpoints básicos:

- `GET /api/curso` – devuelve todos los cursos.
- `GET /api/curso/{id}` – devuelve un curso por ID.
- `POST /api/curso` – crea un curso nuevo usando `ICursoRepository`.

Ejercicio sugerido: crear DTOs para curso y usar AutoMapper, igual que con `Estudiante`.

---

## 10. Swagger / OpenAPI

Swagger está habilitado en `GestionITM.API` para explorar y probar la API.

En `Program.cs`:

```csharp
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
```

### 10.1. Levantar la API y abrir Swagger

1. Asegúrate de tener la BD actualizada:

   ```powershell
   dotnet ef database update --project GestionITM.Infrastructure --startup-project GestionITM.API
   ```

2. Ejecuta la API (`GestionITM.API`) desde Visual Studio (F5 o Ctrl+F5).

3. Se abrirá una URL similar a:

   ```
   https://localhost:xxxx/swagger
   ```

4. Desde Swagger puedes probar todos los endpoints.

### 10.2. Ejemplos de requests

#### Crear estudiante (POST /api/estudiante)

Body de ejemplo:

```json
{
  "nombre": "Juan Pérez",
  "correo": "juan.perez@example.com"
}
```

Respuesta (201 Created, cuerpo simplificado):

```json
{
  "id": 1,
  "nombreCompleto": "Juan Pérez",
  "correo": "juan.perez@example.com"
}
```

#### Listar estudiantes (GET /api/estudiante)

Respuesta de ejemplo:

```json
[
  {
    "id": 1,
    "nombreCompleto": "Juan Pérez",
    "correo": "juan.perez@example.com"
  },
  {
    "id": 2,
    "nombreCompleto": "María López",
    "correo": "maria.lopez@example.com"
  }
]
```

#### Crear cursos de prueba (POST /api/curso)

Ejemplos de cuerpos JSON:

```json
{
  "codigo": "CS001",
  "nombre": "Programación de Software",
  "creditos": 4
}
```

```json
{
  "codigo": "CS002",
  "nombre": "Bases de Datos",
  "creditos": 3
}
```

```json
{
  "codigo": "CS003",
  "nombre": "Arquitectura Cloud",
  "creditos": 3
}
```

Luego, `GET /api/curso` debe mostrar estos tres cursos almacenados en SQL Server.

---

## 11. Terminología básica

- **Entidad**: clase que representa un concepto del dominio (Estudiante, Curso, etc.).
- **DTO**: objeto usado para transferencia de datos entre capas o hacia el cliente.
- **Repositorio**: capa que encapsula el acceso a datos (consultas e inserciones).
- **DbContext**: clase principal de EF Core que representa la conexión y el modelo de la base de datos.
- **Migración**: cambio versionado en el esquema de la BD.
- **AutoMapper**: librería para mapear automáticamente propiedades entre objetos.
- **Controller**: clase de ASP.NET Core que maneja peticiones HTTP.
- **Endpoint**: ruta HTTP específica (por ejemplo, `GET /api/estudiante`).

---

## 12. Siguientes pasos para el estudiante

- Añadir operaciones de actualización (PUT) y eliminación (DELETE) para estudiantes y cursos.
- Crear DTOs para `Curso` y actualizar `CursoController` para usarlos con AutoMapper.
- Modelar las relaciones completas entre `Estudiante`, `Curso` y `Matricula` (incluyendo claves foráneas y navegación).
- Implementar validaciones de negocio adicionales (por ejemplo, evitar correos duplicados).
- Escribir pruebas unitarias para repositorios y controladores.

Este proyecto está pensado como una base para practicar desarrollo backend moderno con .NET 8 y EF Core, siguiendo buenas prácticas y una arquitectura por capas clara.

---

## 13. Clase 10: Manejo Global de Excepciones con Middleware

**Fecha:** Miércoles, 11 de Marzo de 2026.

**Objetivo:** Implementar un componente centralizado que capture cualquier error en la aplicación y devuelva una respuesta JSON estandarizada y elegante al cliente.

### 13.1. Metáfora del "Seguro de Accidentes" del Software

> "Imaginen que nuestra API es un Edificio Inteligente.
>
> - Nivel 1: Si hay un incendio en un apartamento (un error en el código), el apartamento se quema y el humo se esparce por todo el edificio, asustando a los vecinos (el usuario ve el error técnico de SQL).
> - Nivel 5: Instalamos un Sistema de Aspersores y Alarmas Centralizado (Middleware).
>
> No importa en qué piso o habitación ocurra el incendio; el sistema lo detecta, lo contiene, y le avisa a todo el edificio de forma calmada: 'Hubo un inconveniente en el piso 5, estamos trabajando en ello'.
>
> El Middleware es ese guardia que está en la puerta y revisa todo lo que entra y todo lo que sale. Si algo sale 'roto', él lo atrapa, lo arregla y lo entrega bonito."

### 13.2. Definición técnica

- **Middleware:** componente de software que se ensambla en el *pipeline* (tubería) de la aplicación para manejar peticiones y respuestas.
- **Pipeline:** el camino que recorre una petición desde que el cliente hace clic hasta que llega al controlador, y de regreso.
- **Global Exception Handling:** técnica para capturar errores en un solo lugar, evitandoping `try-catch` repetitivos en todos los controladores.

### 13.3. Paso 1: Modelo de respuesta (`ErrorResponse`)

Archivo: `GestionITM.Domain/Models/ErrorResponse.cs`

```csharp
namespace GestionITM.Domain.Models
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Details { get; set; } // Solo para desarrollo
    }
}
```

### 13.4. Paso 2: Middleware de excepciones (`ExceptionMiddleware`)

Archivo: `GestionITM.API/Middleware/ExceptionMiddleware.cs`

Versión base vista en clase (antes de aplicar el reto):

```csharp
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
        _next = next; // El siguiente paso en la tubería
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context); // Intentar seguir el flujo normal
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message); // Guardar el error en el log
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = new ErrorResponse
        {
            StatusCode = context.Response.StatusCode,
            Message = "Ocurrió un error interno en el servidor del ITM.",
            Details = _env.IsDevelopment() ? ex.StackTrace?.ToString() : null
        };

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var json = JsonSerializer.Serialize(response, options);

        await context.Response.WriteAsync(json);
    }
}
```

> Nota: originalmente se enseñó en `Infrastructure`, pero en la versión final vive en `GestionITM.API` porque depende directamente de ASP.NET Core (`HttpContext`, `RequestDelegate`, etc.).

### 13.5. Paso 3: Registrar el Middleware en `Program.cs`

Archivo: `GestionITM.API/Program.cs`

```csharp
var app = builder.Build();

// --- ACTIVAR EL ESCUDO DE PROTECCIÓN ---
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
```

El orden es vital: el middleware de excepciones debe ir al inicio de la configuración del pipeline para poder atrapar cualquier error posterior.

### 13.6. Reto aplicado: personalizar el StatusCode

Trabajo independiente propuesto:

- Si el error es de tipo `KeyNotFoundException`, devolver un **404 Not Found** en lugar de un 500.
- Usar un `switch` para detectar diferentes tipos de excepciones.

Implementación final (resumen) del método `HandleExceptionAsync`:

```csharp
private async Task HandleExceptionAsync(HttpContext context, Exception ex)
{
    context.Response.ContentType = "application/json";

    // ANTES: siempre devolvíamos 500 para cualquier excepción
    //   context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

    // AHORA: determinamos el código según el tipo de excepción
    var statusCode = ex switch
    {
        KeyNotFoundException => (int)HttpStatusCode.NotFound,
        ArgumentException => (int)HttpStatusCode.BadRequest,
        _ => (int)HttpStatusCode.InternalServerError
    };
    context.Response.StatusCode = statusCode;

    var response = new ErrorResponse
    {
        StatusCode = statusCode,
        Message = statusCode switch
        {
            (int)HttpStatusCode.NotFound => "El recurso solicitado no fue encontrado en el sistema del ITM.",
            (int)HttpStatusCode.BadRequest => "La petición enviada no es válida. Verifique los datos.",
            _ => "Ocurrió un error interno en el servidor del ITM."
        },
        Details = _env.IsDevelopment() ? ex.StackTrace?.ToString() : null
    };

    var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
    var json = JsonSerializer.Serialize(response, options);

    await context.Response.WriteAsync(json);
}
```

> Error común (Nivel 1): poner `try-catch` en cada método de cada controlador. Con el middleware global, centralizamos el manejo de errores y mantenemos los controladores limpios y enfocados en la lógica de negocio.
