using System;

/// <summary>
/// Notificador que envía mensajes por correo electrónico.
/// </summary>
public sealed class NotificadorEmail : INotificador
{
    /// <inheritdoc />
    public void Enviar(string mensaje)
    {
        Console.WriteLine($"Enviando correo a usuario@itm.edu.co: {mensaje}");
    }
}
