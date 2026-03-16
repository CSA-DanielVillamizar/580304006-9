using System;

// Aplicación de consola: Calculadora segura ITM
// Requisitos:
// - Leer dos números usando double.TryParse
// - Leer una operación (+, -, *, /) usando switch
// - Controlar división por cero

Console.Title = "Calculadora Segura ITM";

// Guardar el color original de la consola para restaurarlo al final
var originalColor = Console.ForegroundColor;

try
{
	Console.WriteLine("--- CALCULADORA SEGURA ITM v1.0 ---");
	Console.WriteLine();

	// Leer y validar primer número
	Console.Write("Ingrese el primer número: ");
	var input1 = Console.ReadLine();
	if (!double.TryParse(input1, out var numero1))
	{
		Console.ForegroundColor = ConsoleColor.Red;
		Console.WriteLine("Error: La entrada no es un número válido.");
		return;
	}

	// Leer y validar segundo número
	Console.Write("Ingrese el segundo número: ");
	var input2 = Console.ReadLine();
	if (!double.TryParse(input2, out var numero2))
	{
		Console.ForegroundColor = ConsoleColor.Red;
		Console.WriteLine("Error: La entrada no es un número válido.");
		return;
	}

	// Leer operación
	Console.Write("Ingrese la operación (+, -, *, /): ");
	var operacionInput = Console.ReadLine();
	var operacion = string.IsNullOrWhiteSpace(operacionInput)
		? '\0'
		: operacionInput.Trim()[0];

	double resultado;
	switch (operacion)
	{
		case '+':
			resultado = numero1 + numero2;
			break;
		case '-':
			resultado = numero1 - numero2;
			break;
		case '*':
			resultado = numero1 * numero2;
			break;
		case '/':
			if (numero2 == 0)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Error Matemático: No se puede dividir por cero.");
				return;
			}
			resultado = numero1 / numero2;
			break;
		default:
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Opción no válida. Reinicie el programa.");
			return;
	}

	Console.ForegroundColor = ConsoleColor.Green;
	Console.WriteLine();
	Console.WriteLine($"RESULTADO: {numero1} {operacion} {numero2} = {resultado}");
}
finally
{
	// Siempre restaurar el color original
	Console.ForegroundColor = originalColor;
	Console.WriteLine();
	Console.WriteLine("Presione cualquier tecla para salir...");
	Console.ReadKey(intercept: true);
}
