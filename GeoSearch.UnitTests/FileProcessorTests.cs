using GeoSearch.API;
using GeoSearch.API.DTOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GeoSearch.UnitTests
{
    public class FileProcessorTests
    {
        [Fact]
        public async Task GivenGeoCoordinatesShouldCountCorrectNumberOfCoordinatesInRangeAsync()
        {
            var coordinatesInRange = new RadiusAreaGeoCoordinates()
            {
                BorderLatitude = 55,
                BorderLongitude = 55,
                CenterLatitude = 50,
                CenterLongitude = 50
            };
            var fileProcessor = new FileProcessor();
            using(var fileStream = File.OpenRead("Data/GeoCoords.csv"))
            {
                var result = fileProcessor.ProcessStreamedFile(fileStream, coordinatesInRange);
                var finalResult = await result.LastAsync();
                Assert.Equal(2, finalResult);
            }
            
        }
    }
}
