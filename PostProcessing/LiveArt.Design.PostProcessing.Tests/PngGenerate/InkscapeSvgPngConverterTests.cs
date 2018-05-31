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
    public class InkscapeSvgPngConverterTests
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
            InkscapeMoq.Setup(m => m.Execute(It.IsAny<string>())).Returns(new ExternalToolResult() {Success = true});

            Converter =new InkscapeSvgPngConverter(InkscapeMoq.Object);
        }


        [Test]
        public void CanConvert_Return_TheSame_Tool_Can_Execute([Values(true,false)]bool canExecute)
        {
            // arrange
            //reset-up moq
            InkscapeMoq.Setup(m => m.CanExecute()).Returns(canExecute);
            //action
            Converter.CanConvert()

                //assert
                .Should().Be(canExecute, $"external tools.CanExecute return  ${canExecute}");
        }

        [Test]
        public void Convert_ShouldExecute_InkscapeWithArgs()
        {
            //arrange
            const string SvgSourceFile = "input.svg";
            const string OutPngFilePath = "output.svg";
            const int svgWitdh = 100;
            const int svgHeight = 200;
            const string ExpectedArgs = "\"input.svg\" -e \"output.svg\" -a 0:0:100:200";

            //action
            Converter.Convert(SvgSourceFile, OutPngFilePath, svgWitdh, svgHeight)

            //assert
            .Success.Should().BeTrue();

            InkscapeMoq.Verify(m=>m.Execute(ExpectedArgs),Times.Once);
        }
    }
}
