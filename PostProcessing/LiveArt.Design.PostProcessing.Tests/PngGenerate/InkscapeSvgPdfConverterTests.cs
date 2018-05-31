using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LiveArt.Design.PostProcessing.PngGenerate;
using Moq;
using NUnit.Framework;

namespace LiveArt.Design.PostProcessing.Tests.PngGenerate
{
    [TestFixture]
    public class InkscapeSvgPdfConverterTests
    {
        /// <summary>
        ///   class under test
        /// </summary>
        private ISvgConverter Converter;

        private Mock<IExternalTools> InkscapeMoq;

        [SetUp]
        public void SetUp()
        {
            InkscapeMoq = new Mock<IExternalTools>();

            InkscapeMoq.Setup(m => m.CanExecute()).Returns(true);
            InkscapeMoq.Setup(m => m.Execute(It.IsAny<string>())).Returns(new ExternalToolResult() { Success = true });

            Converter = new InkscapeSvgPdfConverter(InkscapeMoq.Object);
        }

        [Test]
        public void Convert_ShouldExecute_InkscapeWithArgs()
        {
            //arrange
            const string SvgSourceFile = "input.svg";
            const string OutPdfFilePath = "output.pdf";
            const int svgWitdh = 100;
            const int svgHeight = 200;
            const string ExpectedArgs = "-T \"input.svg\" -A \"output.pdf\" -a 0:0:100:200 -w 100 -h 200";
            
            //action
            Converter.Convert(SvgSourceFile, OutPdfFilePath, svgWitdh, svgHeight)

            //assert
            .Success.Should().BeTrue();

            InkscapeMoq.Verify(m => m.Execute(ExpectedArgs), Times.Once);
        }
    }
}
