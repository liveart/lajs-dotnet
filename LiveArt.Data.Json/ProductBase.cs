using LiveArt.Data.Json.Common;
using Newtonsoft.Json;

namespace LiveArt.Data.Json
{
    public class ProductBase
    {
        public string ID { get; set; }

        [JsonProperty("color")]
        public ColorValue ColorValue { get; set; }
        public string ColorName { get; set; }
    }
}
