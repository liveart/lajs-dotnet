using System.IO;

namespace LiveArt.Design.PostProcessing
{
    internal interface IWorkingFolder
    {
        
        bool FileExists(string filePath);
      
        /// <summary>
        ///   if folder not exists - it will be created
        /// </summary>
        void EnsureExists();
        void WriteText(string localFileName, string fileContent);
        string GetFullPath(string localFileName);
        void EnsureFolderForFileExist(string fullFilePath);
    }

    internal class WorkingFolder:IWorkingFolder
    {
        public string FolderFullPath;

        public WorkingFolder(string folderFullPath)
        {
            this.FolderFullPath = folderFullPath;
        }
        
       
        public bool FileExists(string filePath)
        {
            return File.Exists(this.GetFullPath(filePath));
        }
       
        
        public void EnsureExists()
        {
            this.EnsureFolderExist(this.FolderFullPath);
        }

      
        public void WriteText(string localFileName, string fileContent)
        {
            var fullFilePath=(GetFullPath(localFileName));
            this.EnsureFolderForFileExist(fullFilePath);
            File.WriteAllText(fullFilePath, fileContent);
        }
      
        public string GetFullPath(string localFileName)
        {
            return Path.Combine(this.FolderFullPath, localFileName);
        }

        public void EnsureFolderForFileExist(string fullFilePath)
        {
            var fileName = Path.GetFileName(fullFilePath);
            var folderFullPath = fullFilePath.Remove(fullFilePath.Length - fileName.Length);
            this.EnsureFolderExist(folderFullPath);
        }
        private void EnsureFolderExist(string folderPath){
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }

       
    }
}
