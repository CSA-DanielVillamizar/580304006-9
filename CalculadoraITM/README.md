# 580304006-9

Este repositorio contiene los proyectos de consola desarrollados en clase:

- `HolaITM`: proyecto de ejemplo que imprime un saludo en consola.
- `CalculadoraITM`: calculadora básica segura que realiza operaciones aritméticas entre dos números (`+`, `-`, `*`, `/`).

## Requisitos

- .NET SDK 8.0 o superior instalado.

## Cómo ejecutar los proyectos

Desde la raíz del repositorio (`C:\Dev\580304006-9`), puedes ejecutar:

```bash
# Ejecutar HolaITM
cd HolaITM/HolaITM
 dotnet run

# Ejecutar CalculadoraITM
cd ../../CalculadoraITM/CalculadoraITM/CalculadoraITM
 dotnet run
```

## Detalles de `CalculadoraITM`

Características principales:

- Solicita dos números por consola.
- Valida que cada entrada sea numérica usando `double.TryParse`.
- Permite elegir la operación: `+`, `-`, `*` o `/`.
- Maneja el error de división por cero mostrando un mensaje en rojo.
- Muestra el resultado con un mensaje en verde y luego restablece el color de la consola.

Manejo de errores:

- Si el usuario ingresa algo que no es un número, el programa muestra:
  - `Error: La entrada no es un número válido.`
  - Termina la ejecución para evitar fallos.
- Si se intenta dividir por cero, muestra:
  - `Error Matemático: No se puede dividir por cero.`

## Estructura del repositorio

- `HolaITM/` – solución y proyecto de consola de saludo.
- `CalculadoraITM/` – solución y proyecto de consola de la calculadora segura.
- `Tarea1/` – repositorio embebido utilizado como tarea (submódulo potencial).
