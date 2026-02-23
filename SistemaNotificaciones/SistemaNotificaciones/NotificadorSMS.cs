using System;

/// <summary>
/// Notificador que envía mensajes por SMS.
/// </summary>
public sealed class NotificadorSMS : INotificador
{
    /// <inheritdoc />
    public void Enviar(string mensaje)
    {
        Console.WriteLine("Enviando SMS...");
    }
}
