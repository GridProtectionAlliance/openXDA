using System.Xml.Linq;

namespace FaultData.DataWriters
{
    public class FaultTypeGenerator
    {
        public static XElement GetFaultType(XElement element)
        {
            XElement returnElement = new XElement("span");

            string faultType = (string)element.Attribute("faultType") ?? "AB";

            if (faultType.Length == 3)
                returnElement.Value = "Three phase";

            else if (faultType.ToLower().Contains("n"))
                returnElement.Value = faultType[0] + "-phase to ground";

            else
                returnElement.Value = faultType[0] + "-phase to " + faultType[1] + "-phase";

            return returnElement;
        }
    }
}
