# Imagen base para compilar la app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar primero la solución y los csproj para aprovechar la cache de Docker
COPY BocciaCoaching.sln ./
COPY BocciaCoaching/*.csproj BocciaCoaching/
RUN dotnet restore BocciaCoaching.sln

# Copiar el resto del código
COPY . .

# Publicar en modo Release
WORKDIR /src/BocciaCoaching
RUN dotnet publish -c Release -o /app/publish

# Imagen base para ejecutar la app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Exponer el puerto que usará Render
ENV ASPNETCORE_URLS=http://+:8080

# Deshabilitar inotify/file watchers para evitar el límite de instancias en Linux
ENV DOTNET_hostBuilder__reloadConfigOnChange=false
ENV DOTNET_USE_POLLING_FILE_WATCHER=false
ENV DOTNET_RUNNING_IN_CONTAINER=true

EXPOSE 8080

# Ejecutar la aplicación
ENTRYPOINT ["dotnet", "BocciaCoaching.dll"]

