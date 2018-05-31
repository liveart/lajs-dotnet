using System.Linq;
using FluentAssertions;
using LiveArt.Design.PostProcessing.PackerExtentions;
using Moq;
using NUnit.Framework;

namespace LiveArt.Design.PostProcessing.Tests.PackerExtentions
{
    [TestFixture]
    public class SaveSvgPreviewPackerExtentionsTests:BaseExtentionsTests
    {
        internal Mock<PostProcessing.IWorkingFolder> mFolder;

      

        [SetUp]
        public void SetUp()
        {
            base.SetUp();
            
            mFolder = new Mock<PostProcessing.IWorkingFolder>();
            this.context.WorkingFolder = mFolder.Object;
        }

        protected override void InvokeExtentionMethod()
        {
            _Packer.SaveSvg();
        }


        [Test]
        public void SaveSvg_LocationName_ShouldBe_SvgFileName()
        {
            InvokeAction();

            //assert
            mFolder.Verify(m => m.WriteText("Front.svg", It.IsAny<string>()), Times.Once);

        }


        [Test]
        public void SaveSvg_LocationSvg_PassedAsFileContent()
        {
            var expectedSvg = this.context.CurrentDesign.locations.First().svg+"";
            InvokeAction();

            //assert
            mFolder.Verify(m => m.WriteText(It.IsAny<string>(), expectedSvg), Times.Once);

        }

        [Test]
        public void SaveSvg_Action_ShouldResolveUrl([Values(true,false)]bool fileExists)
        {
            //arrange
            var svgName="Front.svg";
            var baseUrl = "http://domain/";
            mFolder.Setup(m => m.FileExists(svgName)).Returns(fileExists);
            context.ResolvePathToUrlCallback = (file) => baseUrl + svgName;

            //action
            InvokeAction();

            //asserts
            var svg=this.context.Result.Locations.First().SvgFiles.First();
            svg.FileName.Should().Be(svgName);
            svg.Url.Should().Be("http://domain/Front.svg");
            
        }

       
        private void InvokeAction(){
            this.InvokeExtentionMethod();
            this.targetAction(this.context);
        }

    }


    
}
