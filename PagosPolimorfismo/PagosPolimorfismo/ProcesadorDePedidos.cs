using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PagosPolimorfismo
//El Procesador (Donde ocurre la magia)
{
    public class ProcesadorDePedidos
    {
        // Miren esto: El procesador tiene un espacio para CUALQUIER
        // servicio de pago. No sabe cuál es, ni le importa. Solo sabe que debe ser algo que implemente IPagoService

        private readonly IPagoService _servicioDePago;

        // INYECCIÓN DE DEPENDENCIAS (Manual por el momento)
        // El servicio específico vien desde afuera (Constructor)
        // "Dame un servicio de pago, y yo lo usaré para procesar los pedidos"

        public ProcesadorDePedidos(IPagoService servicioDePago)
        {
            _servicioDePago = servicioDePago;
        }
        public void FinalizarPedido(decimal total)
        {
            Console.WriteLine($"Iniciando proceso de compra por un total de ${total}...");

            // POLIMORFISMO EN ACCIÓN: No sabemos qué tipo de pago es, pero eso no nos importa. Solo sabemos que tiene un método ProcesarPago que podemos llamar.
            // Aquí se ejecuta el código d ela tarjeta, o de Nequi, o de Bitcoin
            // dependiendo de lo que hayamos inyectado al crear el procesador.
            bool resultado = _servicioDePago.ProcesarPago(total);

            if (resultado)
            {
                Console.WriteLine(" Pedido enviado al almacen. ¡Gracias por tu compra!");
            }
            else
            {
                Console.WriteLine(" Pedido pendiente de pago. Por favor, intenta nuevamente.");
            }
        }
    }
}
               