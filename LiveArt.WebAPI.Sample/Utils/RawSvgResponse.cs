using System.Net.Http;

namespace LiveArt.WebAPI.Sample.Utils
{
    public class RawSvgResponse : HttpResponseMessage
    {
        public RawSvgResponse(string rawSvgString,string fileName=null,bool forceDownload=false)
        {
            this.Content = new StringContent(rawSvgString,
                                            System.Text.Encoding.UTF8,
                                            "image/svg+xml");

            
            this.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue(!forceDownload ? "inline" : "attachment");
            
            
            if (fileName != null) this.Content.Headers.ContentDisposition.FileName = fileName;
            

        }

        
    }
}