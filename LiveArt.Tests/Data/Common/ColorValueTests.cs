using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;
using LiveArt.Data.Json.Common;

namespace LiveArt.Tests.Data.Common
{
    [TestFixture]
    public class ColorValueTests
    {
        [Test]
        public void ToString_HtmlHexExpected()
        {
              
            //action
            var hexHtmlString = ColorValue.FromRGB(255, 255, 255).ToString();

            //asserts
            hexHtmlString
                .Should().Be("#FFFFFF");

            
        }

        [Test]
        public void ValueBuildedFromRgb()
        {
            //action
            var intValue = ColorValue.FromRGB(0xF0, 0xE0, 0xD0).Value;

            //assert
            intValue
                .Should().Be(0xF0E0D0);

        }

        [Test]
        public void RgbFromValue()
        {
            var color = new ColorValue(0xF0E0D0);

            //asserts
            color.R.Should().Be(0xF0);
            color.G.Should().Be(0xE0);
            color.B.Should().Be(0xD0);

        }

        [Test]
        public void HexStringWithLeadZero()
        {
            //action
            var hexHtmlString = ColorValue.FromRGB(0, 0, 1).ToString();

            //asserts
            hexHtmlString
                .Should().Be("#000001");

        }

        [Test]
        public void EqualsExplicit()
        {
            //arrange
            var color1 = ColorValue.FromRGB(1, 2, 3);
            var color2 = ColorValue.FromRGB(1, 2, 3);

            //action
            var isEqual = color1.Equals(color2);
            

            //assert
            isEqual.Should().BeTrue();
        }

        [Test]
        public void EqualImplicit()
        {     //arrange
            var color1 = ColorValue.FromRGB(1, 2, 3);
            var color2 = ColorValue.FromRGB(1, 2, 3);

            //action
            var isEqual = (color1 == color2);

            //assert
            isEqual.Should().BeTrue();
        }


        #region FromHtmlHexString
        [Test]
        public void FromHtmlHexString_ValidString(
            [Values(
                "#F0E0D0",
                "   #F0E0D0", // spaces before
                "#F0E0D0    " // spaces after
             )]string validString
           )
        {

            ColorValue.FromHtmlHexString(validString)
                .Should().Be(0xF0E0D0);
        }

        [Test]
        public void FromHtmlHexString_WithNormalization()
        {
            ColorValue.FromHtmlHexString("#FED")
                .Should().Be(0xFFEEDD);
        }


        [Test][ExpectedException(typeof(FormatException))]
        public void FromHtmlHexString_NoSharp()
        {
            ColorValue.FromHtmlHexString("F0E0D0");
        }


        [Test]
        public void FromHtmlHexString_Implicit()
        {
            //action
            ColorValue color = "#F0E0D0";

            //assert
            color
                .Should().Be(0xF0E0D0);

        }

        #endregion

  

    }
}
