using AutoMapper;
using GestionITM.API.Mappings;
using GestionITM.Domain.Interfaces;
using GestionITM.Infrastructure;
using GestionITM.API.Middleware;
using GestionITM.Infrastructure.Repositories;
using GestionITM.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;// Para la configuración de swagger 
using System.Text; // Para Encoding.UTF8.GetBytes
using System.Reflection; // Necesaqrio para Assembly.GetExecutingAssembly() en SwaggerGen
using System.IO; // Necesario para Path.Combine en SwaggerGen

var builder = WebApplication.CreateBuilder(args);

// Ocultar warnings de DataProtection porque la API usa JWT, no Cookies
builder.Logging.AddFilter("Microsoft.AspNetCore.DataProtection", LogLevel.Error);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "GestionITM API",
        Version = "v1"
    });

    //INSTRUCCIÓN NUEVA
    // Localice el archivo xml generado en la carpeta de binario (bin) después de compilar el proyecto. El nombre del archivo suele ser el mismo que el nombre del proyecto, seguido de .xml (por ejemplo, GestionITM.API.xml).
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    // Le dice a Swagger que incluya los comentarios XML para mejorar la documentación de la API. Esto es especialmente útil para describir los endpoints, parámetros y respuestas.
    c.IncludeXmlComments(xmlPath);
});

// 1. Configurar la cadena de conexión
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuración de JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            // Nota docente: este warning (CS8604) aparece porque builder.Configuration["Jwt:Key"]
            // podría ser null y Encoding.UTF8.GetBytes no acepta null.
            // Una forma correcta de resolverlo sería:
            //
            // var jwtKey = builder.Configuration["Jwt:Key"];
            // if (string.IsNullOrWhiteSpace(jwtKey))
            // {
            //     throw new InvalidOperationException("Jwt:Key no está configurado en appsettings.json o en las variables de entorno.");
            // }
            // IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            //
            // Es mejor que usar el operador ! (null-forgiving), porque así el sistema falla
            // de forma clara al arrancar cuando falta la configuración.
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });


// 2. Registrar el Repositorio (Inyección de Dependencias)
// AddScoped significa: "Crea una instancia por cada petición HTTP"
builder.Services.AddScoped<IEstudianteRepository, EstudianteRepository>();
builder.Services.AddScoped<IEstudianteService, EstudianteService>();
builder.Services.AddScoped<ICursoRepository, CursoRepository>();

builder.Services.AddScoped<IProfesorRepository, ProfesorRepository>();
builder.Services.AddScoped<IProfesorService, ProfesorService>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

// Nivel Dios: Aplicar migraciones pendientes automáticamente al arrancar
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocurrió un error al aplicar la migración de la base de datos.");
    }
}

// Configure the HTTP request pipeline. ESCUDO DE EXCEPCIONES GLOBAL
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Docker")
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Bloque de activación de autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();