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
│   └── ILoanService.cs      → Contrato de operaciones para gestión de préstamos
│
└── Services/
    ├── BookService.cs       → Implementa la lógica de negocio para libros
    └── LoanService.cs       → Implementa la lógica de negocio para préstamos

  
```