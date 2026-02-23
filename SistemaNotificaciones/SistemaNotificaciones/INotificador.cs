using System;

/// <summary>
/// Define el contrato para cualquier tipo de notificador.
/// </summary>
public interface INotificador
{
    /// <summary>
    /// Envía un mensaje utilizando el canal de notificación específico.
    /// </summary>
    /// <param name="mensaje">Contenido del mensaje a enviar.</param>
    void Enviar(string mensaje);
}
