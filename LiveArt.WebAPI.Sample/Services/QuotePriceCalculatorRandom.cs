using System;
using LiveArt.Data.Json.Quote;

namespace LiveArt.WebAPI.Sample.Services
{
    public class QuotePriceCalculatorRandom:IQuotePriceCalculator
    {
        public PricesResult Calculate(QuoteRequest quoteRequest)
        {
            var rnd = new Random();
            
            var result = new PricesResult();
            result.ItemPrice = rnd.Next(1000, 9999)/100.0;
            result.DecorationPrice = rnd.Next(1, 10);
            result.SubTotal = result.ItemPrice + result.DecorationPrice;

            double discountPercent = rnd.Next(10, 30);
            result.DiscountPrice = result.SubTotal*(discountPercent/100);

            result.Total = result.SubTotal - result.DecorationPrice;
            return result;
        }
    }
}