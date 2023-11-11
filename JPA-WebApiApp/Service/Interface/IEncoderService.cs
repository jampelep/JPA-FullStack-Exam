namespace JPA_WebApiApp.Service.Interface
{
    public interface IEncoderService
    {
        string GetBase64(string text);
        IAsyncEnumerable<char> IterateEncodedString(string text, CancellationToken cancellationToken);
    }
}
