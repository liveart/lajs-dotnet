using System;
using System.Collections.Generic;
using LiveArt.Data.Json.Common;
using Newtonsoft.Json;

namespace LiveArt.Data.Json.Quote
{
    public enum DesignElementTypes { TXT,SVG,Image}
    public class DesignElement // names as "object" in JSON
    {
        public Double DesignedArea { get; set; }

        [JsonProperty("colors")]
        public ColorsAmount ColorsAmount { get; set; }

        [JsonProperty("colorsList")]
        public IEnumerable<ColorValue> ColorValues { get; set; }

        public DesignElementTypes Type { get; set; }

        public string Text { get; set; }
    }
}
