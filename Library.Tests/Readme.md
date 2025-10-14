# 🧪 Library API – Evaluación Técnica  
## 🧱 Capa 5: Tests (Pruebas Unitarias)

---

### 🧩 Descripción General  

La capa **Library.Tests** contiene todas las **pruebas unitarias** del sistema,  
construidas utilizando el framework **xUnit** junto con **Moq** para la creación de servicios simulados (*mock services*).

Estas pruebas verifican la **correcta funcionalidad de los controladores y servicios** de la API,  
asegurando que las respuestas sean las esperadas en distintos escenarios (éxito, error, validación, etc.).

---

### 🎯 Objetivo de la capa  

El objetivo principal de esta capa es **garantizar la calidad del código** y **detectar errores tempranamente**  
antes de llegar a producción.

A través de estas pruebas se valida que:
- Los controladores devuelvan los **códigos de estado correctos** (`200`, `201`, `400`, `404`, `500`).
- Las operaciones funcionen correctamente incluso con datos vacíos o inválidos.
- Las dependencias sean **inyectadas y controladas** adecuadamente mediante mocks.
- No existan efectos secundarios en operaciones críticas (como creación o eliminación de datos).

---

### 🧱 Estructura del proyecto  

```plaintext
Library.Tests/
│
├── Controllers/
│   ├── LibrosTest.cs        → Pruebas del controlador
|   │   ├── CrearLibro_RetornaCreated_WhenLibroCreado()
|   │   ├── CrearLibro_CuandoAutorNoExiste_RetornaNotFound()
|   │   ├── CrearLibro_CuandoOcurreErrorInterno_RetornaInternalServerError()
|   │   ├── ObtenerLibrosAntesDe2000_RetornaOkConLista()
│   │   └── ObtenerLibrosAntesDe2000_CuandoOcurreErrorInterno_LanzaExcepcion()
|   |
│   ├── AuthorsTest.cs
│   |   ├── GetAuthors_ShouldReturnOk_WithListOfAuthors()
|   │   ├── GetAuthorById_ShouldReturnOk_WhenAuthorExists()
|   │   ├── CreateAuthor_ShouldReturnCreated_WhenAuthorIsCreated()
|   │   ├── UpdateAuthor_ShouldReturnOk_WhenAuthorUpdated()
│   │   └── ObtenerLibrosAntesDe2000_CuandoOcurreErrorInterno_LanzaExcepcion()
|   |
│   ├── AutTest.cs
│   |   ├── Login_ShouldReturnOk_WhenCredentialsAreValid()
|   │   ├── Login_ShouldReturnBadRequest_WhenCredentialsAreInvalid()
|   │   └── Login_ShouldThrowException_WhenServiceFails()
|   |
│   └── PrestamosTests.cs
│       ├── CrearPrestamo_CuandoExitoso_DeberiaRetornarOk()
│       ├── CrearPrestamo_CuandoLibroNoExiste_DeberiaLanzarKeyNotFound()
|       ├── CrearPrestamo_CuandoExcepcion_DeberiaLanzarExcepcion()
|       ├── ObtenerNoDevueltos_CuandoExisten_DeberiaRetornarOk()
|       ├── ObtenerNoDevueltos_CuandoExcepcion_DeberiaLanzarExcepcion()
|       ├── ActualizarDevolucion_CuandoExitoso_DeberiaRetornarOk()
|       ├── ActualizarDevolucion_CuandoNoExiste_DeberiaLanzarKeyNotFound()
|       ├── EliminarPrestamo_CuandoExitoso_DeberiaRetornarOk()
│       └── EliminarPrestamo_CuandoNoExiste_DeberiaLanzarKeyNotFound()
│
├── Middleware/
|
└── Library.Tests.csproj                    → Configuración del proyecto de pruebas

```
-----
### 🧰 Herramientas


| Herramienta                                | Uso                                             |
| ------------------------------------------ | ----------------------------------------------- |
| **xUnit**                                  | Framework principal de testing.                 |
| **FluentAssertions**                                  | escribir aserciones más legibles, expresivas y claras                |
| **Moq**                                    | Simulación de dependencias (interfaces).        |
| **Microsoft.EntityFrameworkCore.InMemory** | Base de datos en memoria para pruebas aisladas. |


-------
### 💡 Estrategia de pruebas

#### 🧱 Aislamiento total

Cada prueba se ejecuta de manera independiente, sin interacción con otras.
Los servicios que dependen de DbContext o APIs externas son mockeados para garantizar un entorno controlado.

#### 🧩 Patrón AAA (Arrange – Act – Assert)

Todas las pruebas siguen la estructura estándar de diseño de tests:
```
// Arrange: Preparar entorno, datos y mocks necesarios
// Act: Ejecutar el método que se desea probar
// Assert: Validar que el resultado sea el esperado
```

#### 🧠 Uso de Base de Datos en Memoria

En las pruebas de la capa Service, se usa UseInMemoryDatabase("LibraryTestDB")
para ejecutar operaciones CRUD reales sobre una base temporal sin afectar datos de producción.

# 🧾 Pruebas Implementadas

----------
### LibrosTests

| Caso de prueba                                | Descripción                                   | Resultado esperado                          |
| --------------------------------------------- | --------------------------------------------- | ------------------------------------------- |
| `CrearLibro_CuandoDatosValidos`               | Crea un nuevo libro correctamente.            | `201 Created` con el libro creado.          |
| `CrearLibro_CuandoAutorNoExiste`              | Simula autor inexistente.                     | `404 NotFound`.                             |
| `CrearLibro_CuandoTituloInvalido`             | Datos inválidos.                              | `400 BadRequest`.                           |
| `ObtenerLibrosAntesDe2000_CuandoExisten`      | Retorna libros publicados antes del año 2000. | `200 OK` con lista de libros.               |
| `ObtenerLibrosAntesDe2000_CuandoNoExisten`    | No hay libros antiguos.                       | `200 OK` con lista vacía.                   |
| `ObtenerLibrosAntesDe2000_CuandoErrorInterno` | Falla en la capa de datos.                    | Lanza excepción (capturada por middleware). |

------------
### PrestamosTests

| Caso de prueba                        | Descripción                       | Resultado esperado             |
| ------------------------------------- | --------------------------------- | ------------------------------ |
| `CrearPrestamo_CuandoExitoso`         | Crea un préstamo correctamente.   | `200 OK` con el objeto creado. |
| `CrearPrestamo_CuandoLibroNoExiste`   | Libro no encontrado.              | `404 NotFound`.                |
| `CrearPrestamo_CuandoExcepcion`       | Falla en la creación.             | `500 InternalServerError`.     |
| `ActualizarDevolucion_CuandoExitoso`  | Actualiza la fecha de devolución. | `200 OK`.                      |
| `ActualizarDevolucion_CuandoNoExiste` | No se encuentra el préstamo.      | `404 NotFound`.                |
| `EliminarPrestamo_CuandoExitoso`      | Se elimina correctamente.         | `200 OK`.                      |
| `EliminarPrestamo_CuandoNoExiste`     | No existe el préstamo.            | `404 NotFound`.                |
| `ObtenerNoDevueltos_CuandoExisten`    | Retorna préstamos activos.        | `200 OK` con lista.            |
| `ObtenerNoDevueltos_CuandoExcepcion`  | Error en la base de datos.        | `500 InternalServerError`.     |

---
### AuthorsTest

| Caso                                                   | Descripción                   | Resultado esperado   |
| ------------------------------------------------------ | ----------------------------- | -------------------- |
| `GetAuthors_ShouldReturnOk_WithListOfAuthors`          | Lista de autores.             | `200 OK`.            |
| `GetAuthorById_ShouldReturnOk_WhenAuthorExists`        | Autor existente.              | `200 OK` con objeto. |
| `CreateAuthor_ShouldReturnCreated_WhenAuthorIsCreated` | Crea un nuevo autor.          | `201 Created`.       |
| `UpdateAuthor_ShouldReturnOk_WhenAuthorUpdated`        | Actualiza un autor existente. | `200 OK`.            |
| `DeleteAuthor_ShouldReturnOk_WhenAuthorDeleted`        | Elimina un autor.             | `200 OK`.            |

---
### AuthTests

| Caso                                                     | Descripción                 | Resultado esperado         |
| -------------------------------------------------------- | --------------------------- | -------------------------- |
| `Login_ShouldReturnOk_WhenCredentialsAreValid`           | Credenciales válidas.       | `200 OK` con token JWT.    |
| `Login_ShouldReturnBadRequest_WhenCredentialsAreInvalid` | Credenciales incorrectas.   | `400 BadRequest`.          |
| `Login_ShouldThrowException_WhenServiceFails`            | Error interno del servicio. | `500 InternalServerError`. |

### 🧩 Middleware Tests

En esta sección se validan los comportamientos del middleware global de manejo de errores.
Se simula la ejecución de controladores que lanzan excepciones para verificar que el middleware capture y retorne una respuesta estandarizada (ApiResponse<object>) con el código HTTP correcto.

| Caso                                              | Descripción                                    | Resultado esperado                              |
| ------------------------------------------------- | ---------------------------------------------- | ----------------------------------------------- |
| `ExceptionMiddleware_CapturaExcepcionGeneral`     | Simula un error no controlado en pipeline.     | `500 InternalServerError` con mensaje genérico. |
| `ExceptionMiddleware_CapturaKeyNotFound`          | Simula error de recurso no encontrado.         | `404 NotFound` con mensaje.                     |
| `ExceptionMiddleware_CapturaBadRequest`           | Simula error de validación o entrada inválida. | `400 BadRequest` con detalle del error.         |
| `ExceptionMiddleware_ContinuaEjecucionSinErrores` | Flujo sin excepción.                           | Llama correctamente al siguiente middleware.    |
