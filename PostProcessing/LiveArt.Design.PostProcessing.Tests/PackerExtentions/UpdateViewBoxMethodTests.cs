using LiveArt.Design.PostProcessing.PackerExtentions;
using NUnit.Framework;

namespace LiveArt.Design.PostProcessing.Tests.PackerExtentions
{
    [TestFixture]
    public class UpdateViewBoxMethodTests : SvgConverterPackerExtentionsTests
    {
        protected override void InvokeExtentionMethod()
        {
            _Packer.UpdateViewBox(this.mSvgConverter.Object);
        }

        protected override void IvokeActionWithDefaultParams()
        {
            InvokeExtentionMethod();
            targetAction(context);
        }
    }
}
