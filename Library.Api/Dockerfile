# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copiar los archivos de proyecto de todos los proyectos necesarios
COPY Library.Api/Library.Api.csproj ./Library.Api/
COPY Library.Application/Library.Application.csproj ./Library.Application/
COPY Library.Infrastructure/Library.Infrastructure.csproj ./Library.Infrastructure/
COPY Library.Domain/Library.Domain.csproj ./Library.Domain/
COPY Library.Common/Library.Common.csproj ./Library.Common/

# Restaurar dependencias de la solución (usando el proyecto de la API como entry point)
RUN dotnet restore Library.Api/Library.Api.csproj

# Copiar todo el código fuente
COPY . .

# Publicar la aplicación
RUN dotnet publish Library.Api/Library.Api.csproj -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Copiar los archivos publicados desde el stage de build
COPY --from=build /app/publish .

# Exponer el puerto que tu API use
EXPOSE 5000

# Ejecutar la aplicación
ENTRYPOINT ["dotnet", "Library.Api.dll"]
