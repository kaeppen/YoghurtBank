$project = "Server"

$password = "W31kvh0HNZXAzafqOrbuXKxFiN4TY"
Write-Host "Starting POSTGRES"
docker run -d -e POSTGRES_USER=dev -e POSTGRES_PASSWORD=W31kvh0HNZXAzafqOrbuXKxFiN4TY --name Yoghurtbase -p 5432:5432 --restart=always postgres
$database = "Yoghurtbase"
$connectionString = "Host=127.0.0.1;Database=Yoghurtbase;Username=dev; Password=$password;"
Write-Host "Configuring Connection String"
dotnet user-secrets init --project $project
dotnet user-secrets set "ConnectionStrings:Yoghurtbase:connectionString" "$connectionString" --project $project


Write-Host "Starting App"
dotnet run --project $project


#det skal testes på en maskine der er "ren" og ikke har user secrets mv. i forvejen. 
#derudover skal det testes på forskellige os

#vi kan selv bestemme om vi vil lave et autogenereret password eller hvad vi vil i guess? 
#tyvstjålet fra rasmus repo 