#!/bin/bash

# Leer el nombre del proyecto de inicio desde .dotnetconfig
startup_project=$(grep -Po '(?<="startupProject": ")[^"]*' .dotnetconfig)

# Verificar si se encontró un proyecto de inicio
if [ -z "$startup_project" ]; then
  echo "No se ha definido un proyecto de inicio en .dotnetconfig."
  exit 1
fi

# Ejecutar el proyecto de inicio
dotnet run --project "$startup_project/$startup_project.csproj"
