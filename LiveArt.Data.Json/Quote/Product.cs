using System.Collections.Generic;

namespace LiveArt.Data.Json.Quote
{
    public class Product:ProductBase
    {
        public IEnumerable<ProductColor> ProductColors { get; set; }
    }
}
