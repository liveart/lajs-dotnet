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
    public class ColorsAmountsTests 
    {
        [Test]
        public void SetValueNotNull_IsManualShouldBeFalse()
        {
            // arrange
            var ca = new ColorsAmount();

            //action
            ca.Value=333;

            //assers
            ca.IsManualProcessRequred.Should().BeFalse("Value is not null");
        }

        [Test]
        public void SetValueNull_IsManualShouldBeTrue()
        {
            // arrange
            var ca = new ColorsAmount();

            //action
            ca.Value = null;

            //assers
            ca.IsManualProcessRequred.Should().BeTrue("Value is null");
        }

        
        [Test]
        public void Parse_Numeric_string_string()
        {
            //action
            var ca = ColorsAmount.Parse("123");
            //asserts
            ca.Value.Should().Be(123);
        }


        [Test]
        [ExpectedException(typeof(FormatException))]
        public void Parse_Invalid_string()
        {
            ColorsAmount.Parse("invlidString");

            //asserts
            // expected exception
        }


        [Test]
        public void Parse_ValidTextString([Values("processColors","",null)]string validTextString)
        {
            //action
            var ca = ColorsAmount.Parse(validTextString);

            //asserts
            ca.IsManualProcessRequred.Should().BeTrue();
        }

        [Test]
        public void Parse_ImplicitString_ProcessColors()
        {
            //action
            ColorsAmount ca = "processColors";

            //assert
            ca.IsManualProcessRequred.Should().BeTrue();
        }

        [Test]
        public void Parse_ImplicitString_Number()
        {
            //action
            ColorsAmount ca = "333";

            //assert
            ca.Value.Should().Be(333);
                
        }

        
    


    }
}
