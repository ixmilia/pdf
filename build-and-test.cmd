@echo off

set SOLUTION=%~dp0IxMilia.Pdf.sln
dotnet restore %SOLUTION%
if errorlevel 1 exit /b 1
dotnet build %SOLUTION%
if errorlevel 1 exit /b 1
dotnet test %SOLUTION%
if errorlevel 1 exit /b 1
