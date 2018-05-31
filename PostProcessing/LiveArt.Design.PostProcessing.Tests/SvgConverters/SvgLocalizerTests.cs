using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using LiveArt.Design.PostProcessing.ImageProcessors;
using LiveArt.Design.PostProcessing.Packer;
using LiveArt.Design.PostProcessing.SvgConverters;
using Moq;
using NUnit.Framework;

namespace LiveArt.Design.PostProcessing.Tests.SvgConverters
{
    [TestFixture]
    public class SvgLocalizerTests : SvgConverterBaseTest
    {
     
        private SvgLocalizer ttarget { get { return (SvgLocalizer)target; } }// class under test casted to concrete implmementation

        private Mock<IImageProcessor> _moqImageProcessor ;
        private IList<IImage> _processedImages;

        internal Mock<PostProcessing.IWorkingFolder> mFolder;
        
        

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _moqImageProcessor = new Mock<IImageProcessor>();
            _processedImages = new List<IImage>();
            _moqImageProcessor.Setup(m => m.Process(It.IsAny<IImage>(), It.IsAny<PackingContext>()))
                                    .Callback<IImage, PackingContext>((image, context) => this._processedImages.Add(image));

            target = new SvgLocalizer( new [] {_moqImageProcessor.Object});
            this.mFolder = new Mock<PostProcessing.IWorkingFolder>();
            convertContext.WorkingFolder = mFolder.Object;
        }

 

        [Test]
        public void Convert_SvgWithoutImageShuldReturnUnmodified()
        {
            // no conversion required
            var simpleSvg = 
@"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no""?><svg>
  <group>
    <rect x=""0"" y=""0"" width=""400"" height=""400"" />
  </group>
</svg>";

            var ttt = convert(simpleSvg);
            Console.WriteLine(ttt.Replace("\"","\"\""));
            //action
            convert(simpleSvg)
                .Should().Be(simpleSvg); // asserts:no convert expected
        }

        [Test]
        public void Convert_ShouldProcess2Images()
        {
            //arrange
            var svgWithImages = @"
                <svg>
                    <image id=""img_in_root""/> <!-- should return -->
                    <group>
                        <image id=""img_in_child""/> <!-- should return, test recursive -->
                        <img id=""img_invalid_tag_name""/> <!-- should be ignored -->
                    </group>
                
                </svg>
            ";

            

            //action
            convert(svgWithImages);
            
            //asserts
            this._processedImages
                .Should().HaveCount(2)
                .And.NotContain((image) => ((Image)image).Element.Attribute("id").Value.Contains("invalid"))
                ;

        }


        [Test]
        public void Convert_ShouldPsevdoIgnoreCaseInImageTagName()
        {
            // it's not "try" real case insensetive, but.. more realistics

            convert(@"
                      <svg>
                        <image/>
                        <Image/>
                        <IMAGE/>
                      </svg>
                    ");

            //asserts
            this._processedImages
                .Should().HaveCount(3)
                ;

        }

        [Test]
        public void Convert_UpdatedImageUrlShouldBeInResultSvg()
        {
            //arrange
            //reset-up mock
            var inputSvg = @"<svg>\r\n  <image href=""someUrlToUpdate.png"" />\r\n</svg>";

            _moqImageProcessor.Reset();
             _moqImageProcessor.Setup(m => m.Process(It.IsAny<IImage>(),convertContext))
                                    .Callback<IImage, PackingContext>((image, context) => image.Url = "UpdatedUrl.png");

            //action
            var outpuSvg=convert(inputSvg);
//            Console.WriteLine(outpuSvg);
            //assert
            outpuSvg.Should().Be(@"<svg>\r\n  <image href=""UpdatedUrl.png"" />\r\n</svg>");


        }


        

        #region ParseImageType

        private static string SvgWithExternalInternalImages = @"
             <svg xmlns:xlink=""http://www.w3.org/1999/xlink"">
                <!-- local images-->
                <image href=""localFile1.png""/>
                <image href=""/localFile2.png""/>
                
                <!-- unknown (igonred) images -->
                <image/>

                <!-- external images -->
                <image href=""http://external1.png""/>
                <image href=""hTTPs://external2.png""/>
                <image href=""//external3.png""/>
                <image xlink:href=""//external4.png""/>
                
            </svg>
            ";

        [Test]
        public void Convert_ShouldProcess4ExternalImage()
        {
            convert(SvgWithExternalInternalImages);

            //assert
            this._processedImages.Where( image=>image.Type==ImageTypes.External)
                .Should().HaveCount(4);

        }

        [Test]
        public void Convert_ShouldProcess2InternalImage()
        {
            convert(SvgWithExternalInternalImages);

            //asserts
            this._processedImages.Where(image => image.Type == ImageTypes.Internal)
                .Should().HaveCount(2);

        }

        [Test]
        public void Convert_ShouldProcess1UnknownImage()
        {
            convert(SvgWithExternalInternalImages);

            //asserts
            this._processedImages.Where(image => image.Type == ImageTypes.Unknow)
                .Should().HaveCount(1);

        }

        
        #endregion

        #region TestFromRealData
        [TestCase("designWithImage.json", 1)]
        public void Convert_RealFromJson(string testDesignJsonFileName, int expectedImagesCount)
        {
            convertFrontFromJsonStr(TestData.getTestDataFile(testDesignJsonFileName));

            //asserts
            this._processedImages
                .Should().HaveCount(expectedImagesCount);
        }

        [TestCase("svg/Front_WithDefaultNameSpace.svg", 1)]
        [TestCase("svg/Front_WithoutDefaultNameSpace.svg", 1)]
        public void Convert_RealFromXml(string svgFileName, int expectedImagesCount)
        {
            convert(TestData.getTestDataFile(svgFileName));

            //asserts
            this._processedImages
                .Should().HaveCount(expectedImagesCount);
        }

      
        #endregion



      



    }

}
