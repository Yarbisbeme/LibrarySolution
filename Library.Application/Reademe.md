# 📚 Library API – Evaluación Técnica (.NET 9)
------

## 🧩 Capa – Application

Esta capa contiene la lógica de negocio y abstrae el acceso a datos a través de los servicios.




### 📁 Estructura

Library.Application/
├─ Dtos/
│ ├─ AuthorDto.cs
│ ├─ BookDto.cs
│ ├─ CreateBookRequest.cs
│ ├─ LoanDto.cs
├─ Interfaces/
│ ├─ IAuthorService.cs
│ ├─ IBookService.cs
│ ├─ ILoanService.cs
├─ Services/
│ ├─ AuthorService.cs
│ ├─ BookService.cs
│ ├─ LoanService.cs
└─ Mappings/
│ ├─ AutoMapper.cs
└─ BaseEntity.cs

-------------

### Elementos de nuestra Application
- **DTOs:** Son las estructuras de datos para la comunicacion con nuestra API.
- **Interfaces:** Definen nuestros contratos para los servicios.
- **Servicios:** implementan los casos de uso (libros, autores, préstamos).
- **AutoMapper:** convierte entidades ↔ DTOs automáticamente.
