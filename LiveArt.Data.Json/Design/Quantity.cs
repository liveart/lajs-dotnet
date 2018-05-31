using Newtonsoft.Json;

namespace LiveArt.Data.Json.Design
{
    public class Quantity
    {
        [JsonProperty("quantity")]
        public int Value { get; set; }

        [JsonProperty("size")]
        public string SizeName { get; set; }

    }
}
