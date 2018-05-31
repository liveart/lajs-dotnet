using LiveArt.Data.Json.Common;
using Newtonsoft.Json;

namespace LiveArt.Data.Json.Quote
{
    public class ProductColor
    {
        public string ID { get; set; }
        public string Name { get; set;}
        
        [JsonProperty("value")]
        public ColorValue ColorValue{get;set;}
    }
}
