using FluentAssertions;
using LiveArt.Design.PostProcessing.PackerExtentions;
using NUnit.Framework;

namespace LiveArt.Design.PostProcessing.Tests.PackerExtentions
{
    [TestFixture]
    public class SetConfigPackerExtentionsTests : BaseExtentionsTests
    {


        [SetUp]
        public void SetUp()
        {
            base.SetUp();
        }

        protected override void InvokeExtentionMethod()
        {
            _Packer.ParseConfig("{\"options\":{\"unit\":\"mm\"}}");
        }

        [Test]
        public void UnitIsParsedFromConfig(){
            InvokeAction();
            context.Config.options.unit
                .Should().Be("mm");
        }

        private void InvokeAction()
        {
            this.InvokeExtentionMethod();
            this.targetAction(this.context);
        }
        
    }
}
