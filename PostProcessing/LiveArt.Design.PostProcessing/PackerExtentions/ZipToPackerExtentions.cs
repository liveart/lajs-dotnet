using LiveArt.Design.PostProcessing.Logging;
using LiveArt.Design.PostProcessing.Packer;

namespace LiveArt.Design.PostProcessing.PackerExtentions
{
    public static class ZipToPackerExtentions
    {
        public static IDesignPacker ZipTo(this IDesignPacker packer, string zipFileName)
        {
            return packer.ZipTo(zipFileName, new ZipWrapper());
        }

        internal static IDesignPacker ZipTo(this IDesignPacker packer, string zipFileName,IZipWrapper zip)
        {
            packer.RegBeforePackAction((context) =>
            {
                context.Logger.DoActionWithLog("zipTo", zipFileName, () =>
                {

                    var folderFullPath = context.WorkingFolder.GetFullPath("");
                    var zipFileFullPath = context.WorkingFolder.GetFullPath(zipFileName);

                    if (!context.WorkingFolder.FileExists(zipFileName))
                    {
                        zip.CreateFromDirectory(folderFullPath, zipFileFullPath);
                    }
                    else
                    {
                        context.Logger.Log("Skip Zip. '{0}' alredy exist", zipFileName);
                    }

                    context.Result.ZipFile = new DesignPack.File(){
                        FileName=zipFileName,
                        Url = context.ResolveToUrl(zipFileName)
                    };
                    
                });
            });
            
            return packer;
        }
    }
}
