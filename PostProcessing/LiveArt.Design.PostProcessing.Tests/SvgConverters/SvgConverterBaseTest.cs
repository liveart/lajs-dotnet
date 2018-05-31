using System.Linq;
using LiveArt.Design.PostProcessing.Packer;
using LiveArt.Design.PostProcessing.SvgConverters;

namespace LiveArt.Design.PostProcessing.Tests.SvgConverters
{
     public abstract class SvgConverterBaseTest
    {
        internal ISvgConverter target; // class under test
        internal PackingContext convertContext;

        public virtual void SetUp()
        {
            this.convertContext = new PackingContext();
        }

        #region Utils

        protected string convertFrontFromJsonStr(string jsonStr)
        {

            var design = Design.PostProcessing.Domain.Design.GeDesignFromJsonStr(jsonStr);
            var front = design.locations.First();

            return convert(front.svg);
        }

        protected string convert(string inputSvg)
        {
            var inputXml = PostProcessing.Utils.XmlFromString(inputSvg);
            var convertedXml = this.target.Convert(inputXml, convertContext);
            return PostProcessing.Utils.XmlToString(convertedXml);

        }




        #endregion

    }
}
