using System;
using LiveArt.Design.PostProcessing.Logging;
using LiveArt.Design.PostProcessing.Packer;
using LiveArt.Design.PostProcessing.SvgConverters;

namespace LiveArt.Design.PostProcessing.PackerExtentions
{
    public static class SvgConverterPackerExtentions
    {

        #region DownloadImages

        public static IDesignPacker DownloadImages(this IDesignPacker packer, string sourceImagesBaseUrl, Func<string, string> filePathFromUrlCallback = null, string targImageFolder = "sources")
        {
            Func<string, string>[] callbacks=null;
            if(filePathFromUrlCallback!=null)callbacks=new []{filePathFromUrlCallback};
            return packer.DownloadImages(sourceImagesBaseUrl, callbacks, targImageFolder);
        }
        public static IDesignPacker DownloadImages(this IDesignPacker packer, string sourceImagesBaseUrl, Func<string, string>[] filePathFromUrlCallbacks = null, string targImageFolder = "sources")
        {
            return packer.DownloadImages(new SvgLocalizer(), sourceImagesBaseUrl, filePathFromUrlCallbacks,targImageFolder);
        }
        internal static IDesignPacker DownloadImages(this IDesignPacker packer, ISvgConverter svgConverter, string sourceImagesBaseUrl, Func<string, string>[] filePathFromUrlCallbacks = null, string targImageFolder = "sources")
        {
            packer.RegBeforePackAction((context) =>
            {
                context.SourceImagesBaseUrl = sourceImagesBaseUrl;
                context.TargetImagesFolder = targImageFolder;
                context.ResolveUrlToRelativePathCallbacks = filePathFromUrlCallbacks;

                doSvgConvertOnEachLocation("localize", context, svgConverter);
                
            });
            return packer;
            
        }
        #endregion

        #region UpdateViewBox
        public static IDesignPacker UpdateViewBox(this IDesignPacker packer)
        {
            return packer.UpdateViewBox(new SvgViewBoxUpdater());
        }

        internal static IDesignPacker UpdateViewBox(this IDesignPacker packer, ISvgConverter svgConverter){

            packer.RegBeforePackAction((context) =>
            {
                doSvgConvertOnEachLocation("updateViewBox", context, svgConverter);

            });
            return packer;
        }
        #endregion //UpdateViewBox


        #region UpdateUnits
        public static IDesignPacker UpdateUnits(this IDesignPacker packer)
        {
            return packer.UpdateUnits(new SvgSetUnitsConverter());
        }

        internal static IDesignPacker UpdateUnits(this IDesignPacker packer, ISvgConverter svgConverter)
        {

            packer.RegBeforePackAction((context) =>
            {
                doSvgConvertOnEachLocation("UpdateUnits", context, svgConverter);

            });
            return packer;
        }
        #endregion


        private static void doSvgConvertOnEachLocation(string actionName,PackingContext context,ISvgConverter svgConverter)
        {
            foreach (var location in context.CurrentDesign.locations)
            {
                context.Logger
                    .DoActionWithLog(actionName, location.name, () =>
                    {
                        context.CurrentLocation = location;

                        var xmlSVG = Utils.XmlFromString(location.svg);
                        var updatedXml = svgConverter.Convert(xmlSVG, context);
                        location.svg = Utils.XmlToString(updatedXml);
                    });
            }
            context.CurrentLocation = null;
        }


     
        
    }
}
