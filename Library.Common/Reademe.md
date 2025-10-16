# 📘 Capa: **Library.Common**

> **Proyecto:** Library API – Evaluación Técnica  
> **Propósito:** Centralizar los *Data Transfer Objects (Dto)* y respuestas comunes utilizadas por el resto de capas.

---

## 🧩 Descripción General

Esta capa basicamente contiene todos los objetos compartidos y modelos de datos simples (Dto) para la comunicacion entre las capas.

Su objetivo principal es **mantener la lógica de transporte de datos desacoplada** del dominio, asegurando que las otras capas puedan intercambiar información sin depender de implementaciones internas o entidades de base de datos.

---

## 🎯 Objetivos de la Capa

- ✅ **Estandarizar las respuestas HTTP.**  
  Mediante la clase `ApiResponse<T>` se asegura que todos los controladores sigan un formato de respuesta uniforme.

- ✅ **Evitar dependencias cruzadas.**  
  Las capas superiores (como API o Application) solo referencian esta capa sin conocer los detalles de persistencia o dominio. Lo cual es excelente.

---

## 🧠 Responsabilidad Principal

Esta capa **no contiene lógica de negocio ni acceso a datos**, solo:
- Estructuras de datos planas (Dto).
- Modelos de respuesta comunes.
- Objetos usados en validaciones o transporte de información entre capas.

---

## 🗂️ Estructura del Proyecto

```plaintext
Library.Common/
└── Dto/
    ├── Auth/
    │   ├── LoginDto.cs              → Modelo de credenciales de acceso (usuario/contraseña)
    │   └── LoginResponse.cs         → Respuesta del servicio de autenticación (token JWT, roles)
    │
    ├── Autores/
    │   ├── AuthorDto.cs             → Datos proporcionados por el usuario para crear/actualizar autores
    │   ├── CreateAuthorDto.cs       → DTO para creación de autores
    │   ├── UpdateAuthorDto.cs       → DTO para actualización de autores
    │   └── AuthorResponse.cs        → Representación del autor devuelta al cliente
    │
    ├── Libros/
    │   ├── LibroDto.cs              → DTO base para la creación y actualización de libros
    │   └── LibroResponseDto.cs      → Modelo devuelto en las respuestas de libros
    │
    └── Prestamos/
        ├── PostPrestamoDto.cs       → DTO para registrar un nuevo préstamo
        ├── ActualizarDevolucionDto.cs → DTO para actualizar la fecha de devolución
        ├── PrestamosNoDevueltosDto.cs → DTO para listar préstamos pendientes
        └── LoanResponse.cs          → Representación

```

### 🔗 Relaciones con Otras Capas

| Capa                       | Dependencia                                                                                |
| -------------------------- | ------------------------------------------------------------------------------------------ |
| **Library.Api**            | Usa los DTOs y `ApiResponse<T>` para construir y devolver respuestas HTTP.                 |
| **Library.Application**    | Utiliza los DTOs como contratos entre la lógica de negocio y la API.                       |
| **Library.Infrastructure** | No depende directamente de esta capa, pero puede mapear entidades a DTOs en algunos casos. |


### 💡 Buenas Prácticas Implementadas

- Todos los modelos siguen la convención PascalCase.

- Los DTOs son inmutables o de propósito claro (crear, actualizar, respuesta).

- Se emplea una estructura uniforme de respuesta (ApiResponse<T>) en toda la solución.

- Las propiedades de los DTOs evitan exponer información sensible o interna.

- Documentación clara y consistente para facilitar el mantenimiento.