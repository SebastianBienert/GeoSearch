using CsvHelper.Configuration;
using System;

namespace GeoSearch.API.Models
{
    public class GeoCoordinate
    {
        private static double EARTH_MEAN_RADIUS = 6371000.0;
        public double Longitude { get; private set; }
        public double Latitude { get; private set; }

        public GeoCoordinate(double latitude, double longitude)
        {
            if (latitude < -90 || latitude > 90) 
                throw new ArgumentException("Invalid latitude coordinate supplied.");
            if (longitude < -180 || longitude > 180)
                throw new ArgumentException("Invalid longitude coordinate supplied.");

            Longitude = longitude;
            Latitude = latitude;
        }

        public double GetDistanceTo(GeoCoordinate otherCoordinates)
        {
            var d1 = Latitude * (Math.PI / 180.0);
            var d2 = otherCoordinates.Latitude * (Math.PI / 180.0);

            var num1 = Longitude * (Math.PI / 180.0);
            var num2 = otherCoordinates.Longitude * (Math.PI / 180.0) - num1;

            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                     Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

            var distance = EARTH_MEAN_RADIUS * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
            return distance;
        }
    }

    public class GeoCoordinateMap : ClassMap<GeoCoordinate>
    {
        public GeoCoordinateMap()
        {
            Parameter("latitude").Name("Latitude");
            Parameter("longitude").Name("Longitude");
        }
    }
}
