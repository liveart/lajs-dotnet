using System.Xml.Linq;
using LiveArt.Design.PostProcessing.Domain;
using LiveArt.Design.PostProcessing.Packer;

namespace LiveArt.Design.PostProcessing.SvgConverters
{
    internal class SvgViewBoxUpdater : SvgModifyBase,ISvgConverter
    {
     

        public XDocument Convert(XDocument svgDocument, PackingContext context)
        {
            this._Logger = context.Logger;
            var loc = context.CurrentLocation;
            if (!string.IsNullOrWhiteSpace(loc.editableArea)) { 
                var rect=Rect.ParseFromTextArea(loc.editableArea);
                var svg = svgDocument.Root;


                ////comment from order.php: set new x,y attrs (for proper AI CS5 render, AI CS6 - incorrect)
                SetOrCreateAttribute(svg, "x", rect.X);
                SetOrCreateAttribute(svg, "y", rect.Y);
                SetOrCreateAttribute(svg, "width", rect.Width);
                SetOrCreateAttribute(svg, "height", rect.Height);

                SetOrCreateAttribute(svg, "viewBox", rect.ToViewBoxString());

            }
            else
            {
                Log("Skip update viewBox, {0}.editableArea not found", loc.name); 
            }
            return svgDocument;
        }

    }
}
