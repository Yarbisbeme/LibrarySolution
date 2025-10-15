# 📚 Library API – Evaluación Técnica  
## 🏗️ Capa 4: Infrastructure  

---

### 🧩 Descripción General  

La capa **Infrastructure** contiene toda la **implementación técnica** necesaria para que la aplicación funcione:  
gestión de base de datos, configuración de entidades, contexto de EF Core y registro de dependencias.  

Su función principal es **conectar la lógica de negocio (Application)** con la **persistencia de datos (Database)**,  
respetando los principios de **Clean Architecture** y **Dependency Inversion**.

---

### 🧱 Responsabilidades  

✅ Configurar y administrar el **contexto de base de datos (`LibraryDbContext`)**.  
✅ Implementar las **configuraciones de entidades** con Fluent API.  
✅ Definir **índices e integridad referencial** para optimizar consultas.  
✅ Gestionar la **inyección de dependencias** mediante `InfrastructureServiceRegistration`.  
✅ Mantener la independencia de la lógica de negocio respecto a los detalles de persistencia.

---

### 📂 Estructura del Proyecto  

```plaintext
Library.Infrastructure/
│
├── Data/
│   ├── LibraryDbContext.cs              → Contexto principal de Entity Framework Core
│   └── Configurations/                  → Configuraciones por entidad (Fluent API)
│       ├── AuthorConfiguration.cs
│       ├── BookConfiguration.cs
│       ├── LoanConfiguration.cs
│
├── Extensions/
│   └── InfrastructureServiceRegistration.cs   → Extensión para registrar servicios e infraestructura
│
└── Library.Infrastructure.csproj

```

-------------------------

### Relacion con otras Capas

┌────────────────────┐
│  Presentation/API  │
└────────▲───────────┘
         │
┌────────┴───────────┐
│   Application      │ ← Define servicios e interfaces
└────────▲───────────┘
         │
┌────────┴───────────┐
│   Infrastructure   │ ← Implementa persistencia (EF Core, SQL Server)
└────────▲───────────┘
         │
┌────────┴───────────┐
│     Domain         │ ← Modelo puro del negocio
└────────────────────┘

----------------
### 🧠 Principios aplicados

| Principio                    | Descripción                                                             |
| ---------------------------- | ----------------------------------------------------------------------- |
| **Separation of Concerns**   | Divide la lógica de negocio y la persistencia.                          |
| **Dependency Inversion**     | Las capas superiores dependen de abstracciones, no de implementaciones. |
| **Fluent API Configuration** | Centraliza reglas de mapeo sin contaminar las entidades del dominio.    |
| **Clean Architecture**       | La infraestructura es reemplazable sin alterar el núcleo del negocio.   |

### librarydbcontext 

