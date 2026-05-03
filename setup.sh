#!/bin/zsh
source ~/.zshrc

dotnet new sln -n FusionIntelligence
dotnet new webapi -n Fusion.API
dotnet new classlib -n Fusion.Core
dotnet new classlib -n Fusion.Infrastructure
dotnet new xunit -n Fusion.Tests

dotnet sln add Fusion.API/Fusion.API.csproj
dotnet sln add Fusion.Core/Fusion.Core.csproj
dotnet sln add Fusion.Infrastructure/Fusion.Infrastructure.csproj
dotnet sln add Fusion.Tests/Fusion.Tests.csproj

dotnet add Fusion.API/Fusion.API.csproj reference Fusion.Core/Fusion.Core.csproj
dotnet add Fusion.API/Fusion.API.csproj reference Fusion.Infrastructure/Fusion.Infrastructure.csproj
dotnet add Fusion.Infrastructure/Fusion.Infrastructure.csproj reference Fusion.Core/Fusion.Core.csproj
dotnet add Fusion.Tests/Fusion.Tests.csproj reference Fusion.API/Fusion.API.csproj
dotnet add Fusion.Tests/Fusion.Tests.csproj reference Fusion.Core/Fusion.Core.csproj
dotnet add Fusion.Tests/Fusion.Tests.csproj reference Fusion.Infrastructure/Fusion.Infrastructure.csproj

rm Fusion.Core/Class1.cs
rm Fusion.Infrastructure/Class1.cs

cd Fusion.Infrastructure
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Design

cd ../Fusion.API
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Serilog.AspNetCore

cd ../Fusion.Tests
dotnet add package Moq
cd ..
