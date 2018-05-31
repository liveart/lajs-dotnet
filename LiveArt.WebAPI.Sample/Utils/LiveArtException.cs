using System;
using System.Net;

namespace LiveArt.WebAPI.Sample.Utils
{
    public class LiveArtException: Exception
    {
        public System.Net.HttpStatusCode StatusCode { get; set; }
        
       public  LiveArtException(string message,HttpStatusCode statusCode=HttpStatusCode.InternalServerError):base(message){
           this.StatusCode = statusCode;
       }
    }
}