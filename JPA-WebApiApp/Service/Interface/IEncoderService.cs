using Microsoft.AspNetCore.Mvc;

namespace JPA_WebApiApp.Service.Interface
{
    public interface IEncoderService 
    {
        string GetBase64(string text);
    }
}
