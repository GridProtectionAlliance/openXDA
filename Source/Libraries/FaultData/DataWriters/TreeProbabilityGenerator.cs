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
            string[] probabilityNames = ((string)element.Attribute("probabilityNames") ?? "High,Medium,Low").Split(',');
            string[] probabilityNumbersStrings = ((string)element.Attribute("probabilityNumbers") ?? ".64,.86,1.0").Split(',');
            double[] cutoffs = probabilityNumbersStrings.Select(probabilityNumber => Convert.ToDouble(probabilityNumber)).ToArray();

            returnElement.Value = "Undetermined";
            if (faultType.ToLower().Contains("n") && probabilityNames.Length == cutoffs.Length)
                for (int i = 0; i < probabilityNames.Length; i++)
                    if (reactanceRatio < cutoffs[i])
                    {
                        returnElement.Value = probabilityNames[i];
                        break;
                    }

            return returnElement;
        }
    }
}
