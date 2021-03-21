using GeoSearch.API.Models;
using System;
using Xunit;

namespace GeoSearch.UnitTests
{
    public class GeoCoordinateTests
    {
        [Fact]
        public void GivenInvalidCoordinateShouldThrowException()
        {
            var invalidLatitudeEx = Assert.Throws<ArgumentException>(() => new GeoCoordinate(181.00, 12.05));
            var invalidLongitudeEx = Assert.Throws<ArgumentException>(() => new GeoCoordinate(78.00, -1255.05));
            Assert.Equal("Invalid latitude coordinate supplied.", invalidLatitudeEx.Message);
            Assert.Equal("Invalid longitude coordinate supplied.", invalidLongitudeEx.Message);
        }

        [Fact]
        public void GivenTwoPointsShouldCalculateCorrectDistance()
        {
            var sourcePoint = new GeoCoordinate(51.1094854, 17.0265926);
            var destinationPoint = new GeoCoordinate(51.6702041, 16.5156865);

            var distance = sourcePoint.GetDistanceTo(destinationPoint);

            Assert.Equal(71722.3, distance, 1);
        }
    }
}
