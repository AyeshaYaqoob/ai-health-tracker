# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files first to cache dependencies
COPY ["HealthTracker.sln", "./"]
COPY ["HealthTracker.API/HealthTracker.API.csproj", "HealthTracker.API/"]
COPY ["HealthTracker.Application/HealthTracker.Application.csproj", "HealthTracker.Application/"]
COPY ["HealthTracker.Domain/HealthTracker.Domain.csproj", "HealthTracker.Domain/"]
COPY ["HealthTracker.Infrastructure/HealthTracker.Infrastructure.csproj", "HealthTracker.Infrastructure/"]
COPY ["HealthTracker.Tests/HealthTracker.Tests.csproj", "HealthTracker.Tests/"]
COPY ["HashGen/HashGen.csproj", "HashGen/"]

RUN dotnet restore "HealthTracker.sln"

# Copy the rest of the code and build
COPY . .
WORKDIR "/src/HealthTracker.API"
RUN dotnet publish "HealthTracker.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Run
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080

ENTRYPOINT ["dotnet", "HealthTracker.API.dll"]
