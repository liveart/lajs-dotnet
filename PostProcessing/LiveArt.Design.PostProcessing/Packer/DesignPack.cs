using System.Collections.Generic;
using System.Linq;
using LiveArt.Design.PostProcessing.Logging;

namespace LiveArt.Design.PostProcessing.Packer
{
    // result of DesignPacker
    public class DesignPack
    {
        public Location[] Locations { get; set; }
        public DesignPack(){
            this.Locations=new Location[]{};
        }

        public IEnumerable<LogLine> LogLines { get; set; }
        public File ZipFile;

        public class Location
        {
            public string Name { get; set; }
            public IList<File> SvgFiles { get; set; }
            public File PngPreview { get; set; }
            public File Pdf { get; set; }

            public Location()
            {
                SvgFiles = new List<File>();
            }
        }

        public class File
        {
            public string FileName { get; set; }
            public string Url { get; set; }
        }
    }

   

    internal static class DesignPackExtentions
    {
        public static DesignPack.Location GetResultLocation(this DesignPack pack, string locationName)
        {
            return pack.Locations.First(l => l.Name == locationName);
        }
    }


}
