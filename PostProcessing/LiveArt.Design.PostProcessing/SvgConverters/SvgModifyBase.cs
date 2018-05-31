using System.Xml.Linq;
using LiveArt.Design.PostProcessing.Logging;

namespace LiveArt.Design.PostProcessing.SvgConverters
{
    internal class SvgModifyBase
    {
        protected ILogger _Logger;

        protected void Log(string logMessage, params object[] args)
        {
            this._Logger.Log(logMessage, args);
        }

        protected void SetOrCreateAttribute(XElement element, string attrName, double newValue)
        {
            SetOrCreateAttribute(element, attrName, newValue.ToString().Replace(",", "."));
        }
        protected void SetOrCreateAttribute(XElement element, string attrName, string newValue)
        {

            var attr = element.Attribute(attrName);
            if (attr != null)
            {
                if (attr.Value != newValue)
                {
                    Log("Update svg.@{0} from \"{1}\" to \"{2}\"", attrName, attr.Value, newValue);
                    attr.Value = newValue;
                }

            }
            else
            {
                attr = new XAttribute(attrName, newValue);
                Log("Add svg.@{0}=\"{1}\"", attrName, newValue);
                element.Add(attr);
            }
        }
    }
}
