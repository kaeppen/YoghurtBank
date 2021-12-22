$project = "Server"

$password = "W31kvh0HNZXAzafqOrbuXKxFiN4TY"
Write-Host "Starting POSTGRES"
docker run -d -e POSTGRES_USER=dev -e POSTGRES_PASSWORD=W31kvh0HNZXAzafqOrbuXKxFiN4TY --name Yoghurtbase -p 5432:5432 --restart=always postgres
$database = "Yoghurtbase"
$connectionString = "Host=localhost;Database=Yoghurtbase;Username=dev; Password=$password;"
Write-Host "Configuring Connection String"
dotnet user-secrets init --project $project
dotnet user-secrets set "ConnectionStrings:Yoghurtbase:connectionString" "$connectionString" --project $project


Write-Host "Starting App"
dotnet run --project $project

