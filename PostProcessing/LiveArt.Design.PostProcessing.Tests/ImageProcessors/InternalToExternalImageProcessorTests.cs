using FluentAssertions;
using LiveArt.Design.PostProcessing.ImageProcessors;
using LiveArt.Design.PostProcessing.Packer;
using NUnit.Framework;

namespace LiveArt.Design.PostProcessing.Tests.ImageProcessors
{
    [TestFixture]
    public class InternalToExternalImageProcessorTests
    {

        IImageProcessor target;// class under test
        PackingContext convertContext;

        [SetUp]
        public void SetUp()
        {
            this.target = new InternalToExternalImageProcessor();
            convertContext=new PackingContext(){
                SourceImagesBaseUrl = "http://domain/designs/designFolder/" 

            };
        }

        [TestCase("someImage.png", "http://domain/designs/designFolder/someImage.png")]
        [TestCase("someFolder/someImage.png", "http://domain/designs/designFolder/someFolder/someImage.png")]
        [TestCase("someFolder\\someImage.png", "http://domain/designs/designFolder/someFolder/someImage.png")]
        [TestCase("../extFolder/someImage.png", "http://domain/designs/extFolder/someImage.png")]
        [TestCase("someScript.php?param=val1&val=2", "http://domain/designs/designFolder/someScript.php?param=val1&val=2")]
        [TestCase("someScript.php?param=val1&val=2", "http://domain/designs/designFolder/someScript.php?param=val1&val=2")]
        public void Convert_Image(string inputLocalUrl,string expectedFullUrl)
        {
            var image = new ImageStub() { Type = ImageTypes.Internal, Url = inputLocalUrl };
            //do convert
            this.target.Process(image, convertContext);

            image.Url
                 .Should().Be(expectedFullUrl);
            
        }


        [TestCase(ImageTypes.External,false)]
        [TestCase(ImageTypes.Internal, true)]
        [TestCase(ImageTypes.Unknow, false)]
        public void CanProcess(ImageTypes imageType, bool expectedCanProcess)
        {
            var image = new ImageStub(){Type = imageType};

            this.target.CanProcess(image, convertContext)
                .Should().Be(expectedCanProcess);
        }

      
    }
}
