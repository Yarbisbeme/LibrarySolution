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
│
├── Dto/
|   ├── Auth/
│       ├── LoginDto.cs            → Credenciales de autenticación (login)
│       └── LoginResponse.cs        → Token JWT devuelto al iniciar sesión
|
|   ├── Autores/
|       ├── AuthorDto                  → Dto para representar los datos dados por el usuario
│       ├── CreateAuthor.cs            → Datos de la tabla
|       └── AuthorResponse.cs          → Representacion de los autores
|
|   ├── Libros/
│       ├── LibroDto.cs          → Datos requeridos para crear un libro
│       ├── LibroResponseDto.cs        → Representación del libro en respuestas
|       ├──
|       └──
|
|   ├── Prestamos/
│       ├── PostPrestamoDto.cs         → Datos para crear un préstamo
│       ├── ActualizarDevolucionDto.cs → DTO para actualizar fecha de devolución
│       ├── PrestamosNoDevueltos.cs    → Dto para los Prestamos sin devolver
|       └── LoanResponse               → Representación de préstamos activos
│   
|   └── ApiResponse.cs                 → Estructura estándar de respuesta HTTP
│
└── Readme.md                          → Documentación técnica de la capa

```
