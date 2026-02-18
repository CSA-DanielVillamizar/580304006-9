using System;

public class Class1
{
    Console.WriteLine("Por favor, ingresa tu edad:");
string inputEdad = Console.ReadLine();


    // Forma nivel 1 (Novato): Usar int.Parse para convertir el texto a número entero
    // int edad = int.Parse(inputEdad); : //  Si inputEdad es "hola", el programa explota

    // Forma nivel 3 (Intermedio): Usar Convert.ToInt32 para convertir el texto a número entero
    // int edad = Convert.ToInt32(inputEdad); : // Si inputEdad es "hola", el programa explota

    // Forma nivel 5 (Avanzado): Usar int.TryParse para intentar convertir el texto a número entero sin que el programa explote
    int edad;
    bool conversionExitosa = int.TryParse(inputEdad, out edad);
if (!esNumeroValido)
    Console.WriteLine("¡Error! Debes ingresar un número entero (ej.: 25).");
        // aquí podríamo hacer un bucle para volver a pedir la edad hasta que el usuario ingrese un número válido
        return; // para salir del programa si la edad no es válida
        }
if (edad < 0 || edad > 120)
    Console.WriteLine("¡Error! La edad debe estar entre 0 y 120 años.");
        // aquí podríamo hacer un bucle para volver a pedir la edad hasta que el usuario ingrese un número válido
        return; // para salir del programa si la edad no es válida
        }
Console.WriteLine($"Tu edad es: {edad} años.");
}


