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

## CSV files format
Application processes csv files with Longitude and Latitude headers given as decimal values.
File should use comma as separator.

#### Example
|Longitude|Latitude|
|---------|--------|
|51.05    | 52.15  |
|55       | 55     |
|65       | 65     |
|-51      | -52    |

    
