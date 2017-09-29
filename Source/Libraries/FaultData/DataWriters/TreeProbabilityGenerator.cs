using GSF.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FaultData.DataWriters
{
    public class TreeProbabilityGenerator
    {
        public static XElement GetTreeProbability(AdoDataConnection connection, XElement element)
        {
            XElement returnElement = new XElement("span");

            string faultType = (string)element.Attribute("faultType") ?? "AB";
            double reactanceRatio = Convert.ToDouble((string)element.Attribute("reactanceRatio") ?? "-1.0");
            reactanceRatio = Math.Abs(reactanceRatio);
            double distance = Convert.ToDouble((string)element.Attribute("distance") ?? "-1.0");

            double lowToMediumBorder = (1.27 * distance) / Math.Sqrt(Math.Pow((.31 * distance + 10), 2) + Math.Pow((1.27 * distance), 2));
            double mediumToHighBorder = (1.27 * distance) / Math.Sqrt(Math.Pow((.31 * distance + 20), 2) + Math.Pow((1.27 * distance), 2));

            returnElement.Value = "Undetermined";

            if (faultType.ToLower().Contains("n"))
            {
                if (reactanceRatio <= mediumToHighBorder)
                    returnElement.Value = "High";
                else if (reactanceRatio <= lowToMediumBorder && reactanceRatio >= mediumToHighBorder)
                    returnElement.Value = "Medium";
                else
                    returnElement.Value = "Low";
            }

            return returnElement;
        }
    }
}
