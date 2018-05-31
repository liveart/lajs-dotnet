using System.Net.Http;

namespace LiveArt.WebAPI.Sample.Utils
{
    public class RawTextPlainResponse: HttpResponseMessage
    {
        public RawTextPlainResponse(string text)
        {
            this.Content = new StringContent(text,
                                            System.Text.Encoding.UTF8,
                                            "text/plain");
        }
      
    }
}