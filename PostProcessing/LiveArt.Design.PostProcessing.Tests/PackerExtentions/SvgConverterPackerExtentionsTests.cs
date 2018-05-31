using System.Linq;
using System.Xml.Linq;
using FluentAssertions;
using LiveArt.Design.PostProcessing.Domain;
using LiveArt.Design.PostProcessing.Packer;
using LiveArt.Design.PostProcessing.SvgConverters;
using Moq;
using NUnit.Framework;

namespace LiveArt.Design.PostProcessing.Tests.PackerExtentions
{

    public abstract class SvgConverterPackerExtentionsTests : BaseExtentionsTests
    {
        internal Mock<ISvgConverter> mSvgConverter;
        

        [SetUp]
        public void SetUp()
        {
            base.SetUp();

            this.mSvgConverter = new Mock<ISvgConverter>();



        }

        [Test]
        public void ParsedSvgShouldPassedToProcess()
        {
            //arrange
            XDocument passedSvg = null;
            mSvgConverter.Setup(m => m.Convert(It.IsAny<XDocument>(), It.IsAny<PackingContext>()))
                .Returns<XDocument, PackingContext>((xSvg, context) =>
                {
                    passedSvg = xSvg;
                    return xSvg;
                });

            //action
            IvokeActionWithDefaultParams();
            
            
            //assert
            mSvgConverter.Verify(m => m.Convert(It.IsAny<XDocument>(), It.IsAny<PackingContext>()));

            passedSvg.Should().NotBeNull()
                    .And.Subject.Should().NotBeNull();

        }


        [Test]
        public void SvgStrShouldBeUpdated()
        {
            //arrange
            var updatedXmlStr = "<svg>updated</svg>";
            XDocument updatedSvg = XDocument.Parse("<svg>updated</svg>");
            mSvgConverter.Setup(m => m.Convert(It.IsAny<XDocument>(), It.IsAny<PackingContext>()))
                                    .Returns<XDocument, PackingContext>((xSvg, context) => updatedSvg);

            

            //action
            IvokeActionWithDefaultParams();
            
            //assert
            this.context.CurrentDesign.locations.First()
                .svg
                    .Should().Be(updatedXmlStr);


        }

      



        [Test]
        public void ShouldSetCurrentLocationInContext()
        {
            Location passedCurrentLocation=null;
            mSvgConverter.Setup(m=>m.Convert(It.IsAny<XDocument>(),It.IsAny<PackingContext>()))
                                .Callback<XDocument, PackingContext>((doc, con) => passedCurrentLocation = con.CurrentLocation);
            //action
            IvokeActionWithDefaultParams();

            passedCurrentLocation
                .Should().NotBeNull();
        }
        [Test]
        public void ShouldClearCurrentLocationInContextAfterAction()
        {
            //action
            IvokeActionWithDefaultParams();

            context.CurrentLocation
                .Should().BeNull();
        }


        protected abstract void IvokeActionWithDefaultParams();



      
      
       
    }
}
