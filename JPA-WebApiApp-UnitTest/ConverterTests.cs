using JPA_WebApiApp.Controllers;
using JPA_WebApiApp.Service;
using Microsoft.Extensions.Logging;
using Moq;

namespace JPA_WebApiApp_UnitTest
{
    public class Tests
    {
        #region Members
        EncoderService? _encoderService { get; set; }
        ConverterController? _converterController { get; set; }
        ILogger<EncoderService> _logger { get; set; }
        #endregion

        #region Constructor        
        [SetUp]
        public void Setup()
        {
            var mLogger = new Mock<ILogger<EncoderService>>();
            _encoderService = new EncoderService(mLogger.Object);
            _converterController = new ConverterController(_encoderService);
        }
        #endregion

        #region Methods
        [Test]
        [TestCase("Hello, World!")] //SGVsbG8sIFdvcmxkIQ==
        [TestCase("Good morning!")] //R29vZCBtb3JuaW5nIQ==
        [TestCase("Cool! Thanks!")] //Q29vbCEgVGhhbmtzIQ==
        public void TestBase64Encode(string input)
        {
            //Act
            var result = _encoderService?.GetBase64(input);

            //Assert
            var expectedResult = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(input));
            Assert.That(expectedResult, Is.EqualTo(result));
        }

        [Test]
        [TestCase("Hello, World!")] //SGVsbG8sIFdvcmxkIQ==
        [TestCase("Good morning!")] //R29vZCBtb3JuaW5nIQ==
        [TestCase("Cool! Thanks!")] //Q29vbCEgVGhhbmtzIQ==
        public async Task TestEncodeToBase64Controller(string inputs)
        {
            // Arrange
            var cancellationToken = new CancellationToken();

            // Act
            var result = new List<char>();
            await foreach (var character in _converterController?.EncodeToBase64(inputs, cancellationToken))
            {
                result.Add(character);
            }

            // Assert
            var expectedResult = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(inputs))
                .ToCharArray()
                .ToList();
            CollectionAssert.AreEqual(expectedResult, result);
        }
        #endregion
    }
}