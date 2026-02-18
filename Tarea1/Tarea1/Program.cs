using System;
namespace Tarea1

{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 1. Pedir el Nombre (Esto ya es texto, no necesita conversión)
            Console.WriteLine("Por favor ingresa su nombre: ");

            string nombre = Console.ReadLine();

            // 2. Pedir la Edad (Esto es un número, necesitamos convertirlo a entero)
            Console.WriteLine("Por favor ingresa su edad: ");

            string entradaEdad = Console.ReadLine();


            // Aquie está el reto: Convertir  el texto a número entero
            int edad = int.Parse(entradaEdad);


            // Pedir el Semestre (Esto es un número, necesitamos convertirlo a entero)

            Console.WriteLine("Por favor ingresa tu semestre: ");
            string entradaSemestre = Console.ReadLine();
            // Usamos Convert.ToInt32 para variar y que veamos otra forma de convertir texto a número entero
            int semestre = Convert.ToInt32(entradaSemestre);

            Console.WriteLine($"--------------------------------------");

            // Imprimir el mensaje final con los datos ingresados
            // Usamos el signo de dólar para interpolar las variables dentro del string

            Console.WriteLine($"Hola {nombre}, tienes {edad} años y estás en el semestre {semestre}.");

            // Esperar a que el usuario presione una tecla antes de cerrar la consola
            Console.ReadKey();
        }
    }
}

