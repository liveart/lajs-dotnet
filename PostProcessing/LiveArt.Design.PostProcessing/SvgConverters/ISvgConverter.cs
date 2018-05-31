using System.Xml.Linq;
using LiveArt.Design.PostProcessing.Packer;

namespace LiveArt.Design.PostProcessing.SvgConverters
{
    internal interface ISvgConverter
    {
        XDocument Convert(XDocument svgDocument,PackingContext context);
    }
}
