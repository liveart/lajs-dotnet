using System.Net.Http;

namespace LiveArt.WebAPI.Sample.Utils
{
    public class RawJsonResponse : HttpResponseMessage
    {

        public RawJsonResponse(string rawJsonString)
        {
            this.Content = new StringContent(rawJsonString,
                                            System.Text.Encoding.UTF8,
                                            "application/json");
        }
    }
}