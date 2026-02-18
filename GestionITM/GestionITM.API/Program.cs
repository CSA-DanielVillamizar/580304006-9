using GestionITM.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registrar ApplicationDbContext
//
// Opción 1 (por defecto): SQL Server local (localdb)
//   - Cadena de conexión configurada en appsettings.json bajo "DefaultConnection".
//   - Se usa UseSqlServer(connectionString).
//
// Opción 2 (ejemplo educativo): SQLite
//   - Cambiar la cadena de conexión en appsettings.json a: "Data Source=GestionITM.db".
//   - Cambiar UseSqlServer por UseSqlite(connectionString).
//
// Esto permite a los estudiantes ver claramente qué cambiar para usar otro proveedor de base de datos.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
