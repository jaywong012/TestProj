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

2. Generate coverlet.xml

   ```bash
   dotnet test --settings "D:\DDD-React-Project\NewProject\Test\coverlet.runsettings" --collect:"XPlat Code Coverage" --results-directory "D:\DDD-React-Project\NewProject\coverage" -v diag
   ```

3. Coverlet

   ```bash
   reportgenerator -reports:"D:/DDD-React-Project/NewProject/coverage/${guid}/coverage.opencover.xml" -targetdir:"D:/DDD-React-Project/NewProject/coverage-report"
   ```

4. Combind-command

   ```bash
   $baseDirectory = "D:\DDD-React-Project\NewProject\coverage"
   dotnet test --settings "D:\DDD-React-Project\NewProject\Test\coverlet.runsettings" `
    --collect:"XPlat Code Coverage" --results-directory $baseDirectory  `
    -v diag
   Start-Sleep -Seconds 2
   $latestFolder = Get-ChildItem -Path $baseDirectory -Directory |
               Sort-Object CreationTime -Descending |
               Select-Object -First 1
   if ($latestFolder) {
      $guid = $latestFolder.Name
      reportgenerator -reports:"$baseDirectory\$guid\coverage.opencover.xml" -targetdir:"D:/DDD-React-Project/NewProject/coverage-report"
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

---

Feel free to reach out if you encounter any issues!

## Remind: Check solution config to make sure nothing affect to the build
