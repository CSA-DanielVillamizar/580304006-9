using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PagosPolimorfismo
{
    public class PagoBitcoin : IPagoService
    {
        public bool ProcesarPago(decimal monto)
        {
            // Aquí iría la lógica específica para procesar un pago con Bitcoin
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"[BITCOIN] Conenctando con la red de Bitcoin...");
            Console.WriteLine($"[BITCOIN] Debitando {monto} en BTC de tu wallet.");
            Console.ResetColor();
            return true; // Simulamos que el pago fue exitoso
        }

    }
}
