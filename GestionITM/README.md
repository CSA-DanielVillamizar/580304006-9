# GestionITM

Solución basada en .NET 8 para la gestión académica del ITM: estudiantes, matrículas, cursos y otros elementos relacionados. La arquitectura se organiza en capas (API, dominio e infraestructura).

## Proyectos

1. `GestionITM.API`
   - Tipo: ASP.NET Core Web API (.NET 8).
   - Responsabilidad: Exponer endpoints HTTP/REST para consumir la lógica del dominio.
   - Dependencias de proyecto:
     - `GestionITM.Domain`
     - `GestionITM.Infrastructure`

2. `GestionITM.Domain`
   - Tipo: Class Library (.NET 8).
   - Responsabilidad: Contener las entidades de dominio, reglas de negocio y contratos para estudiantes, matrículas, cursos, productos académicos, etc.
   - Comentarios: No referencia a otras capas para mantener una arquitectura limpia.

3. `GestionITM.Infrastructure`
   - Tipo: Class Library (.NET 8).
   - Responsabilidad: Implementar acceso a datos, persistencia y servicios externos para el dominio académico (por ejemplo, tablas de estudiantes, matrículas y cursos).
   - Dependencias de proyecto:
     - `GestionITM.Domain`

## Requisitos

- .NET SDK 8.0 o superior
- Visual Studio 2022/2026 o VS Code con extensiones C#
 - SQL Server local (`(localdb)\\MSSQLLocalDB`) para la configuración por defecto de la base de datos
   - Opcional: se puede usar SQLite cambiando la configuración (ver sección de base de datos)

## Cómo ejecutar

En la raíz de la solución (`GestionITM`):

```bash
cd GestionITM.API
 dotnet run
```

La API se iniciará en la URL configurada (por defecto `https://localhost:5001` / `http://localhost:5000`).

### Ejecutar con SQL Server (opción por defecto)

1. Asegúrate de tener instalado SQL Server local (`(localdb)\\MSSQLLocalDB`).
2. Verifica que en `GestionITM.API/appsettings.json` la cadena de conexión apunte a SQL Server local:

   ```jsonc
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\\\MSSQLLocalDB;Database=GestionITMDb;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```

3. Revisa que en `GestionITM.API/Program.cs` se use `UseSqlServer`:

   ```csharp
   builder.Services.AddDbContext<ApplicationDbContext>(options =>
       options.UseSqlServer(connectionString));
   ```

4. Desde la raíz de la solución ejecuta:

   ```bash
   dotnet ef database update \
     --project GestionITM.Infrastructure/GestionITM.Infrastructure.csproj \
     --startup-project GestionITM.API/GestionITM.API.csproj

   cd GestionITM.API
   dotnet run
   ```

### Ejecutar con SQLite (opción educativa)

1. Cambia la cadena de conexión en `GestionITM.API/appsettings.json`:

   ```jsonc
   "ConnectionStrings": {
     "DefaultConnection": "Data Source=GestionITM.db"
   }
   ```

2. Cambia el proveedor en `GestionITM.API/Program.cs` a `UseSqlite`:

   ```csharp
   builder.Services.AddDbContext<ApplicationDbContext>(options =>
       options.UseSqlite(connectionString));
   ```

3. Desde la raíz de la solución ejecuta para crear/actualizar la base de datos SQLite:

   ```bash
   dotnet ef database update \
     --project GestionITM.Infrastructure/GestionITM.Infrastructure.csproj \
     --startup-project GestionITM.API/GestionITM.API.csproj

   cd GestionITM.API
   dotnet run
   ```

## Estructura de carpetas

- `GestionITM.API/` – Capa de presentación / API.
- `GestionITM.Domain/` – Entidades y lógica de dominio.
- `GestionITM.Infrastructure/` – Implementaciones de infraestructura (por ejemplo, acceso a datos).

## Convenciones

- Target framework: `.NET 8` para todos los proyectos.
- `Nullable` habilitado.
- Uso de `ImplicitUsings` habilitado.

## Próximos pasos

- Documentar endpoints en Swagger/OpenAPI.
- Agregar pruebas unitarias y de integración.
- Definir modelo de datos completo en `GestionITM.Domain`.

## Base de datos: SQL Server local vs SQLite

Por defecto, el proyecto está configurado para usar **SQL Server local** (`(localdb)\\MSSQLLocalDB`) mediante `UseSqlServer` en `GestionITM.API/Program.cs`.

Para propósitos educativos, también se documenta cómo cambiar a **SQLite**.

### Opción 1 (por defecto): SQL Server local `(localdb)\\MSSQLLocalDB`

- Archivo: `GestionITM.API/appsettings.json`

```jsonc
"ConnectionStrings": {
  // Opción 1 (por defecto): SQL Server local (localdb)
  // Usada actualmente en Program.cs con UseSqlServer
  "DefaultConnection": "Server=(localdb)\\\\MSSQLLocalDB;Database=GestionITMDb;Trusted_Connection=True;TrustServerCertificate=True;"

  // Opción 2 (ejemplo educativo): SQLite
  // Para usarla, cambia Program.cs a UseSqlite y ajusta esta cadena:
  // "DefaultConnection": "Data Source=GestionITM.db"
}
```

- Archivo: `GestionITM.API/Program.cs`

```csharp
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
```

Con esta configuración se usará una base de datos de SQL Server local llamada `GestionITMDb`.

### Opción 2 (educativa): SQLite (`.db` en disco)

Para cambiar a SQLite, los estudiantes deben hacer **dos pasos**:

1. **Cambiar la cadena de conexión** en `GestionITM.API/appsettings.json`:

   ```jsonc
   "ConnectionStrings": {
     // "DefaultConnection": "Server=(localdb)\\\\MSSQLLocalDB;Database=GestionITMDb;Trusted_Connection=True;TrustServerCertificate=True;"
     "DefaultConnection": "Data Source=GestionITM.db"
   }
   ```

2. **Cambiar el proveedor de EF Core** en `GestionITM.API/Program.cs`:

   ```csharp
   var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

   // Para SQL Server (opción por defecto)
   // builder.Services.AddDbContext<ApplicationDbContext>(options =>
   //     options.UseSqlServer(connectionString));

   // Para SQLite (opción educativa)
   builder.Services.AddDbContext<ApplicationDbContext>(options =>
       options.UseSqlite(connectionString));
   ```

Después de cambiar de proveedor (SQL Server ↔ SQLite), es recomendable:

- Eliminar la base de datos anterior (si ya existía) o usar un nombre/cadena distintos.
- Regenerar migraciones si fuese necesario, o ejecutar de nuevo:

```bash
dotnet ef database update \
  --project GestionITM.Infrastructure/GestionITM.Infrastructure.csproj \
  --startup-project GestionITM.API/GestionITM.API.csproj
```

Esta sección sirve como guía para que los estudiantes entiendan **qué archivo tocar y qué línea cambiar** cuando se quiere usar un proveedor de base de datos distinto.

## Uso de EF Core y migraciones (paso a paso)

1. **Definir o modificar entidades** en `GestionITM.Domain/Entities` (por ejemplo, `Estudiante`, `Curso`, `Matricula`, `Product`).
2. **Agregar los DbSet correspondientes** en `GestionITM.Infrastructure/ApplicationDbContext`:

   ```csharp
   public DbSet<Estudiante> Estudiantes { get; set; }
   public DbSet<Curso> Cursos { get; set; }
   public DbSet<Matricula> Matriculas { get; set; }
   public DbSet<Product> Products { get; set; }
   ```

3. **Crear una nueva migración** cuando cambie el modelo:

   Desde la raíz de la solución:

   ```bash
   dotnet ef migrations add NombreDeLaMigracion \
     --project GestionITM.Infrastructure/GestionITM.Infrastructure.csproj \
     --startup-project GestionITM.API/GestionITM.API.csproj
   ```

   Ejemplos de nombres: `InitialCreate`, `AddMatriculaEntity`, `UpdateCursoCredits`.

4. **Aplicar las migraciones a la base de datos**:

   ```bash
   dotnet ef database update \
     --project GestionITM.Infrastructure/GestionITM.Infrastructure.csproj \
     --startup-project GestionITM.API/GestionITM.API.csproj
   ```

5. **Ejecutar la API** una vez que la base de datos está actualizada:

   ```bash
   cd GestionITM.API
   dotnet run
   ```

Con estos pasos los estudiantes pueden ver el flujo completo de trabajo con EF Core:

- Modificar entidades de dominio.
- Actualizar el `DbContext`.
- Generar migraciones.
- Actualizar la base de datos.
- Probar los cambios a través de la API.

## Chuleta rápida de la API

- Ejecutar API (desde la raíz de la solución):

  ```bash
  dotnet ef database update \
    --project GestionITM.Infrastructure/GestionITM.Infrastructure.csproj \
    --startup-project GestionITM.API/GestionITM.API.csproj

  cd GestionITM.API
  dotnet run
  ```

- Probar `EstudiantesController` con `curl` (suponiendo `https://localhost:5001`):

  - Obtener todos los estudiantes:

    ```bash
    curl -k https://localhost:5001/api/Estudiantes
    ```

  - Crear un estudiante:

    ```bash
    curl -k -X POST https://localhost:5001/api/Estudiantes \
      -H "Content-Type: application/json" \
      -d '{"nombre":"Juan Pérez","correo":"juan.perez@example.com","fechaInscripcion":"2026-01-01T00:00:00"}'
    ```

  - Obtener un estudiante por id:

    ```bash
    curl -k https://localhost:5001/api/Estudiantes/1
    ```

## Ejercicios sugeridos para los estudiantes

1. **Agregar un nuevo campo a `Curso` y seguir el flujo completo de EF Core**
   - Editar `GestionITM.Domain/Entities/Curso.cs` y agregar, por ejemplo:
     ```csharp
     [MaxLength(500)]
     public string Descripcion { get; set; } = string.Empty;
     ```
   - Crear una nueva migración:
     ```bash
     dotnet ef migrations add AddDescripcionToCurso \
       --project GestionITM.Infrastructure/GestionITM.Infrastructure.csproj \
       --startup-project GestionITM.API/GestionITM.API.csproj
     ```
   - Actualizar la base de datos:
     ```bash
     dotnet ef database update \
       --project GestionITM.Infrastructure/GestionITM.Infrastructure.csproj \
       --startup-project GestionITM.API/GestionITM.API.csproj
     ```
   - Probar la API (por ejemplo, crear/consultar cursos cuando exista un `CursosController`).

2. **Crear un `CursosController` similar a `EstudiantesController`**
   - Crear `GestionITM.API/Controllers/CursosController.cs`.
   - Implementar acciones `GET`, `GET/{id}`, `POST`, `PUT`, `DELETE` usando `ApplicationDbContext` y `DbSet<Curso>`.
   - Probar los endpoints desde Swagger o con `curl`.

3. **Implementar un CRUD para `Matricula`**
   - Revisar la entidad `Matricula` en `GestionITM.Domain/Entities/Matricula.cs`.
   - Crear un `MatriculasController` en la API.
   - Probar la creación de matrículas relacionando estudiantes y cursos.

4. **Cambiar de SQL Server a SQLite y repetir una migración**
   - Cambiar la conexión y el proveedor según la sección "Base de datos: SQL Server local vs SQLite".
   - Crear una nueva migración de prueba (por ejemplo, agregando un campo opcional a `Estudiante`).
   - Aplicar la migración y comprobar que se refleja en la nueva base de datos.
