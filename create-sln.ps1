Write-Host "=== Re-creating solution file... ==="
Write-Host

Remove-Item template-app.sln
dotnet new sln
dotnet sln add (ls -r ./src/**/*.csproj) --in-root
dotnet sln add (ls -r ./tests/**/*.csproj) -s tests

Write-Host
Write-Host "=== Finished creating the solution file. ==="
Write-Host
Write-Host "This powershell script will now be deleted."
pause
# Remove-Item create-sln.ps1
