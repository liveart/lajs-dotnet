using System;
using LiveArt.Design.PostProcessing.Packer;

namespace LiveArt.Design.PostProcessing.ImageProcessors
{
    internal class InternalToExternalImageProcessor : IImageProcessor
    {
        public void Process(IImage image, PackingContext context)
        {
            if (CanProcess(image, context))
            {
                var origUrl=image.Url; // save 

                var baseUri=new Uri(context.SourceImagesBaseUrl);
                var newUri=new Uri(baseUri,image.Url);
                image.Url=newUri.ToString();

                context.Logger.Log("Update image url from '{0}', to '{1}'", origUrl, newUri);
            }
            
        }
        public bool CanProcess(IImage image, PackingContext context)
        {
            return image.Type == ImageTypes.Internal;
        }
    }
}
