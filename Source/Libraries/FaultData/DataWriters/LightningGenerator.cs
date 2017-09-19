using GSF.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FaultData.DataWriters
{
    public class LightningGenerator
    {
        public static XElement GetLightningInfo(AdoDataConnection connection, XElement element)
        {
            XElement returnElement = new XElement("span");

            returnElement.Value = "No lightning detected";
            return returnElement;
        }
    }
}
