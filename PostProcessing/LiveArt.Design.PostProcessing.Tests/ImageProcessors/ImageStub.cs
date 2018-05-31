using LiveArt.Design.PostProcessing.ImageProcessors;

namespace LiveArt.Design.PostProcessing.Tests.ImageProcessors
{
    internal class ImageStub:IImage
    {
        public string Url { get; set; }
        public ImageTypes Type { get; set; }
    }
}
