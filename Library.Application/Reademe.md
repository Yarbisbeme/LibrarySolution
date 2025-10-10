# 📚 Library API – Evaluación Técnica
---

## 🧩 Capa: Application

En esta capa se concentrará toda la **lógica de negocio** del proyecto.  
Aquí definiré los **servicios**, **interfaces**, **DTOs** y **mapeos** que permitirán que la API funcione correctamente sin depender directamente del acceso a datos.  

El propósito de esta capa será **mantener la lógica central separada del resto de las capas**, garantizando una arquitectura limpia, modular y fácil de mantener.

---

### 📁 Estructura del proyecto

```powershell
Library.Application/
├── Dtos/
│   ├── AuthorDto.cs
│   ├── BookDto.cs
│   ├── CreateBookRequest.cs
│   ├── LoanDto.cs
│
├── Interfaces/
│   ├── IAuthorService.cs
│   ├── IBookService.cs
│   ├── ILoanService.cs
│
├── Services/
│   ├── AuthorService.cs
│   ├── BookService.cs
│   ├── LoanService.cs
│
└── Mappings/
    ├── AutoMapper.cs
```