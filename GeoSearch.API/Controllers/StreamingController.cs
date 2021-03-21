using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using GeoSearch.API.DTOs;
using GeoSearch.API.Hubs;
using GeoSearch.API.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace GeoSearch.API.Controllers
{
    [ApiController]
    [Route("geodata")]
    public class StreamingController : Controller
    {
        private readonly ILogger<StreamingController> _logger;
        private readonly string[] _permittedExtensions = { ".csv" };
        private IHubContext<GeoHub, IGeoHub> _geoHub;
        private FileProcessor _fileProcessor;
        // Get the default form options so that we can use them to set the default 
        // limits for request body data.
        private static readonly FormOptions _defaultFormOptions = new FormOptions();

        public StreamingController
        (
          ILogger<StreamingController> logger,
          IHubContext<GeoHub, IGeoHub> geoHub,
          FileProcessor fileProcessor
        )
        {
            _logger = logger;
            _geoHub = geoHub;
            _fileProcessor = fileProcessor;
        }

        [HttpPost("coordinatesInRange/processFile")]
        [DisableFormValueModelBinding]
        public async Task<IActionResult> ProcessGeoCoordinatesInRange()
        {
            if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                ModelState.AddModelError("File", "The request couldn't be processed - multipart content type required");
                _logger.LogError("The request couldn't be processed - multipart content type required");
                return BadRequest(ModelState);
            }
            var formAccumulator = new KeyValueAccumulator();
            var boundary = MultipartRequestHelper.GetBoundary(MediaTypeHeaderValue.Parse(Request.ContentType), _defaultFormOptions.MultipartBoundaryLengthLimit);
            var reader = new MultipartReader(boundary, HttpContext.Request.Body);
            var section = await reader.ReadNextSectionAsync();

            while (section != null)
            {
                var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition);
                if (hasContentDispositionHeader)
                {
                    if(MultipartRequestHelper.HasFormDataContentDisposition(contentDisposition))
                    {
                        var value = await FileHelpers.ProcessStreamedFormData(section.Body, contentDisposition);
                        formAccumulator.Append(contentDisposition.Name.Value, value);
                    }
                    else if (MultipartRequestHelper.HasFileContentDisposition(contentDisposition))
                    {
                        var radiusArea = new RadiusAreaGeoCoordinates();
                        var formValueProvider = new FormValueProvider(BindingSource.Form, new FormCollection(formAccumulator.GetResults()), CultureInfo.CurrentCulture);
                        var bindingSuccessful = await TryUpdateModelAsync(radiusArea, prefix: "", valueProvider: formValueProvider);

                        if(FileHelpers.IsValidFileExtension(contentDisposition.FileName.Value, section.Body, _permittedExtensions))
                        {
                            ModelState.AddModelError("File", "This file extension is not permitted.");
                            _logger.LogError("This file extension is not permitted.");
                        }

                        await foreach(var numberOfCoordsInRange in _fileProcessor.ProcessStreamedFile(section.Body, radiusArea))
                        {
                            _geoHub.Clients.Client(radiusArea.HubConnectionId).GeoCountUpdate(numberOfCoordsInRange, 0);
                            _logger.LogInformation($"Coords in range: {numberOfCoordsInRange}");
                        }
                    }
                }
                section = await reader.ReadNextSectionAsync();
            }
            return Created(nameof(StreamingController), null);
        }
        
    }

}
