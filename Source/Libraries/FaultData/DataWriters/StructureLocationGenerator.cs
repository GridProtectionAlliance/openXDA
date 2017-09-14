using GSF.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FaultData.DataWriters
{
    class StructureLocationGenerator
    {
        #region [ Static ]

        public static XElement GetStructureLocationInformation(AdoDataConnection connection, XElement element)
        {

            string station;
            string lineAssetKey;
            double distance;

            station = (string)element.Attribute("station") ?? "-1";
            lineAssetKey = (string)element.Attribute("line") ?? "-1";
            distance = Convert.ToDouble((string)element.Attribute("distance") ?? "-1");
            string[] returnColumns = ((string)element.Attribute("fields")).Split(',') ?? new string[0];

            string elementType = returnColumns.Length > 1 ? "table" : "span";
            XElement returnElement = new XElement(elementType);

            string testSite = "http://localhost:8989/StructureCrawlerTest.cshtml";
            string tvaSite = "http://chaptpsnet.cha.tva.gov:8025/TLI/StructureCrawler/FaultFinder.asp?Station=" + station + "&Line=" + lineAssetKey + "&Mileage=" + distance;
            string site = testSite;

            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = client.GetAsync(site).Result)
            using (HttpContent content = response.Content)
            {
                string result = content.ReadAsStringAsync().Result;
                result = @"SegLocName,StrNumber,StrLocName,FeetBeyondStructure,Latitude,Longitude,Drawing,Imagepath 
SEG5345 - 1,168,1228630,518,36.5074407,-86.3690534,http://chaptpsnet.cha.tva.gov:8043/DrawingLookupDL.asp?Drawing=LW-8707 s8,";

                string[] lines = result.Split('\n');
                string[] receivedColumns = lines[0].Split(',');
                List<int> columnIndexes = new List<int>();

                foreach(string column in returnColumns)
                {
                    columnIndexes.Add(Array.IndexOf(receivedColumns, column));
                }

                for (int line = 1; line < lines.Length; line++)
                {
                    foreach (int column in columnIndexes)
                    {

                    }
                }
            }

            return returnElement;

        }

        static async void GetStructureInfo()
        {

        }
        #endregion
    }
}
