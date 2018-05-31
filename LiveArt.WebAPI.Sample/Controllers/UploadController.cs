using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using LiveArt.Data.Json.Upload;
using LiveArt.WebAPI.Sample.Utils;

namespace LiveArt.WebAPI.Sample.Controllers
{
    public class UploadController : ApiController
    {
        public const string UPLOAD_IMAGES_FOLDER = "~/Files/UploadedImages";
        private readonly string UploadFolderPhysicalPath = System.Web.Hosting.HostingEnvironment.MapPath(UPLOAD_IMAGES_FOLDER);

        protected const bool DEFAULT_USE_ABSOLUTE_URL = true; // set true for cross-domain service usage 

        public UploadController()
        {
            System.IO.Directory.CreateDirectory(UploadFolderPhysicalPath);// ensure upload folder exists
        }

        

        [HttpPost]
        public async Task<UploadImageResponse> Image(bool useAbsoluteUrl = DEFAULT_USE_ABSOLUTE_URL)
        {
            
            if (!Request.Content.IsMimeMultipartContent())
            {
                return await DownloadImage(useAbsoluteUrl);
            }

            var provider = new MultipartFormDataStreamProvider(UploadFolderPhysicalPath);

            
            try
            {
                
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);

                var uploadedFiles = provider.FileData.Select(f => BuildFileInfo(f, useAbsoluteUrl));

                Func<UploadedFileInfo,bool> ValidImagePredicate=(fInfo)=>IsImage(fInfo) && fInfo.InputFieldName=="image";
                Func<UploadedFileInfo, bool> NotValidImagePredicate = (fInfo) => !ValidImagePredicate(fInfo);

                // delete/ignore all invalid uploaded files
                foreach(var toIgnoreFile in uploadedFiles.Where(NotValidImagePredicate))File.Delete(toIgnoreFile.TmpFilePath);
                
                // process valid uploaded image
                var uploadedImage = uploadedFiles.FirstOrDefault(ValidImagePredicate); // 
                if(uploadedImage!=null){

                    File.Move(uploadedImage.TmpFilePath, uploadedImage.UniqueFilePath);// make it presistent
                    return new UploadImageResponse()
                    {
                        Url = uploadedImage.UniqueResolvedUrl
                    };
                } else {
                    throw new UploadedImageNotFound();
                }

            }
            catch
            {
                throw new UploadException();
            }
        }

        private async Task<UploadImageResponse> DownloadImage(bool useAbsoluteUrl)
        {
            var form = await Request.Content.ReadAsFormDataAsync();
            var fileUrl = form["fileurl"]; // yaki, but we can't use default binding 

            if(string.IsNullOrEmpty(fileUrl)) throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType); // it's not upload or addByUrl request

            //if you need real file name, you can do it like this: http://stackoverflow.com/questions/20492355/get-original-filename-when-downloading-with-webclient
            
            var sourceUrl=new Uri(fileUrl);
            var sourceFileInfo=new FileInfo(sourceUrl.AbsolutePath);
            if(!IsImageExtention(sourceFileInfo.Extension))throw new InvalidImageFormat();

            var targetFileName=GetUniqueFileName(sourceFileInfo.Name);
            var targetPath = Path.Combine(UploadFolderPhysicalPath, targetFileName);
            var relativeTargetPath = Path.Combine(UPLOAD_IMAGES_FOLDER, targetFileName);
            
            using (var wc=new WebClient())
            {
                await wc.DownloadFileTaskAsync(fileUrl, targetPath);
                if(!File.Exists(targetPath)) throw  new UploadedImageNotFound(); //re-check filed downloaded
                return new UploadImageResponse()
                {
                    Url = useAbsoluteUrl ? ResolveAsAbsoluteUrl(relativeTargetPath) : ResolveAsRelativeFromRootUrl(relativeTargetPath)
                };
            }

        }


       

        private UploadedFileInfo BuildFileInfo(MultipartFileData fileData, bool useAbsoluteUrl)
        {
            var fileName=fileData.Headers.ContentDisposition.FileName;
            
             var fileInfo = new UploadedFileInfo();
             fileInfo.FileName=fileData.Headers.ContentDisposition.FileName.Replace("\"", string.Empty); //this is here because Chrome submits files in quotation marks which get treated as part of the filename and get escaped
             fileInfo.TmpFilePath=fileData.LocalFileName;

             fileInfo.UniqueFileName = GetUniqueFileName(fileInfo.FileName);
             fileInfo.UniqueFilePath = Path.Combine(UploadFolderPhysicalPath, fileInfo.UniqueFileName);
             fileInfo.UniqueRootedFilePath = Path.Combine(UPLOAD_IMAGES_FOLDER, fileInfo.UniqueFileName);  // path like "~/uploadFOlder/filename.jpg"

             fileInfo.Extention = Path.GetExtension(fileInfo.UniqueFileName).ToLower();
             fileInfo.InputFieldName = fileData.Headers.ContentDisposition.Name.Replace("\"", string.Empty).ToLower(); // toto parse 

             fileInfo.UniqueResolvedUrl = useAbsoluteUrl ? ResolveAsAbsoluteUrl(fileInfo.UniqueRootedFilePath) : ResolveAsRelativeFromRootUrl(fileInfo.UniqueRootedFilePath);


             return fileInfo;
        }

  



        #region DTO objects

        private class UploadedFileInfo{
            public string FileName { get; set; } // file name from user
            public string TmpFilePath { get; set; } // file name MultipartFormDataStreamProvider

            public string UniqueFileName { get; set; } // resulted file name in upload folders
            public string UniqueFilePath { get; set; }
            public string UniqueRootedFilePath { get; set; } // path like "~/uploadFOlder/filename-GUID.jpg"

            public string Extention { get; set; }
            public string InputFieldName { get; set; }

            public string UniqueResolvedUrl { get; set; }
            
        }


        #endregion 

        #region Utils

        

        protected string ResolveAsAbsoluteUrl(string rootedFilePath)
        {
            return RequestContext.Url.Content(rootedFilePath);
        }

        protected string ResolveAsRelativeFromRootUrl(string rootedFilePath)
        {
            var baseUrlLength = ResolveAsAbsoluteUrl("~/").Length;
            return ResolveAsAbsoluteUrl(rootedFilePath).Remove(0, baseUrlLength-1);
        }

        private string GetUniqueFileName(string fileName)
        {

            var uniqFileName = fileName; // if file not exists yet it original name used

            var fullPath = Path.Combine(UploadFolderPhysicalPath, fileName);
            if (File.Exists(fullPath))
            { // can't use original name
                // generate new file name
                var nameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                var extension = Path.GetExtension(fileName); // ".jpg", ".png" e.t.c

                uniqFileName = string.Format("{0}-{1}{2}", nameWithoutExtension, Guid.NewGuid(), extension);
            }

            return uniqFileName;

        }

        private bool IsImage(UploadedFileInfo fileInfo)
        {
            return IsImageExtention(fileInfo.Extention);
        }

        private bool IsImageExtention(string ext)
        {
            string[] imagesExts = new[] { ".png", ".jpeg", ".jpg", ".png", ".svg", ".gif" }; // checking content-type more "right way", but check extention is  easier
            return imagesExts.Contains(ext);
        }
        #endregion

        #region Exceptions
        public class UploadException : LiveArtException // unknow error during upload
        {
            public UploadException () : base("Can't upload image", System.Net.HttpStatusCode.InternalServerError)
            {
            }
        }

        public class UploadedImageNotFound : LiveArtException 
        {
            public UploadedImageNotFound() : base("valid image not uploaded") { }
        }

        public class InvalidImageFormat : LiveArtException
        {
            public InvalidImageFormat() : base("Incorrect image type!", System.Net.HttpStatusCode.InternalServerError)
            {
            }
        }
        #endregion

    }
}
