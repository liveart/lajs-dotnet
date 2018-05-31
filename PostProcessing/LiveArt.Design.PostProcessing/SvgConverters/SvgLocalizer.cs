using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using LiveArt.Design.PostProcessing.ImageProcessors;
using LiveArt.Design.PostProcessing.Packer;

namespace LiveArt.Design.PostProcessing.SvgConverters
{
    internal class SvgLocalizer : ISvgConverter
    {
        private IEnumerable<IImageProcessor> _ImageProcessors;

        private XDocument _xml; // current processed xml document


        internal SvgLocalizer(IEnumerable<IImageProcessor> imageProcessors)
        {
            this._ImageProcessors = imageProcessors;
        }

        public SvgLocalizer()
        {
             this._ImageProcessors = new IImageProcessor[]{
                new InternalToExternalImageProcessor(),
                new ExternalImageProcessor(new WebClientWrapper())
            };
            
        }

        
        public XDocument Convert(XDocument svgDocument,PackingContext context)
        {
            this._xml = svgDocument;

            var allImages = this.GetImages();
            foreach (var imageProcessor in this._ImageProcessors)
            {
                foreach (var image in allImages)
                {
                    imageProcessor.Process(image,context);
                }
            }
            
            return this._xml;
        }

        private IEnumerable<Image> GetImages()
        {
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(new NameTable());
            nsmgr.AddNamespace("defns", _xml.Root.Name.NamespaceName);
         

            var images = Enumerable.Empty<Image>();
            foreach (string tagXPath in new[] { "//defns:image", "//defns:Image", "//defns:IMAGE" })
            {
                images=images.Union(_xml.XPathSelectElements(tagXPath,nsmgr).Select(xelement=>new Image(xelement)));
            }
            
            
            return images;
        }

        

       
    }
}
