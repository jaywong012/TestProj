# =============================
# Stage 0: Install dependencies, build and run tests
# =============================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS test
WORKDIR /src

COPY ["NewProject/NewProject.sln", "./"]
COPY ["NewProject/NewProject/NewProject.APIs.csproj", "NewProject/"]
COPY ["NewProject/Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["NewProject/Domain/Domain.csproj", "Domain/"]
COPY ["NewProject/Application/Application.csproj", "Application/"]
COPY ["NewProject/Test/Test.csproj", "Test/"]

RUN dotnet restore

COPY NewProject/ ./

RUN dotnet build "NewProject.sln" -c Release --no-restore

# Run tests and force build failure on test failure
RUN dotnet test

# =============================
# Stage 1: Publish the application
# =============================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS publish
WORKDIR /src
COPY --from=test /src ./
RUN dotnet publish "NewProject/NewProject.APIs.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=publish /app/publish ./
EXPOSE 7124
ENTRYPOINT ["dotnet", "NewProject.APIs.dll"]
