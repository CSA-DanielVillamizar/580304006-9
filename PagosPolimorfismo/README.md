# PagosPolimorfismo

Ejercicio: **"El Poder de las Interfaces y el Polimorfismo"**

Este proyecto de consola en .NET 8 muestra cómo utilizar **interfaces** y **polimorfismo** para modelar diferentes formas de pago de manera flexible y extensible.

## Objetivo del ejercicio

- Entender qué es una **interfaz** (`IPagoService`) y cómo define un **contrato** común.
- Ver cómo distintas implementaciones (efectivo, tarjeta, Nequi) pueden compartir la misma interfaz pero comportarse de forma diferente.
- Ver cómo un **procesador** (`ProcesadorDePedidos`) puede trabajar con cualquier tipo de pago sin conocer los detalles de cada implementación.

## Estructura principal

- `IPagoService`: interfaz que define el método `bool ProcesarPago(decimal monto);`.
- `PagosEfectivo`: implementación de pago en efectivo.
- `PagoTarjetaCredito`: implementación de pago con tarjeta de crédito.
- `PagoNequi`: implementación de pago usando Nequi.
- `ProcesadorDePedidos`: clase que orquesta el proceso de pago usando una instancia de `IPagoService` (donde ocurre la "magia" del polimorfismo).
- `Program`: punto de entrada de la aplicación, donde se selecciona el método de pago y se ejecuta el flujo.

## Conceptos clave

1. **Interfaz como contrato**  
   `IPagoService` define lo que cualquier medio de pago debe ser capaz de hacer: procesar un pago por un monto específico.

2. **Polimorfismo**  
   El código que procesa el pago no necesita saber si es efectivo, tarjeta o Nequi. Solo depende de la interfaz `IPagoService`.  
   - Se puede cambiar la implementación en tiempo de ejecución (por ejemplo, según la opción que elija el usuario).
   - Es fácil agregar nuevos medios de pago (por ejemplo, `PagoBilleteraDigital`) sin modificar el código existente, solo implementando la interfaz.

3. **Abierto a extensión, cerrado a modificación (Principio OCP)**  
   El diseño permite **agregar** nuevos tipos de pago sin tener que **cambiar** el procesador ni la interfaz.

## Cómo ejecutar el proyecto

1. Abrir la solución en Visual Studio (`PagosPolimorfismo.slnx`).
2. Establecer el proyecto `PagosPolimorfismo` como proyecto de inicio.
3. Ejecutar (F5 o Ctrl+F5).
4. Seguir las instrucciones en consola y elegir el tipo de pago para ver cómo se comporta cada implementación.

## Ideas para extender el ejercicio

- Agregar validaciones (por ejemplo, montos mínimos o máximos).
- Crear nuevos métodos de pago (PSE, PayPal, etc.) implementando `IPagoService`.
- Implementar un menú más completo utilizando `ProcesadorDePedidos` para centralizar la lógica.
