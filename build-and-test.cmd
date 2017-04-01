set TEST_PROJECT=.\src\IxMilia.Pdf.Test\IxMilia.Pdf.Test.csproj
dotnet restore %TEST_PROJECT%
if errorlevel 1 exit /b 1
dotnet test %TEST_PROJECT%
