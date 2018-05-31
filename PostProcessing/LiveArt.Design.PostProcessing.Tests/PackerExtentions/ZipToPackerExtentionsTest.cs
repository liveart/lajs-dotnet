using FluentAssertions;
using LiveArt.Design.PostProcessing.PackerExtentions;
using Moq;
using NUnit.Framework;

namespace LiveArt.Design.PostProcessing.Tests.PackerExtentions
{
    [TestFixture]
    public class ZipToPackerExtentionsTest : BaseExtentionsTests
    {
        
        internal Mock<PostProcessing.IWorkingFolder> mFolder;
        internal Mock<PostProcessing.IZipWrapper> mZip;

        private const string FolderFullPath="c:\\fullpath\\";
        private const string ZipFileName="design.zip";
        private const string ZipFileNameFullPath = FolderFullPath + ZipFileName;
        
        


        [SetUp]
        public void SetUp()
        {
            base.SetUp();

            mFolder = new Mock<PostProcessing.IWorkingFolder>();
            mFolder.Setup(m => m.GetFullPath(ZipFileName)).Returns(ZipFileNameFullPath);
            mFolder.Setup(m => m.GetFullPath("")).Returns(FolderFullPath);
            
            this.context.WorkingFolder = mFolder.Object;
            

            mZip = new Mock<PostProcessing.IZipWrapper>();


        }

        protected override void InvokeExtentionMethod()
        {
            _Packer.ZipTo(ZipFileName,mZip.Object);
        }


        [TestCase(false,true)]
        [TestCase(true, false)]
        public void ZipTo_Action_FullNamePassedToZip(bool zipFileAlreadyExist,bool zipShouldExecute)
        {
            mFolder.Setup(m => m.FileExists(ZipFileName)).Returns(zipFileAlreadyExist);
            InvokeAction();

            //asserts
            if(zipShouldExecute)mZip.Verify(m => m.CreateFromDirectory(FolderFullPath, ZipFileNameFullPath), Times.Once);
            else mZip.Verify(m => m.CreateFromDirectory(FolderFullPath, ZipFileNameFullPath), Times.Never);

        }

        

        [Test]
        public void ZipTo_Action_ShouldSetFullFileNameToResult()
        {
            //arrange
            context.ResolvePathToUrlCallback = (file) => "http://domain/" + file;

            InvokeAction();

            //assert
            var zipFile = context.Result.ZipFile;

            zipFile.FileName.Should().Be(ZipFileName);
            zipFile.Url.Should().Be("http://domain/" + ZipFileName);

        }

        private void InvokeAction()
        {
            this.InvokeExtentionMethod();
            this.targetAction(this.context);
        }

    }
}
