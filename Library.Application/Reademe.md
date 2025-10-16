# 📘 Library API – Evaluación Técnica  
## 🧠 Capa: Application

---

### 🧩 Descripción General

La capa **`Application`** es el **núcleo de la lógica de negocio** del proyecto.  
Aquí se definen los **servicios**, **interfaces** y **casos de uso** que conectan la API con la capa de infraestructura, aplicando las reglas del dominio y la coordinación de procesos.

El propósito principal de esta capa es **mantener la lógica central separada del resto de las capas**, garantizando una arquitectura **limpia, modular y escalable**.

---

### 🎯 Objetivos de la Capa

- ✅ Centralizar toda la **lógica de negocio** del sistema.  
- ✅ Definir las **interfaces de servicios** (`IBookService`, `ILoanService`) para mantener independencia entre la API y la capa de datos.  
- ✅ Implementar servicios concretos que gestionan entidades como **Libros** y **Préstamos**.  
- ✅ Aplicar principios de **Inyección de Dependencias (DI)** para que la capa de infraestructura proporcione las implementaciones concretas de los repositorios.  

---

### 🏗️ Estructura del Proyecto

```plaintext
Library.Application/
│
├── Interfaces/
│   ├── IAuthorService.cs   → Contrato para la gestión de autores
│   ├── IAuthService.cs     → Contrato para la autenticación de usuarios
│   ├── IBookService.cs     → Contrato para la gestión de libros
│   └── ILoanService.cs     → Contrato para la gestión de préstamos
│
└── Services/
    ├── AuthorService.cs     → Implementa la lógica de negocio de los autores
    │   ├── GetAllAuthorsAsync   → Obtiene todos los autores
    │   ├── GetAuthorByIdAsync   → Obtiene un autor por su ID
    │   ├── CreateAuthorAsync    → Crea un nuevo autor
    │   ├── UpdateAuthorAsync    → Actualiza los datos de un autor existente
    │   └── DeleteAuthorAsync    → Elimina un autor del sistema
    │
    ├── AuthService.cs       → Implementa la lógica de negocio para autenticación
    │   ├── LoginAsync        → Autentica un usuario y genera su token JWT
    │   └── GenerateToken     → Método privado para generar el token
    │
    ├── BookService.cs       → Implementa la lógica de negocio de libros
    │   ├── ObtenerLibrosAntesDe2000Async → Obtiene libros publicados antes del año 2000
    │   ├── ObtenerLibrosPorAutorAsync    → Obtiene libros por autor
    │   ├── ObtenerLibroPorTituloAsync    → Busca libros por título
    │   ├── CrearLibroAsync               → Crea un nuevo libro
    │   ├── ActualizarLibroAsync          → Actualiza la información de un libro
    │   └── EliminarLibroAsync            → Elimina un libro del catálogo
    │
    └── LoanService.cs       → Implementa la lógica de negocio para préstamos
        ├── PostLoan                 → Registra un nuevo préstamo
        ├── ObtenerPrestamosNoDevueltosAsync → Obtiene préstamos no devueltos
        ├── UpdateReturnDateAsync    → Actualiza la fecha de devolución
        └── DeleteLoanAsync          → Elimina un préstamo

  
```

----
### 🧱 Principios aplicados
---
| Principio                          | Descripción                                                                                                    |
| ---------------------------------- | -------------------------------------------------------------------------------------------------------------- |
| **Clean Architecture**             | Cada capa tiene una responsabilidad única.                                                                     |
| **Dependency Inversion**           | La API depende de las interfaces, no de las implementaciones.                                                  |
| **Separation of Concerns**         | La lógica de negocio está aislada de la capa de presentación e infraestructura.                                |
| **Single Responsibility**          | Cada servicio se encarga de un único conjunto de operaciones.                                                  |
| **Inyección de dependencias (DI)** | Las dependencias (configuración, loggers) se inyectan automáticamente por el contenedor de .NET. |

### 🔗 Referencias entre Capas

```
  <ItemGroup>
    <ProjectReference Include="..\Library.Infrastructure\Library.Infrastructure.csproj" />
    <ProjectReference Include="..\Library.Domain\Library.Domain.csproj" />
    <ProjectReference Include="..\library.common\Library.Common.csproj" />
  </ItemGroup>
```

**- `library.infrasrtucture:`** Acceso al contexto
**- `Library.Domain:`** creacion de nuevos objetos (libros, prestamos, autores)
**- `Library.Common:`** Acceso a los Data Transfer Object (DTO).

### 💡 Buenas Prácticas Implementadas

- Uso de `async/await` en todos los servicios para operaciones asíncronas.

- Retorno de `DTOs` en lugar de entidades del dominio, protegiendo la integridad del modelo.

- Implementación consistente de excepciones controladas, delegando su manejo al `middleware global`.

- Lógica de negocio desacoplada de los `Controllers` y la `persistencia (Interfaces)`.

- Código fácilmente testeable, con servicios que pueden ser mockeados en `unit test`.