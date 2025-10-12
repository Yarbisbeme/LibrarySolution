# 📚 Library API – Evaluación Técnica

## 🧭 Objetivo

Esta API fue desarrollada como parte de una evaluación técnica cuyo objetivo es implementar una **librería digital** con operaciones **CRUD** sobre tres entidades principales:

- **Autores**: Authors
- **Libros**: Book
- **Préstamos**: Loan

La solución implementa **.NET 9** con **Entity Framework Core** y **JWT Authentication** para proteger los endpoints que modifican datos.

---


---

## 🧱 Arquitectura del Proyecto  

El proyecto sigue el patrón **Clean Architecture (Arquitectura Limpia)**, separando responsabilidades en capas independientes:

```plaintext
LibrarySolution/
│
├── Library.Api/              → Capa de presentación (controladores y endpoints)
├── Library.Application/      → Lógica de negocio (servicios e interfaces)
├── Library.Domain/           → Entidades del dominio (modelo puro)
├── Library.Infrastructure/   → Acceso a datos (EF Core, repositorio, migraciones)
├── Library.Common/           → DTOs y modelos comunes
└── Library.Tests/            → Pruebas unitarias (xUnit, Moq, EF InMemory)
```
---

## ⚙️ Ventajas de la separacion

Esta separación facilita:
- La mantenibilidad del código  
- El testeo independiente de cada capa  
- La extensibilidad (por ejemplo, cambiar de SQL Server a PostgreSQL sin afectar la lógica de negocio)

---

## 🧠 Beneficios de esta arquitectura

- Aislamiento de dependencias.  
- Facilidad para probar cada capa de forma independiente.  
- Flexibilidad ante cambios futuros (por ejemplo, cambiar la BD o el framework web).  
- Reutilización de código y claridad estructural.  

---

## ⚙️ Decisiones Técnicas

| 🧩 Componente | 🛠️ Decisión | 💡 Justificación |
|---------------|--------------|------------------|
| **Framework principal** | ASP.NET Core 9.0 | Rendimiento, soporte moderno y facilidad para crear APIs RESTful. |
| **ORM** | Entity Framework Core | Permite un acceso a datos fluido, seguro y con migraciones automáticas. |
| **Base de datos** | SQL Server | Amplio soporte, estabilidad y compatibilidad con EF Core. |
| **Autenticación** | JWT (JSON Web Token) | Estándar moderno para autenticación y autorización basada en roles. |
| **Pruebas unitarias** | xUnit + Moq + InMemory | Permite validar la lógica sin dependencias reales. |
| **Fluent API (EF Core)** | Configuración en código | Evita dependencias en anotaciones y mantiene la lógica de mapeo centralizada. |
| **Índices e optimización** | Índices en `fecha_devolucion` y claves foráneas | Mejoran el rendimiento en consultas frecuentes (especialmente “no devueltos”). |

---

## 🚀 Ejecución del Proyecto

### 1️⃣ Clonar el repositorio

```bash
git clone https://github.com/tuusuario/LibraryAPI.git
cd LibrarySolution
```

### Configurar base de datos

```
"ConnectionStrings": {
  "LibraryConnection": "Server=localhost;Database=LibraryDB;Trusted_Connection=True;TrustServerCertificate=True;"
},
"Jwt": {
  "Secret": "TuClaveSuperSecreta12345",
  "Issuer": "LibraryAPI",
  "Audience": "LibraryUsers"
}

```
-----
### Aplicar Migraciones
```
cd Library.Api
dotnet ef database update
```

-----
### AEjecutar la Api
```
dotnet run
```
-----
### Api disponible 
```
http://localhost:5000/swagger
```

-----
### 🔍 Optimización de Consultas 

Durante la optimización se aplicaron mejoras a la consulta de préstamos no devueltos:

Diagnóstico Inicial:
Activación de EnableSensitiveDataLogging() y LogTo(Console.WriteLine) para capturar el SQL generado.

Problema Detectado:
Consulta sin índice sobre fecha_devolucion, causando table scans.

Solución Implementada:
En LoanConfiguration.cs, se añadió índice con Fluent API:

```
builder.HasIndex(p => p.FechaDevolucion);
```

Resultado:
Plan de ejecución mejorado significativamente (búsqueda indexada → reducción de tiempo y lectura de páginas).

📈 Beneficio: Mejora del rendimiento de la consulta GET /prestamos/no-devueltos.

-----
### 🧪 Pruebas Unitarias

Las pruebas se realizaron con xUnit y Moq.
Ejecuta las pruebas con

```
dotnet test --logger "console;verbosity=normal"
```

Los resultados esperados son:

```
dotnet test --logger "console;verbosity=normal"
```

```
Pruebas totales: 6
✔ Correctas: 6
❌ Incorrectas: 0
Duración: 00:00:18
```

## 🧠 Decisiones Clave del Diseño
### 🔹 Separación por Capas

Cada capa cumple un rol claro y evita dependencias circulares:

Domain: Entidades puras (sin dependencias externas).

Application: Lógica de negocio.

Infrastructure: Persistencia y configuración de base de datos.

Common: DTOs y utilidades compartidas.

API: Controladores, autenticación y presentación.

👉 Esto garantiza un código limpio, testeable y escalable.

----
### 🔹 Fluent API en lugar de Data Annotations

Centraliza toda la configuración de las entidades.

Facilita agregar índices, relaciones y restricciones desde un único lugar.

Evita mezclar lógica de dominio con detalles de persistencia.

----------
### 🔹 Uso de índices

Se aplicaron índices a:

Prestamos.FechaDevolucion

Claves foráneas (LibroId, UsuarioId)

Esto reduce el tiempo de ejecución en consultas frecuentes y mejora la escalabilidad del sistema.

----
### 🧩 Swagger/OpenAPI

La API incluye documentación completa mediante Swagger UI:

Disponible en: http://localhost:5000/swagger

Autenticación integrada con JWT.

Descripciones y ejemplos XML generados automáticamente desde el código.

------
### 🧰 Tecnologías Utilizadas
| Tecnología                 | Uso                          |
| -------------------------- | ---------------------------- |
| **.NET 9.0 (C#)**          | Framework principal          |
| **Entity Framework Core**  | ORM para acceso a datos      |
| **SQL Server**             | Base de datos relacional     |
| **JWT + ASP.NET Identity** | Autenticación y autorización |
| **xUnit + Moq**            | Pruebas unitarias            |
| **Swagger / OpenAPI**      | Documentación interactiva    |
| **Fluent API (EF)**        | Configuración de entidades   |
| **InMemory DB (EF)**       | Testing aislado              |

### 🎥 Entregables Finales

✅ Código fuente completo
✅ Base de datos con migraciones automáticas
✅ Pruebas unitarias 100% ejecutadas
✅ Capturas de plan de ejecución antes/después
✅ Video demostrativo (2-3 minutos):

----
### Ejecución de la API

Uso de Swagger

Generación y uso del token JWT

Acceso a un endpoint protegido

👨‍💻 Autor

Yarbis Beltre Mercedes
📧 yarbisbeme@gmail.com

💼 Proyecto desarrollado como parte de la Evaluación Técnica - Library API 2025