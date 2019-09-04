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

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GSF.ComponentModel;
using GSF.Diagnostics;
using GSF.IO;
using GSF.Windows.Forms;
using Microsoft.Azure.EventHubs;
using Newtonsoft.Json.Linq;

namespace XDACloudDataPusher
{
    public partial class MainForm : Form
    {
        #region [ Members ]

        // Constants
        private const string XDAJsonApiPath = "/api/jsonapi/";

        // Fields
        private readonly LogPublisher m_log;
        private Settings m_settings;
        private bool m_formLoaded;
        private volatile bool m_formClosing;

        #endregion

        #region [ Constructors ]

        public MainForm()
        {
            InitializeComponent();
            
            // Set initial default values for date/time pickers
            StartDateTimePicker.Value = DateTimePicker.MinimumDateTime;
            EndDateTimePicker.Value = DateTimePicker.MaximumDateTime;

            // Create a new log publisher instance
            m_log = Logger.CreatePublisher(typeof(MainForm), MessageClass.Application);
        }

        #endregion

        #region [ Methods ]

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                // Load current settings registering a symbolic reference to this form instance for use by default value expressions
                m_settings = new Settings(new Dictionary<string, object> {{ "Form", this }}.RegisterSymbols());

                // Restore last window size/location
                this.RestoreLayout();

                m_formLoaded = true;
            }
            catch (Exception ex)
            {
                m_log.Publish(MessageLevel.Error, "FormLoad", "Failed while loading settings", exception: ex);

            #if DEBUG
                throw;
            #endif
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                m_formClosing = true;

                // Save current window size/location
                this.SaveLayout();

                // Save any updates to current screen values
                m_settings.Save();
            }
            catch (Exception ex)
            {
                m_log.Publish(MessageLevel.Error, "FormClosing", "Failed while saving settings", exception: ex);

            #if DEBUG
                throw;
            #endif
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_settings?.Dispose();
        }

        private void XDAUrlTextBox_TextChanged(object sender, EventArgs e)
        {
            FormElementChanged(sender, e);
            XDAJsonApiUrl = XDAUrlTextBox.Text;
        }

        private void CancelExportButton_Click(object sender, EventArgs e)
        {
        }

        private void PushToCloudButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ConnectionStringTextBox.Text))
            {
                MessageBox.Show(this, "No cloud service connection string was defined.\n\nCannot push openXDA event data to the cloud.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (StartDateTimePicker.Value == DateTimePicker.MinimumDateTime && EndDateTimePicker.Value == DateTimePicker.MaximumDateTime)
            {
                if (MessageBox.Show(this, "No start and end times have been defined for the export query.\n\nAre you sure you want to export all available openXDA event data?", "Export Volume Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
            }

            // Spin up export operations task
            Task.Run(async () =>
            {
                const int ProgressSteps = 6;

                try
                {
                    SetButtonsEnabledState(false);
                    SetExportProgressBarMaximum(ProgressSteps);
                    int progress = 0;

                    void IncrementProgress()
                    {
                        UpdateExportProgressBar(progress);
                        progress++;
                    }

                    IncrementProgress();

                    // Export event data
                    if (m_settings.ExportEventData)
                    {
                        List<byte[]> records = new List<byte[]>();

                        await PushToCloud(records, "EventData");
                    }
                    IncrementProgress();

                    // Export fault data
                    if (m_settings.ExportFaultData)
                    {
                        List<byte[]> records = new List<byte[]>();

                        await PushToCloud(records, "FaultData");
                    }
                    IncrementProgress();

                    // Export disturbance data
                    if (m_settings.ExportDisturbanceData)
                    {
                        List<byte[]> records = new List<byte[]>();

                        await PushToCloud(records, "DisturbanceData");
                    }
                    IncrementProgress();

                    // Export breaker operation data
                    if (m_settings.ExportBreakerOperationData)
                    {
                        List<byte[]> records = new List<byte[]>();

                        await PushToCloud(records, "BreakerOperationData");
                    }
                    IncrementProgress();

                    // Export waveform data
                    if (m_settings.ExportWaveformData)
                    {
                        List<byte[]> records = new List<byte[]>();

                        await PushToCloud(records, "WaveformData");
                    }
                    IncrementProgress();

                    // Export frequency domain data
                    if (m_settings.ExportFrequencyDomainData)
                    {
                        List<byte[]> records = new List<byte[]>();

                        await PushToCloud(records, "FrequencyDomainData");
                    }
                    IncrementProgress();
                }
                catch (Exception ex)
                {
                    ShowErrorMessage($"Failed during export: {ex.Message}");
                }
                finally
                {
                    UpdateExportProgressBar(ProgressSteps);
                    SetButtonsEnabledState(true);
                }
            });
        }

        private async Task PushToCloud(List<byte[]> records, string type)
        {
            if (AzureRadioButton.Checked)
            {
                // Establish connection to Azure Event Hub
                EventHubClient eventHub;

                try
                {
                    EventHubsConnectionStringBuilder builder = new EventHubsConnectionStringBuilder(m_settings.CloudRepostioryConnectionString);
                    eventHub = EventHubClient.CreateFromConnectionString(builder.ToString());
                }
                catch (Exception ex)
                {
                    ShowErrorMessage($"Failed to connect to Azure Event Hub: {ex.Message}");
                    return;
                }

                await PushToAzure(eventHub, records, type);
            }
            else if (AWSRadioButton.Checked)
            {
                // TODO: Establish connection to Amazon Kinesis
                await PushToAWS(null, records, type);
            }
            else
            {
                ShowErrorMessage("No repository type selected for cloud data push");
            }
        }

        private async Task PushToAzure(EventHubClient eventHub, List<byte[]> records, string type)
        {
            List<EventData> samples = new List<EventData>();
            int size = 0;

            async Task pushToEventHub()
            {
                // Write data to event hub
                if (samples.Count > 0)
                    await eventHub.SendAsync(samples, type);

                samples.Clear();
            }

            foreach (byte[] bytes in records)
            {
                EventData record = new EventData(bytes);

                // Keep total post size under specified limit
                if (size + bytes.Length < m_settings.PostSizeLimit)
                {
                    samples.Add(record);
                }
                else
                {
                    await pushToEventHub();
                    samples.Add(record);
                    size = 0;
                }

                size += bytes.Length;
            }

            // Push any remaining events
            await pushToEventHub();
        }

        private /*async*/ Task PushToAWS(object connection, List<byte[]> records, string type)
        {
            // TODO: Add ability to push to Kinesis
            return Task.CompletedTask;
        }

        // Form Element Accessors -- these functions allow access to form elements from non-UI threads

        private void FormElementChanged(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<object, EventArgs>(FormElementChanged), sender, e);
            }
            else
            {
                if (Visible && m_formLoaded)
                    m_settings?.UpdateProperties();
            }
        }

        private void ShowUpdateMessage(string message)
        {
            if (m_formClosing)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(ShowUpdateMessage), message);
            }
            else
            {
                lock (MessageOutputTextBox)
                    MessageOutputTextBox.AppendText($"{message}{Environment.NewLine}");

                m_log.Publish(MessageLevel.Info, "StatusMessage", message);
            }
        }

        private void ShowErrorMessage(string message)
        {
            if (m_formClosing)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(ShowErrorMessage), message);
            }
            else
            {
                lock (MessageOutputTextBox)
                    MessageOutputTextBox.AppendText($"ERROR: {message}{Environment.NewLine}");

                m_log.Publish(MessageLevel.Error, "ErrorMessage", message);
            }
        }

        private void ClearUpdateMessages()
        {
            if (m_formClosing)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action(ClearUpdateMessages));
            }
            else
            {
                lock (MessageOutputTextBox)
                    MessageOutputTextBox.Text = "";
            }
        }

        private void SetButtonsEnabledState(bool enabled)
        {
            if (m_formClosing)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action<bool>(SetButtonsEnabledState), enabled);
            }
            else
            {
                LoadSourcesButton.Enabled = enabled;
                SelectAllEventsButton.Enabled = enabled;
                UnselectAllEventsButton.Enabled = enabled;
                PushToCloudButton.Enabled = enabled;
                CancelExportButton.Enabled = !enabled;
            }
        }

        private void UpdateExportProgressBar(int value)
        {
            if (m_formClosing)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action<int>(UpdateExportProgressBar), value);
            }
            else
            {
                if (value < ExportProgressBar.Minimum)
                    value = ExportProgressBar.Minimum;

                if (value > ExportProgressBar.Maximum)
                    ExportProgressBar.Maximum = value;

                ExportProgressBar.Value = value;
            }
        }

        private void SetExportProgressBarMaximum(int maximum)
        {
            if (m_formClosing)
                return;

            if (InvokeRequired)
                BeginInvoke(new Action<int>(SetExportProgressBarMaximum), maximum);
            else
                ExportProgressBar.Maximum = maximum;
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly HttpClient s_http;
        private static string s_xdaJsonApiUrl;

        // Static Constructor
        static MainForm()
        {
            // Create a shared HTTP client instance
            s_http = new HttpClient(new HttpClientHandler { UseCookies = false });
        }

        // Static Properties
        private static string XDAJsonApiUrl
        {
            get => s_xdaJsonApiUrl;
            set => s_xdaJsonApiUrl = $"{FilePath.RemovePathSuffix(value)}{XDAJsonApiPath}";
        }

        // Static Methods
        private static async Task<JArray> CallAPIFunction(string function, string content = null)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"{XDAJsonApiUrl}{function}");

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
