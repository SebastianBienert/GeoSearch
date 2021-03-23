using System.ComponentModel.DataAnnotations;

namespace GeoSearch.API.DTOs
{
    public class RadiusAreaGeoCoordinatesDTO
    {
        [Required]
        public double CenterLongitude { get; set; }
        [Required]
        public double CenterLatitude { get; set; }
        [Required]
        public double BorderLongitude { get; set; }
        [Required]
        public double BorderLatitude { get; set; }
        public string HubConnectionId { get; set; }
    }
}
