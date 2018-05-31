using System;
using System.Text.RegularExpressions;
using LiveArt.Design.PostProcessing.Logging;
using LiveArt.Design.PostProcessing.Packer;

namespace LiveArt.Design.PostProcessing.PackerExtentions
{
    public static class SaveSvgPreviewPackerExtentions
    {
        public static IDesignPacker SaveSvg(this IDesignPacker packer)
        {
            return packer.SaveSvg((safeLocationName) => string.Format("{0}.svg", safeLocationName));
        }
        public static IDesignPacker SaveSvg(this IDesignPacker packer,Func<string,string> getPreviewFileName)
        {

            return packer.RegBeforePackAction((context) =>
            {
                foreach (var location in context.CurrentDesign.locations)
                {
                    context.Logger
                        .DoActionWithLog("SaveSvg", location.name, () =>
                        {
                            var svgFile = new DesignPack.File();

                            svgFile.FileName = getPreviewFileName(Utils.GetSafeLocationName(location.name));
                            context.WorkingFolder.WriteText(svgFile.FileName, location.svg);

                            svgFile.Url = context.ResolveToUrl(svgFile.FileName);
                            var resultLocation = context.Result.GetResultLocation(location.name);

                            resultLocation.SvgFiles.Add(svgFile);
                        });
                }

            });
        }


    }
}
