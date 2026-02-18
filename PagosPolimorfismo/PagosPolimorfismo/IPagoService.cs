using System;

namespace PagosPolimorfismo
//Definir el Contrato (La Interfaz)
{
    // summary
    // Contrato: Cualquier cosa que quiera procesar un  pago, debe implementar esta interfaz
    // DEBE saber esto. No me importa cómo lo haga.
    //</summary>
    public interface IPagoService
    {
        // solo definimos la firma:  Qué entra y qué sale, pero no el cómo se hace
		// No hay llaves {} ni codigo aquí, solo la firma del método
		bool ProcesarPago(decimal monto);
    }
}
