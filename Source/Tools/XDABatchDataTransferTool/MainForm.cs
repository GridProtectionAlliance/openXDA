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
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using GSF;
using GSF.ComponentModel;
using GSF.Diagnostics;
using GSF.IO;
using GSF.Reflection;
using GSF.Threading;
using GSF.Units;
using GSF.Windows.Forms;
using Microsoft.Azure.EventHubs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using openXDA.Model;
using CancellationToken = System.Threading.CancellationToken;
using EventData = Microsoft.Azure.EventHubs.EventData;
using EventFilter = openXDA.Adapters.JSONApiController.EventJSON;
using EventDataFilter = openXDA.Adapters.JSONApiController.EventDataJSON;

// ReSharper disable ClassNeverInstantiated.Local
// ReSharper disable CoVariantArrayConversion
// ReSharper disable UnusedParameter.Local
namespace XDABatchDataTransferTool
{
    public partial class MainForm : Form //-V3073
    {
        #region [ Members ]

        // Nested Types
        private class DescriptiveLine : Line
        {
            public override string ToString() => $"{AssetKey}: {VoltageKV}kV{(string.IsNullOrWhiteSpace(Description) ? "" : $" - {Description}")}";
        }

        private class DescriptiveMeter : Meter
        {
            public override string ToString() => $"{AssetKey}: {Name}{(string.IsNullOrWhiteSpace(Description) ? "" : $" - {Description}")}";
        }

        // Constants
        private const string DateTimeFormat = "MMM dd, yyyy HH:mm:ss";
        private const string XDAJsonApiPath = "/api/jsonapi/";

        // Fields
        private readonly LogPublisher m_log;
        private Settings m_settings;
        private bool m_formLoaded;
        private volatile bool m_formClosing;
        private CancellationTokenSource m_cancellationTokenSource;
        private readonly ShortSynchronizedOperation m_updateSelections;
        private string m_lineIDList;
        private string m_meterIDList;

        #endregion

        #region [ Constructors ]

        public MainForm()
        {
            InitializeComponent();
            
            // Set initial default values for date/time pickers
            ResetTimeRange();

            // Create a new log publisher instance
            m_log = Logger.CreatePublisher(typeof(MainForm), MessageClass.Application);

            // Set formats for date/time pickers
            StartDateTimePicker.CustomFormat = DateTimeFormat;
            EndDateTimePicker.CustomFormat = DateTimeFormat;

            // Save base text for key labels so they can be appended to
            SelectedTimeRangeLabel.Tag = SelectedTimeRangeLabel.Text;
            SelectSourcesLabel.Tag = SelectSourcesLabel.Text;

            // Using synchronized operation to prevent over-aggressive updates
            m_updateSelections = new ShortSynchronizedOperation(() =>
            {
                Invoke(new Action(() =>
                {
                    CheckedListBox.CheckedItemCollection checkedItems = SelectedSourcesCheckedListBox.CheckedItems;

                    // Update selected count on UI
                    SelectSourcesLabel.Text = $"{SelectSourcesLabel.Tag} {checkedItems.Count:N0} selected";

                    // Update selected ID lists
                    m_lineIDList = m_meterIDList = null;

                    if (m_settings.SourceQueryTypeIsLines)
                        m_lineIDList = string.Join(",", checkedItems.Cast<Line>().Select(line => $"{line.ID}"));
                    else if (m_settings.SourceQueryTypeIsMeters)
                        m_meterIDList = string.Join(",", checkedItems.Cast<Meter>().Select(meter => $"{meter.ID}"));
                }));
            },
            ex => m_log.Publish(MessageLevel.Error, "UpdateSelectedCount", "Failed updating selected count", exception: ex));
        }

        #endregion

        #region [ Methods ]

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                // Load current settings registering a symbolic reference to this form instance for use by value expressions
                m_settings = new Settings(new Dictionary<string, object> {{ "Form", this }}.RegisterSymbols());
                m_settings.PropertyChanged += Settings_PropertyChanged;

                // After attaching to event, refresh all properties to pickup any needed initial values
                m_settings.UpdateProperties();

                // Assign application version to visible label
                Version version = AssemblyInfo.EntryAssembly.Version;
                VersionLabel.Text = $"Version {version.Major}.{version.Minor}.{version.Build}";

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
                // Make sure any pending operation is canceled
                m_cancellationTokenSource?.Cancel();

                m_formClosing = true;

                // Save current window size/location
                this.SaveLayout();

                // Save any updates to current screen values
                m_settings?.Save();
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
            m_cancellationTokenSource?.Dispose();
        }

        private void Settings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (m_formClosing)
                return;

            switch (e.PropertyName)
            {
                case nameof(Settings.XDARootURL):
                    XDAJsonApiUrl = m_settings.XDARootURL;
                    break;
                case nameof(Settings.StartDateTimeForQuery):
                case nameof(Settings.EndDateTimeForQuery):
                    CalculateSelectedTimeRange();
                    break;
                case nameof(Settings.ExportWaveformData):
                case nameof(Settings.ExportEventData):
                    if (m_settings.ExportWaveformData && !m_settings.ExportEventData)
                        ExportEventDataCheckBox.Checked = true;
                    break;
            }
        }

        private void LoadSourcesButton_Click(object sender, EventArgs e)
        {
            SetButtonsEnabledState(false);
            SelectedSourcesCheckedListBox.Items.Clear();

            Task.Run(async () =>
            {
                using (m_cancellationTokenSource)
                    m_cancellationTokenSource = new CancellationTokenSource();

                CancellationToken cancellationToken = m_cancellationTokenSource.Token;

                try
                {
                    Ticks startTime = DateTime.UtcNow.Ticks;

                    if (m_settings.SourceQueryTypeIsLines)
                    {
                        ShowUpdateMessage($"Loading configured openXDA lines from \"{GetAPIFunctionURL("GetLines")}\"...");

                        DescriptiveLine[] lines = (await CallAPIFunction("GetLines", cancellationToken)).ToObject<DescriptiveLine[]>();
                       
                        BeginInvoke(new Action(() =>
                        {
                            try
                            {
                                SelectedSourcesCheckedListBox.ValueMember = "ID";
                                SelectedSourcesCheckedListBox.DisplayMember = null;
                                SelectedSourcesCheckedListBox.Items.AddRange(lines);
                                ShowUpdateMessage($"Successfully loaded {lines.Length:N0} lines in {(DateTime.UtcNow.Ticks - startTime).ToElapsedTimeString(3)}.");
                            }
                            catch (Exception ex)
                            {
                                ShowErrorMessage($"Failed to load line list selection: {ex.Message}");
                            }
                        }));
                    }
                    else if (m_settings.SourceQueryTypeIsMeters)
                    {
                        ShowUpdateMessage($"Loading configured openXDA meters from \"{GetAPIFunctionURL("GetMeters")}\"...");

                        DescriptiveMeter[] meters = (await CallAPIFunction("GetMeters", cancellationToken)).ToObject<DescriptiveMeter[]>();

                        BeginInvoke(new Action(() =>
                        {
                            try
                            {
                                SelectedSourcesCheckedListBox.ValueMember = "ID";                            
                                SelectedSourcesCheckedListBox.DisplayMember = null;
                                SelectedSourcesCheckedListBox.Items.AddRange(meters);
                                ShowUpdateMessage($"Successfully loaded {meters.Length:N0} meters in {(DateTime.UtcNow.Ticks - startTime).ToElapsedTimeString(3)}.");
                            }
                            catch (Exception ex)
                            {
                                ShowErrorMessage($"Failed to load meter list selection: {ex.Message}");
                            }
                        }));
                    }
                    else
                    {
                        throw new InvalidOperationException("No source type was selected for openXDA load operation.");
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessage($"Failed to load source data: {ex.Message}");
                }
                finally
                {
                    SetButtonsEnabledState(true);
                }
            });
        }

        private void SelectedSourcesCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            m_updateSelections.RunOnceAsync();
        }

        private void SelectAllEventsButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < SelectedSourcesCheckedListBox.Items.Count; i++)
                SelectedSourcesCheckedListBox.SetItemChecked(i, true);
        }

        private void UnselectAllEventsButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < SelectedSourcesCheckedListBox.Items.Count; i++)
                SelectedSourcesCheckedListBox.SetItemChecked(i, false);
        }

        private void ClearTimeRangeButton_Click(object sender, EventArgs e)
        {
            ResetTimeRange();
        }

        private void CancelExportButton_Click(object sender, EventArgs e)
        {
            m_cancellationTokenSource?.Cancel();
        }

        private void ClearTextButton_Click(object sender, EventArgs e)
        {
            ClearUpdateMessages();
        }

        private void TransferToRepositoryButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(m_settings.RepositoryConnectionString))
            {
                MessageBox.Show(this, "No repository connection string was defined.\n\nCannot transfer openXDA event data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (m_settings.StartDateTimeForQuery <= DateTimePicker.MinimumDateTime && m_settings.EndDateTimeForQuery >= DateTimePicker.MaximumDateTime)
            {
                if (MessageBox.Show(this, "No start and end times have been defined for the export query.\n\nAre you sure you want to export all available openXDA event data?", "Export Volume Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
            }

            if (string.IsNullOrEmpty(m_lineIDList) && string.IsNullOrEmpty(m_meterIDList))
            {
                if (MessageBox.Show(this, "No meters or lines have been selected for the export query.\n\nAre you sure you want to export all available openXDA event data?", "Export Volume Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
            }

            SetButtonsEnabledState(false);

            // Spin up export operations task
            Task.Run(async () =>
            {
                const int ProgressSteps = 7; // 6 tasks plus 1 for complete

                using (m_cancellationTokenSource)
                    m_cancellationTokenSource = new CancellationTokenSource();

                CancellationToken cancellationToken = m_cancellationTokenSource.Token;

                try
                {
                    SetExportProgressBarMaximum(ProgressSteps);
                    int progress = 0;

                    void IncrementProgress()
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        UpdateExportProgressBar(progress);
                        progress++;
                    }

                    // Setup event query filter
                    string eventFilter = JsonConvert.SerializeObject(new EventFilter
                    {
                        LineIDList = m_lineIDList,
                        MeterIDList = m_meterIDList,
                        StartDate = m_settings.StartDateTimeForQuery > DateTimePicker.MinimumDateTime ? m_settings.StartDateTimeForQuery : (DateTime?)null,
                        EndDate = m_settings.EndDateTimeForQuery < DateTimePicker.MaximumDateTime ? m_settings.EndDateTimeForQuery : (DateTime?)null
                    });

                    IncrementProgress();

                    Event[] events = null;

                    // Export event data
                    if (m_settings.ExportEventData)
                        events = await QueryAndTransferToRepository<Event>("GetEvents", eventFilter, cancellationToken);

                    if (events?.Length > 0)
                        SetExportProgressBarMaximum(ProgressSteps + (m_settings.ExportWaveformData ? events.Length - 1 : 0) + (m_settings.ExportFrequencyDomainData ? events.Length - 1 : 0));

                    IncrementProgress();

                    // Export fault data
                    if (m_settings.ExportFaultData)
                        await QueryAndTransferToRepository<Fault>("GetFaults", eventFilter, cancellationToken);

                    IncrementProgress();

                    // Export disturbance data
                    if (m_settings.ExportDisturbanceData)
                        await QueryAndTransferToRepository<Disturbance>("GetDisturbances", eventFilter, cancellationToken);

                    IncrementProgress();

                    // Export breaker operation data
                    if (m_settings.ExportBreakerOperationData)
                        await QueryAndTransferToRepository<BreakerOperation>("GetBreakerOperations", eventFilter, cancellationToken);

                    IncrementProgress();

                    if (events?.Length > 0)
                    {
                        // Export waveform data
                        if (m_settings.ExportWaveformData)
                        {
                            foreach (Event record in events)
                            {
                                string eventDataFilter = JsonConvert.SerializeObject(new EventDataFilter
                                {
                                    EventID = $"{record.ID}"
                                });

                                await QueryAndTransferToRepository<dynamic>("GetEventWaveformData", eventDataFilter, cancellationToken, $"Event {record.ID} Waveform Data");
                                IncrementProgress();
                            }
                        }
                        else
                        {
                            IncrementProgress();
                        }

                        // Export frequency domain data
                        if (m_settings.ExportFrequencyDomainData)
                        {
                            foreach (Event record in events)
                            {
                                string eventDataFilter = JsonConvert.SerializeObject(new EventDataFilter
                                {
                                    EventID = $"{record.ID}"
                                });

                                await QueryAndTransferToRepository<dynamic>("GetEventFrequencyDomainData", eventDataFilter, cancellationToken, $"Event {record.ID} Frequency Domain Data");
                                IncrementProgress();
                            }
                        }
                        else
                        {
                            IncrementProgress();
                        }
                    }                    
                }
                catch (Exception ex)
                {
                    ShowErrorMessage(cancellationToken.IsCancellationRequested ? "Export canceled." : $"Failed during export: {ex.Message}");
                }
                finally
                {
                    UpdateExportProgressBar(ProgressSteps);
                    SetButtonsEnabledState(true);
                }
            });
        }

        private async Task<T[]> QueryAndTransferToRepository<T>(string function, string eventFilter, CancellationToken cancellationToken, string typeName = null)
        {
            Ticks startTime = DateTime.UtcNow.Ticks;
            
            if (typeName == null)
                typeName = typeof(T).Name;
            
            ShowUpdateMessage($"Querying {typeName} records from \"{GetAPIFunctionURL(function)}\"...");
            JArray result;

            try
            {
                result = await CallAPIFunction(function, cancellationToken, eventFilter);
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Failed during {typeName} API query: {ex.Message}");
                return new T[0];
            }

            ShowUpdateMessage($"Deserializing {typeName} query results...");
            T[] results;

            try
            {
                results = result.ToObject<T[]>();
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Failed during {typeName} query result deserialization: {ex.Message}");
                return new T[0];
            }

            if (results.Length == 0)
            {
                ShowUpdateMessage($"No {typeName} records were found, skipping repository transfer operations...");
                return new T[0];
            }

            ShowUpdateMessage($"Serializing {typeName} records...");
            List<byte[]> records;

            try
            {
                records = results.Select(value => Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value).ToString())).ToList();
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Failed during {typeName} record serialization: {ex.Message}");
                return new T[0];
            }

            ShowUpdateMessage($"Successfully queried and serialized {records.Count:N0} {typeName} records in {(DateTime.UtcNow.Ticks - startTime).ToElapsedTimeString(3)}.");
            
            await TransferToRepository(records, typeName, cancellationToken);

            return results;
        }

        private async Task TransferToRepository(List<byte[]> records, string typeName, CancellationToken cancellationToken)
        {
            Ticks startTime = DateTime.UtcNow.Ticks;
            ShowUpdateMessage($"Transferring {typeName} records to the repository...");

            if (m_settings.RepositoryIsAzure)
            {
                // Establish connection to Azure Event Hub
                EventHubClient eventHub;

                try
                {
                    EventHubsConnectionStringBuilder builder = new EventHubsConnectionStringBuilder(m_settings.RepositoryConnectionString);
                    eventHub = EventHubClient.CreateFromConnectionString(builder.ToString());
                }
                catch (Exception ex)
                {
                    ShowErrorMessage($"Failed to connect to Azure Event Hub: {ex.Message}");
                    return;
                }

                await TransferToAzure(eventHub, records, typeName, cancellationToken);

                ShowUpdateMessage($"Successfully transferred {SI2.ToScaledString(records.Sum(record => (long)record.Length), "B")} of {records.Count:N0} JSON serialized {typeName} records to Azure Event Hub in {(DateTime.UtcNow.Ticks - startTime).ToElapsedTimeString(3)}.");
            }
            else if (m_settings.RepositoryIsAWS)
            {
                // TODO: Establish connection to Amazon Kinesis
                await TransferToAWS(null, records, typeName, cancellationToken);

                ShowUpdateMessage($"Successfully transferred {SI2.ToScaledString(records.Sum(record => (long)record.Length), "B")} of {records.Count:N0} JSON serialized {typeName} records to Amazon Kinesis in {(DateTime.UtcNow.Ticks - startTime).ToElapsedTimeString(3)}.");
            }
            else if (m_settings.RepositoryIsGoogle)
            {
                // TODO: Establish connection to Google Pub/Sub
                await TransferToGoogle(null, records, typeName, cancellationToken);

                ShowUpdateMessage($"Successfully transferred {SI2.ToScaledString(records.Sum(record => (long)record.Length), "B")} of {records.Count:N0} JSON serialized {typeName} records to Google Pub/Sub in {(DateTime.UtcNow.Ticks - startTime).ToElapsedTimeString(3)}.");
            }
            else if (m_settings.RepositoryIsPQDS)
            {
                await TransferToPQDS(null, records, typeName, cancellationToken);

                ShowUpdateMessage($"Successfully transferred {SI2.ToScaledString(records.Sum(record => (long)record.Length), "B")} of {records.Count:N0} JSON serialized {typeName} records to PQDS.csv in {(DateTime.UtcNow.Ticks - startTime).ToElapsedTimeString(3)}.");
            }
            else
            {
                ShowErrorMessage("No repository type selected for data transfer");
            }
        }

        private async Task TransferToAzure(EventHubClient eventHub, List<byte[]> records, string typeName, CancellationToken cancellationToken)
        {
            List<EventData> samples = new List<EventData>();
            int size = 0;

            async Task SendToEventHub()
            {
                cancellationToken.ThrowIfCancellationRequested();

                // Write data to event hub
                if (samples.Count > 0)
                    await eventHub.SendAsync(samples, typeName);

                samples.Clear();
            }

            foreach (byte[] bytes in records)
            {
                cancellationToken.ThrowIfCancellationRequested();

                EventData record = new EventData(bytes);

                // Keep total post size under specified limit
                if (size + bytes.Length < m_settings.PostSizeLimit)
                {
                    samples.Add(record);
                }
                else
                {
                    await SendToEventHub();
                    samples.Add(record);
                    size = 0;
                }

                size += bytes.Length;
            }

            // Transfer any remaining events
            await SendToEventHub();
        }

        private /*async*/ Task TransferToAWS(object connection, List<byte[]> records, string typeName, CancellationToken cancellationToken)
        {
            // TODO: Add ability to transfer to Amazon Kinesis minding post size limit
            return Task.CompletedTask;
        }

        private /*async*/ Task TransferToGoogle(object connection, List<byte[]> records, string typeName, CancellationToken cancellationToken)
        {
            // TODO: Add ability to transfer to Google Pub/Sub minding post size limit
            return Task.CompletedTask;
        }

        private async Task TransferToPQDS(object connection, List<byte[]> records, string typeName, CancellationToken cancellationToken)
        {
            byte[] newLine = Encoding.UTF8.GetBytes(Environment.NewLine);
            string rootPath = "";

            if (Directory.Exists(m_settings.RepositoryConnectionString))
                rootPath = FilePath.AddPathSuffix(m_settings.RepositoryConnectionString);

            string filePath = FilePath.GetAbsolutePath($"{rootPath}{typeName}.txt");

            if (File.Exists(filePath))
                File.Delete(filePath);

            using (FileStream destination = File.OpenWrite(filePath))
            {
                foreach (byte[] record in records)
                {
                    if (record?.Length > 0)
                    {
                        await destination.WriteAsync(record, 0, record.Length, cancellationToken);
                        await destination.WriteAsync(newLine, 0, newLine.Length, cancellationToken);
                    }
                }
            }
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
                try
                {
                    if (Visible && m_formLoaded)
                        m_settings?.UpdateProperties();
                }
                catch (Exception ex)
                {
                    ShowErrorMessage($"Failed to update settings for form element change: {ex.Message}");
                }
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
                try
                {
                    m_log.Publish(MessageLevel.Info, "StatusMessage", message);

                    lock (MessageOutputTextBox)
                        MessageOutputTextBox.AppendText($"{message}{Environment.NewLine}{Environment.NewLine}");
                }
                catch (Exception ex)
                {
                    m_log.Publish(MessageLevel.Error, "StatusMessage", exception: ex);
                }
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
                try
                {
                    m_log.Publish(MessageLevel.Error, "ErrorMessage", message);

                    lock (MessageOutputTextBox)
                        MessageOutputTextBox.AppendText($"ERROR: {message}{Environment.NewLine}{Environment.NewLine}");
                }
                catch (Exception ex)
                {
                    m_log.Publish(MessageLevel.Error, "ErrorMessage", exception: ex);
                }
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
                try
                {
                    lock (MessageOutputTextBox)
                        MessageOutputTextBox.Text = "";
                }
                catch (Exception ex)
                {
                    ShowErrorMessage($"Failed to clear messages: {ex.Message}");
                }
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
                try
                {
                    LoadSourcesButton.Enabled = enabled;
                    SelectAllEventsButton.Enabled = enabled;
                    UnselectAllEventsButton.Enabled = enabled;
                    ClearTimeRangeButton.Enabled = enabled;
                    TransferToRepositoryButton.Enabled = enabled;
                    CancelExportButton.Enabled = !enabled;
                }
                catch (Exception ex)
                {
                    ShowErrorMessage($"Failed to assign buttons enabled state: {ex.Message}");
                }
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
                try
                {
                    if (value < ExportProgressBar.Minimum)
                        value = ExportProgressBar.Minimum;

                    if (value > ExportProgressBar.Maximum)
                        ExportProgressBar.Maximum = value;

                    ExportProgressBar.Value = value;
                }
                catch (Exception ex)
                {
                    ShowErrorMessage($"Failed to update export progress bar current value: {ex.Message}");
                }
            }
        }

        private void SetExportProgressBarMaximum(int maximum)
        {
            if (m_formClosing)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action<int>(SetExportProgressBarMaximum), maximum);
            }
            else
            {
                try
                {
                    ExportProgressBar.Maximum = maximum;
                }
                catch (Exception ex)
                {
                    ShowErrorMessage($"Failed to update export progress bar maximum value: {ex.Message}");
                }
            }
        }

        private void ResetTimeRange()
        {
            if (m_formClosing)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action(ResetTimeRange));
            }
            else
            {
                try
                {
                    StartDateTimePicker.Value = DateTimePicker.MinimumDateTime;
                    EndDateTimePicker.Value = DateTimePicker.MaximumDateTime;
                }
                catch (Exception ex)
                {
                    ShowErrorMessage($"Failed to reset time range: {ex.Message}");
                }
            }
        }

        private void CalculateSelectedTimeRange()
        {
            if (m_formClosing)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action(CalculateSelectedTimeRange));
            }
            else
            {
                try
                {
                    if (StartDateTimePicker.Value <= DateTimePicker.MinimumDateTime && EndDateTimePicker.Value >= DateTimePicker.MaximumDateTime)
                        SelectedTimeRangeLabel.Text = $"{SelectedTimeRangeLabel.Tag} Maximum (No Time Filter)";
                    else
                        SelectedTimeRangeLabel.Text = $"{SelectedTimeRangeLabel.Tag} {new Ticks(EndDateTimePicker.Value - StartDateTimePicker.Value).ToElapsedTimeString(0)}";
                }
                catch (Exception ex)
                {
                    ShowErrorMessage($"Failed to calculate selected time range: {ex.Message}");
                }
            }
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
        private static string GetAPIFunctionURL(string function) => $"{XDAJsonApiUrl}{function}";

        private static async Task<JArray> CallAPIFunction(string function, CancellationToken cancellationToken, string content = null)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, GetAPIFunctionURL(function));

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrWhiteSpace(content))
                request.Content = new StringContent(content, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await s_http.SendAsync(request, cancellationToken);

            content = await response.Content.ReadAsStringAsync();

            cancellationToken.ThrowIfCancellationRequested();

            return string.IsNullOrWhiteSpace(content) || content == "null" ? new JArray() : JArray.Parse(content);
        }

        #endregion
    }
}
