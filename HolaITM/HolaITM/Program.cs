namespace HolaITM // Apellido de nuestro código, ¡como el apellido de una familia!
{
    internal class Program // La clase es como la receta de un pastel, define cómo se hace algo. En este caso, es la receta para nuestro programa.
    {
        static void Main(string[] args) // Esta es la puerta de entrada, el punto de partida de nuestro programa
        {
            // Definición explícita del tipo de dato
            string mensaje = "Bienvenidos al ITM";
            int anio = 2026;

            // ERROR COMÚN (Nivel 1): Intentar sumar peras con manzanas
            // anio = "Dos mil veintiseis"; // ¡Descomenta esto y mira el error rojo!

            Console.WriteLine(mensaje + " - Ciclo: " + anio);

            // Pausa para que la consola no se cierre sola
            Console.ReadLine();
        }
        }
    }




