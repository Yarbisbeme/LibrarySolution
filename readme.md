# 📚 Library API – Evaluación Técnica

## 🧭 Objetivo

Esta API fue desarrollada como parte de una evaluación técnica cuyo objetivo es implementar una **librería digital** con operaciones **CRUD** sobre tres entidades principales:

- **Autores**: Authors
- **Libros**: Book
- **Préstamos**: Loan

La solución implementa **.NET 8** con **Entity Framework Core** y **JWT Authentication** para proteger los endpoints que modifican datos.

---

## 🏗️ Arquitectura del Proyecto

El proyecto utiliza una arquitectura **en capas limpias (Clean Architecture)**, separando responsabilidades en diferentes niveles:

```bash
src/
│ ├─ Library.Api/ → Capa de presentación (controladores y endpoints REST)
│ ├─ Library.Application/ → Lógica de negocio, servicios y DTOs
│ ├─ Library.Domain/ → Entidades de dominio puras
│ ├─ Library.Infrastructure/ → Persistencia y acceso a datos (EF Core, repositorios)
│ └─ Library.Common/ → Utilidades, respuestas estándar y excepciones
│
└─ tests/
│ └─ Library.Tests/ → Pruebas unitarias y de integración
```
---

## ⚙️ Ventajas de la separacion

Esta separación facilita:
- La mantenibilidad del código  
- El testeo independiente de cada capa  
- La extensibilidad (por ejemplo, cambiar de SQL Server a PostgreSQL sin afectar la lógica de negocio)

---

## ⚙️ Tecnologías principales

| Tecnología | Uso |
|-------------|-----|
| **.NET (ASP.NET Core)** | Framework principal para la API |
| **Entity Framework Core** | ORM para el acceso a base de datos |
| **SQL Server** | Motor de base de datos |
| **JWT (JSON Web Token)** | Autenticación y autorización con roles |
| **xUnit + Moq** | Frameworks de testing unitario |
| **Swagger / OpenAPI** | Documentación interactiva de la API |
| **AutoMapper** | Mapeo entre entidades y DTOs |
| **FluentValidation** | Validación de modelos (opcional) |


