using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using FluentAssertions;
using LiveArt.Data.Json.Quote;

namespace LiveArt.Tests.Data.Deserialize
{
    [TestFixture]
    public class QuoteInfoDesserializeTest
    {

        #region Location

        [Test]
        public void LocationExists()
        {
            //action
            var quote = this.ParseSample();

            //asserts
            quote.Locations.Should().NotBeNull();
            quote.Locations.Should().HaveCount(2);


        }

        [Test]
        public void Locatoin_CheckSimpleProperties()
        {
            //action
            var quote = this.ParseSample();
            var location = quote.Locations.First(); // front location

            //asserts
            location.Name.Should().Be("Front");
            location.ImagesCount.Should().Be(2);
            location.LetteringsCount.Should().Be(1);
            location.DesignElementsCount.Should().Be(3,"it's sum of ImagesCount and LetteringCount");

        }

        [Test]
        public void Location_ColorsAmount()
        {
            //action
            var quote = this.ParseSample();

           quote.Locations.First(l => l.Name == "Front")
                .ColorsAmount.IsManualProcessRequred.Should().BeTrue();

            quote.Locations.First(l => l.Name == "Back")
                .ColorsAmount.Value.Should().Be(2);

        }

        #endregion

        [Test]
        public void DesignedArea()
        {
            //action
            var quote = this.ParseSample();
            var location = quote.Locations.First(); // Front location

            //asserts
            location.DesignedArea
                .Should().Be(7.2625806264285595); 
            location.DesignedAreaRect
                .Should().Be(14.53293808222984); 
        }

        [Test]
        public void DesignedArea_Dimentions()
        {
            //action
            var quote = this.ParseSample();
            var location = quote.Locations.First(); // Front location

            //asserts
            location.DesignedHeight
                .Should().Be(3.7529685135723656);
            location.DesignedWidth
                .Should().Be(3.7331249999999998); 

            
        }

        [Test]
        public void Location_ObjectLists()
        {
            //action
            var quote = this.ParseSample();

            //asserts
            quote.Locations.First().DesignElements
                .Should().HaveCount(3);

            // only count cheched, more detail tesring of DesginElement class see in QuoteInfoDesignElementsTests.cs
        }

        [Test]
        public void Location_ColorsList_Front_ShouldNotBeEmpty()
        {
            //action
            var quote = this.ParseSample();
            var location = quote.Locations.First(); // Front

            //assert
            location.ColorValues
                .Should().HaveCount(1,"'colorsList' is not empty in json front location");
        }

        [Test]
        public void Location_ColorsList_Back_ShouldNotBeEmpty()
        {
            //action
            var quote = this.ParseSample();
            var location = quote.Locations.Last(); // Back

            //assert
            location.ColorValues
                .Should().HaveCount(2,"'colorsList' is not empty array in json back location");
        }

        [Test]
        public void Product_SimpleProperties()
        {
            //action
            var quote = this.ParseSample();
            var product = quote.Product;

            //Asserts
            product.ID.Should().Be("11");
        }

        [Test]
        public void Product_ProducColors()
        {
            //action
            var quote = this.ParseSample();
            var productColor = quote.Product.ProductColors.First();

            //assert
            productColor.ID
                .Should().Be(".bg.fill");
            productColor.Name
                .Should().Be("White");
            productColor.ColorValue
                .Should().Be(0xFFFFFF);

            
        }

        [Test]
        public void Quantities()
        {
            //action
            var quote = this.ParseSample();
            var quoteQuantity = quote.Quantities.First();

            //assert
            quoteQuantity.Quantity
                .Should().Be(1);
            quoteQuantity.Size
                .Should().Be("XS");

        }

        
        private QuoteRequest ParseSample()
        {
            var json = TestData.TestDataProvider.QuoteInfoJson;
            QuoteRequest quote = JsonConvert.DeserializeObject<QuoteRequest>(json);
            quote.Should().NotBeNull("quote should be parsed");
            return quote;
        }

        
    }
}
