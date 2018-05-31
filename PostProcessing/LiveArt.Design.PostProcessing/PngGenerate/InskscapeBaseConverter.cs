using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveArt.Design.PostProcessing.PngGenerate
{
    public abstract class InskscapeBaseConverter: ISvgConverter
    {
        private readonly IExternalTools Inkscape;

        protected InskscapeBaseConverter(IExternalTools inkscape)
        {
            Inkscape = inkscape;
        }

        public bool CanConvert()
        {
            return Inkscape.CanExecute();
        }

        protected abstract string BuildInkscapeArguments(string svgSourceFilePath, string svgOrPdfPngOutFilePath, int width, int height);
        public SvgConverterResult Convert(string svgSourceFilePath, string svgOrPdfPngOutFilePath, int width, int height)
        {
            var argsStr = BuildInkscapeArguments(svgSourceFilePath, svgOrPdfPngOutFilePath, width, height);
            var result = Inkscape.Execute(argsStr);
            return new SvgConverterResult
            {
                Success = result.Success,
                Messages = result.StdOut,
                Errors = result.StrError
            };
        }

        

        public void SetWorkingFolder(string workingFolderFullPath)
        {
            this.Inkscape.SetWorkingFolder(workingFolderFullPath);
        }
    }
}
