using GSF.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using HtmlAgilityPack;
using System.Net.Http;

namespace FaultData.DataWriters
{
    public class LightningGenerator
    {
        public static XElement GetLightningInfo(AdoDataConnection connection, XElement element, List<Attachment> attachemnts = null)
        {
            XElement returnElement = new XElement("span");

            //string url = element.Value.Trim();
            //string queryResultType = (string)element.Attribute("queryResultType") ?? "url,pdf";
            //string elementType = (string)element.Attribute("returnElementType") ?? "png";

            returnElement.Value = "No lightning detected";
            return returnElement;
        }

        private static string ToUrl(string url, bool decode)
        {
            HtmlWeb webClient = new HtmlWeb();
            HtmlDocument doc = webClient.Load(url);

            string returnUrl;

            if (decode)
                returnUrl = doc.DocumentNode.Descendants("html").FirstOrDefault().InnerText.Trim();
            else
                returnUrl = doc.DocumentNode.InnerText;

            return returnUrl;
        }

        private static void ToPDF(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                var data = client.GetByteArrayAsync(url);
            }
        }
    }
}
