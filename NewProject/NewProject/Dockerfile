FROM mcr.microsoft.com/dotnet/sdk:8.0 AS test
WORKDIR /src

# Copy solution and projects
COPY ["NewProject/NewProject.sln", "./"]
COPY ["NewProject/NewProject/NewProject.APIs.csproj", "NewProject/"]
COPY ["NewProject/Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["NewProject/Domain/Domain.csproj", "Domain/"]
COPY ["NewProject/Application/Application.csproj", "Application/"]
COPY ["NewProject/Test/Test.csproj", "Test/"]

# Restore dependencies
RUN dotnet restore

# Copy entire source
COPY NewProject/ ./

# Build the solution
RUN dotnet build "NewProject.sln" -c Release --no-restore

# Run tests and force build failure on test failure
RUN dotnet test

# Publish the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS publish
WORKDIR /src
COPY --from=test /src ./
RUN dotnet publish "NewProject/NewProject.APIs.csproj" -c Release -o /app/publish

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=publish /app/publish ./
EXPOSE 7124
ENTRYPOINT ["dotnet", "NewProject.APIs.dll"]
