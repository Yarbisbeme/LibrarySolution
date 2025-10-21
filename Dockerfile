# ========================
# STAGE 1: BUILD
# ========================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar los archivos de proyecto primero (mejor cache)
COPY Library.Api/Library.Api.csproj Library.Api/
COPY Library.Application/Library.Application.csproj Library.Application/
COPY Library.Infrastructure/Library.Infrastructure.csproj Library.Infrastructure/
COPY Library.Domain/Library.Domain.csproj Library.Domain/
COPY Library.Common/Library.Common.csproj Library.Common/

# Restaurar dependencias
RUN dotnet restore Library.Api/Library.Api.csproj

# Copiar el resto del código fuente
COPY . .

# Compilar y publicar en modo Release
WORKDIR /src/Library.Api
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# ========================
# STAGE 2: RUNTIME
# ========================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Crear usuario no-root para seguridad
RUN adduser --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

# Variables de entorno por defecto
ENV ASPNETCORE_URLS=http://+:5000 \
    ASPNETCORE_ENVIRONMENT=Production \
    TZ=America/Santo_Domingo

# Copiar los artefactos publicados desde build stage
COPY --from=build /app/publish .

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=10s --retries=3 \
    CMD curl -f http://localhost:5000/health || exit 1

# Exponer puerto
EXPOSE 5000

# Entry point
ENTRYPOINT ["dotnet", "Library.Api.dll"]