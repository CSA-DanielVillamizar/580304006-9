using PagosPolimorfismo;

class Program
{
    static void Main(string[] args)
    {
        decimal totalCompra = 150000m; // La 'm' es para indicar decimal

        Console.WriteLine("--- BIENVENIDO AL E-COMMERCE ITM ---");
        Console.WriteLine("Seleccione su método de pago:");
        Console.WriteLine("1. Tarjeta de Crédito");
        Console.WriteLine("2. Nequi");
        Console.WriteLine("3. Efectivo");

        string opcion = Console.ReadLine();

        // Variable polimórfica: Puede guardar cualquiera de las 3 clases
        IPagoService metodoSeleccionado = null;

        // Factory simple (Fábrica de objetos)
        switch (opcion)
        {
            case "1":
                metodoSeleccionado = new PagoTarjetaCredito();
                break;
            case "2":
                metodoSeleccionado = new PagoNequi();
                break;
            case "3":
                metodoSeleccionado = new PagoEfectivo();
                break;
            case "4":
                metodoSeleccionado = new PagoBitcoin();
                break;
            default:
                Console.WriteLine("Opción no válida. Se usará Efectivo por defecto.");
                metodoSeleccionado = new PagoEfectivo();
                break;
            
        }

        Console.WriteLine("\n--- PROCESANDO ---");

        // MAGIA: Le paso el método seleccionado al procesador.
        // El procesador NO SABE qué elegí, solo sabe que cumple la interfaz.
        var procesador = new ProcesadorDePedidos(metodoSeleccionado);

        procesador.FinalizarPedido(totalCompra);

        Console.ReadKey();
    }
}