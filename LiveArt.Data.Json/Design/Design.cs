using System.Collections.Generic;
using Newtonsoft.Json;

namespace LiveArt.Data.Json.Design
{
    
    public class Design
    {
        public Product Product{get;set;}
        public IEnumerable<Location> Locations{get;set;}
        public IEnumerable<Quantity> Quantities { get; set; }
        public IEnumerable<Price> Prices { get; set; }

        public static Design Parse(string designJsonStr)
        {
            return JsonConvert.DeserializeObject<DataContainer<Design>>(designJsonStr).Data;
        }
    }
}
