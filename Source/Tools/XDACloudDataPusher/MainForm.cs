//******************************************************************************************************
//  MainForm.cs - Gbtc
//
//  Copyright © 2019, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may not use this
//  file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  09/03/2019 - J. Ritchie Carroll
//       Generated original version of source code.
//
//******************************************************************************************************

using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GSF.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace XDACloudDataPusher
{
    public partial class MainForm : Form
    {
        #region [ Members ]

        // Fields

        #endregion

        #region [ Constructors ]

        public MainForm()
        {
            InitializeComponent();
        }

        #endregion

        #region [ Methods ]

        private void MainForm_Load(object sender, System.EventArgs e)
        {
            this.RestoreLocation();
            StartDateTimePicker.Value = DateTimePicker.MinimumDateTime;
            EndDateTimePicker.Value = DateTimePicker.MaximumDateTime;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.SaveLocation();
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly HttpClient s_http;

        // Static Constructor
        static MainForm()
        {
            // Create a shared HTTP client instance
            s_http = new HttpClient(new HttpClientHandler { UseCookies = false });
        }

        // Static Methods

        private static async Task<dynamic> CallAPIFunction(string url, string content = null)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if ((object)content != null)
                request.Content = new StringContent(content, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await s_http.SendAsync(request);

            content = await response.Content.ReadAsStringAsync();

            return JArray.Parse(content);
        }

        #endregion
    }
}
