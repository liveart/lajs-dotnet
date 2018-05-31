using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveArt.Design.PostProcessing.PngGenerate
{
    internal interface ISvgConverter
    {
        bool CanConvert();
        SvgConverterResult Convert(string svgSourceFilePath, string pngOutFilePath,int width,int height);
        void SetWorkingFolder(string workingFolderFullPath);

    }


    public class SvgConverterResult
    {
        public bool Success { get; set; }
        public List<string> Messages { get; set; }
        public List<string> Errors { get; set; }
    }
}
