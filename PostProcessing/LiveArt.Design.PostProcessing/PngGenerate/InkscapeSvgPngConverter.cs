using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveArt.Design.PostProcessing.PngGenerate
{
    public class InkscapeSvgPngConverter: InskscapeBaseConverter
    {
        public InkscapeSvgPngConverter(IExternalTools inkscape) : base(inkscape)
        {
        }

        protected override string BuildInkscapeArguments(string svgSourceFilePath, string pngOutFilePath, int width, int height)
        {
            var argsStr = $"\"{svgSourceFilePath}\" -e \"{pngOutFilePath}\" -a 0:0:{width}:{height}";
            return argsStr;
        }

    }
}
