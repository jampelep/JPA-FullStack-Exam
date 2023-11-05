using JPA_WebApiApp.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Text;

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
            Console.WriteLine("---Encoding Starts...");

            //validation
            if (string.IsNullOrEmpty(text))
            {
                BadRequest("Input text is empty.");
                yield break;
            }

            // Simulate a time-consuming operation
            await Task.Delay(5000, cancellationToken);

            // Check if the request was canceled
            if (cancellationToken.IsCancellationRequested)
            {
                BadRequest("Request has been cancelled.");
                yield break;
            }

            //encode string to Base64
            var base64Value = _encoderService.GetBase64(text);

            //iterate each character
            foreach (var character in base64Value.ToCharArray())
            {
                cancellationToken.ThrowIfCancellationRequested();
                Console.WriteLine(character.ToString());
                yield return character;

                //Initiate a random pause between characters (1-5 seconds)
                var delayMilliseconds = new Random().Next(1000, 5000);
                await Task.Delay(delayMilliseconds, cancellationToken);
            }
        }
    }
}
