using System;

/// <summary>
/// Notificador que envía mensajes por WhatsApp.
/// </summary>
public sealed class NotificadorWhatsApp : INotificador
{
    /// <inheritdoc />
    public void Enviar(string mensaje)
    {
        Console.WriteLine($"Enviando WhatsApp al +57...: {mensaje}");
    }
}
