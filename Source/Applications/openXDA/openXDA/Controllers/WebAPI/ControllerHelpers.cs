using GSF.Configuration;
using GSF.Net.Security;
using GSF.Security.Model;
using GSF.Web;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace openXDA.Controllers.Helpers
{
    public class ControllerHelpers
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ControllerHelpers));

        public static bool HandleCertificateValidation(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            SimpleCertificateChecker simpleCertificateChecker = new SimpleCertificateChecker();

            CategorizedSettingsElementCollection systemSettings = ConfigurationFile.Current.Settings["systemSettings"];
            systemSettings.Add("CertFile", "", "This is a certfile.");
            systemSettings.Add("ValidPolicyErrors", "None", "Password for OpenXDA Web API access.");
            systemSettings.Add("ValidChainFlags", "NoError", "Password for OpenXDA Web API access.");

            try
            {
                simpleCertificateChecker.ValidPolicyErrors = (SslPolicyErrors)Enum.Parse(typeof(SslPolicyErrors), (systemSettings["ValidPolicyErrors"].Value != "All" ? systemSettings["ValidPolicyErrors"].Value : "7"));
                simpleCertificateChecker.ValidChainFlags = (X509ChainStatusFlags)Enum.Parse(typeof(X509ChainStatusFlags), (systemSettings["ValidChainFlags"].Value != "All" ? systemSettings["ValidChainFlags"].Value : (~0).ToString()));
                simpleCertificateChecker.TrustedCertificates.Add((!string.IsNullOrEmpty(systemSettings["CertFile"].Value) ? new X509Certificate2(systemSettings["CertFile"].Value) : certificate));
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }

            return simpleCertificateChecker.ValidateRemoteCertificate(sender, certificate, chain, sslPolicyErrors);
        }

        public static string GenerateAntiForgeryToken(string instance, UserAccount userAccount)
        {
            using (WebRequestHandler handler = new WebRequestHandler())
            using (HttpClient client = new HttpClient(handler))
            {
                handler.ServerCertificateValidationCallback += HandleCertificateValidation;

                client.BaseAddress = new Uri(instance);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.AddBasicAuthenticationHeader(userAccount.AccountName, userAccount.Password);

                HttpResponseMessage response = client.GetAsync("api/rvht").Result;

                if (!response.IsSuccessStatusCode)
                    throw new InvalidOperationException($"Server returned status code {response.StatusCode}: {response.ReasonPhrase}");

                return response.Content.ReadAsStringAsync().Result;
            }
        }


    }
}