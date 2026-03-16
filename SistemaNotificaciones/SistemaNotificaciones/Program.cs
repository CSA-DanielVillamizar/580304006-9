/// <summary>
/// Punto de entrada del sistema de notificaciones.
/// </summary>
Console.WriteLine("=== Sistema de Notificaciones ===");
Console.WriteLine("Seleccione el tipo de notificación:");
Console.WriteLine("1. Email");
Console.WriteLine("2. WhatsApp");
Console.WriteLine("3. SMS");
Console.Write("Opción: ");

var opcion = Console.ReadLine();

var selector = new NotificadorSelector();
var notificador = selector.Seleccionar(opcion);

if (notificador is null)
{
    Console.WriteLine("Opción no válida. Terminando el programa.");
    return;
}

Console.Write("Ingrese el mensaje a enviar: ");
var mensaje = Console.ReadLine();

if (string.IsNullOrWhiteSpace(mensaje))
{
    Console.WriteLine("No se puede enviar un mensaje vacío.");
    return;
}

notificador.Enviar(mensaje);

Console.WriteLine("Notificación enviada correctamente.");
