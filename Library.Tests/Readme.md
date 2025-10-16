# 📋 Documentación de Pruebas Unitarias - Library API

## 📑 Tabla de Contenidos
- [Descripción General](#descripción-general)
- [Tecnologías Utilizadas](#tecnologías-utilizadas)
- [Estructura de Pruebas](#estructura-de-pruebas)
- [Controladores Testeados](#controladores-testeados)
- [Ejecución de Pruebas](#ejecución-de-pruebas)
- [Cobertura de Código](#cobertura-de-código)

---

## 📖 Descripción General

Este proyecto contiene pruebas unitarias completas para todos los controladores de la API de Library. Las pruebas siguen el patrón **AAA (Arrange-Act-Assert)** y utilizan mocks para aislar la lógica de negocio.

### Objetivo
Garantizar que cada endpoint de la API funcione correctamente bajo diferentes escenarios: casos exitosos, errores esperados y excepciones.

---

## 🛠️ Tecnologías Utilizadas

| Tecnología | Versión | Propósito |
|------------|---------|-----------|
| **xUnit** | Latest | Framework de pruebas principal |
| **Moq** | Latest | Creación de objetos mock |
| **FluentAssertions** | Latest | Aserciones expresivas y legibles |
| **Microsoft.NET.Test.Sdk** | Latest | Ejecutor de pruebas |

### Instalación de Paquetes
```bash
dotnet add package xunit
dotnet add package xunit.runner.visualstudio
dotnet add package Moq
dotnet add package FluentAssertions
dotnet add package Microsoft.NET.Test.Sdk
```

---

## 📂 Estructura de Pruebas

```
Library.Tests/
│
├── Controllers/
│   ├── AuthControllerTests.cs
│   ├── PrestamosTests.cs
│   ├── AuthorControllerTests.cs
│   └── LibrosControllerTests.cs
│
└── Library.Tests.csproj
```

---

## 🎯 Controladores Testeados

### 1. **AuthControllerTests** 🔐

Valida la funcionalidad de autenticación y autorización.

#### Casos de Prueba

| Método de Prueba | Escenario | Resultado Esperado | Cobertura |
|------------------|-----------|-------------------|-----------|
| `Login_ShouldReturnOk_WhenCredentialsAreValid` | Login con credenciales válidas | Status 200 OK con token JWT | ✅ Exitoso |
| `Login_ShouldReturnBadRequest_WhenCredentialsAreInvalid` | Login con credenciales incorrectas | Status 400 BadRequest | ⚠️ Error esperado |
| `Login_ShouldThrowException_WhenServiceFails` | Fallo inesperado en el servicio | Excepción lanzada | ❌ Error del sistema |

#### Ejemplo de Implementación
```csharp
[Fact]
public async Task Login_ShouldReturnOk_WhenCredentialsAreValid()
{
    // Arrange: Configurar credenciales válidas y mock
    var loginDto = new LoginDto { Username = "admin", Password = "12345" };
    var loginResponse = new LoginResponseDto 
    { 
        Token = "fake-jwt-token", 
        Username = "admin", 
        Role = "admin" 
    };
    
    // Act: Ejecutar login
    var result = await _controller.Login(loginDto);
    
    // Assert: Verificar respuesta exitosa
    result.Should().BeOfType<OkObjectResult>();
}
```

---

### 2. **PrestamosTests** 📚

Valida la gestión de préstamos de libros.

#### Casos de Prueba

| Endpoint | Método de Prueba | Escenario | HTTP Status |
|----------|------------------|-----------|-------------|
| `POST /api/prestamos` | `CrearPrestamo_CuandoExitoso_DeberiaRetornarOk` | Crear préstamo exitoso | 200 OK |
| `POST /api/prestamos` | `CrearPrestamo_CuandoLibroNoExiste_DeberiaLanzarKeyNotFound` | Libro no encontrado | KeyNotFoundException |
| `POST /api/prestamos` | `CrearPrestamo_CuandoExcepcion_DeberiaLanzarExcepcion` | Error interno | Exception |
| `GET /api/prestamos/no-devueltos` | `ObtenerNoDevueltos_CuandoExisten_DeberiaRetornarOk` | Obtener préstamos pendientes | 200 OK |
| `GET /api/prestamos/no-devueltos` | `ObtenerNoDevueltos_CuandoExcepcion_DeberiaLanzarExcepcion` | Error de base de datos | Exception |
| `PUT /api/prestamos/{id}` | `ActualizarDevolucion_CuandoExitoso_DeberiaRetornarOk` | Actualizar fecha de devolución | 200 OK |
| `PUT /api/prestamos/{id}` | `ActualizarDevolucion_CuandoNoExiste_DeberiaLanzarKeyNotFound` | Préstamo no encontrado | KeyNotFoundException |
| `DELETE /api/prestamos/{id}` | `EliminarPrestamo_CuandoExitoso_DeberiaRetornarOk` | Eliminar préstamo | 200 OK |
| `DELETE /api/prestamos/{id}` | `EliminarPrestamo_CuandoNoExiste_DeberiaLanzarKeyNotFound` | Préstamo no encontrado | KeyNotFoundException |

#### Datos de Prueba
```csharp
// Ejemplo: Crear un préstamo
var dto = new PostPrestamoDto 
{ 
    BookId = 1, 
    Devolucion_Prestamo = DateTime.UtcNow.AddDays(7) 
};

var response = new LoanResponse
{
    LibroId = 1,
    Titulo = "El Principito",
    Autor = "Antoine de Saint-Exupéry",
    Fecha_Prestamo = DateTime.UtcNow,
    Fecha_Devolucion = dto.Devolucion_Prestamo
};
```

---

### 3. **AuthorControllerTests** ✍️

Valida las operaciones CRUD de autores.

#### Casos de Prueba

| Endpoint | Método de Prueba | Descripción |
|----------|------------------|-------------|
| `GET /api/author/GetAuthors` | `GetAuthors_ReturnsOkResult_WithListOfAuthors` | Obtiene todos los autores |
| `GET /api/author/GetAuthors` | `GetAuthors_ReturnsOkResult_WithEmptyList_WhenNoAuthors` | Lista vacía cuando no hay autores |
| `GET /api/author/GetAuthorById/{id}` | `GetAuthorById_ReturnsOkResult_WithAuthor_WhenAuthorExists` | Obtiene un autor específico |
| `POST /api/author/CreateAuthor` | `CreateAuthor_ReturnsCreatedAtAction_WithNewAuthor` | Crea un nuevo autor |
| `PUT /api/author/UpdateAuthor/{id}` | `UpdateAuthor_ReturnsOkResult_WithUpdatedAuthor` | Actualiza un autor existente |
| `DELETE /api/author/DeleteAuthor/{id}` | `DeleteAuthor_ReturnsOkResult_WithTrue_WhenDeleteSuccessful` | Elimina un autor exitosamente |
| `DELETE /api/author/DeleteAuthor/{id}` | `DeleteAuthor_ReturnsOkResult_WithFalse_WhenDeleteFails` | Falla al eliminar (autor no existe) |

---

### 4. **LibrosControllerTests** 📖

Valida las operaciones de gestión de libros.

#### Casos de Prueba

| Endpoint | Método de Prueba | Descripción |
|----------|------------------|-------------|
| `GET /api/libros/antes-de-2000` | `ObtenerLibrosAntesDe2000_ReturnsOkResult_WithListOfBooks` | Obtiene libros publicados antes del 2000 |
| `GET /api/libros/antes-de-2000` | `ObtenerLibrosAntesDe2000_ReturnsEmptyList_WhenNoBooksFound` | Lista vacía cuando no hay resultados |
| `GET /api/libros/GetBooksByAuthor` | `GetBooksByAuthor_ReturnsOkResult_WithBooksByAuthor` | Busca libros por autor |
| `GET /api/libros/GetBooksByAuthor` | `GetBooksByAuthor_ReturnsEmptyList_WhenAuthorNotFound` | Autor no encontrado |
| `GET /api/libros/GetBooksByTitle` | `GetListBooksByTitle_ReturnsOkResult_WithBooks` | Busca libros por título |
| `POST /api/libros/PostBook` | `CrearLibro_ReturnsCreatedAtAction_WithNewBook` | Crea un nuevo libro |
| `POST /api/libros/PostBook` | `CrearLibro_ThrowsException_WhenModelStateInvalid` | Validación de ModelState |
| `DELETE /api/libros/DeleteBook` | `DeleteBook_ReturnsOkResult_WithTrue_WhenDeleteSuccessful` | Elimina un libro exitosamente |
| `DELETE /api/libros/DeleteBook` | `DeleteBook_ReturnsOkResult_WithFalse_WhenBookNotFound` | Libro no encontrado |
| `PUT /api/libros/UpdateBook` | `UpdateBook_ReturnsOkResult_WithUpdatedBook` | Actualiza un libro |
| `PUT /api/libros/UpdateBook` | `UpdateBook_MapsPropertiesCorrectly` | Verifica mapeo correcto de propiedades |

---

## ▶️ Ejecución de Pruebas

### Ejecutar todas las pruebas
```bash
dotnet test
```

### Ejecutar pruebas con output detallado
```bash
dotnet test --verbosity detailed
```

### Ejecutar pruebas de un controlador específico
```bash
dotnet test --filter "FullyQualifiedName~AuthControllerTests"
dotnet test --filter "FullyQualifiedName~PrestamosTests"
dotnet test --filter "FullyQualifiedName~AuthorControllerTests"
dotnet test --filter "FullyQualifiedName~LibrosControllerTests"
```

### Ejecutar una prueba específica
```bash
dotnet test --filter "FullyQualifiedName~Login_ShouldReturnOk_WhenCredentialsAreValid"
```

---

## 📊 Cobertura de Código

### Generar reporte de cobertura
```bash
dotnet test --collect:"XPlat Code Coverage"
```

### Instalar herramienta de reportes
```bash
dotnet tool install -g dotnet-reportgenerator-globaltool
```

### Generar reporte HTML
```bash
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html
```

### Estadísticas Actuales

| Controlador | Pruebas | Cobertura Estimada |
|-------------|---------|-------------------|
| AuthController | 3 | ~95% |
| PrestamosController | 9 | ~90% |
| AuthorController | 7 | ~95% |
| LibrosController | 11 | ~90% |
| **Total** | **30** | **~92%** |

---

## 🎨 Patrón de Pruebas

### Patrón AAA (Arrange-Act-Assert)

Todas las pruebas siguen este patrón estándar:

```csharp
[Fact]
public async Task NombrePrueba()
{
    // Arrange: Configurar datos y mocks
    var dto = new ExampleDto { ... };
    _mockService.Setup(s => s.Method(dto)).ReturnsAsync(result);
    
    // Act: Ejecutar el método a probar
    var result = await _controller.Method(dto);
    
    // Assert: Verificar el resultado
    result.Should().BeOfType<OkObjectResult>();
    _mockService.Verify(s => s.Method(dto), Times.Once);
}
```

---

## 📝 Convenciones de Nomenclatura

### Nomenclatura de Métodos de Prueba
```
[MethodName]_[Should|When]_[ExpectedBehavior]_[StateUnderTest]
```

#### Ejemplos:
- ✅ `Login_ShouldReturnOk_WhenCredentialsAreValid`
- ✅ `CrearPrestamo_CuandoExitoso_DeberiaRetornarOk`
- ✅ `GetAuthors_ReturnsOkResult_WithListOfAuthors`

---

## 🔍 Verificaciones Comunes

### 1. Verificar tipo de resultado
```csharp
result.Should().BeOfType<OkObjectResult>();
```

### 2. Verificar ApiResponse
```csharp
var response = okResult.Value as ApiResponse<T>;
response.Success.Should().BeTrue();
response.Data.Should().NotBeNull();
```

### 3. Verificar llamadas al servicio
```csharp
_mockService.Verify(s => s.Method(It.IsAny<T>()), Times.Once);
```

### 4. Verificar excepciones
```csharp
await Assert.ThrowsAsync<KeyNotFoundException>(() => _controller.Method(dto));
```

---

## 🚀 Mejores Prácticas

### ✅ DO (Hacer)
- Usar nombres descriptivos para las pruebas
- Seguir el patrón AAA
- Mantener las pruebas simples y enfocadas
- Usar mocks para dependencias externas
- Verificar tanto éxito como fallos
- Probar casos límite

### ❌ DON'T (No Hacer)
- No probar múltiples escenarios en una sola prueba
- No usar datos reales de base de datos
- No ignorar excepciones esperadas
- No duplicar lógica de pruebas
- No usar Thread.Sleep en pruebas asíncronas

---

## 📚 Recursos Adicionales

- [xUnit Documentation](https://xunit.net/)
- [Moq Documentation](https://github.com/moq/moq4)
- [FluentAssertions Documentation](https://fluentassertions.com/)
- [ASP.NET Core Testing](https://docs.microsoft.com/en-us/aspnet/core/test/)

---

## 👥 Contribuciones

Para agregar nuevas pruebas:

1. Crear una nueva clase de prueba en `Library.Tests/Controllers/`
2. Seguir el patrón AAA
3. Usar nomenclatura consistente
4. Documentar casos de prueba complejos
5. Ejecutar todas las pruebas antes de commit

---

