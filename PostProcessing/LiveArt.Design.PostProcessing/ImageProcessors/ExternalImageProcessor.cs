using System;
using System.IO;
using System.Linq;
using LiveArt.Design.PostProcessing.Packer;

namespace LiveArt.Design.PostProcessing.ImageProcessors
{
    internal class ExternalImageProcessor:IImageProcessor
    {

        
        private string[] ImagesExtentions=new []{
                                ".png",".jpg",".jpeg",".gif",".bmp"
                             };


        private const string ExtentionForNonImage = ".png";

        private IWebClientWrapper _WebClient;

        public ExternalImageProcessor(IWebClientWrapper webClient)
        {
            this._WebClient = webClient;
        }

        private PackingContext _context;

        public void Process(IImage image, PackingContext context)
        {
            this._context = context; // to use in private fields
            if (this.CanProcess(image, context))
            {
                var externalUrl = image.Url;
                var relativePath = ConvertExternalUrlToRelativePath(externalUrl);
                var relativeUrl =relativePath.Replace("\\", "/");

                bool needUpdateUrl=true;// if error keep original url
                if (!context.WorkingFolder.FileExists(relativePath))
                {
                    var fullLocalPath = context.WorkingFolder.GetFullPath(relativePath);
                    context.Logger.Log("Try download '{0}' to '{1}'", externalUrl,fullLocalPath);
                    try
                    {
                        context.WorkingFolder.EnsureFolderForFileExist(fullLocalPath);
                        _WebClient.DownloadFile(externalUrl, fullLocalPath);
                    }
                    catch (System.Exception ex)
                    {
                        context.Logger.Fail("Fail download '{0}':{1}",externalUrl,ex.ToString());
                        needUpdateUrl=true;
                    }
                        

                } else {
                      context.Logger.Log("Skip download for '{0}'", externalUrl);
                }

                if(needUpdateUrl){
                    image.Url = relativeUrl; // update url in svg
                    context.Logger.Log("Update image url from '{0}', to '{1}'", externalUrl, relativeUrl);
                }
                
            }
        }

        public bool CanProcess(IImage image, PackingContext context)
        {
            return image.Type == ImageTypes.External;
        }

        private string ConvertExternalUrlToRelativePath(string externalUrl)
        {
            var relativePath = Path.Combine(_context.TargetImagesFolder, GetFileNameFromUrl(externalUrl));
            return relativePath;
        }

        private string GetFileNameFromUrl(string url)
        {
            var fileName = GetFileNameByContextResolver(url);
            if (fileName != null) return fileName;
            else return GenerateFileNameFromUrl(url);
        }

        private string GetFileNameByContextResolver(string url)
        {
            if (_context.ResolveUrlToRelativePathCallbacks == null) return null;// resolvers not set, can't resolve
            foreach (var resolver in _context.ResolveUrlToRelativePathCallbacks)
            {
                var fileName = resolver(url);
                if (fileName != null) return MakeFileNameUniq(url,fileName);
            }
            return null;// resolves can't handle this urls
        }

        private string MakeFileNameUniq(string url,string fileNameAndExt)
        {
            var fileExtention = Path.GetExtension(fileNameAndExt);
            var fileNamePath = fileNameAndExt.Remove(fileNameAndExt.Length-fileExtention.Length);
            

            var urlHashCode = (UInt32)url.ToLower().GetHashCode();

            return string.Format("{0}-{1}{2}", fileNamePath, urlHashCode, fileExtention);

        }
        
        private string GenerateFileNameFromUrl(string url){
            var urlWithoutQuery = url.Split('?')[0];

            var fileName = Path.GetFileNameWithoutExtension(urlWithoutQuery);
            var fileExtention = Path.GetExtension(urlWithoutQuery);

            if (!ImagesExtentions.Contains(fileExtention, StringComparer.OrdinalIgnoreCase)) // handle .php, .aspx e.t.c
            {
                fileName += fileExtention;// move script extention to fileNamePath
                fileExtention = ExtentionForNonImage;
            }
            return MakeFileNameUniq(url, fileName + fileExtention);
            
        }

        
    }
}
