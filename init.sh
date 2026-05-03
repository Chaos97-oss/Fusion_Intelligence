#!/bin/zsh
source ~/.zshrc

dotnet new sln -n FusionIntelligence

dotnet sln add Fusion.API/Fusion.API.csproj
dotnet sln add Fusion.Core/Fusion.Core.csproj
dotnet sln add Fusion.Infrastructure/Fusion.Infrastructure.csproj
dotnet sln add Fusion.Tests/Fusion.Tests.csproj

# Restore and build to ensure everything is correct
dotnet build FusionIntelligence.sln

# Run tests
dotnet test Fusion.Tests/Fusion.Tests.csproj
