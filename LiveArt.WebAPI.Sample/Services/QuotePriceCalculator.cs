using System.Linq;
using LiveArt.Data.Json.Quote;
using LiveArt.WebAPI.Sample.Repositories;

namespace LiveArt.WebAPI.Sample.Services
{
    public class QuotePriceCalculator: IQuotePriceCalculator
    {
        private PriceSettings _Settings;
        private IProductRepository ProductRepository;

        
        public QuotePriceCalculator(PriceSettings settings, IProductRepository productRepository)
        {
            _Settings = settings;
            ProductRepository = productRepository;
        }

        public PricesResult Calculate(QuoteRequest quoteRequest)
        {
            return new PricesResult()
            {
                ItemPrice = GetItemPriceValue(quoteRequest),
                DecorationPrice = GetItemDecorationPriceValue(quoteRequest),
                DiscountPrice = GetDiscountPriceValue(quoteRequest),
                SubTotal = getSubTotal(quoteRequest),
                Total = GetTotal(quoteRequest)
            };
        }

        private double GetItemPriceValue(QuoteRequest qRequest)
        {
            if (qRequest.Product == null) return _Settings.DefaultProductPrice;
            var productId = qRequest.Product.ID;
            var product = this.ProductRepository.Get(productId);

            return product != null ? product.Price : _Settings.DefaultProductPrice;
        }

        private double GetItemDecorationPriceValue(QuoteRequest qRequest)
        {
            // now let's calculate decoration based on colors used, like for screenprinting

            var colosNum = qRequest.Locations
                            .Where(loc => !loc.ColorsAmount.IsManualProcessRequred) // ignore location with unknown colors amount
                            .Sum(loc => (double)loc.ColorsAmount.Value);

            return colosNum * _Settings.ColorPricePerItem;
        }

        private double GetDiscountPriceValue(QuoteRequest qRequest)
        {
            var subTotal = getSubTotal(qRequest);
            return subTotal * _Settings.DiscountPercent;
        }

        private double getSubTotal(QuoteRequest qRequest)
        {
            var qty = qRequest.Quantities.Sum(q => q.Quantity); // // disregard sizes
            var itemPrice = GetItemPriceValue(qRequest);
            var decoPrice = GetItemDecorationPriceValue(qRequest);
            return qty * (itemPrice + decoPrice);
        }

        private double GetTotal(QuoteRequest qRequest)
        {
            return getSubTotal(qRequest) - GetDiscountPriceValue(qRequest);
        }

        public class PriceSettings
        {
            public double DefaultProductPrice { get; set; }
            public double ColorPricePerItem { get; set; }
            public double DiscountPercent { get; set; }
        }
    }
}