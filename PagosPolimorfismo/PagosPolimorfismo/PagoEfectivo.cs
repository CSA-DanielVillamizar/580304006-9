using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PagosPolimorfismo
//Las Implementaciones (Los "Bombillos")
{
    // OPCIÓN C: Pago en efectivo (El bombillo incandescente)
    public class PagoEfectivo : IPagoService
    {
        public bool ProcesarPago(decimal monto)
        {
            // Aquí iría la lógica específica para procesar un pago en efectivo
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[EFECTIVO] Recibiendo {monto} en efectivo...");
            Console.WriteLine($"[EFECTIVO] Entregando cambio si es necesario.");
            Console.ResetColor();
            return true; // Simulamos que el pago fue exitoso
        }
    
    }
}
