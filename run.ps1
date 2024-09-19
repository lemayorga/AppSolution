# Leer el archivo .dotnetconfig y extraer el nombre del proyecto de inicio
$config = Get-Content .dotnetconfig | ConvertFrom-Json
$startupProject = $config.startupProject

if (-not $startupProject) {
    Write-Host "No se ha definido un proyecto de inicio en .dotnetconfig."
    exit 1
}

# Ejecutar el proyecto de inicio
dotnet run --project "src/$startupProject/$startupProject.csproj"

# Ejecutar en poweshell en windows .\run.ps1
# En windows ejecutar antes: Win + R y pegar: powershell Set-ExecutionPolicy -Scope "CurrentUser" -ExecutionPolicy "RemoteSigned"

#  Ejecutar en poweshell en linux   ./run.sh