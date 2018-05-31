using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using FluentAssertions;
using LiveArt.Data;
using LiveArt.Tests.TestData;
using System.IO;
using LiveArt.Data.Json;
using LiveArt.Data.Json.Design;
using Newtonsoft.Json.Linq;

namespace LiveArt.Tests.Data.Deserialize
{
    [TestFixture]
    public class DesignDeserializeTests
    {

        [Test]
        public void DeserializeDesign()
        {
            //arrange
            var jsonInputStr = TestDataProvider.DesignJson;
          //  Console.Write(jsonInputStr);

            //action
            var container = JsonConvert.DeserializeObject<DataContainer<Design>>(jsonInputStr);
            var design = container.Data;

            //asserts
            design.Should().NotBeNull();
            assertProduct(design.Product);

            //small check for locations
            design.Locations.Should().NotBeNullOrEmpty();
            design.Locations.First().Name.Should().Be("Front");

            //quantity
            assertQuantity(design.Quantities.FirstOrDefault());

            //assert prices
            assertPriceItem(design.Prices.FirstOrDefault());
            assertPriceTotal(design.Prices.LastOrDefault());


        }

        [Test]
        public void DeserializeStub()
        {
            var jsonInputStr = "{}";

            //action
            var design = JsonConvert.DeserializeObject<Design>(jsonInputStr);
           
            //asserts
            design.Should().NotBeNull();
        }

        [Test]
        public void DeserializeProduct()
        {
            //arrange
            dynamic designJson = JObject.Parse(TestDataProvider.DesignJson);
            string productJsonStr = designJson.data.product.ToString();
           // Console.WriteLine(productJsonStr);

            //action
            var product = JsonConvert.DeserializeObject<Product>(productJsonStr);

            //asserts
            assertProduct(product);
            product.ColorValue.Should().Be(0xFFFFFF);// #FFF in json expanded to #FFFFFF on parse
            product.ColorName.Should().Be("White");

        }

        private void assertProduct(Product product){
            product.Should().NotBeNull();
            product.ID.Should().Be("11");
            product.Name.Should().Be("T-shirt");
        }

        [Test]
        public void DeserializeLocation()
        {
            //arrange
            dynamic designJson = JObject.Parse(TestDataProvider.DesignJson);
            string locationStr = designJson.data.locations[0].ToString();

            //action
            var location = JsonConvert.DeserializeObject<Location>(locationStr);

            //asserts
            location.Name.Should().Be("Front");
            location.SVG.Should().Contain("<svg height=\"543\" version=\"1.1\"");
        }

        [Test]
        public void DeserializeQuantity()
        {
            //arrange
            dynamic designJson = JObject.Parse(TestDataProvider.DesignJson);
            string oneQuoteStr = designJson.data.quantities[0].ToString();
            //Console.WriteLine(oneQuoteStr);
              
            //action
            var quantity = JsonConvert.DeserializeObject<Quantity>(oneQuoteStr);

            //asserts
            assertQuantity(quantity);
            
        }

        private void assertQuantity(Quantity quantity){
            quantity.Should().NotBeNull();
            quantity.Value.Should().Be(5); // in JSON it's named "quantity"
            quantity.SizeName.Should().Be("XS");// in JSON it's named "size"
        }

        [Test]
        public void DeserializePrice()
        {
            //arrange
            dynamic designJson = JObject.Parse(TestDataProvider.DesignJson);
            string priceItemStr = designJson.data.prices[0].ToString();
        
            //action
            var price = JsonConvert.DeserializeObject<Price>(priceItemStr);

            //asserts

            assertPriceItem(price);
        }

        private void assertPriceItem(Price price)
        {
                
            price.Label.Should().Be("Item Price");
            price.Value.Should().Be(20.00); // in JSON it's names "price"
            price.IsTotal.Should().BeFalse("default value is false");
        }
        

        [Test]
        public void DeserializeTotalPrice()
        {
            //arrange
            dynamic designJson = JObject.Parse(TestDataProvider.DesignJson);
            string priceItemStr = designJson.data.prices[3].ToString(); // last price is total



            //action
            var price = JsonConvert.DeserializeObject<Price>(priceItemStr);

            assertPriceTotal(price);
        }

        private void assertPriceTotal(Price price)
        {
            //asserts
            price.Label.Should().Be("Total");
            price.Value.Should().Be(124.69); // in JSON it's names "price"
            price.IsTotal.Should().BeTrue("isTotal:true in JSON");
        }

        
    }
}
