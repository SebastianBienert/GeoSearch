using CsvHelper;
using GeoSearch.API.DTOs;
using GeoSearch.API.Models;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace GeoSearch.API
{
    public class FileProcessor
    {
        private readonly int BATCH_SIZE = 10000;
        public async IAsyncEnumerable<int> ProcessStreamedFile(Stream fileStream, RadiusAreaGeoCoordinates dto)
        {
            var center = new GeoCoordinate(dto.CenterLatitude, dto.CenterLongitude);
            var radius = center.GetDistanceTo(new GeoCoordinate(dto.BorderLatitude, dto.BorderLongitude));
            using (var streamReader = new StreamReader(fileStream))
            using (var csv = new CsvReader(streamReader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<GeoCoordinateMap>();
                var count = 0;
                await foreach (var x in csv.GetRecordsAsync<GeoCoordinate>().Where(x => x.GetDistanceTo(center) <= radius))
                {
                    count++;
                    if (count % BATCH_SIZE == 0)
                    {
                        yield return count;
                    }
                }
                yield return count;
            }
        }
    }
}
