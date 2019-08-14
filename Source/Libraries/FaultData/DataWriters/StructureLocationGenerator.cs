using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Net;
using HtmlAgilityPack;
using log4net.Core;
using log4net;
using System.Text;

namespace FaultData.DataWriters
{
    public class StructureLocationGenerator
    {
        #region [ Members ]

        // Nested Types
        private class CommonForm
        {
            #region [ Members ]

            private List<string> m_headers;
            private List<List<string>> m_body;
            private List<double> m_latitudes;
            private List<double> m_longitudes;

            #endregion

            #region [ Constructors ]

            public CommonForm()
            {
                m_headers = new List<string>();
                m_body = new List<List<string>>();
                m_latitudes = new List<double>();
                m_longitudes = new List<double>();
            }

            #endregion

            #region [ Properties ]

            public List<string> Headers
            {
                get
                {
                    return m_headers;
                }

                set
                {
                    m_headers = value;
                }
            }

            public List<List<string>> Body
            {
                get
                {
                    return m_body;
                }

                set
                {
                    m_body = value;
                }
            }

            public List<List<string>> RawData
            {
                get
                {
                    List<List<string>> returnValue = new List<List<string>>();
                    returnValue.Add(m_headers);
                    returnValue.AddRange(m_body);
                    return returnValue;
                }
            }

            public List<double> Latitudes
            {
                get
                {
                    return m_latitudes;
                }
                set
                {
                    m_latitudes = value;
                }
            }

            public List<double> Longitudes
            {
                get
                {
                    return m_longitudes;
                }
                set
                {
                    m_longitudes = value;
                }
            }

            #endregion

        }

        #endregion

        #region [ Static ]

        private static readonly ILog Log = LogManager.GetLogger(typeof(StructureLocationGenerator));

        public static object GetStructureLocationInformation(XElement element)
        {
            try
            {
                string url = element.Value.Trim();
                Log.Debug(url);
                string userName = (string)element.Attribute("userName");
                string password = (string)element.Attribute("password");
                string domain = (string)element.Attribute("domain");
                string authenticationType = (string)element.Attribute("authenticationType");

                string queryResultType = (string)element.Attribute("queryResultType") ?? "htmlcsv";
                string elementType = (string)element.Attribute("returnElementType") ?? "span";

                string[] returnHeaders = ((string)element.Attribute("returnFields") ?? "").Split(',');
                string[] returnHeaderNames = ((string)element.Attribute("returnFieldNames") ?? "").Split(',');

                string makeGoogleMapsLinkString = (string)element.Attribute("makeGoogleMapsLink") ?? "false";
                bool makeGoogleMapsLink;
                if (!bool.TryParse(makeGoogleMapsLinkString, out makeGoogleMapsLink))
                    makeGoogleMapsLink = false;

                ICredentials credential = null;
                if (userName != null && password != null && domain != null && authenticationType != null)
                {
                    NetworkCredential networkCredential = new NetworkCredential(userName, password, domain);

                    Log.Debug($"userName: {userName}, password: {password}, domain: {domain}");

                    CredentialCache cache = new CredentialCache();
                    cache.Add(new Uri(url), authenticationType, networkCredential);
                    credential = cache;
                }

                // If queryResult is formatted in html, we should decode that to a regular string
                bool decode = queryResultType.ToLower().Contains("html");
                string structureInfo = GetStructureInfo(url, decode, credential);

                CommonForm intermediateResult = queryResultType.Contains("csv") ? FromCsv(structureInfo, returnHeaders, returnHeaderNames) : new CommonForm();


                return elementType == "table" ? ToTable(intermediateResult) : elementType == "span" ? ToSpan(intermediateResult, makeGoogleMapsLink) : element;
            }
            catch(Exception e)
            {
                Log.Debug(e.Message + "\nStackTrace\n" + e.StackTrace);
                return element;
            }

        }

        private static CommonForm FromCsv(string input, string[] returnHeaders, string[] returnHeaderNames)
        {
            CommonForm commonForm = new CommonForm();
            input = input.Trim();

            string[] rows = input.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string[] receivedHeaders = rows[0].Split(',');
            List<int> returnHeaderIndexes = new List<int>();

            int latitudeIndex = Array.IndexOf(receivedHeaders, receivedHeaders.Where(header => header.ToLower() == "latitude").FirstOrDefault());
            int longitudeIndex = Array.IndexOf(receivedHeaders, receivedHeaders.Where(header => header.ToLower() == "longitude").FirstOrDefault());

            // Add column names as first List<string> in returnValue
            for (int i = 0; i < returnHeaders.Length; i++)
            {
                int indexOfReturnHeader = Array.IndexOf(receivedHeaders, returnHeaders[i]);
                returnHeaderIndexes.Add(indexOfReturnHeader);
                commonForm.Headers.Add(returnHeaderNames[i]);
            }

            // Add each row of data as a new List<string> in result
            for (int row = 1; row < rows.Length; row++)
            {
                string[] rowValues = rows[row].Split(',');
                List<string> newRow = new List<string>();
                if (rowValues.Length == receivedHeaders.Length)
                {
                    foreach (int index in returnHeaderIndexes)
                    {
                        if (index != -1)
                            newRow.Add(rowValues[index]);
                        else
                            newRow.Add("field not found");
                    }
                    commonForm.Body.Add(newRow);

                    if (latitudeIndex != -1 && longitudeIndex != -1)
                    {
                        double latitude;
                        double.TryParse(rowValues[latitudeIndex], out latitude);
                        commonForm.Latitudes.Add(latitude);

                        double longitude;
                        double.TryParse(rowValues[longitudeIndex], out longitude);
                        commonForm.Longitudes.Add(longitude);
                    }
                }
            }

            commonForm.Body = commonForm.Body.Where(list => list.Count > 0).ToList();
            return commonForm;
        }

        private static XElement ToTable(CommonForm input)
        {
            // The template will wrap this element in a table to allow for the template to use it's own attributes for the table.
            // The span tag will (should be) ignored since it is inside a table.
            XElement returnElement = new XElement("span");

            string html = "";

            // Add first List<string> as table headers
            html += "<tr>";
            foreach (string columnHeader in input.Headers)
            {
                html += "<th>" + columnHeader + "</th>";
            }
            html += "</tr>";

            // Add rest of List<strings> as table cell values
            foreach (List<string> row in input.Body)
            {
                html += "<tr>";
                foreach (string value in row)
                {
                    html += "<td>" + value + "</td>";
                }
                html += "</tr>";
            }

            returnElement.Value = html;

            return returnElement;
        }

        private static XElement ToSpan(CommonForm input, bool makeGoogleMapsLink)
        {
            XElement returnElement = new XElement("span");

            string html = "";

            if (makeGoogleMapsLink)
            {
                string latitude = input.Latitudes.FirstOrDefault().ToString("###.######");
                string longitude = input.Longitudes.FirstOrDefault().ToString("###.######");
                html = "<a href = 'https://www.google.com/maps/@?api=1&map_action=map&center=" + latitude + "," + longitude + "&zoom=21&basemap=satellite'>" + input.Body.FirstOrDefault()?.FirstOrDefault() + "</a>";
            }
            else
                html = input.Body.FirstOrDefault()?.FirstOrDefault();

            returnElement.Value = html;
            return returnElement;
        }

        private static string ToFormat(CommonForm input, string headerFormat, string rowFormat)
        {
            object[] headerData = input.Headers.AsEnumerable<object>().ToArray();
            string header = string.Format(headerFormat, headerData);

            List<string> rows = input.Body
                .Select(rowData => rowData.AsEnumerable<object>().ToArray())
                .Select(rowData => string.Format(rowFormat, rowData))
                .ToList();

            List<string> lines = new[] { header }.Concat(rows).ToList();

            return string.Join(Environment.NewLine, lines);
        }

        private static string GetStructureInfo(string url, bool decode, ICredentials credential = null)
        {
            HtmlWeb webClient = new HtmlWeb();
            webClient.PreRequest += request => { request.Credentials = credential; return true; };
            HtmlDocument doc = webClient.Load(url);

            string result;

            if (decode)
                result = doc.DocumentNode.Descendants("html").FirstOrDefault().InnerText.Trim();
            else
                result = doc.DocumentNode.InnerText;

            Log.Debug(result);
            return result;
        }

        #endregion
    }
}
