using System.Collections.Generic;
using System.Web.Http.ModelBinding;
using LiveArt.Data.Json.Infrastructure;

namespace LiveArt.Data.Json.Quote
{
    [ModelBinder(typeof(DataFieldToJsonModelBinder))]
    public  class QuoteRequest
    {
        public IEnumerable<Location> Locations { get; set; }
        public Product Product { get; set; }
        public IEnumerable<QuantityQuantity> Quantities { get; set; }
    }
}
