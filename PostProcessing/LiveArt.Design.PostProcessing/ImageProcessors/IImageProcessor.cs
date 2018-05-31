using LiveArt.Design.PostProcessing.Packer;

namespace LiveArt.Design.PostProcessing.ImageProcessors
{
    internal interface IImageProcessor<TConvertContext>
    {
        void Process(IImage image,TConvertContext context);
        bool CanProcess(IImage image, TConvertContext context);
    }

    internal interface IImageProcessor : IImageProcessor<PackingContext>
    {

    }
}
