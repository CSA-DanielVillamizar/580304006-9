# 🚀 Guía de Integración y Despliegue Continuo (CI/CD)

¡Hola equipo! En este proyecto hemos configurado herramientas profesionales del mundo real para automatizar la revisión y publicación de nuestro código. A esto se le conoce como **CI/CD** (Integración Continua y Despliegue Continuo).

## 🤔 ¿Qué es CI/CD?
- **CI (Integración Continua):** Es como un "profesor estricto" o un "detector de mentiras" que revisa tu tarea automáticamente. Cada vez que subes código al repositorio, un robot lo descarga, intenta compilarlo y le pasa todas las pruebas unitarias. Si algo falla, te avisa de inmediato antes de que dañes el proyecto principal.
- **CD (Despliegue Continuo):** Una vez que el código pasó las pruebas en el CI, otro robot toma ese código limpio, lo empaqueta (en nuestro caso, en un contenedor de Docker) y lo envía automáticamente a un almacén público o servidor real.

---

## 🤖 Nuestros Archivos Mágicos (El Cerebro del Robot)
En la carpeta `.github/workflows/` hemos creado dos archivos YAML que le dan las instrucciones paso a paso a los servidores gratuitos de Microsoft (GitHub Actions).

### 1. `ci-pipeline.yml` (El Evaluador)
**¿Cuándo se ejecuta?** Cada vez que hacemos un `push` o enviamos un `pull_request` a las ramas principales (`main` o `develop`).
**¿Qué pasos hace?**
1. Pide un servidor prestado a GitHub (con Ubuntu Linux).
2. Instala la herramienta de **.NET 8**.
3. Descarga nuestro código y restaura los instaladores (Paquetes NuGet).
4. **Compila** el código asegurando que no hay errores de sintaxis (punto y coma faltante, variables mal llamadas, etc).
5. **Ejecuta las pruebas unitarias**. ¡Si una prueba falla (Ej: el camino triste falló), todo el proceso se detiene en rojo (❌)!
6. Como bonus de nivel 5, valida que nuestro `Dockerfile` funcione y el proyecto se pueda containerizar de manera exitosa.

### 2. `cd-pipeline.yml` (El Repartidor a Producción)
**¿Cuándo se ejecuta?** Automáticamente cuando el código es integrado de forma oficial en la rama `main`.
**¿Qué pasos hace?**
1. Descarga la versión final del código.
2. **Inicia sesión en Docker Hub** usando nuestras credenciales de manera segura.
3. Construye la imagen definitiva de la aplicación (la API).
4. Sube (Push) esa imagen a internet a través del portal de **Docker Hub** con la etiqueta `latest`, dejándola lista para ser descargada en cualquier servidor de AWS o Azure en el mundo.

---

## 🔐 ¿Cómo crear los "Secretos" para el Despliegue Continuo?
Para que GitHub Actions pueda publicar la imagen a tu nombre en Docker Hub, necesita tus llaves. ¡Pero NUNCA debemos escribir una contraseña públicamente en el código! Para eso usamos la bóveda fuerte llamada **GitHub Secrets**.

### Paso 1: Generar un Token de Acceso en Docker Hub
1. Inicia sesión en [Docker Hub](https://hub.docker.com/).
2. Ve a perfil en la esquina superior derecha y entra a **Account settings**.
3. En el menú selecciona **Personal access tokens**.
4. Haz clic en **New Access Token**.
5. Ponle una descripción fácil de recordar, como `Permisos-GitHub-Actions`.
7. Al crearlo, te mostrará una contraseña súper larga (empieza por `dckr_pat_...`). **¡Cópiala y guárdala!** Solo te la mostrará una vez.

### Paso 2: Guardar los Secretos en GitHub de manera segura
1. Ve a la página principal de tu repositorio en [GitHub.com].
2. Entra a la pestaña superior llamada **Settings** (Configuración).
3. En el menú de la izquierda, baja hasta la sección **Secrets and variables** y haz clic en **Actions**.
4. Haz clic en el botón verde **New repository secret**.
5. **Crea el primer secreto:**
   - **Name:** `DOCKERHUB_USERNAME`
   - **Secret:** *(Tu nombre de usuario exacto de Docker Hub, en minúsculas sin espacios)*
   - Haz clic en *Add secret*.
6. **Crea el segundo secreto:**
   - **Name:** `DOCKERHUB_TOKEN`
   - **Secret:** *(Pega aquí el Token larguísimo que copiaste de Docker Hub en el Paso 1)*.
   - Haz clic en *Add secret*.

¡Y listo! Al tener esto configurado, GitHub tendrá permiso para conectarse a tu Docker Hub por ti, permitiendo que todas tus futuras actualizaciones de código lleguen en minutos empaquetadas al mundo exterior. 🚀
