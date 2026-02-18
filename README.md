# 580304006-9 - Proyectos del curso

Repositorio general del curso 580304006-9. Cada carpeta en la raíz corresponde a un proyecto desarrollado en clase. Usa este archivo como índice para saber qué contiene cada carpeta y cómo navegar.

## Estructura de carpetas del repositorio

- `GestionITM/`
  - Proyecto más completo del curso.
  - Solución ASP.NET Core / .NET 8 orientada a la **gestión académica del ITM** (estudiantes, matrículas, cursos, etc.).
  - Incluye arquitectura por capas:
    - `GestionITM.API/` – Web API con controladores (por ejemplo, `EstudiantesController`).
    - `GestionITM.Domain/` – Entidades de dominio (`Estudiante`, `Curso`, `Matricula`, `Product`) y reglas de negocio.
    - `GestionITM.Infrastructure/` – `ApplicationDbContext` y acceso a datos con EF Core.
  - Entra en `GestionITM/README.md` para ver la **guía completa**:
    - Requisitos.
    - Configuración con SQL Server local o SQLite.
    - Uso de EF Core y migraciones.
    - Ejemplos de ejecución y pruebas con `curl`.
    - Ejercicios sugeridos para los estudiantes.

- `CalculadoraITM/`
  - Proyecto de consola en C#.
  - Sirve para practicar:
    - Estructura básica de un proyecto .NET.
    - Entrada y salida por consola.
    - Operaciones aritméticas simples y control de flujo.

- `HolaITM/`
  - Proyecto de introducción a C#.
  - Ejemplo tipo "Hola mundo" para:
    - Ver cómo compilar y ejecutar un programa sencillo.
    - Entender la estructura mínima (`Program.cs`, `Main`, etc.).

- `PagosPolimorfismo/`
  - Proyecto orientado a practicar **Programación Orientada a Objetos (POO)**.
  - Enfocado en:
    - Herencia.
    - Polimorfismo.
    - Clases base y clases derivadas para distintos tipos de pagos.

- `Tarea1/`
  - Proyecto asociado a una de las primeras tareas del curso.
  - Normalmente incluye ejercicios de:
    - Tipos básicos.
    - Estructuras de control.
    - Primeros pasos en diseño de clases.

## Cómo usar este repositorio

1. **Clonar el repositorio**:

2. **Elegir la carpeta del proyecto** que quieras estudiar o ejecutar, por ejemplo:

- `cd GestionITM`
- `cd CalculadoraITM`
- `cd HolaITM`
- etc.

1. **Abrir el proyecto en Visual Studio** o VS Code.

2. Para el proyecto `GestionITM`, leer primero `GestionITM/README.md`, que contiene una guía detallada (requisitos, EF Core, SQL Server/SQLite, migraciones, API, ejercicios, etc.).

## Requisitos generales

- .NET SDK 8.0 o superior.
- Visual Studio 2022/2026 o VS Code con extensión de C# instalada.
- Para proyectos que usan base de datos (como `GestionITM`):
- SQL Server local (`(localdb)\\MSSQLLocalDB`) **o**
- SQLite (ver instrucciones específicas en el README de `GestionITM`).

## Recomendación para estudiantes

- Empezar por proyectos sencillos como `HolaITM` y `CalculadoraITM` para familiarizarse con C# y .NET.
- Continuar con `PagosPolimorfismo` y `Tarea1` para reforzar POO y estructuras de control.
- Finalmente, estudiar `GestionITM` como ejemplo de solución más completa:
- Arquitectura por capas.
- Web API.
- EF Core y migraciones.
- Documentación y buenas prácticas.

Este repositorio está pensado como **material de enseñanza**: cada carpeta es un ejemplo práctico de los temas vistos en clase.
