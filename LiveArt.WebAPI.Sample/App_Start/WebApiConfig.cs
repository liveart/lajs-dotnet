using System.Web.Http;
using LiveArt.WebAPI.Sample.Utils;
using Newtonsoft.Json.Serialization;

namespace LiveArt.WebAPI.Sample
{
    public class WebApiConfig
    {
        public const string LIVEART_ROUTE_NAME = "LiveArtRoute";
        public const string LIVEART_LOCATIONS_ROUTE_NAME = LIVEART_ROUTE_NAME + ".location";

        public static void Register(HttpConfiguration config,string routeTemplatePrefix="api")
        {
            config.Routes.MapHttpRoute(
                name: LIVEART_ROUTE_NAME,
                routeTemplate: routeTemplatePrefix+"/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: LIVEART_LOCATIONS_ROUTE_NAME,
                routeTemplate: routeTemplatePrefix+"/{controller}/{action}/{designID}/{locationName}",
                 defaults: new {  }
            );

            //TODO:right way http://www.strathweb.com/2013/06/supporting-only-json-in-asp-net-web-api-the-right-way/
            config.Formatters.Remove(config.Formatters.XmlFormatter); // Force all responses as "json"

            //newContractResolver.IgnoreSerializableAttribute = false;
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();  // convert C# property name convention to javascript name-convention

            #if DEBUG
                config.Formatters.JsonFormatter.Indent = true; // pretify json responses

                var cors = new System.Web.Http.Cors.EnableCorsAttribute("*", "*", "*");
                
                // in production will be better this:
                // var cors = new System.Web.Http.Cors.EnableCorsAttribute("www.yourdomain.com", "*", "*");
                config.EnableCors(cors); // remove it if you do not use cross-domain-requests  (services and designer hosted in the same domain)
            #endif
         
            config.Filters.Add(new LiveArtExceptionAttribute());
        }
    }
}