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
    public class QuoteInfoDesignElementsTests
    {
        [Test]
        public void Property_DesignArea()
        {
            //arrange
            var json=@"
                 {
                    ""colors"": 1,
                    ""colorsList"": [
                        ""#000000""
                    ],
                    ""designedArea"": 123.456,
                    ""type"": ""txt""
                }
            ";

            //action
            var element = JsonConvert.DeserializeObject<DesignElement>(json);

            //asserts
            element.DesignedArea
                .Should().Be(123.456);
        }

        [Test]
        public void Property_Text()
        {
            //arrange
            var json = @"
                 {
                    ""colors"": 1,
                    ""colorsList"": [
                        ""#000000""
                    ],
                    ""designedArea"": 2.6878500000000001,
                    ""text"": ""Add some text"",
                    ""type"": ""txt""
                }
            ";

            //action
            var element = JsonConvert.DeserializeObject<DesignElement>(json);

            //asserts
            element.Text
                .Should().Be("Add some text");
        }

        [Test]
        public void Property_ColorsAmount_Number()
        {
            //arrange
            var json = @"
                {
                    ""colors"": 3,
                    ""designedArea"": 23208.783168620044,
                    ""type"": ""svg""
                }
            ";

            //action
            var element = JsonConvert.DeserializeObject<DesignElement>(json);

            //asserts
            element.ColorsAmount.Value
                .Should().Be(3);
        }

        [Test]
        public void Property_ColorsAmount_String()
        {
            //arrange
            var json = @"
               {
                    ""colors"": ""processColors"",
                    ""designedArea"": 8237.319999999987,
                    ""type"": ""image""
                }
            ";

            //action
            var element = JsonConvert.DeserializeObject<DesignElement>(json);

            //asserts
            element.ColorsAmount.IsManualProcessRequred
                .Should().BeTrue();

        }

        [Test]
        public void Property_ColorsList()
        {
            //arrange
            var json = @"
                {
                    ""colors"": 1,
                    ""colorsList"": [
                        ""#F0E0D0""
                    ],
                    ""designedArea"": 6342,
                    ""type"": ""txt""
                }
            ";

            //action
            var element = JsonConvert.DeserializeObject<DesignElement>(json);

            //asserts
            element.ColorValues
                .Should().HaveCount(1);
            element.ColorValues.First()
                .Should().Be(0xF0E0D0);


        }

        [Test, Sequential]
        public void Property_Type(
              [Values("txt","svg","image")]
              string typeAsString,

              [Values(DesignElementTypes.TXT,DesignElementTypes.SVG,DesignElementTypes.Image)]
              DesignElementTypes expectedType
            )
        {
                    //arrange
            var json = @"
                {
                    ""colors"": 1,
                    ""colorsList"": [
                        ""#F0E0D0""
                    ],
                    ""designedArea"": 6342,
                    ""type"": ""$type$""
                }
            ".Replace("$type$",typeAsString);

            //action
            var element = JsonConvert.DeserializeObject<DesignElement>(json);

            //assert
            element.Type
                .Should().Be(expectedType);

            

        }

      
        
    }
}
