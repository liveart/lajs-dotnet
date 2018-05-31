using System.Collections.Generic;
using System.Web.Http;
using LiveArt.Data.Json.Quote;
using LiveArt.WebAPI.Sample.Repositories;
using LiveArt.WebAPI.Sample.Services;
using LiveArt.WebAPI.Sample.Utils;

namespace LiveArt.WebAPI.Sample.Controllers
{
    public class QuoteController : ApiController
    {
        private IQuotePriceCalculator PriceCalculator;
        public QuoteController(): 
            this(new QuotePriceCalculator(  
                    new QuotePriceCalculator.PriceSettings()
                    {
                        DefaultProductPrice = 20, // $20 for "unknown product",
                        ColorPricePerItem = 5,//  each color is $5 per item
                        DiscountPercent = 0.0025 // 0.25%  discount for subtotal
                    }, 
                    new ProductRepositoryStub()) // required for get product price
                  )

            // or uncomment this to use alternative price calculation (based on random generator)
            //this (new QuotePriceCalculatorRandom());
        {

        }
        public QuoteController(IQuotePriceCalculator priceCalculator)
        {
            PriceCalculator = priceCalculator;
        }

        public QuoteResponse Calculate(QuoteRequest qRequest)
        {
            ValidateQuoteInfo(qRequest);
            // Do price calculation
            var calcResult = PriceCalculator.Calculate(qRequest);

            // Convert calculation result to price items with labels
            var priceItems = new List<QuoteResponse.PriceItem>();
            priceItems.Add(new QuoteResponse.PriceItem("Item Price", calcResult.ItemPrice));
            priceItems.Add(new QuoteResponse.PriceItem("Item Decoration", calcResult.DecorationPrice));
            priceItems.Add(new QuoteResponse.PriceItem("Discount 0.25%", -1*calcResult.DiscountPrice));
            priceItems.Add(new QuoteResponse.PriceItem("Total", calcResult.Total) { IsTotal = true });

            // build response
            return new QuoteResponse()
            {
                Prices = priceItems,
            };

        }

        private void ValidateQuoteInfo(QuoteRequest qRequest)
        {
            if (qRequest == null) throw new InvalidQuoteDataException("data is null");
            if (qRequest.Locations == null) throw new InvalidQuoteDataException("Locations not found");
            //if (qRequest.Product== null) throw new InvalidQuoteDataException("Product not found");
            if (qRequest.Quantities == null) throw new InvalidQuoteDataException("Quantities not found");
        }

       

        #region Exception
        public class InvalidQuoteDataException:LiveArtException{
            public InvalidQuoteDataException(string invalidDataMessage="")
                :base("Invalid posted data:"+invalidDataMessage,System.Net.HttpStatusCode.BadRequest)
            {
            }
        }
        #endregion
    }
}
