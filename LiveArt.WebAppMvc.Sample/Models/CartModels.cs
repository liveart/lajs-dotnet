using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LiveArt.Data.Json.Common;
using LiveArt.Data.Json.Design;

namespace LiveArt.WebAppMvc.Sample.Models
{
    public class DesignDetailsViewModel
    {
        public string DesignId { get; set; }
        public ProductViewModel Product { get; set; }
        public List<LocationViewModel> Locations { get; set; } 
        public List<PriceRowViewModel> Prices { get; set; }
        public string ZipFileUrl { get; set; }
    }

    public class ProductViewModel
    {
        public  string Name { get; set; }
        public ColorViewModel Color { get; set; }
        public bool HasColor { get { return Color != null; } }
    }

    public class ColorViewModel
    {
        public string Name { get; set; }
        public string HtmlHexValue { get; set; }
    }

    public class LocationViewModel
    {
        public string Name { get; set; }
        public string SvgPreviewUrl { get; set; }
        public string SvgPreviewDownloadUrl { get; set; }
    }

    public class PriceRowViewModel
    {
        public string Label { get; set; }
        
        public Money Value { get; set; }// TODO: change type to Decimal, value converter will be required
        public bool IsTotal { get; set; }// false is default value
    }
    
}