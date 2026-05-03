# Fusion Intelligence Backend Engineering Assessment

This is the submission for the Fusion Intelligence Backend Engineering Assessment.

## Setup Instructions

1. **Prerequisites**: .NET 8 SDK
2. **Initialization**: Run `./init.sh` (or `bash init.sh`) in the root directory to generate the Solution file and build the projects.
3. **Run API**: 
   ```bash
   cd Fusion.API
   dotnet run
   ```
4. **Testing**:
   ```bash
   dotnet test Fusion.Tests/Fusion.Tests.csproj
   ```

## Architectural Decisions

- **Clean Architecture**: The project is split into `Fusion.Core`, `Fusion.Infrastructure`, and `Fusion.API` to ensure separation of concerns. The Core layer has no dependencies on external frameworks like EF Core.
- **Persistence**: Used EF Core with SQLite for an easy, portable setup. The database (`app.db`) is automatically created and seeded on application startup.
- **Authentication**: Implemented a lightweight `ApiKeyMiddleware` to protect write endpoints. The key is configured in `appsettings.json` (`X-Api-Key: Secret_Assessment_Key_123`).
- **Assignment Logic**: "Prevent multiple active assignments" was interpreted as: an agent can only have one order that is either `Assigned` or `InTransit` at any given time.

## Trade-offs Made
- Chose SQLite over SQL Server for zero-configuration setup for reviewers.
- Used API Key auth instead of JWT to keep the focus on the business logic rather than complex Identity setup.
- The `DeliveryAgent` creation is implemented as a simple CRUD endpoint, and seed data is provided out-of-the-box.
