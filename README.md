# GeoSearch
![Docker Cloud Build Status](https://img.shields.io/docker/cloud/build/fff1234/geosearch)

https://geosearchq.azurewebsites.net/


## CSV files format
Application processes csv files with Longitude and Latitude headers given as decimal values.
File should use comma as separator. Maximum file size is one gigabyte.

#### Example
|Longitude|Latitude|
|---------|--------|
|51.05    | 52.15  |
|55       | 55     |
|65       | 65     |
|-51      | -52    |

Example [file](example.csv) with ~1M entries can be found in this repository. 

## How to run app locally
### 1. Using docker registry
    docker pull fff1234/geosearch:latest
    docker run -p 8090:80 fff1234/geosearch:latest

### 2. Using github repository
#### Clone the project
	git clone https://github.com/SebastianBienert/GeoSearch.git

### 2.1 Docker build & start the application
	docker build . -t geosearch
    docker run -p 8090:80 geosearch
### or
### 2.1 Dotnet core cli build & start the application
    dotnet run --project GeoSearch.API/GeoSearch.API.csproj



    
