using LiveArt.Design.PostProcessing.PackerExtentions;
using NUnit.Framework;

namespace LiveArt.Design.PostProcessing.Tests.PackerExtentions
{

    [TestFixture]
    public class UpdateUnitsMethodTests : SvgConverterPackerExtentionsTests
    {
        protected override void InvokeExtentionMethod()
        {
            _Packer.UpdateUnits(mSvgConverter.Object);
        }

        protected override void IvokeActionWithDefaultParams()
        {
            InvokeExtentionMethod();
            targetAction(context);
        }
    }
}
