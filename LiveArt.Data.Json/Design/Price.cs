using LiveArt.Data.Json.Common;
using Newtonsoft.Json;

namespace LiveArt.Data.Json.Design
{
    
    public class Price
    {
        public string Label { get; set; }
        [JsonProperty("price")]
        public Money Value { get; set; }// TODO: change type to Decimal, value converter will be required
        public bool IsTotal { get; set; }// false is default value
    }
}
