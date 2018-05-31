using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using Newtonsoft.Json;

namespace LiveArt.Data.Json.Infrastructure
{
    /// <summary>
    ///    Used to bind model from "data" field of application/x-www-form-urlencoded requests 
    ///    It's temporary for "LiveArtJs json post" style :(  . Will be replaced by [FromBody] attribute, when LiveArtJs will post json, as "application/json" content type, instead of form field
    /// </summary>
    public class DataFieldToJsonModelBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            var formData = actionContext.Request.Content.ReadAsFormDataAsync().Result;
            var jsonStr = formData["data"];
            try
            {
                bindingContext.Model = JsonConvert.DeserializeObject(jsonStr, bindingContext.ModelType);
                return true;

            }
            catch (System.Exception ex)
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName,
                    "Cannot deserialize from 'data' field.\r\n" + ex.Message);
            }
            ;

            return false;
        }
    }

}