# 📚 Library API – Evaluación Técnica (.NET 8)
------

## 🧩 Capa 1 – Domain

La capa **Domain** define las entidades base del sistema.  
No depende de ninguna otra capa ni de frameworks externos, siguiendo el principio de **Pure Domain Model**.

### 📁 Estructura

Library.Domain/
├─ Entities/
│ ├─ Author.cs
│ ├─ Book.cs
│ ├─ Loan.cs
└─ Common/
└─ BaseEntity.cs


### 🧱 Entidades

#### Author

Entidad para los autores 🙎


#### Book

Entidad para los libros 📓📕


#### Loan

Entidad para los prestamos de los libros ↩️


### 🧠 Decisiones de diseño

- Se utiliza una clase base para evitar duplicación de propiedades comunes (CreatedAt, UpdatedAt).

- Las relaciones (Author → Books, Book → Loans) se definen con colecciones, pero sin dependencias a EF Core (la configuración se hace en la capa Infrastructure).

- Los nombres siguen convenciones C# (PascalCase), aunque las columnas de base de datos se ajustarán al formato del enunciado mediante Fluent API.