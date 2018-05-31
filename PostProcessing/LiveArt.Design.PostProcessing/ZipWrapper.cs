using System.IO.Compression;

namespace LiveArt.Design.PostProcessing
{
    internal interface IZipWrapper
    {
        void CreateFromDirectory(string fullSourceDirectoryName, string fullDestinationArchiveFileName);

    }

    internal class ZipWrapper:IZipWrapper
    {
        public void CreateFromDirectory(string fullSourceDirectoryName, string fullDestinationArchiveFileName)
        {

            var tempZipFile=System.IO.Path.GetTempFileName()+".zip";
            System.IO.Compression.ZipFile.CreateFromDirectory(fullSourceDirectoryName, tempZipFile,CompressionLevel.Optimal, false, new ZipFileNameEncoder());
            System.IO.File.Move(tempZipFile, fullDestinationArchiveFileName);
        }
    }
}
