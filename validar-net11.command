#!/bin/bash
# Valida la migración a .NET 11: SDK, restore, build y tests. Resultado en validacion-net11.log
cd "$(dirname "$0")"
export PATH="$PATH:/usr/local/share/dotnet:$HOME/.dotnet:/opt/homebrew/bin"
export DOTNET_CLI_TELEMETRY_OPTOUT=1
LOG=validacion-net11.log
{
  echo "=== $(date)"
  echo "=== SDKs"; dotnet --list-sdks; echo "=== runtimes"; dotnet --list-runtimes
  echo "=== RESTORE"; dotnet restore BocciaCoaching/BocciaCoaching.csproj; echo "RESTORE_EXIT=$?"
  echo "=== BUILD"; dotnet build BocciaCoaching/BocciaCoaching.csproj -c Release --no-restore -v q -clp:NoSummary; echo "BUILD_EXIT=$?"
  echo "=== TESTS"; dotnet test tests/BocciaCoaching.Tests/BocciaCoaching.Tests.csproj -c Release -v q; echo "TEST_EXIT=$?"
  echo "=== PAQUETES DESACTUALIZADOS"; dotnet list BocciaCoaching/BocciaCoaching.csproj package --outdated
  echo "=== VULNERABLES"; dotnet list BocciaCoaching/BocciaCoaching.csproj package --vulnerable --include-transitive
  echo "=== FIN"
} > "$LOG" 2>&1
echo "Validación terminada. Puedes cerrar esta ventana."
