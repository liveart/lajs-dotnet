using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace LiveArt.Design.PostProcessing.Tests.Domain
{
    [TestFixture]
    public class DesignTests
    {
        [Test]
        public void FromString_DesignShouldNotBeNull()
        {
            //action
            Design.PostProcessing.Domain.Design.GeDesignFromJsonStr(TestData.SavedDesignJsonStr)
                .Should().NotBeNull();
        }


        [Test]
        public void FromString_LocationShouldParsed()
        {
            var design = Design.PostProcessing.Domain.Design.GeDesignFromJsonStr(TestData.SavedDesignJsonStr);

            //asserts
            design.locations
                .Should().HaveCount(1);

            var frontLocation = design.locations.First();
            frontLocation.name.Should().Be("Front");
            frontLocation.svg.Should().StartWith("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?><svg");

        }

       

    }
}
