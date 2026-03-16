# Sistema de Notificaciones (Ejemplo de Polimorfismo en C#)

Este proyecto es una aplicación de consola en .NET 8 que muestra cómo aplicar **polimorfismo** y una **arquitectura limpia y organizada** en un sistema sencillo de notificaciones.

## ¿Qué hace la aplicación?

1. Muestra un menú al usuario para elegir cómo quiere recibir su notificación:
   - `1` → Email
   - `2` → WhatsApp
   - `3` → SMS
2. Pide al usuario que escriba un mensaje.
3. Usa **polimorfismo** para enviar el mensaje usando la implementación adecuada de `INotificador`.

## Estructura del proyecto

- `Program.cs`
  - Punto de entrada de la aplicación (top-level statements en C# 12).
  - Orquesta el flujo: muestra el menú, lee la opción, lee el mensaje y llama a `INotificador.Enviar`.

- `INotificador.cs`
  - Contiene la interfaz `INotificador`:
    ```csharp
    public interface INotificador
    {
        void Enviar(string mensaje);
    }
    ```
  - Define el **contrato** que todas las notificaciones deben cumplir.

- `NotificadorEmail.cs`
  - Implementación concreta de `INotificador` para correo electrónico.
  - Muestra por consola:
    ```text
    Enviando correo a usuario@itm.edu.co: [mensaje]
    ```

- `NotificadorWhatsApp.cs`
  - Implementación concreta de `INotificador` para WhatsApp.
  - Muestra por consola:
    ```text
    Enviando WhatsApp al +57...: [mensaje]
    ```

- `NotificadorSMS.cs`
  - Implementación concreta de `INotificador` para SMS.
  - Muestra por consola:
    ```text
    Enviando SMS...
    ```

- `NotificadorSelector.cs`
  - Clase que encapsula la lógica de selección del notificador según la opción del usuario.
  - Devuelve una instancia de `INotificador` o `null` si la opción no es válida.

## Conceptos usados

### 1. Interfaz (`INotificador`)

Una **interfaz** define un contrato: qué métodos debe ofrecer una clase, sin decir cómo se implementan.

En este caso:

- `INotificador` obliga a que cualquier notificador tenga el método:
  ```csharp
  void Enviar(string mensaje);
  ```

Esto permite tratar a todas las implementaciones (`NotificadorEmail`, `NotificadorWhatsApp`, `NotificadorSMS`) de la misma forma.

### 2. Polimorfismo

El **polimorfismo** permite que, a través de una referencia del tipo interfaz (`INotificador`), podamos llamar al método `Enviar` y que, en tiempo de ejecución, se ejecute la implementación correcta según el objeto real:

```csharp
INotificador notificador = new NotificadorEmail();
notificador.Enviar("Hola"); // Llama a NotificadorEmail.Enviar
```

En este proyecto:

- El usuario elige el tipo de notificación.
- `NotificadorSelector` devuelve una implementación diferente de `INotificador` según la opción.
- El código de `Program.cs` **no necesita saber** cómo se envía el mensaje, solo llama a `Enviar`.

Esto mejora la **extensibilidad**: si mañana agregamos `NotificadorPush`, casi no hay que tocar el código existente.

### 3. Arquitectura limpia (separación de responsabilidades)

Aunque es un ejemplo pequeño, se aplican ideas de **arquitectura limpia**:

- Cada archivo tiene una responsabilidad clara:
  - Interfaz (`INotificador`) separada.
  - Cada implementacion en su propio archivo (`NotificadorEmail`, `NotificadorWhatsApp`, `NotificadorSMS`).
  - Lógica de selección en `NotificadorSelector`.
  - Flujo de consola en `Program.cs`.

Esto hace el código más:

- Fácil de leer.
- Fácil de mantener.
- Fácil de extender.

### 4. Comentarios XML (`/// <summary> ... />`)

Todo el código público está documentado usando **comentarios XML** de C#:

- `/// <summary>Descripción...</summary>`
- `/// <param name="mensaje">Descripción del parámetro.</param>`

Beneficios:

- Mejora la **calidad del software**.
- Ayuda a otros desarrolladores (o a ti en el futuro) a entender el propósito de cada clase y método.
- Visual Studio puede mostrar estas descripciones como ayuda emergente (IntelliSense).

## Cómo ejecutar el proyecto

1. Asegúrate de tener instalado **.NET 8 SDK**.
2. Desde la carpeta de la solución, ejecuta:

```bash
cd SistemaNotificaciones
dotnet run
```

3. Sigue las instrucciones en la consola:
   - Elige el tipo de notificación.
   - Escribe el mensaje.

## Posibles extensiones

- Agregar nuevos tipos de notificador (por ejemplo, `NotificadorPush`).
- Validar mejor la entrada del usuario.
- Separar aún más por capas (Domain / Application / Infrastructure) si el ejercicio crece.
