using System.Collections.Generic;
using LiveArt.Data.Json.Common;
using Newtonsoft.Json;

namespace LiveArt.Data.Json.Quote
{
    public class Location
    {
        public string Name { get; set; }
        
        [JsonProperty("images")]
        public int ImagesCount { get; set; }

        [JsonProperty("letterings")]
        public int LetteringsCount { get; set; }

        [JsonProperty("objectCount")]
        public int DesignElementsCount { get; set; }

        [JsonProperty("objects")]
        public IEnumerable<DesignElement> DesignElements { get; set; }


        #region Design Area

        public double DesignedArea { get; set; }
        public double DesignedAreaRect { get; set; }

        public double DesignedHeight { get; set; }
        public double DesignedWidth { get; set; }

        #endregion

        [JsonProperty("colors")]
        public ColorsAmount ColorsAmount { get; set; }

        [JsonProperty("colorsList")]
        public IEnumerable<ColorValue> ColorValues { get; set; }

        
    }
}
