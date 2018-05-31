using FluentAssertions;
using LiveArt.Design.PostProcessing.Domain;
using LiveArt.Design.PostProcessing.SvgConverters;
using NUnit.Framework;

namespace LiveArt.Design.PostProcessing.Tests.SvgConverters
{
    [TestFixture]
    public class SvgSetUnitsConverterTests:SvgConverterBaseTest
    {
        const string EmptySvg = "<svg></svg>";

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            target = new SvgSetUnitsConverter();
            SetUnit("px");
            
        }


        [TestCase("{\"data\":{\"product\":{\"size\":{\"width\":0,\"height\":0}}}}")]
        [TestCase("{\"data\":{\"product\":{\"size\":{\"width\":\"\",\"height\":\"\"}}}}")]
        [TestCase("{\"data\":{\"product\":{\"size\":{}}}}")]
        [TestCase("{\"data\":{\"product\":{}}}")]
        [TestCase("{\"data\":{}}")]
        public void Convert_WhenProductSizeNotSet_ShouldNOTModifySvg(string designJson)
        {
            this.convertContext.CurrentDesign=Design.PostProcessing.Domain.Design.GeDesignFromJsonStr(designJson);
            convert(EmptySvg)
                .Should().Be(EmptySvg);
        }

        [TestCase("{\"data\":{\"product\":{\"size\":{\"width\":11,\"height\":22}}}}")]
        public void Convert_WhenUnitNotSet_ShouldNotModifySvg(string designJson)
        {
            SetUnit("");

            this.convertContext.CurrentDesign = Design.PostProcessing.Domain.Design.GeDesignFromJsonStr(designJson);
            convert(EmptySvg)
                .Should().Be(EmptySvg);
        }


        [TestCase("{\"data\":{\"product\":{\"size\":{\"attrName\":33.22}}}}","width")]
        [TestCase("{\"data\":{\"product\":{\"size\":{\"attrName\":33.22}}}}", "height")]
        public void Convert_ShouldUpdateAttr(string designJson,string attrName)
        {
            designJson = designJson.Replace("attrName", attrName);
            

            this.convertContext.CurrentDesign = Design.PostProcessing.Domain.Design.GeDesignFromJsonStr(designJson);
            convert(EmptySvg)
               // .Should().Contain("\"attrName\":\"33\"".Replace("attrName",attrName));
                .Should().Contain("attrName=\"33.22px\"".Replace("attrName",attrName));
        }


        private void SetUnit(string units )
        {
            this.convertContext.Config = new Config()
            {
                options = new ConfigOptions()
                {
                    unit = units
                }
            };
        }

        //"{\"data\":{\"product\":{\"size\":{\"width\":4.8,\"height\":8.2}}}}"


    }
}
