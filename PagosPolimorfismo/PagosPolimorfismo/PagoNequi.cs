using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PagosPolimorfismo
//Las Implementaciones (Los "Bombillos")
{
    // OPCIÓN B: Pago con Nequi (El bombillo fluorescente)
    public class PagoNequi : IPagoService
    {
        public bool ProcesarPago(decimal monto)
        {
            // Aquí iría la lógica específica para procesar un pago con Nequi
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"[NEQUI] Conenctando con Nequi...");
            Console.WriteLine($"[NEQUI] Debitando {monto} de tu cuenta Nequi.");
            Console.ResetColor();
            return true; // Simulamos que el pago fue exitoso
        }
    
    }
}
