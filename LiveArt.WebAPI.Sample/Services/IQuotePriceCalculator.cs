using LiveArt.Data.Json.Quote;

namespace LiveArt.WebAPI.Sample.Services
{
    public interface IQuotePriceCalculator
    {
      /*  double GetItemPriceValue();
        double GetItemDecorationPriceValue();
        double GetDiscountPriceValue();
        double GetTotal();*/

        PricesResult Calculate(QuoteRequest quoteRequest);
    }

    public class PricesResult
    {
        public double ItemPrice { get; set; }
        public double DecorationPrice { get; set; }
        public double DiscountPrice { get; set; }
        public double SubTotal { get; set; }
        public double Total { get; set; }
    }
}
