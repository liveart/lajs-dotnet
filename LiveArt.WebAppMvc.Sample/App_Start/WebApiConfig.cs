using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace LiveArt.WebAppMvc.Sample
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // register liveart backend services
            WebAPI.Sample.WebApiConfig.Register(config,"la-api");  // use in _LiveArtJS/config/config.json endpoints like "la-api/SomeControllerName/SomeControllerAction"
        }
    }
}
