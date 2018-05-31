using FluentAssertions;
using NUnit.Framework;
using LiveArt.Design.PostProcessing.PackerExtentions;

namespace LiveArt.Design.PostProcessing.Tests.PackerExtentions
{
    [TestFixture]
    public class DownloadImagesMethodTests : SvgConverterPackerExtentionsTests
    {

        protected override void InvokeExtentionMethod()
        {
            _Packer.DownloadImages(this.mSvgConverter.Object, "http://someExternalUrl");
        }

        protected override void IvokeActionWithDefaultParams()
        {
            executeDownloadAction("http://someUrl");
        }


        [Test]
        public void DownloadImages_ShouldSetOriginalUrlToContext()
        {
            //action
            executeDownloadAction("origDesignUrl");

            context.SourceImagesBaseUrl
                .Should().Be("origDesignUrl");
        }


        [Test]
        public void DownloadImages_ShouldSetInContext_TargetImagesFolder_Default_IfNotPassed()
        {
            //action
            executeDownloadAction("origDesignUrl");

            context.TargetImagesFolder
                .Should().Be("sources");//it's default value
        }


        [Test]
        public void DownloadImages_ShouldSetInContext_TargetImagesFolder_IfNotPassed()
        {
            //action
            executeDownloadAction("origDesignUrl", "imagesSubFolder");

            context.TargetImagesFolder
                .Should().Be("imagesSubFolder");
        }


        private void executeDownloadAction(string baseDesignUrl, string targetImageFolder = null)
        {
            if (targetImageFolder == null) _Packer.DownloadImages(this.mSvgConverter.Object, baseDesignUrl);
            else _Packer.DownloadImages(this.mSvgConverter.Object, baseDesignUrl, targImageFolder:targetImageFolder);

            targetAction(context);
        }
        

    }
}
