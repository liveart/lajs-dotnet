using System.IO;
using LiveArt.Design.PostProcessing.Domain;

namespace LiveArt.Design.PostProcessing.Tests{
    public static class TestData
    {
        public static string SavedDesignJsonStr{
            get { return getTestDataFile("savedDesign.json"); }
        }

        public static string DesignWithOneRelativeImageJsonStr
        {
            get { return getTestDataFile("designWithImage.json "); }
        }
        
        public static string getTestDataFile(string fileName){
            return File.ReadAllText("../../TestData/"+fileName);
        }

        internal static Design.PostProcessing.Domain.Design DesignWithFrontLocation
        {
            get { return new Design.PostProcessing.Domain.Design()
            {
                locations = new[] { 
                    new Location(){
                        name="Front",
                        svg="<svg>some svg</svg>"
                    }
                }
            };}
        }
        
    }
}
