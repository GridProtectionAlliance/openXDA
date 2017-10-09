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

            string reactanceRatioString = (string)element.Attribute("reactanceRatio") ?? "1.0";
            double reactanceRatio;
            if (!double.TryParse(reactanceRatioString, out reactanceRatio))
                reactanceRatio = 1.0;
            reactanceRatio = Math.Abs(reactanceRatio);

            string distanceString = (string)element.Attribute("distance") ?? "-1.0";
            double distance;
            if (!double.TryParse(distanceString, out distance))
                distance = -1.0;

            string RsString = (string)element.Attribute("Rs") ?? "1.27";
            double Rs;
            if (!double.TryParse(RsString, out Rs))
                Rs = 1.27;

            string XsString = (string)element.Attribute("Xs") ?? ".31";
            double Xs;
            if (!double.TryParse(XsString, out Xs))
                Xs = .31;

            double lowToMediumBorder = (Rs * distance) / Math.Sqrt(Math.Pow((Xs * distance + 10), 2) + Math.Pow((Rs * distance), 2));
            double mediumToHighBorder = (Rs * distance) / Math.Sqrt(Math.Pow((Xs * distance + 20), 2) + Math.Pow((Rs * distance), 2));

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
