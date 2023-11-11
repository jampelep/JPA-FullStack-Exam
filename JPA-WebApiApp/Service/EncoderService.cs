using JPA_WebApiApp.Service.Interface;
using System.Text;

namespace JPA_WebApiApp.Service
{
    public class EncoderService : IEncoderService
    {
        #region Members
        private readonly ILogger<EncoderService> _logger;
        #endregion

        #region Constructor
        public EncoderService(ILogger<EncoderService> logger)
        {
            _logger = logger;
        }

        #endregion

        #region Methods

        public string GetBase64(string text)
        {
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(text);
                return Convert.ToBase64String(bytes);
            }
            catch
            {
                return string.Empty;
            }
        }

        public async IAsyncEnumerable<char> IterateEncodedString(string text, CancellationToken cancellationToken)
        {
            //encode string to Base64
            var base64Value = GetBase64(text);
            var randomizer = new Random();

            foreach (var character in base64Value.ToCharArray())
            {
                cancellationToken.ThrowIfCancellationRequested();
                _logger.LogInformation(character.ToString());

                yield return character;

                // Initiate a random pause between characters (1-5 seconds)
                var delayMilliseconds = randomizer.Next(1000, 6000);
                await Task.Delay(delayMilliseconds, cancellationToken);
            }
        }
        #endregion
    }
}
