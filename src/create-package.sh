#!/bin/sh -e

PROJECT=./IxMilia.Pdf/IxMilia.Pdf.csproj
dotnet restore $PROJECT
dotnet pack --include-symbols --include-source --configuration Release $PROJECT

