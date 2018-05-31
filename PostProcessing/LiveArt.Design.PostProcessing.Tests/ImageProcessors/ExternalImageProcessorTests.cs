using System;
using System.Text.RegularExpressions;
using FluentAssertions;
using LiveArt.Design.PostProcessing.ImageProcessors;
using LiveArt.Design.PostProcessing.Packer;
using Moq;
using NUnit.Framework;

namespace LiveArt.Design.PostProcessing.Tests.ImageProcessors
{
    [TestFixture]
    public class ExternalImageProcessorTests
    {
        IImageProcessor target;// class under test
        PackingContext convertContext;

        //mocks
        Mock<PostProcessing.IWebClientWrapper> mWebClient;
        Mock<PostProcessing.IWorkingFolder> mFolder;

        [SetUp]
        public void SetUp()
        {
            this.mWebClient = new Mock<PostProcessing.IWebClientWrapper>();
            this.mFolder = new Mock<PostProcessing.IWorkingFolder>();

            this.target = new ExternalImageProcessor(mWebClient.Object);
            convertContext = new PackingContext()
            {
                TargetImagesFolder="sources",
                WorkingFolder=mFolder.Object
            };
        }

        [TestCase(ImageTypes.External, true)]
        [TestCase(ImageTypes.Internal, false)]
        [TestCase(ImageTypes.Unknow, false)]
        public void CanProcess(ImageTypes imageType, bool expectedCanProcess)
        {
            var image = new ImageStub() { Type = imageType };

            this.target.CanProcess(image, convertContext)
                .Should().Be(expectedCanProcess);
        }

        #region Process_RelativeUrl


        [TestCase("http://domain.com/designFolder/someImage.png",@"^sources/someImage-\d+\.png$")]
        [TestCase("http://domain.com/designFolder/someImage.gif?param1=value#urlFragmen", @"^sources/someImage-\d+\.gif$")]
        [TestCase("http://domain.com/designFolder/someImage.PHP?param1=value#urlFragmen", @"^sources/someImage.PHP-\d+\.png$")]
        [TestCase("http://domain.com/designFolder/someImage.PHP?param1=value&param2=otherImage.png", @"^sources/someImage.PHP-\d+\.png$")]


        public void Process_ShouldUpdateUrl_ToRelative(string externalUrl, string relativeUrl)
        {
            converedtUrl(externalUrl)
                .Should().MatchRegex(relativeUrl);
        }


        static Regex regexFilePathFromUrl = new Regex("[\\?&]path=(?<gPath>[^&$]*)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
        [TestCase("download?filesystem=mainFilesystem&path=Masks/choodie_mask.png", @"sources/Masks/choodie_mask-\d+\.png")]
        [TestCase("download?filesystem=mainFilesystem&path=Masks\\choodie_mask.png", @"sources/Masks/choodie_mask-\d+\.png")]
        [TestCase("http://domain.com/designFolder/someImage.png", @"^sources/someImage-\d+\.png$")] // not handled by resorvel should have name "as usal"
        public void Process_ShoulUpdateUrl_ToRelativeBy_ContextFileNameResolver(string externalUrl, string relativeUrl)
        {

            this.convertContext.ResolveUrlToRelativePathCallbacks=new Func<string,string>[]{
                (url)=>{
                    var m = regexFilePathFromUrl.Match(url);
                    return m.Success?m.Groups["gPath"].Value:null;
                }
            };

            converedtUrl(externalUrl)
            .Should().MatchRegex(relativeUrl);
        }

        [Test]
        public void Process_RelativeUrlShould_BeUniqueForDifferentPath()
        {
            var relativeUrl1 = converedtUrl("http://domain1/sampleImage.png");
            var relativeUrl2 = converedtUrl("http://domain2/sampleImage.png");

            relativeUrl1.Should().NotBe(relativeUrl2);
               
        }

        [Test]
        public void Process_RelativeUrlShould_IgnoreCaseOnHashCodeGeneration()
        {
            var relativeUrl1 = converedtUrl("http://domain1/sampleImage.png");
            var relativeUrl2 = converedtUrl("http://DOMAIN1/sampleImage.png");

            relativeUrl1.Should().Be(relativeUrl2);
        }
      
        #endregion

        #region ProcessWithWebClient


        [TestCase(false, true, TestName = "Should DOWNLOAD NOT Exist File")]
        [TestCase(true, false, TestName = "Should SKIP exist file")]
        public void Process_WebClient_Invoke(bool fileExist,bool shouldExecuted)
        {
            //arrange
            var inputUrl = "http://domain1/designFolder/sampleImage.png";
            var relativePath = "sources\\sampleImage-1063029573.png";
            var fullLocalPath = @"c:\packedDesign\someDesign" + relativePath;
            mFolder.Setup(m => m.FileExists(relativePath)).Returns(fileExist);
            mFolder.Setup(m => m.GetFullPath(relativePath)).Returns(fullLocalPath);

            //action
            converedtUrl(inputUrl);

            //assert
            if (shouldExecuted)
            {
                mFolder.Verify(m => m.EnsureFolderForFileExist(fullLocalPath),Times.Once);
                mWebClient.Verify(m => m.DownloadFile(inputUrl, fullLocalPath), Times.Once);
            }
            else mWebClient.Verify(m => m.DownloadFile(inputUrl, fullLocalPath), Times.Never);
        }

        

      

        #endregion ProcessWithWebClient


        #region Utils
        /// <summary>
        ///   return relativeUrl
        /// </summary>
        /// <param name="externalUrl"></param>
        private string converedtUrl(string externalUrl)
        {
            var image = new ImageStub() { Type = ImageTypes.External, Url = externalUrl };

            this.target.Process(image, convertContext);

            return image.Url;
        }
        #endregion


    }
}
