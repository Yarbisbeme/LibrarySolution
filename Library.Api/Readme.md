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
|     └── Login(POST) 
│   ├── LibrosController.cs # Controlador de libros (Get, post)
|     ├── CrearLibro(POST) 
|     └── ObtenerLibrosAntesDe2000(GET) 
│   ├── LoanController.cs # Controlador de prestamos (Get, Put, Delete)
|     ├── ActualizarDevolucion(PUT)
|     ├── EliminarPrestamo(DELETE)
|     └── ObtenerNoDevueltos(GET)
│
├── Program.cs # Configuración principal del host y servicios
├── appsettings.json # Configuración de JWT, logging, etc.
└── Properties/
    └── launchSettings.json # Configuración de entorno local

```

----
## 🚀 Ejecución local

**Para ejecutar el proyecto:**

```
dotnet build
dotnet run --project Library.Api
```

Por defecto, la API se levantará en:

```
https://localhost:5001
http://localhost:5000
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


