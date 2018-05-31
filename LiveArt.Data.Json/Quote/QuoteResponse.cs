using System.Collections.Generic;
using Newtonsoft.Json;

namespace LiveArt.Data.Json.Quote
{
    public class QuoteResponse
    {
        public IEnumerable<PriceItem> Prices { get; set; }

        public QuoteResponse()
        {
            this.Prices = new List<PriceItem>();
        }

        public class PriceItem
        {
            private const string PRICE_FORMAT = "$ {0:0.00}";



            public string Label { get; set; }

            [JsonIgnore]
            public double PriceValue { get; set; }

            public string Price // formated price
            {
                get
                {
                    return string.Format(System.Globalization.CultureInfo.InvariantCulture, PRICE_FORMAT, PriceValue);
                }
            }

            [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
            public bool IsTotal { get; set; }



            public PriceItem() { }
            public PriceItem(string label, double priceValue)
            {
                this.Label = label;
                this.PriceValue = priceValue;
            }
        }
    }
}