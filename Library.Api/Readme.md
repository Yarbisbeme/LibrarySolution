# 📚 Library.Api — Documentación Técnica para Desarrolladores

## 🧩 Descripción general

**Library.Api** es la capa de presentación de la solución **LibrarySolution**, desarrollada con **.NET 9**.  
Su función principal es exponer endpoints RESTful para manejar autenticación, autorización y comunicación con las capas internas de la aplicación (Application, Domain, Infrastructure).


---

## 🏗️ Arquitectura y propósito

`Library.Api` sigue el principio de **separación de responsabilidades**, actuando como punto de entrada HTTP.  
Las responsabilidades clave de esta capa son:

- Recibir y validar solicitudes HTTP.
- Llamar a los servicios de aplicación (`IAuthService`, etc.).
- Manejar autenticación JWT y autorización por roles.
- Retornar respuestas estandarizadas mediante `ApiResponse<T>`.
- Proveer documentación automática con Swagger/OpenAPI.

### Estructura del proyecto

```
Library.Api/
│
├── Controllers/
│   ├── AuthController.cs # Controlador de autenticación (login, tokens)
|   |   └── Login(POST) 
|   |
│   ├── AuthorController.cs # Controlador de los autores (login, tokens)
|   |   ├── GetAuthors(Get) 
|   |   ├── GetAuthorsById(Get) 
|   |   ├── CreateAuthor(POST) 
|   |   ├── UpdateAuthor(PUT)
|   |   └── DeleteAuthor(DELETE)
|   |
│   ├── LibrosController.cs # Controlador de libros (Get, post)
|   |   ├── GetBooksByAuthor(Get) 
|   |   ├── ObtenerLibrosAntesDe2000(GET) 
|   |   ├── GetListBooksByTitle(GET) 
|   |   ├── CrearLibros(POST) 
|   |   ├── DeleteBook(Delete) 
|   |   └── UpdateBook(PUT)
|   |
│   └── LoanController.cs # Controlador de prestamos (Get, Put, Delete)
|       ├── CrearPrestamo(POST) 
|       ├── ActualizarDevolucion(PUT)
|       ├── EliminarPrestamo(DELETE)
|       └── ObtenerNoDevueltos(GET)
│
├── Program.cs # Configuración principal del host y servicios
├── appsettings.json # Configuración de JWT, logging, etc.
└── Properties/
    └── launchSettings.json # Configuración de entorno local

```

----
## 🚀 Ejecución local

#### Para ejecutar el proyecto:

**1️⃣ Requisitos previos**

Asegúrate de tener instalado:

- ✅ .NET 9 SDK
👉 https://dotnet.microsoft.com/download

- ✅ SQL Server (Express o Developer)
👉 o también puedes usar Docker con una imagen de SQL Server

- ✅ Visual Studio 2022 o VS Code (opcional, pero recomendado)

**2️⃣ Clonar el repositorio**

```
git clone https://github.com/yarbisbeme/LibrarySolution.git
cd LibrarySolution
```

**3️⃣ Configurar la cadena de conexión**

Abrir el archivo
```
cd Library.Api/appsettings.json
```
y actualizar esta parte según tu entorno local:

```
"ConnectionStrings": {
  "DefaultConnection": "Server=DESKTOP-OJ0R17N\\SQLEXPRESS;Database=LibraryDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```
**4️⃣ Aplicar las migraciones a la base de datos**

Abre la consola en el proyecto `Library.Infrastructure`:
```
cd Library.Infrastructure
dotnet ef database update
```
**5️⃣ Ejecutar la API**

Desde el proyecto Library.Api:
```
cd ../Library.Api
dotnet run
```
Si todo esta correcto vera algo como:
```
Now listening on: http://localhost:5001
Now listening on: https://localhost:7000
```
Una vez iniciada, Swagger se habilita automáticamente en:

```
https://localhost:5001/swagger
```

----
## 🔐 Autenticación y Autorización
**Flujo de autenticación**

El cliente envía sus credenciales a `POST /api/auth/login`.

Si las credenciales son válidas, se genera un `token JWT` con información del usuario (claims).

El cliente incluye ese token en cada petición protegida mediante el encabezado:

```
Authorization: Bearer {token}
```

El middleware de ASP.NET Core valida automáticamente el token y autoriza el acceso.


### 🧰 Respuestas estándar (ApiResponse<T>)

Todas las respuestas de la API utilizan el formato unificado ApiResponse<T>, para garantizar consistencia en las estructuras devueltas.

```
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }

    public static ApiResponse<T> SuccessResponse(T data, string message = "Operación exitosa");
    public static ApiResponse<T> ErrorResponse(string message, List<string>? errors = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Errors = errors ?? new List<string>()
            };
        }
}
```

**Ejemplo:**

```
{
  "success": true,
  "message": "Operación exitosa",
  "data": { ... }
}
```

----
### 🧩 Dependencias

| Paquete                                         | Uso                                           |
| ----------------------------------------------- | --------------------------------------------- |
| `Microsoft.AspNetCore.Authentication.JwtBearer` | Validación de tokens JWT                      |
| `Microsoft.IdentityModel.Tokens`                | Generación y firma de tokens                  |
| `Swashbuckle.AspNetCore`                        | Generación de documentación Swagger           |
| `Microsoft.Extensions.Logging`                  | Registro de logs de aplicación                |
| `Microsoft.Extensions.Configuration`            | Acceso a configuración y variables de entorno |



----
## ⚙️ Middleware de Excepciones Global – `ExceptionMiddleware`

El ExceptionMiddleware es el componente encargado de manejar todas las excepciones globales en la capa API.
Su función principal es interceptar los errores que ocurren durante el procesamiento de las solicitudes HTTP y devolver una respuesta estandarizada en formato JSON utilizando el modelo ApiResponse<T>.

Esto garantiza que el cliente siempre reciba una estructura consistente, sin importar en qué punto del código ocurrió el error.

----
#### 🎯 Objetivos

- Centralizar el manejo de errores en un único punto.

- Evitar duplicar bloques `try/catch` en los controladores o servicios.

- Estandarizar la estructura de las respuestas de error.

- Registrar los errores en los logs con el nivel adecuado (`Warning`, `Error`, etc.).

- Facilitar la depuración y el mantenimiento del sistema.

----
#### 🏗️ Estructura del Middleware

Archivo:
`Library.Api/Middleware/ExceptionMiddleware.cs`

**🔹 Componentes Principales**
**1. `InvokeAsync(HttpContext context)`**

Método principal del middleware.

Ejecuta la siguiente capa del pipeline (_next(context)).

Si ocurre una excepción, la captura y delega su manejo a HandleExceptionAsync.

**2. `HandleExceptionAsync(HttpContext context, string message, HttpStatusCode statusCode)`**

Construye la respuesta HTTP de error:

Define el `ContentType` como `application/json`.

Asigna el código de estado HTTP correspondiente.

Serializa un `ApiResponse<object>` con el mensaje del error.

**3. `ExceptionMiddlewareExtensions`**

Clase estática con un método de extensión:
```
app.UseGlobalExceptionHandler();
```
Permite registrar el middleware fácilmente desde Program.cs.

-----
#### ⚠️ Tipos de Excepciones Manejadas

| Tipo de Excepción           | Código HTTP                 | Descripción breve                                  |
| --------------------------- | --------------------------- | -------------------------------------------------- |
| `KeyNotFoundException`      | `404 Not Found`             | El recurso solicitado no existe.                   |
| `ArgumentException`         | `400 Bad Request`           | Se envió un argumento o parámetro inválido.        |
| `InvalidOperationException` | `500 Internal Server Error` | Error lógico o de flujo interno.                   |
| `DbUpdateException`         | `500 Internal Server Error` | Error al guardar o actualizar en la base de datos. |
| `Exception` *(general)*     | `500 Internal Server Error` | Error inesperado del servidor.                     |

----

### 🧪 Pruebas locales con Swagger o Postman


#### Swagger UI

Accede desde el navegador:
```
https://localhost:5001/swagger
```
#### Postman

Ejemplo de petición login:

```
POST https://localhost:5001/api/auth/login
Content-Type: application/json

{
  "username": "admin",
  "password": "admin123"
}
```

Luego, copia el token recibido y úsalo como encabezado:

```
Authorization: Bearer eyJhbGciOiJIUzI1NiIs...
```


