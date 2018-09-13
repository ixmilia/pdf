@echo off

set PROJECT=%~dp0\IxMilia.Pdf\IxMilia.Pdf.csproj
dotnet restore %PROJECT%
if errorlevel 1 exit /b 1
dotnet pack --configuration Release %PROJECT%
