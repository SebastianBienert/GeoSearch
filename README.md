# GeoSearch

## How to run app

### Clone the project
	git clone https://github.com/SebastianBienert/GeoSearch.git

### Docker build & start the application
	docker build . -t geosearch
    docker run -p 8090:80 geosearch

### or

### Dotnet core cli build & start the application
    dotnet run --project GeoSearch.API/GeoSearch.API.csproj
    
