using System;
using System.Linq;
using System.Xml.Linq;

namespace LiveArt.Design.PostProcessing.ImageProcessors
{

    public enum ImageTypes { Unknow, External, Internal };
    internal interface IImage
    {
        string Url { get; set; }
        ImageTypes Type{get;}
        
    }
    internal class Image:IImage
    {
        

        public XElement Element { get; protected set; }
        private XAttribute _Href { get; set; }

        public ImageTypes Type
        {
            get
            {
                var url = this.Url;
                if (string.IsNullOrWhiteSpace(url)) return ImageTypes.Unknow;
                url = this.Url.Trim();
                if (url.StartsWith("//")) return ImageTypes.External;

                var uri = new Uri(url, UriKind.RelativeOrAbsolute);
                return uri.IsAbsoluteUri?ImageTypes.External:ImageTypes.Internal;
            }
        }
     
        public string Url {
            get{
                return _Href!=null?_Href.Value:null;
            }
            set
            {
                if (this._Href == null)
                {
                    this._Href = new XAttribute("href", value);
                    this.Element.Add(this._Href);
                }
                else this._Href.SetValue(value);
            }
        }

    

        public Image(XElement imageXElement)
        {
            this.Element = imageXElement;
            this._Href = imageXElement.Attributes().Where(ia => ia.Name.LocalName == "href").FirstOrDefault();

        }


        
    }
}
