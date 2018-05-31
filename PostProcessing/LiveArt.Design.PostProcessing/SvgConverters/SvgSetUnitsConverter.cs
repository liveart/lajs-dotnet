using System.Xml.Linq;
using LiveArt.Design.PostProcessing.Packer;

namespace LiveArt.Design.PostProcessing.SvgConverters
{
    internal class SvgSetUnitsConverter :SvgModifyBase, ISvgConverter
    {
        


        public XDocument Convert(XDocument svgDocument, PackingContext context)
        {
            this._Logger = context.Logger;
            if (UnitIsSet(context) && SizeIsSet(context))
            {
                var size=context.CurrentDesign.product.size;
                convertAttribute(context,svgDocument.Root, "width", size.width);
                convertAttribute(context,svgDocument.Root, "height", size.height);
            }
            return svgDocument;
        }

        private bool SizeIsSet(PackingContext context)
        {
            return context.CurrentDesign != null
                && context.CurrentDesign.product != null
                && context.CurrentDesign.product.size != null;
        }

        private bool UnitIsSet(PackingContext context)
        {
            return context.Config != null
                   && context.Config.options != null
                   && !string.IsNullOrWhiteSpace(context.Config.options.unit);
        }

        private void convertAttribute(PackingContext context,XElement svgRoot,string attrName,double? newAttrValue)
        {
            if (newAttrValue != null && newAttrValue != 0)
            {
               
                var attrValueWithUnits = newAttrValue.ToString().Replace(",",".") + context.Config.options.unit;
                this.SetOrCreateAttribute(svgRoot, attrName, attrValueWithUnits);
            }

        }

      

    }
}
