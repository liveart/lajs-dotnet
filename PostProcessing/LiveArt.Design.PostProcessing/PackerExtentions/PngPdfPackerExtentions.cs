using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveArt.Design.PostProcessing.Domain;
using LiveArt.Design.PostProcessing.Logging;
using LiveArt.Design.PostProcessing.Packer;
using LiveArt.Design.PostProcessing.PngGenerate;

namespace LiveArt.Design.PostProcessing.PackerExtentions
{
    public static class PngPdfPackerExtentions
    {
        #region Generate Png
        public static IDesignPacker GeneratePng(this IDesignPacker packer, string pathToInkscape)
        {
            var inkscape = new ExternalTools(pathToInkscape);
            var converter = new InkscapeSvgPngConverter(inkscape);
            return packer.GeneratePng(converter);
        }

        internal static IDesignPacker GeneratePng(this IDesignPacker packer, ISvgConverter pngConverter)
        {

            return packer.GeneratePng(
                safeLocationName => $"{safeLocationName}.png",
                safeLocationName => $"{safeLocationName}.svg",
                pngConverter
                );
        }


        internal static IDesignPacker GeneratePng(this IDesignPacker packer, Func<string, string> getOutPngFileName,
            Func<string, string> getSvgSourceFileName, ISvgConverter pngConverter)
        {
            return packer.Generate("GeneratePng",getOutPngFileName, getSvgSourceFileName, pngConverter, 
                (location,pngFile)=>location.PngPreview=pngFile
                );
        }

        #endregion

        #region Generate Pdf
        public static IDesignPacker GeneratePdf(this IDesignPacker packer, string pathToInkscape)
        {
            var inkscape = new ExternalTools(pathToInkscape);
            var converter = new InkscapeSvgPdfConverter(inkscape);
            return packer.GeneratePdf(converter);
        }

        internal static IDesignPacker GeneratePdf(this IDesignPacker packer, ISvgConverter PdfConverter)
        {

            return packer.GeneratePdf(
                safeLocationName => $"{safeLocationName}-UNITS.pdf",
                safeLocationName => $"{safeLocationName}-UNITS.svg",
                PdfConverter
                );
        }


        internal static IDesignPacker GeneratePdf(this IDesignPacker packer, Func<string, string> getOutPdfFileName,
            Func<string, string> getSvgSourceFileName, ISvgConverter PdfConverter)
        {
            return packer.Generate("GeneratePdf", getOutPdfFileName, getSvgSourceFileName, PdfConverter,
                (location, PdfFile) => location.Pdf = PdfFile
                );
        }

        #endregion

        internal static IDesignPacker Generate(this IDesignPacker packer,string logActionName, Func<string, string> getOutPngFileName,
            Func<string, string> getSvgSourceFileName, ISvgConverter pngOrPdfConverter,Action<DesignPack.Location,DesignPack.File> setResultFileAction )
        {
            return packer.RegBeforePackAction(context =>
            {
                var folderFullPath = context.WorkingFolder.GetFullPath("");
                pngOrPdfConverter.SetWorkingFolder(folderFullPath);

                foreach (var location in context.CurrentDesign.locations)
                {
                    context.Logger
                        .DoActionWithLog(logActionName, location.name, () =>
                        {
                            
                            if (location.height == null || location.width == null)
                            {
                                //extact with/height from svg, hope  UpdateViewBox() was executed
                                var xml = Utils.XmlFromString(location.svg);
                                location.width = int.Parse(xml.Root.Attribute("width").Value);
                                location.height = int.Parse(xml.Root.Attribute("height").Value);
                            }
                            



                            // arrange file names

                            var locationSafeName = Utils.GetSafeLocationName(location.name);
                            var svgFileName = getSvgSourceFileName(locationSafeName);
                            var pngFileName = getOutPngFileName(locationSafeName);
                            //do action
                            var result=pngOrPdfConverter.Convert(svgFileName, pngFileName, (int)location.width, (int)location.height);

                            // copy result to logger
                            foreach (var line in result.Messages)context.Logger.Log("inkscape>"+line);
                            foreach (var line in result.Errors) context.Logger.Fail("inkscape>" + line);

                            if (!result.Success) context.Logger.Fail($"Fail generate {pngFileName}");
                            else
                            {
                                var pngOrPdfFile = new DesignPack.File();
                                pngOrPdfFile.FileName = pngFileName;
                                pngOrPdfFile.Url = context.ResolveToUrl(pngOrPdfFile.FileName);

                                var resultLocation = context.Result.GetResultLocation(location.name);
                                setResultFileAction(resultLocation, pngOrPdfFile);

                            }

                        });
                }
            });
        }
       



    }
}
