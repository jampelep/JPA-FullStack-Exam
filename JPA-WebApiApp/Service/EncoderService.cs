using JPA_WebApiApp.Service.Interface;
using static System.Net.Mime.MediaTypeNames;
using System.Text;

namespace JPA_WebApiApp.Service
{
    public class EncoderService : IEncoderService
    {
        #region Members
        #endregion

        #region Constructor
        public EncoderService() { }

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
        #endregion
    }
}
