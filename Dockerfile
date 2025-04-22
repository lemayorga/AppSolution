# Usa la imagen oficial de .NET 8 ASP.NET para el entorno de ejecución en Linux
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
USER app
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_URLS=http://*:80

# Usa la imagen oficial de .NET 8 SDK para la compilación en Linux
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AppSolution.sln", "./"]
COPY ["/src/SG.API/SG.API.csproj","SG.API/"]
COPY ["/src/SG.Application/SG.Application.csproj","SG.Application/"]
COPY ["/src/SG.Domain/SG.Domain.csproj","SG.Domain/"]
COPY ["/src/SG.Infrastructure/SG.Infrastructure.csproj","SG.Infrastructure/"]
COPY ["/src/SG.Infrastructure/SG.Infrastructure.Auth.csproj","SG.Infrastructure.Auth/"]
COPY ["/src/SG.Shared/SG.Shared.csproj","SG.Shared/"]

# Restaura las dependencias para el proyecto principal
RUN dotnet restore "/src/SG.API/SG.API.csproj"
COPY /src .

# Establece el directorio de trabajo en el proyecto principal
WORKDIR "/src/SG.API"
# Compila el proyecto
RUN dotnet build "SG.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publica el proyecto
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SG.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Etapa final: usa la imagen de runtime para ejecutar la aplicación
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet","SG.API.dll"]