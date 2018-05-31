using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveArt.Design.PostProcessing.PackerExtentions;
using LiveArt.Design.PostProcessing.PngGenerate;
using Moq;
using NUnit.Framework;

namespace LiveArt.Design.PostProcessing.Tests.PackerExtentions
{
    [TestFixture]
    public class PngPdfPackerExtentionsTests : BaseExtentionsTests
    {
        private Mock<ISvgConverter> ConverterMoq;

        [SetUp]
        public void SetUp()
        {
            base.SetUp();
            ConverterMoq=new Mock<ISvgConverter>();
            this.context.CurrentDesign.locations.First().svg = "<svg height=\"100\" width=\"200\"/>";
            this.context.WorkingFolder=new Mock<IWorkingFolder>().Object;
        }
    


        protected override void InvokeExtentionMethod()
        {
            _Packer.GeneratePng(ConverterMoq.Object);
        }

        [Test]
        public void GeneratePng_ExecuteConverter_WithValidFileNames()
        {
            
            //action
            InvokePngGeneration();

            //assert
            
            ConverterMoq.Verify(m=>m.Convert("Front.svg", "Front.png",It.IsAny<int>(),It.IsAny<int>()),Times.Once);
            
        }

        [Test]
        public void GeneratePng_ExecuteConverter_With_WidthHeight_FromLocationXml()
        {
            //arrange
            

            //action
            InvokePngGeneration();
            

            //assert
            ConverterMoq.Verify(m => m.Convert(It.IsAny<string>(), It.IsAny<string>(), 200,100), Times.Once);

        }


        [Test]
        public void GeneratePdf_ExecuteConverter_WithValidFileNames()
        {

            //action
            InvokePdfGeneration();

            //assert

            ConverterMoq.Verify(m => m.Convert("Front.svg", "Front-UNITS.pdf", It.IsAny<int>(), It.IsAny<int>()), Times.Once);

        }


        private void InvokePngGeneration()
        {
            this.InvokeExtentionMethod();
            this.targetAction(this.context);
        }

        private void InvokePdfGeneration()
        {
            _Packer.GeneratePdf(ConverterMoq.Object);
            this.targetAction(this.context);
        }

    }
    
}
