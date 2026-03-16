using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PagosPolimorfismo
//Las Implementaciones (Los "Bombillos")
{
    // OPCIÓN A:  Pago con tarjeta (El bombillo led)
    public class PagoTarjetaCredito : IPagoService
    {
        public bool ProcesarPago(decimal monto)
        {
            // Aquí iría la lógica específica para procesar un pago con tarjeta de crédito
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[TARJETA] Conenctando con Visa/MasterCard...");
            Console.WriteLine($"[TARJETA] Debitando {monto} de la cuenta terminada en **** 1234.");
            Console.ResetColor();
            return true; // Simulamos que el pago fue exitoso
        }
    }
}



