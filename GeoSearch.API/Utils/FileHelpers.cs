using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;

namespace GeoSearch.API.Utils
{
    public static class FileHelpers
    {
        public async static Task<string> ProcessStreamedFormData(Stream stream, ContentDispositionHeaderValue contentDisposition)
        {
            using (var streamReader = new StreamReader(stream))
            {
                var value = await streamReader.ReadToEndAsync();
                if (string.Equals(value, "undefined", StringComparison.OrdinalIgnoreCase))
                {
                    value = string.Empty;
                }
                return value;
            }
        }
        public static bool IsValidFileExtension(string fileName, Stream data, string[] permittedExtensions)
        {
            if (string.IsNullOrEmpty(fileName) || data == null || data.Length == 0)
            {
                return false;
            }

            var ext = Path.GetExtension(fileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            {
                return false;
            }

            return true;
        }
    }
}