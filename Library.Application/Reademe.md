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
│   ├── IAuthorService.cs      → Contrato de operaciones para Gestion de Autores
│   ├── IAuthService.cs        → Contrato de operaciones para autenticacion
│   ├── IBookService.cs        → Contrato de operaciones para gestión de libros
│   └── ILoanService.cs        → Contrato de operaciones para gestión de préstamos
│
└── Services/
    ├── AuthorService.cs       → Implementa la lógica de negocio para los Autores
        ├── GetAllAuthorsAsync → Logica para Obtener todos los autores
        ├── GetAuthorByIdAsync → Logica para Obtener un autor por su Id
        ├── CreateAuthorAsync  → Logica para Crear a los autores
        ├── UpdateAuthorAsync  → Logica para Actualizar informacion de autores
        └── DeleteAuthorAsync  → Logica para autenticacion

    ├── AuthService.cs         → Implementa la lógica de negocio para Autenticacion
        ├── GenerateToken      → Metodo privado para generar los Token
        └── LoginAsync         → Logica para autenticacion

    ├── BookService.cs         → Implementa la lógica de negocio para libros
        ├── ObtenerLibrosAntesDe2000Async → Logica para Obtener prestamos no devueltos
        └── CrearLibroAsync    → Implementa la lógica de negocio para crear libros

    └── LoanService.cs         → Implementa la lógica de negocio para préstamos
        ├── ObtenerPrestamosNoDevueltosAsync → Logica para Obtener prestamos no devueltos
        ├── PostLoan           → Logica para Crear a los prestamos
        ├── UpdateReturnDateAsync  → Logica para Actualizar informacion de prestamos
        └── DeleteLoanAsync    → Logica para autenticacion
  
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
