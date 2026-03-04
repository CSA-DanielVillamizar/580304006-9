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

## Resumen de clases (para estudiantes)

### Clase 4 – ORM y Persistencia con Entity Framework Core

- Problema: desajuste de impedancia entre Objetos (C#) y Tablas (SQL).
- EF Core como ORM: traduce clases C# a tablas SQL y viceversa.
- Enfoque Code First: primero definimos entidades (`Estudiante`), luego usamos migraciones para generar la base de datos.
- Pasos clave:
  - Agregar paquetes `Microsoft.EntityFrameworkCore.SqlServer` y `Microsoft.EntityFrameworkCore.Design` en `GestionITM.Infrastructure`.
  - Definir la entidad `Estudiante` en `GestionITM.Domain.Entities` con data annotations (`[Required]`, `[MaxLength]`).
  - Crear `ApplicationDbContext` en `GestionITM.Infrastructure` con `DbSet<Estudiante> Estudiantes`.
  - Ejecutar comandos:
    ```powershell
    dotnet ef migrations add InitialCreate --project GestionITM.Infrastructure --startup-project GestionITM.API
    dotnet ef database update --project GestionITM.Infrastructure --startup-project GestionITM.API
    ```

### Clase 5 – Patrón Repositorio e Inyección de Dependencias

- Patrón Repositorio: define un contrato (`IEstudianteRepository`, `ICursoRepository`) que abstrae el acceso a datos.
- Inyección de dependencias: las clases piden sus dependencias (interfaces) por constructor.
- Implementamos:
  - `IEstudianteRepository` en `GestionITM.Domain.Interfaces`.
  - `EstudianteRepository` en `GestionITM.Infrastructure.Repositories` usando `ApplicationDbContext`.
- Registro en `GestionITM.API.Program.cs`:
  ```csharp
  builder.Services.AddDbContext<ApplicationDbContext>(options =>
      options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

  builder.Services.AddScoped<IEstudianteRepository, EstudianteRepository>();
  builder.Services.AddScoped<ICursoRepository, CursoRepository>();
  ```
- Todo el acceso a datos pasa por las interfaces, nunca por `new ApplicationDbContext()` directo.

### Clase 6 – Web APIs, Controladores y Swagger

- API como “mesero”: recibe peticiones HTTP, llama a repositorios y devuelve JSON.
- Creamos `EstudianteController` y `CursoController` en `GestionITM.API.Controllers`.
- Endpoints base:
  - `GET /api/estudiante` y `GET /api/estudiante/{id}`.
  - `POST /api/estudiante`.
  - `GET /api/curso` y `POST /api/curso`.
- Probamos la API con Swagger (`/swagger`), sin necesidad de frontend.
- Controladores inicialmente devolvían entidades (`Estudiante`, `Curso`) directamente.

### Clase 7 – DTOs y AutoMapper

- Problema: no exponer directamente las entidades de base de datos al exterior.
- Creamos DTOs en `GestionITM.Domain.Dtos`:
  - `EstudianteDto` (salida).
  - `EstudianteCreateDto` (entrada para crear).
- Configuramos AutoMapper en `GestionITM.API.Mappings.MappingProfile`:
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
- Registro en `Program.cs`:
  ```csharp
  builder.Services.AddAutoMapper(typeof(MappingProfile));
  ```
- Actualizamos `EstudianteController` para usar `IMapper` y devolver `IEnumerable<EstudianteDto>` en lugar de entidades.

### Clase 8 – Migraciones avanzadas y versionamiento de datos

- Migraciones = historial de cambios de la base de datos (como commits de Git).
- Agregamos propiedades nuevas a `Estudiante` (por ejemplo `Telefono`) y creamos migraciones:
  ```powershell
  dotnet ef migrations add AgregarTelefonoEstudiante --project GestionITM.Infrastructure --startup-project GestionITM.API
  dotnet ef database update --project GestionITM.Infrastructure --startup-project GestionITM.API
  ```
- Entendimos:
  - Método `Up` agrega columnas/tablas.
  - Método `Down` revierte los cambios.
  - Tabla `__EFMigrationsHistory` registra qué migraciones están aplicadas.
- Vimos comandos para rollback (`dotnet ef database update NombreMigracionAnterior`) y para quitar última migración (`dotnet ef migrations remove`).

### Clase 9 – Capa de Servicios y Lógica de Negocio

- Introducimos la Service Layer para centralizar reglas de negocio.
- Definimos `IEstudianteService` en `GestionITM.Domain.Interfaces`.
- Implementamos `EstudianteService` en `GestionITM.Infrastructure.Services`:
  - Usa `IEstudianteRepository` + `IMapper`.
  - Aplica reglas de negocio, por ejemplo: solo correos `@correo.itm.edu.co` son válidos.
  - Asigna `FechaInscripcion = DateTime.UtcNow` al registrar.
- Registro del servicio en `Program.cs`:
  ```csharp
  builder.Services.AddScoped<IEstudianteService, EstudianteService>();
  ```
- Actualizamos `EstudianteController` para depender de `IEstudianteService` en lugar del repositorio y del mapper:
  - El controlador solo orquesta; el servicio se encarga de validaciones, mapeos y llamadas al repositorio.

---

## 1. Estructura de la solución

Dentro de la carpeta `GestionITM/` hay una solución con tres proyectos principales:

- `GestionITM.API/`
  - Proyecto ASP.NET Core Web API.
  - Expone endpoints REST como:
    - `/api/estudiante`
    - `/api/curso`
  - Configura Swagger, EF Core, AutoMapper e inyección de dependencias.

- `GestionITM.Domain/`
  - Entidades de dominio (modelo de datos):
    - `Estudiante`
    - `Curso`
    - `Matricula`
    - `Product`
  - Interfaces de repositorio:
    - `IEstudianteRepository`
    - `ICursoRepository`
  - DTOs:
    - `EstudianteDto`
    - `EstudianteCreateDto`

- `GestionITM.Infrastructure/`
  - `ApplicationDbContext`: DbContext de EF Core.
  - Repositorios concretos que implementan las interfaces de `Domain`.
  - Migraciones de EF Core (carpeta `Migrations`).

Arquitectura por capas clásica:

`API (Controllers) -> Domain (Entidades + Interfaces) -> Infrastructure (EF Core + Repositorios)`

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

Dependencias inyectadas (versión final Nivel 5):

- `IEstudianteService _service`

El controlador ya no habla con el repositorio ni con AutoMapper directamente; todo eso vive en el servicio.

### 9.2. CursoController

Archivo: `GestionITM.API/Controllers/CursoController.cs`

Ruta base: `/api/curso`

Endpoints básicos:

- `GET /api/curso` – devuelve todos los cursos.
- `GET /api/curso/{id}` – devuelve un curso por ID.
- `POST /api/curso` – crea un curso nuevo usando `ICursoRepository` (pendiente de actualizar a DTOs + servicios, como ejercicio).

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

## 13. Cómo funciona una migración de EF Core (paso a paso)

### 13.1. ¿Qué es una migración?

Una **migración** es un archivo de código C# generado por EF Core que describe un cambio en el esquema de la base de datos:

- Se guarda en la carpeta `GestionITM.Infrastructure/Migrations`.
- Tiene dos métodos principales:
  - `Up`: aplica el cambio (por ejemplo, crear una tabla o agregar una columna).
  - `Down`: revierte el cambio (por ejemplo, borrar la tabla o quitar la columna).

EF Core mantiene un **historial** de qué migraciones ya se aplicaron en la base de datos (tabla interna `__EFMigrationsHistory`).

### 13.2. Ejemplo real: migración `AgregarTelefonoEstudiante`

Cuando agregamos la propiedad `Telefono` a la entidad `Estudiante`:

1. Cambiamos la clase `Estudiante`:

```csharp
public class Estudiante
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public DateTime FechaInscripcion { get; set; }

    // Nueva propiedad
    public string Telefono { get; set; } = string.Empty;
}
```

2. Creamos la migración:

```powershell
dotnet ef migrations add AgregarTelefonoEstudiante --project GestionITM.Infrastructure --startup-project GestionITM.API
```

3. EF Core generó un archivo `20260302120518_AgregarTelefonoEstudiante.cs` similar a:

```csharp
protected override void Up(MigrationBuilder migrationBuilder)
{
    migrationBuilder.AddColumn<string>(
        name: "Telefono",
        table: "Estudiantes",
        type: "nvarchar(20)",
        maxLength: 20,
        nullable: false,
        defaultValue: "");
}

protected override void Down(MigrationBuilder migrationBuilder)
{
    migrationBuilder.DropColumn(
        name: "Telefono",
        table: "Estudiantes");
}
```

- `Up` → agrega la columna `Telefono` a la tabla `Estudiantes`.
- `Down` → la quita si se revierte la migración.

4. Aplicamos la migración a la base de datos:

```powershell
dotnet ef database update --project GestionITM.Infrastructure --startup-project GestionITM.API
```

Después de este comando:

- La tabla `Estudiantes` en SQL Server tiene una nueva columna `Telefono`.
- La tabla `__EFMigrationsHistory` registra que `AgregarTelefonoEstudiante` está aplicada.

### 13.3. Resumen del ciclo de trabajo con migraciones

1. **Modificar el modelo** (por ejemplo, agregar una propiedad a una entidad).
2. **Crear migración**:

```powershell
dotnet ef migrations add NombreDeLaMigracion --project GestionITM.Infrastructure --startup-project GestionITM.API
```

3. **Revisar la migración generada** (archivos en `Infrastructure/Migrations`).
4. **Aplicar migración**:

```powershell
dotnet ef database update --project GestionITM.Infrastructure --startup-project GestionITM.API
```

5. (Opcional) Verificar en SQL Server Management Studio que el cambio existe en la base de datos.

---

## 14. Conceptos clave explicados para el estudiante

### 14.1. Entidad (Entity)

Es una **clase de C#** que representa un concepto del mundo real dentro del dominio de la aplicación.

Ejemplo: `Estudiante`, `Curso`, `Matricula`.

Características:

- Se mapea a una **tabla** de la base de datos (por EF Core).
- Cada instancia suele tener una **clave primaria** (`Id`).

### 14.2. DTO (Data Transfer Object)

Es una clase que se usa para **transportar datos** hacia o desde la API, sin exponer directamente toda la entidad.

Ejemplos:

- `EstudianteDto` → lo que la API devuelve al cliente.
- `EstudianteCreateDto` → lo que la API recibe en un POST.

Motivación:

- Controlar qué campos se exponen.
- Separar el modelo de dominio del contrato de la API.

### 14.3. Repositorio

Patrón que encapsula la lógica de **acceso a datos**:

- Define operaciones como `ObtenerTodoAsync`, `ObtenerPorIdAsync`, `AgregarAsync`.
- Oculta los detalles de EF Core a las capas superiores.

En este proyecto:

- Interfaces en `GestionITM.Domain/Interfaces`.
- Implementaciones en `GestionITM.Infrastructure/Repositories`.

### 14.4. DbContext

`ApplicationDbContext` es la clase que:

- Representa la **sesión con la base de datos**.
- Expone `DbSet<T>` para cada entidad (`DbSet<Estudiante>`, `DbSet<Curso>`, etc.).
- Sabe cómo generar el modelo y las migraciones.

EF Core lo usa para:

- Traducir operaciones LINQ a SQL.
- Guardar cambios (`SaveChangesAsync`).

### 14.5. Migración

Archivo C# que describe un **cambio incremental** en el esquema de la base de datos:

- Se crea con `dotnet ef migrations add ...`.
- Tiene métodos `Up` y `Down`.
- EF Core aplica las migraciones en orden con `dotnet ef database update`.

Ventajas:

- Puedes versionar la base de datos igual que versionas el código.
- Puedes reproducir el mismo esquema en diferentes entornos (desarrollo, prueba, producción).

### 14.6. AutoMapper

Librería que automatiza la **conversión entre tipos** (por ejemplo, entre entidad y DTO):

- Configuración en `MappingProfile` con `CreateMap<Origen, Destino>()`.
- Uso en controladores: `_mapper.Map<Destino>(origen)`.

En este proyecto:

- `CreateMap<Estudiante, EstudianteDto>();`
- `CreateMap<EstudianteCreateDto, Estudiante>();`

### 14.7. Controller y Endpoint

- **Controller**: clase de ASP.NET Core que maneja **peticiones HTTP**.
- Ejemplo: `EstudianteController`.
- **Endpoint**: método público del controlador decorado con atributos como `[HttpGet]`, `[HttpPost]`, etc.
- Ejemplo: `GET /api/estudiante`, `POST /api/estudiante`.

Flujo típico:

1. El cliente hace una petición HTTP (GET/POST/PUT/DELETE).
2. ASP.NET Core la enruta al método del controlador correspondiente.
3. El controlador usa repositorios y AutoMapper para procesar la petición.
4. El controlador devuelve una respuesta con código de estado (200, 201, 404, etc.) y datos (normalmente DTOs).
