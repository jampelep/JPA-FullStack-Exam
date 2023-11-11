using JPA_WebApiApp.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace JPA_WebApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConverterController : ControllerBase
    {
        private readonly IEncoderService _encoderService;

        public ConverterController(IEncoderService encoderService)
        {
            _encoderService = encoderService;
        }

        [HttpGet]
        public async IAsyncEnumerable<char> EncodeToBase64(string text, CancellationToken cancellationToken)
        {
            //validation
            if (string.IsNullOrEmpty(text))
            {
                BadRequest("Input text is empty.");
                yield break;
            }

            // Check if the request was canceled
            if (cancellationToken.IsCancellationRequested)
            {
                BadRequest("Request has been cancelled.");
                yield break;
            }

            //iterate each character
            await foreach (var character in _encoderService.IterateEncodedString(text, cancellationToken))
            {
                yield return character;
            }
        }
    }
}
