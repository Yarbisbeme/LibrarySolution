# 📚 Library API – Evaluación Técnica  
## 🧩 Capa 1: Domain  

---

### 🧠 Descripción General  

La capa **`Domain`** representa el **núcleo del modelo de negocio** de la aplicación.  
Aquí se definen las **entidades base**, sus **propiedades**, **reglas de negocio internas** y las **interfaces** que describen su comportamiento.

Esta capa **no depende de ninguna otra** ni de frameworks externos, siguiendo el principio de **Pure Domain Model** o **Domain-Driven Design (DDD)**.  
Su objetivo es mantener la lógica empresarial independiente de los detalles de infraestructura.

---

### 🏗️ Estructura del Proyecto  

```plaintext
Library.Domain/
│
├── Entities/                → Entidades principales del dominio
│   ├── Author.cs
│   ├── Book.cs
│   ├── Loan.cs
│
├── Interfaces/              → Contratos que describen el comportamiento de las entidades
│   ├── IAuthor.cs
│   ├── IBook.cs
│   ├── ILoan.cs
│
└── Common/                  → Clases base y abstracciones
    └── BaseEntity.cs

```



-------------------------
### Rol dentro de la Arquitectura

┌────────────────────┐
│  Presentation/API  │
└────────▲───────────┘
         │
┌────────┴───────────┐
│   Application      │ ← Contiene la lógica de negocio y usa las entidades
└────────▲───────────┘
         │
┌────────┴───────────┐
│     Domain         │ ← Modelo puro del negocio
└────────▲───────────┘
         │
┌────────┴───────────┐
│  Infrastructure    │ ← Implementa persistencia (EF Core)
└────────────────────┘

---------------------

### 🧩 Conclusión

La capa Domain es la columna vertebral del sistema.
Aquí se definen los conceptos fundamentales del negocio y sus reglas sin depender de detalles técnicos.
Esto asegura que el modelo de dominio pueda evolucionar independientemente de las tecnologías usadas (base de datos, frameworks, UI, etc.).