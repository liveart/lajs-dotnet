using System.Web.Http;
using System.Web.Http.Filters;
using Newtonsoft.Json.Linq;

namespace LiveArt.WebAPI.Sample.Utils
{
    public class LiveArtExceptionAttribute : ExceptionFilterAttribute 
    {
        public override void OnException(HttpActionExecutedContext context)
        {

            
            var errorJson = JObject.FromObject(new
            {
                error = new
                {
                    message=context.Exception.Message, 
                    exceptionDetails=context.Exception.ToString()
                }
            });

            if (context.Response == null) context.Response = new RawJsonResponse(errorJson.ToString());

            if (context.Exception is LiveArtException)context.Response.StatusCode = ((LiveArtException)context.Exception).StatusCode;
            else if (context.Exception is HttpResponseException) context.Response.StatusCode = ((HttpResponseException)context.Exception).Response.StatusCode;
            else context.Response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            

        }
    }
}