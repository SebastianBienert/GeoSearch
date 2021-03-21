namespace GeoSearch.API.DTOs
{
    public class RadiusAreaGeoCoordinates
    {
        public double CenterLongitude { get; set; }
        public double CenterLatitude { get; set; }
        public double BorderLongitude { get; set; }
        public double BorderLatitude { get; set; }
        public string HubConnectionId { get; set; }
    }
}
