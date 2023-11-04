using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace JPA_WebApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConverterController : ControllerBase
    {
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
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            string base64Text = Convert.ToBase64String(bytes);
            var characterArray = base64Text.ToCharArray();

            //iterate each character
            foreach (var character in characterArray)
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
