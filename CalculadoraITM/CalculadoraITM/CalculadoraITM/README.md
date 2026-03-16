# CalculadoraITM

Aplicación de consola en C# (.NET 8) que implementa una calculadora básica con validaciones para practicar lógica de programación.

## Enunciado del ejercicio

Ejercicio de Lógica:

- Crear una nueva App de Consola: `CalculadoraITM`.
- Debe pedir dos números y una operación (`+`, `-`, `*`, `/`).

Requisitos Nivel 5:

1. Usar `TryParse` para los números.
2. Usar un `switch` para la operación.
3. Controlar la división por cero.

## Cómo funciona la aplicación

Al ejecutar el programa, se sigue este flujo:

1. Muestra el título:  
   `--- CALCULADORA SEGURA ITM v1.0 ---`
2. Solicita el primer número.
3. Valida el primer número con `double.TryParse`:
   - Si no es válido, muestra en rojo:  
     `Error: La entrada no es un número válido.`  
     y termina la ejecución.
4. Solicita el segundo número.
5. Valida el segundo número con `double.TryParse` (igual que el primero).
6. Solicita la operación (`+`, `-`, `*`, `/`).
7. Usa un `switch` sobre la operación:
   - `+` → suma
   - `-` → resta
   - `*` → multiplicación
   - `/` → división (con validación adicional)
   - cualquier otra entrada → mensaje en amarillo:  
     `Opción no válida. Reinicie el programa.` y termina.
8. En el caso de la división:
   - Si el segundo número es `0`, muestra en rojo:  
     `Error Matemático: No se puede dividir por cero.`  
     y termina sin realizar la operación.
9. Si todo es correcto, muestra el resultado en verde:  
   `RESULTADO: num1 operacion num2 = resultado`
10. Restaura el color original de la consola y espera que el usuario presione una tecla para salir.

## Tecnologías usadas

- Lenguaje: C# 12
- Runtime/SDK: .NET 8
- Tipo de proyecto: Aplicación de consola

## Ejemplos de uso

### Ejemplo 1: Suma

- Entrada:
  - Primer número: `5`
  - Segundo número: `3`
  - Operación: `+`
- Salida:
  - `RESULTADO: 5 + 3 = 8`

### Ejemplo 2: División por cero

- Entrada:
  - Primer número: `10`
  - Segundo número: `0`
  - Operación: `/`
- Salida:
  - `Error Matemático: No se puede dividir por cero.`

### Ejemplo 3: Entrada inválida

- Entrada:
  - Primer número: `abc`
- Salida:
  - `Error: La entrada no es un número válido.`

## Cómo ejecutar el proyecto

Desde la raíz del repositorio:

cd CalculadoraITM/CalculadoraITM/CalculadoraITM dotnet run

Asegúrate de tener instalado:

- .NET SDK 8.0 o superior.