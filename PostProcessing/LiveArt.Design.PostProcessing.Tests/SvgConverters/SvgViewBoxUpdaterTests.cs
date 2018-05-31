using FluentAssertions;
using LiveArt.Design.PostProcessing.Domain;
using LiveArt.Design.PostProcessing.SvgConverters;
using NUnit.Framework;

namespace LiveArt.Design.PostProcessing.Tests.SvgConverters
{
    [TestFixture]
    public class SvgViewBoxUpdaterTests : SvgConverterBaseTest
    {

        Rect editableArea = new Rect(10.1, 20.2, 30.3, 40.4);

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            target = new SvgViewBoxUpdater();
            this.convertContext.CurrentLocation = new Location()
            {
                editableArea = editableArea.ToTextAreaString()
            };


        }


        [TestCase("")]
        [TestCase(null)]
        public void Convert_ShouldNotUpdate_IfTextAreaNotSetInLocation(string editableAreaString)
        {
            this.convertContext.CurrentLocation.editableArea = editableAreaString;

            convert("<svg></svg>")
                .Should().Be("<svg></svg>");
        }

        [Test]
        public void Convert_ShouldCreateViewBoxAndXYAndWidthHeightIfNotExists()
        {
            convert("<svg></svg>")
                 .Should().Be(string.Format("<svg x=\"{0}\" y=\"{1}\" width=\"{2}\" height=\"{3}\" viewBox=\"{4}\"></svg>",
                        FixComa(editableArea.X),
                        FixComa(editableArea.Y),
                        FixComa(editableArea.Width),
                        FixComa(editableArea.Height),
                        editableArea.ToViewBoxString()
                ));
        }

        [Test]
        public void Convert_ShouldUpdateViewBoxAndXYAndWidthHeight()
        {
            convert("<svg x=\"3\" y=\"5\" width=\"11\" height=\"22\" viewBox=\"1 2 3 4\"></svg>")
             .Should().Be(string.Format("<svg x=\"{0}\" y=\"{1}\" width=\"{2}\" height=\"{3}\" viewBox=\"{4}\"></svg>",
                        FixComa(editableArea.X),
                        FixComa(editableArea.Y),
                        FixComa(editableArea.Width),
                        FixComa(editableArea.Height),
                        editableArea.ToViewBoxString()
                ));
        }

        [Test]
        public void Convert_ShouldNotContainsComa()
        {
            convert("<svg x=\"3\" y=\"5\" viewBox=\"1 2 3 4\"></svg>")
               .Should().NotContain(",");
        }

        private string FixComa(double val)
        {
            return val.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
