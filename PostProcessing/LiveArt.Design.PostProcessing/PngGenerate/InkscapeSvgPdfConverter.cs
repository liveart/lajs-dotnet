using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveArt.Design.PostProcessing.PngGenerate
{
    public class InkscapeSvgPdfConverter: InskscapeBaseConverter
    {
        public InkscapeSvgPdfConverter(IExternalTools inkscape) : base(inkscape)
        {
        }

        protected override string BuildInkscapeArguments(string svgSourceFilePath, string pdfOutFilePath, int width, int height)
        {
            var argsStr = $"-T \"{svgSourceFilePath}\" -A \"{pdfOutFilePath}\" -a 0:0:{width}:{height} -w {width} -h {height}";
            return argsStr;
        }
    }
}
