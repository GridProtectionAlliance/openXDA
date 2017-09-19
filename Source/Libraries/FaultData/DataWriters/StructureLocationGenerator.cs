using GSF.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Net;

namespace FaultData.DataWriters
{
    public class StructureLocationGenerator
    {
        #region [ Static ]

        public static XElement GetStructureLocationInformation(AdoDataConnection connection, XElement element)
        {
            string url = (string)element.Attribute("url") ?? "";
            string queryResultType = (string)element.Attribute("queryResultType") ?? "htmlcsv";
            string elementType = (string)element.Attribute("returnElementType") ?? "table";

            string[] returnFields = ((string)element.Attribute("returnFields")).Split(',') ?? new string[0];
            string[] returnFieldNames = ((string)element.Attribute("returnFieldNames")).Split(',') ?? new string[0];
            
            // Build query url
            string tvaSite = url;
            string testSite = "http://gridprotectionalliance.org/dev/structureTest.asp";
            string site = testSite;

            // If queryResult is formatted in html, we should decode that to a regular string
            bool decode = queryResultType.ToLower().Contains("html");
            string structureInfo = GetStructureInfo(site, decode);

            List<List<string>> intermediateResult = queryResultType == "csv" ? FromCsvToIntermediate(structureInfo, returnFields, returnFieldNames) : new List<List<string>>();
            return elementType == "table" ? IntermediateToTable(intermediateResult) : elementType == "span" ? ToSpan(intermediateResult) : element;
        }

        private static List<List<string>> FromCsvToIntermediate(string input, string[] returnFields, string[] returnFieldNames)
        {
            List<List<string>> result = new List<List<string>>();

            string[] rows = input.Split('\n');
            string[] receivedColumns = rows[0].Split(',');
            List<int> returnFieldIndexes = new List<int>();

            // Add column names as first List<string> in returnValue
            result.Add(new List<string>());
            for (int i = 0; i < returnFields.Length; i++)
            {
                int indexOfReturnField = Array.IndexOf(receivedColumns, returnFields[i]);
                returnFieldIndexes.Add(indexOfReturnField);
                result.ElementAt(0).Add(returnFieldNames[i]);
            }

            // Add each row of data as a new List<string> in result
            for (int row = 1; row < rows.Length; row++)
            {
                string[] rowValues = rows[row].Split(',');
                result.Add(new List<string>());

                foreach (int index in returnFieldIndexes)
                {
                    if (index != -1)
                        result.ElementAt(row).Add(rowValues[index]);
                    else
                        result.ElementAt(row).Add("field not found");
                }
            }

            return result;
        }

        private static XElement IntermediateToTable(List<List<string>> input)
        {
            // The template will wrap this element in a table to allow for the template to use it's own attributes for the table.
            // The span tag will (should be) ignored since it is inside a table.
            XElement returnElement = new XElement("span");

            string returnElementValue = "";

            // Add first List<string> as table headers
            returnElementValue += "<tr>";
            if (input.Count > 0)
            {
                foreach (string columnHeader in input.ElementAt(0))
                {
                    returnElementValue += "<th>" + columnHeader + "</th>";
                }
            }
            returnElementValue += "</tr>";

            // Add rest of List<strings> as table cell values
            for (int row = 1; row < input.Count; row++)
            {
                returnElementValue += "<tr>";
                foreach (string value in input.ElementAt(row))
                {
                    returnElementValue += "<td>" + value + "</td>";
                }
                returnElementValue += "</tr>";
            }

            returnElement.Value = returnElementValue;

            return returnElement;
        }

        private static XElement ToSpan(List<List<string>> input)
        {
            XElement returnElement = new XElement("span");

            if (input.Count > 1)
                returnElement.Value = input.ElementAt(1).FirstOrDefault();

            return returnElement;
        }

        static private string GetStructureInfo(string url, bool decode)
        {
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = client.GetAsync(url).Result)
            using (HttpContent content = response.Content)
            {
                string result = content.ReadAsStringAsync().Result;
                if (decode)
                    result = WebUtility.HtmlDecode(result);

                return result;
           }
        }

        #endregion
    }
}
