/// <summary>
/// Encapsula la lógica de selección del notificador según la opción ingresada por el usuario.
/// </summary>
public sealed class NotificadorSelector
{
    /// <summary>
    /// Selecciona el notificador adecuado según la opción proporcionada.
    /// </summary>
    /// <param name="opcion">Opción ingresada por el usuario.</param>
    /// <returns>Una instancia de <see cref="INotificador"/> o <c>null</c> si la opción es inválida.</returns>
    public INotificador? Seleccionar(string? opcion) => opcion switch
    {
        "1" => new NotificadorEmail(),
        "2" => new NotificadorWhatsApp(),
        "3" => new NotificadorSMS(),
        _ => null
    };
}
