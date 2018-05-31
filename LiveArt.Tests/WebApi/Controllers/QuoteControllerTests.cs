using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;
using LiveArt.Data.Json.Quote;
using LiveArt.WebAPI.Sample.Controllers;
using Newtonsoft.Json;

namespace LiveArt.Tests.WebApi.Controllers
{
    [TestFixture]
    public class QuoteControllerTests
    {
        QuoteController controller;

        [SetUp]
        public void SetUp()
        {
            controller = new QuoteController();
        }


        [Test]
        public void CalculationSample()
        {
            //arrange 
            var quoteInfo = JsonConvert.DeserializeObject<QuoteRequest>(TestData.TestDataProvider.QuoteInfoJson);

            var expecedResponse = this.ExpectedSampleResponse();
            var expectedNames = expecedResponse.Prices.Select(p => p.Label);
            var expectedPriceValues = expecedResponse.Prices.Select(p => p.PriceValue);
            var expexedIsTotal = expecedResponse.Prices.Select(p => p.IsTotal);

            //action
            var response = controller.Calculate(quoteInfo);

            //assert

            response.Prices.Select(p => p.Label)
                .Should().BeEquivalentTo(expectedNames);

            response.Prices.Select(p => p.PriceValue)
                .Should().BeEquivalentTo(expectedPriceValues);
            response.Prices.Select(p => p.IsTotal)
                .Should().BeEquivalentTo(expexedIsTotal);
                
        }

        #region TestData utils
        private QuoteResponse ExpectedSampleResponse()
        {
            var prices = new List<QuoteResponse.PriceItem>();
            prices.Add(new QuoteResponse.PriceItem("Item Price", 20)); // default item price $20
            prices.Add(new QuoteResponse.PriceItem("Item Decoration", 10)); // $5*2(colosNum)
            prices.Add(new QuoteResponse.PriceItem("Discount 0.25%", -0.225)); // (30+10)*3(qty)*0.0025
            prices.Add(new QuoteResponse.PriceItem("Total", 89.775) { IsTotal = true }); // subTotal=(20+10)*3, total=90(subTotal)-0.225(discount)=89.775

            // build response

            return new QuoteResponse()
            {
                Prices = prices,
            };

        }

        #endregion
    }
}
