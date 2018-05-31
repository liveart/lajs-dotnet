using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace LiveArt.Design.PostProcessing.Tests
{
    [TestFixture]
    public class UtilsTests
    {
        [Test]
        public void XmlToString_NoNewLineAndSpaces_Around() // Workaround to fix render in Inksckape
        {
            //arrange
            var xmlDocument = Utils.XmlFromString(@"
<text>
  <tspan>Some Text</tspan>
</text>
");

            //action
            var xmlStr=Utils.XmlToString(xmlDocument);

            //assert
            xmlStr.Should().Be("<text><tspan>Some Text</tspan></text>");
        }
    }
}
