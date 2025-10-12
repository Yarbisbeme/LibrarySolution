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
│   ├── IBookService.cs      → Contrato de operaciones para gestión de libros
│   ├── IAuthService.cs      → Contrato de operaciones para autenticacion
│   └── ILoanService.cs      → Contrato de operaciones para gestión de préstamos
│
└── Services/
    ├── BookService.cs       → Implementa la lógica de negocio para libros
    ├── AuthService.cs       → Implementa la lógica de negocio para Autenticacion
    └── LoanService.cs       → Implementa la lógica de negocio para préstamos

  
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
