using System.IO;
using System.IO.Compression;
using FluentAssertions;
using LiveArt.Design.PostProcessing;
using NUnit.Framework;

namespace LiveArt.Design.PostProcessing.Tests
{
    [TestFixture]
    public class ZipWrapperTests
    {
        private string _WorkingDirFullPath;

        private string SourceFolderForPack;
        private string TargetZipName;

        [SetUp]
        public void SetUp()
        {
            _WorkingDirFullPath = Path.GetFullPath("TestWorkFolder/WorkingFolfer");

            PrepareSourcesForPack();
        }

        [TearDown]
        public void TearDown()
        {
            var parentWorkingFolder = Path.GetFullPath("TestWorkFolder");
            if (Directory.Exists(parentWorkingFolder)) Directory.Delete(parentWorkingFolder, true);
        }

        [Test]
        public void Zip_Should_Contains_OnlyFiles()
        {
            DoZipAction();

            using (var zip = ZipFile.OpenRead(TargetZipName))
            {
                zip.Entries.Count.Should().Be(2, "only files added to zip, not folder"); // this is default behavior of IO.Compression, but not TotalCommander.zip
            }
        }

        [Test]
        public void Zip_Should_NotContains_Backward_Slash_InFileNames()
        {
            DoZipAction();

            using (var zip = ZipFile.OpenRead(TargetZipName))
            {
                zip.Entries.Should().NotContain(entry => entry.FullName.Contains("\\"),"it's can't extract on unix/max system");
            }
        }



        private void DoZipAction()
        {
            TargetZipName = Path.Combine(_WorkingDirFullPath, "ZipWrapperResult.zip");
            var zipWrapper=new ZipWrapper(); // class under test
            zipWrapper.CreateFromDirectory(SourceFolderForPack,TargetZipName);

        }


        private void PrepareSourcesForPack()
        {

            SourceFolderForPack = Path.Combine(_WorkingDirFullPath, "SourceForPack");
            Directory.CreateDirectory(SourceFolderForPack);

            File.WriteAllText(Path.Combine(SourceFolderForPack,"root.txt"),"File in root");


            var subFolderPath = Path.Combine(SourceFolderForPack, "SubFolder");
            Directory.CreateDirectory(subFolderPath);

            File.WriteAllText(Path.Combine(subFolderPath, "fileInSubFolder.txt"), "File in folder");

        }

    }
}
