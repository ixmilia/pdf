#!/bin/sh -e

TEST_PROJECT=./src/IxMilia.Pdf.Test/IxMilia.Pdf.Test.csproj
dotnet restore $TEST_PROJECT
dotnet test $TEST_PROJECT
