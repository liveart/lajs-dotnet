using LiveArt.Design.PostProcessing.Domain;
using LiveArt.Design.PostProcessing.Packer;

namespace LiveArt.Design.PostProcessing.PackerExtentions
{
    public static class SetConfigPackerExtentions
    {
        public static IDesignPacker ParseConfig(this IDesignPacker packer, string configJsonStr)
        {
            packer.RegBeforePackAction((context) => ParseConfig(context, configJsonStr));
            return packer;
        }

        private static void ParseConfig(PackingContext context, string configJsonStr)
        {
            context.Logger.Log("Parse config...");
            context.Config = Newtonsoft.Json.JsonConvert.DeserializeObject<Config>(configJsonStr);
            context.Logger.Log("done");
        }

        public static IDesignPacker LoadConfig(this IDesignPacker packer,string filePath)
        {
            packer.RegBeforePackAction((context) =>
            {
                context.Logger.Log("Read config from '{0}'",filePath);
                var configJsonStr = System.IO.File.ReadAllText(filePath);

                ParseConfig(context, configJsonStr);
            });
            
            return packer;
        }
    }
}
