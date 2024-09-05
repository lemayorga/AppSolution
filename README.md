# Solución de API Rest para el Programa de Base   🚀

## Descripción

Este repositorio contiene una solución completa desarrollado en C# utilizando .NET 8 y Entity Framework Core con el enfoque de Code First que incluye:

## Arquitectura del Proyecto

El proyecto sigue la **Arquitectura Clean**  asegurando la separación de preocupaciones, alta testabilidad, y facilidad para realizar cambios o escalar.

### Componentes del Proyecto  📁
  - **Domain** (`src/SG.Domain`): Contiene las entidades, interfaces y lógica de negocio.
  - **Infrastructure** (`src/SG.Infrastructure`): Implementaciones de repositorios, acceso a datos usando Entity Framework Core, y migraciones.
  - **Application** (`src/SG.Application`): Manejo de casos de uso, validaciones, y lógica de aplicación (Dtos,ViewModels,Automapper).
  - **API Rest** (`src/SG.API/SG.API`): Exposición de los endpoints HTTP y controladores de la API.

## Requisitos previos 📋

- **.NET 8 SDK**: Requerido para ejecutar ambos proyectos de API.
- **Entity Framework Core 8.08**: ORM utilizado para el acceso a la base de datos.
- **Herramientas de desarrollo**: Un editor o IDE como Visual Studio o Visual Studio Code.
- **SQL Server** (o base de datos compatible con EF Core): Base de datos predeterminada para las APIs.
- **Postgre Sql** (o base de datos compatible con EF Core): Base de datos predeterminada para las APIs.

## Configuraciones ⚙️

1. **Configurar la cadena de conexión:**

Editar el archivo `appsettings.json` (para desarrollo seria: `appsettings.Development.json`) para configurar la cadena de conexión a tu base de datos, se basa en la variable `DatabaseProvider` la cual se establece el gestor de Base de Datos (Postgresql y SQL Server) siendo los valores permitidos los valores existentes dentro de `ConnectionStrings`, posterior configurar la ConnectionStrings correspondiente al DatabaseProvider.
```json
{
  "DatabaseProvider": "PostgreSql", 
  "ConnectionStrings": {
    "PostgreSql": "User ID =driverAdmin;Password=12345678;Server=localhost;Port=5432;Database=SampleDriverDb; Integrated Security=true;Pooling=true;",
    "SqlServer": "Server=localhost;Database=YourDatabase;User Id=YourUsername;Password=XXXX;"
  }  
}
```

2. **Aplicar migraciones:**

Ejecutarlo en el powershell en la carpeta de tu repositorio. 
```
dotnet ef database update --verbose --project "src/SG.Infrastructure" --startup-project "src/SG.API"
```

3. **Crear una nueva migración:**

Ejecutarlo en el powershell en la carpeta de tu repositorio. Reemplazar en el comando de abajo la palabra `_MY_NEW_MIGRATION_` con el nombre de la nueva migración a crear.

```
dotnet ef migrations  add _MY_NEW_MIGRATION_ --verbose --project "src/SG.Infrastructure" --startup-project "src/SG.API" -o "Data/Migrations"
```


## Ejecutar el proyecto
```
dotnet run  --project  "src/SG.API" 
```
<!-- 
Acceder al swagger: 

http://localhost:5203/swagger/index.html

APi dev:

http://localhost:5203/api/home -->