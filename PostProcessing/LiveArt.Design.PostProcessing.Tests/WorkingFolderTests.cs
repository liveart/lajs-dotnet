using System;
using System.IO;
using FluentAssertions;
using LiveArt.Design.PostProcessing;
using NUnit.Framework;

namespace LiveArt.Design.PostProcessing.Tests
{
    [TestFixture]
    public class WorkingFolderTests
    {
        IWorkingFolder target;// class under test
        private string _WorkingDirFullPath;
        
        [SetUp]
        public void SetUp()
        {
            _WorkingDirFullPath=Path.GetFullPath("TestWorkFolder/WorkingFolfer");
            this.target = new WorkingFolder(_WorkingDirFullPath);
            this.target.EnsureExists();
        }

        [TearDown]
        public void TearDown()
        {
            var parentWorkingFolder = Path.GetFullPath("TestWorkFolder");
            if (Directory.Exists(parentWorkingFolder)) Directory.Delete(parentWorkingFolder, true);
        }


        #region FileExists
        [Test]
        public void FileExists_ExistFileShouldReturnTrue()
        {
            this.CreateExistFile("someFile.txt");

            AssertionExtensions.Should((bool) this.target.FileExists("SomeFile.txt")).BeTrue();
        }

        [Test]
        public void FileExists_NotExistFileShouldReturnFalse()
        {
            AssertionExtensions.Should((bool) this.target.FileExists("NotExistFile.txt")).BeFalse();
        }
        #endregion //FileExists

   

        [TestCase("somefile.txt")]
        [TestCase("subfolder\\somefile.txt")]
        [TestCase("subfolder1\\subfolder2\\subfolder3\\somefile.txt")]
        public void WriteText_LocalFile_ShouldCreateFileWithPassedContext(string filePath)
        {

           this.target.WriteText(filePath, "someContext");

            //assert
            File.ReadAllText(Path.Combine(this._WorkingDirFullPath, filePath))
              .Should().Be("someContext");

        }


     
        [Test]
        public  void GetFullPath_ForLocalFile_ShouldReturn(){
            AssertionExtensions.Should((string) target.GetFullPath("somefile.txt")).Be(Path.Combine(this._WorkingDirFullPath,"somefile.txt"));
        }

     

        [Test]
        public void EnsureExists_FolderExists()
        {
            //executed in SetUp() method:target.EnsureExists(); 
            Directory.Exists(this._WorkingDirFullPath)
                .Should().BeTrue();
            Console.WriteLine("Working dir is:{0}",this._WorkingDirFullPath);
        }

        


        #region Utils
        private string CreateExistFile(string fileName){
            var filePath = Path.Combine(this._WorkingDirFullPath, fileName);
            var folderPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            File.WriteAllText(filePath,"some file content");
            return filePath;
        }
        #endregion

    }
}
