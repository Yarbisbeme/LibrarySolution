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
│   ├── Libros/
│   │   ├── LibrosController_PostTests.cs   → Pruebas del endpoint POST /libros
│   │   └── LibrosController_Test.cs        → Pruebas del endpoint GET /libros/antes-de-2000
│   │
│   └── Prestamos/
│       └── PrestamosTests.cs               → Pruebas del endpoint GET /prestamos/no-devueltos
│
└── Library.Tests.csproj                    → Configuración del proyecto de pruebas

```
-----
### 🧰 Herramientas


| Herramienta                                | Uso                                             |
| ------------------------------------------ | ----------------------------------------------- |
| **xUnit**                                  | Framework principal de testing.                 |
| **Moq**                                    | Simulación de dependencias (interfaces).        |
| **Microsoft.EntityFrameworkCore.InMemory** | Base de datos en memoria para pruebas aisladas. |


-------
### Las pruebas implementadas

----------
### LibrosController

| Caso                                       | Descripción                                       | Resultado Esperado                     |
| ------------------------------------------ | ------------------------------------------------- | -------------------------------------- |
| `ObtenerLibrosAntesDe2000_CuandoExisten`   | Retorna los libros publicados antes del año 2000. | `200 OK` con lista de libros.          |
| `ObtenerLibrosAntesDe2000_CuandoNoExisten` | No hay libros previos a 2000.                     | `200 OK` con lista vacía.              |
| `CrearLibro_CuandoDatosValidos`            | Se crea correctamente un libro nuevo.             | `201 Created` con el objeto creado.    |
| `CrearLibro_CuandoAutorNoExiste`           | Autor no válido.                                  | `400 BadRequest` con mensaje de error. |
| `CrearLibro_CuandoTituloInvalido`          | Modelo no válido.                                 | `400 BadRequest`.                      |

------------
### PrestamosController
| Caso                                    | Descripción                                  | Resultado Esperado               |
| --------------------------------------- | -------------------------------------------- | -------------------------------- |
| `ObtenerNoDevueltos_CuandoExisten`      | Devuelve préstamos pendientes de devolución. | `200 OK` con lista de préstamos. |
| `ObtenerNoDevueltos_CuandoNoExisten`    | No hay préstamos pendientes.                 | `200 OK` con lista vacía.        |
| `ActualizarDevolucion_CuandoIdNoExiste` | No se encuentra el préstamo.                 | `404 NotFound`.                  |
| `EliminarPrestamo_CuandoExito`          | Se elimina correctamente.                    | `200 OK`.                        |


### 💡 Estrategia de pruebas

Aislamiento total:
Cada prueba utiliza Moq para simular la capa Application (IBookService, ILoanService),
evitando acceso real a la base de datos.

AAA Pattern (Arrange – Act – Assert):
Cada método de prueba sigue esta estructura estándar:

```
// Arrange: preparar datos o mocks
// Act: ejecutar la acción del controlador
// Assert: verificar el resultado esperado
```

Uso de InMemoryDatabase:
Para pruebas que requieren datos persistentes sin afectar la BD real:

```
options.UseInMemoryDatabase("LibraryTestDB");
```

