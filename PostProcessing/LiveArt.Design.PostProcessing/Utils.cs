using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace LiveArt.Design.PostProcessing
{
    internal class Utils
    {
        public static  XDocument XmlFromString(string xmlStr)
        {
            var xml = XDocument.Parse(xmlStr);

            // xml.Declaration = new XDeclaration("1.0", "UTF-8", "no"); 
            return xml;

        }

        public static string XmlToString(XDocument xml)
        {
            var declaration = xml.Declaration;
               return string.Concat(declaration != null ? declaration.ToString() : string.Empty, xml.ToString(SaveOptions.DisableFormatting)); // No Indent, ident - wronk render in or Inksckape
          

        }

        internal static Regex regSafeSvgName = new Regex(
                       "[^a-zA-Z0-9\\.]",
                     RegexOptions.IgnoreCase
                     | RegexOptions.CultureInvariant
                     | RegexOptions.IgnorePatternWhitespace
                     | RegexOptions.Compiled
                     );

        internal static string GetSafeLocationName(string locName)
        {
            return regSafeSvgName.Replace(locName, "-");
        }
    }
}
