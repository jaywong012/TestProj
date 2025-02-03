# README.md

## Steps to Run Backend (BE)

1. Navigate to the `Infrastructure` directory:
   ```bash
   cd Infrastructure
   ```
2. Run the database update command:
   ```bash
   update-database
   ```

## Steps to generate report (Coverlet)

1. Install coverlet global

   ```bash
   dotnet tool install --global dotnet-reportgenerator-globaltool
   ```

2. Navigate to current Test directory and run command

   ```bash
   $projectRoot = Get-Location

   $coverageDirectory = Join-Path $projectRoot "coverage"
   $settingsFile    = Join-Path $projectRoot "coverlet.runsettings"
   $reportDir       = Join-Path $projectRoot "coverage-report"

   dotnet test --settings $settingsFile --collect:"XPlat Code Coverage" --results-directory $coverageDirectory -v quiet

   Start-Sleep -Seconds 2

   $latestFolder = Get-ChildItem -Path $coverageDirectory -Directory |
                  Sort-Object CreationTime -Descending |
                  Select-Object -First 1

   if ($latestFolder) {
      $guid = $latestFolder.Name
      $coverageFile = Join-Path $latestFolder.FullName "coverage.opencover.xml"
      
      reportgenerator -reports:$coverageFile -targetdir:$reportDir
      Write-Host "Report generated successfully for folder: $guid"
   } else {
      Write-Host "No folders found in the coverage directory after running tests."
   }
   ```

---



## Steps to Test Docker

1. **List Docker Containers**  
   Use the following command to list all running Docker containers:

   ```bash
   docker ps
   ```

2. **Access a Docker Container**  
   Replace `<container_name>` with your container's name to execute the following command:

   ```bash
   docker exec -it <container_name> sh
   ```

3. **Install Necessary Files**  
   Inside the container, update the package list and install `curl`:

   ```bash
   apt-get update && apt-get install -y curl
   ```

4. **Test if the Application is Available**  
   Run this command to test if the application is accessible:
   ```bash
   curl http://localhost:7124/swagger
   ```

5. **Docker build**
   ```bash
   docker-compose up --build
   ```

---

## Steps to run FE jest
1. Navigate FE project

2. Run command

   ```bash
   npm run test
   ```

## Remind: Check solution config to make sure nothing affect to the build
