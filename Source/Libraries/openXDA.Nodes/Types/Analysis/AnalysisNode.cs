//******************************************************************************************************
//  AnalysisNode.cs - Gbtc
//
//  Copyright © 2021, Grid Protection Alliance.  All Rights Reserved.
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
//  01/07/2021 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using FaultData;
using FaultData.Configuration;
using FaultData.DataAnalysis;
using FaultData.DataOperations;
using FaultData.DataReaders;
using FaultData.DataSets;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using GSF.Threading;
using log4net;
using openXDA.Configuration;
using openXDA.Model;
using IDataReader = FaultData.DataReaders.IDataReader;

namespace openXDA.Nodes.Types.Analysis
{
    public class AnalysisNode : NodeBase, IDisposable
    {
        #region [ Members ]

        // Nested Types
        private class NodeSettings
        {
            public NodeSettings(Action<object> configure) =>
                configure(this);

            [Category]
            [SettingName(SystemSection.CategoryName)]
            public SystemSection SystemSettings { get; } = new SystemSection();

            [Category]
            [SettingName(TaskProcessorSection.CategoryName)]
            public TaskProcessorSection TaskProcessorSettings { get; } = new TaskProcessorSection();
        }

        private class PollingSynchronizedOperation : SynchronizedOperationBase
        {
            private Func<Task> PollingFunction { get; }

            public PollingSynchronizedOperation(AnalysisNode node)
                : base(() => { })
            {
                PollingFunction = node.GetPollingFunction();
            }

            protected override void ExecuteActionAsync()
            {
                Task.Run(async () =>
                {
                    try { await PollingFunction(); }
                    catch (Exception ex) { Log.Error(ex.Message, ex); }

                    if (ExecuteAction())
                        ExecuteActionAsync();
                });
            }
        }

        private class AnalysisWebController : ApiController
        {
            private AnalysisNode Node { get; }

            public AnalysisWebController(AnalysisNode node) =>
                Node = node;

            [HttpPost]
            public void PollTaskQueue() =>
                Node.PollingOperation.RunOnceAsync();
        }

        #endregion

        #region [ Constructors ]

        public AnalysisNode(Host host, Node definition, NodeType type)
            : base(host, definition, type)
        {
            PollingOperation = new PollingSynchronizedOperation(this);

            Action<object> configurator = GetConfigurator();
            UpdateTaskProcessorSettings(configurator);
            PollingOperation.RunOnceAsync();
        }

        #endregion

        #region [ Properties ]

        private PollingSynchronizedOperation PollingOperation { get; }
        private string MeterFilterQuery { get; set; }
        private int ProcessingThreadCount { get; set; }
        private bool IsDisposed { get; set; }

        #endregion

        #region [ Methods ]

        public override IHttpController CreateWebController() =>
            new AnalysisWebController(this);

        public void Dispose() =>
            IsDisposed = true;

        protected override void OnReconfigure(Action<object> configurator) =>
            UpdateTaskProcessorSettings(configurator);

        private Func<Task> GetPollingFunction()
        {
            AnalysisTaskProcessor taskProcessor = new AnalysisTaskProcessor(Definition.ID, CreateDbConnection);

            // This is used to track the processing thread
            // count so resource usage can be throttled
            int threadCount = 0;
            int GetThreadCount() => Interlocked.CompareExchange(ref threadCount, 0, 0);

            // This will be used to notify the polling function when processing threads have completed
            TaskCreationOptions runContinuationsAsynchronously = TaskCreationOptions.RunContinuationsAsynchronously;
            TaskCompletionSource<object> taskCompletionSource = null;

            return async () =>
            {
                if (IsDisposed)
                    return;

                int maxThreadCount = ProcessingThreadCount;

                // If there are too many processing threads,
                // wait for some to complete before polling
                while (GetThreadCount() >= maxThreadCount)
                {
                    TaskCompletionSource<object> processingThreadComplete = new TaskCompletionSource<object>(runContinuationsAsynchronously);
                    Interlocked.Exchange(ref taskCompletionSource, processingThreadComplete);

                    // Check again to avoid race conditions
                    if (GetThreadCount() >= maxThreadCount)
                        await processingThreadComplete.Task;
                }

                if (IsDisposed)
                    return;

                AnalysisTask task = taskProcessor.Poll(MeterFilterQuery);

                // It's not necessary to keep
                // polling if the queue is empty
                if (task is null)
                    return;

                // Add another processing thread for the polled task
                Interlocked.Increment(ref threadCount);

                _ = Task.Run(() =>
                {
                    try
                    {
                        Process(task);
                        taskProcessor.Dequeue(task);
                    }
                    finally
                    {
                        Interlocked.Decrement(ref threadCount);

                        // Notify that processing thread has completed
                        TaskCompletionSource<object> processingThreadComplete = Interlocked.Exchange(ref taskCompletionSource, null);
                        processingThreadComplete?.SetResult(null);
                    }

                    // Ensure there are no more tasks
                    // in the queue for the same meter
                    PollingOperation.RunOnceAsync();
                });

                // Keep polling until the queue is empty
                PollingOperation.RunOnceAsync();
            };
        }

        private void Process(AnalysisTask task)
        {
            try
            {
                FileGroup fileGroup = task.FileGroup;
                Meter meter = task.Meter;
                Process(fileGroup, meter);
                SaveMeterConfiguration(fileGroup, meter);
            }
            catch (Exception ex)
            {
                DataFile dataFile = task.FileGroup.DataFiles.First();
                string filePath = Path.ChangeExtension(dataFile.FilePath, "*");
                string message = $"An error occurred while processing file group \"{filePath}\": {ex.Message}";
                Exception wrapper = new Exception(message, ex);
                Log.Error(wrapper.Message, wrapper);
            }
        }

        private void Process(FileGroup fileGroup, Meter meter)
        {
            DateTime startTime = DateTime.UtcNow;
            Action<object> configurator = GetConfigurator();
            TimeZoneConverter timeZoneConverter = new TimeZoneConverter(configurator);
            fileGroup.ProcessingStartTime = timeZoneConverter.ToXDATimeZone(startTime);
            fileGroup.ProcessingVersion++;
            UpdateFileGroup(fileGroup);

            DataFile dataFile = fileGroup.DataFiles.First();
            string fileGroupPath = Path.ChangeExtension(dataFile.FilePath, "*");
            Log.Info($"Reading data from file group \"{fileGroupPath}\"...");

            DataReaderFactory readerFactory = new DataReaderFactory(CreateDbConnection);
            IDataReader reader = readerFactory.CreateDataReader(fileGroup);
            MeterDataSet meterDataSet = reader.Parse(fileGroup);

            if (!(meterDataSet is null))
            {
                DataFile primaryFile = reader.GetPrimaryDataFile(fileGroup);
                meterDataSet.CreateDbConnection = CreateDbConnection;
                meterDataSet.FilePath = primaryFile.FilePath;
                meterDataSet.FileGroup = fileGroup;
                meterDataSet.Configure = configurator;
                meterDataSet.Meter.AssetKey = meter.AssetKey;

                // Shift date/time values to the configured time zone and set the start and end time values on the file group
                NodeSettings settings = new NodeSettings(configurator);
                TimeZoneInfo defaultMeterTimeZone = settings.SystemSettings.DefaultMeterTimeZoneInfo;
                TimeZoneInfo meterTimeZone = meter.GetTimeZoneInfo(defaultMeterTimeZone);
                Func<DateTime, DateTime> toXDATime = timeZoneConverter.GetMeterTimeZoneConverter(meterTimeZone);
                ShiftTime(meterDataSet, toXDATime);
                SetDataTimeRange(meterDataSet);
                UpdateFileGroup(fileGroup);

                Log.Info($"Processing file group \"{fileGroupPath}\"...");
                Process(meterDataSet);
                Log.Info($"Finished processing file group \"{fileGroupPath}\".");
            }

            DateTime endTime = DateTime.UtcNow;
            fileGroup.ProcessingEndTime = timeZoneConverter.ToXDATimeZone(endTime);
            UpdateFileGroup(fileGroup);
        }

        private void Process(MeterDataSet meterDataSet)
        {
            List<DataOperation> dataOperationDefinitions;

            using (AdoDataConnection connection = CreateDbConnection())
            {
                TableOperations<DataOperation> dataOperationTable = new TableOperations<DataOperation>(connection);

                // Load the data operations from the database
                dataOperationDefinitions = dataOperationTable
                    .QueryRecords("LoadOrder")
                    .ToList();
            }

            PluginFactory<IDataOperation> dataOperationFactory = new PluginFactory<IDataOperation>();

            foreach (DataOperation dataOperationDefinition in dataOperationDefinitions)
            {
                if (IsDisposed)
                    return;

                try
                {
                    Log.Debug($"Executing data operation '{dataOperationDefinition.UnqualifiedTypeName}'...");

                    // Call the execute method on the data operation to perform in-memory data transformations
                    string assemblyName = dataOperationDefinition.AssemblyName;
                    string typeName = dataOperationDefinition.TypeName;
                    IDataOperation dataOperation = dataOperationFactory.Create(assemblyName, typeName);

                    using (dataOperation as IDisposable)
                    {
                        meterDataSet.Configure(dataOperation);
                        dataOperation.Execute(meterDataSet);
                    }

                    Log.Debug($"Finished executing data operation '{dataOperationDefinition.UnqualifiedTypeName}'.");
                }
                catch (Exception ex)
                {
                    // Log the error and skip to the next data operation
                    string message = $"An error occurred while executing data operation of type '{dataOperationDefinition.TypeName}' on data from meter '{meterDataSet.Meter.AssetKey}': {ex.Message}";
                    Exception wrapper = new Exception(message, ex);
                    Log.Error(wrapper.Message, wrapper);
                }
            }

            FileGroup fileGroup = meterDataSet.FileGroup;
            _ = NotifyEventEmailNode(fileGroup.ID, fileGroup.ProcessingVersion);
            _ = NotifyEPRICapBankAnalysisNode(fileGroup.ID, fileGroup.ProcessingVersion);
        }

        // Saves the current meter configuration to the database.
        private void SaveMeterConfiguration(FileGroup fileGroup, Meter meter)
        {
            MeterSettingsSheet meterSettingsSheet = new MeterSettingsSheet(meter);

            using (AdoDataConnection connection = CreateDbConnection())
            {
                const string ConfigKey = "openXDA";
                TableOperations<MeterConfiguration> meterConfigurationTable = new TableOperations<MeterConfiguration>(connection);
                meterSettingsSheet.UpdateConfiguration(meterConfigurationTable, ConfigKey);

                RecordRestriction queryRestriction =
                    new RecordRestriction("MeterID = {0}", meter.ID) &
                    new RecordRestriction("ConfigKey = {0}", ConfigKey) &
                    new RecordRestriction("DiffID IS NULL");

                MeterConfiguration currentConfiguration = meterConfigurationTable.QueryRecord("ID DESC", queryRestriction);
                connection.ExecuteNonQuery("DELETE FROM FileGroupMeterConfiguration WHERE FileGroupID = {0} AND MeterConfigurationID IN (SELECT ID FROM MeterConfiguration WHERE ConfigKey = {1})", fileGroup.ID, ConfigKey);
                connection.ExecuteNonQuery("INSERT INTO FileGroupMeterConfiguration VALUES({0}, {1})", fileGroup.ID, currentConfiguration.ID);
            }
        }

        private void ShiftTime(MeterDataSet meterDataSet, Func<DateTime, DateTime> toXDATime)
        {
            foreach (DataSeries dataSeries in meterDataSet.DataSeries)
            {
                foreach (DataPoint dataPoint in dataSeries.DataPoints)
                    dataPoint.Time = toXDATime(dataPoint.Time);
            }

            foreach (DataSeries dataSeries in meterDataSet.Digitals)
            {
                foreach (DataPoint dataPoint in dataSeries.DataPoints)
                    dataPoint.Time = toXDATime(dataPoint.Time);
            }

            for (int i = 0; i < meterDataSet.ReportedDisturbances.Count; i++)
            {
                ReportedDisturbance disturbance = meterDataSet.ReportedDisturbances[i];
                GSF.PQDIF.Logical.Phase phase = disturbance.Phase;
                DateTime time = toXDATime(disturbance.Time);
                double max = disturbance.Maximum;
                double min = disturbance.Minimum;
                double avg = disturbance.Average;
                TimeSpan duration = disturbance.Duration;
                GSF.PQDIF.Logical.QuantityUnits units = disturbance.Units;
                meterDataSet.ReportedDisturbances[i] = new ReportedDisturbance(phase, time, max, min, avg, duration, units);
            }
        }

        private void UpdateFileGroup(FileGroup fileGroup)
        {
            using (AdoDataConnection connection = CreateDbConnection())
            {
                TableOperations<FileGroup> fileGroupTable = new TableOperations<FileGroup>(connection);
                fileGroupTable.UpdateRecord(fileGroup);
            }
        }

        private void UpdateTaskProcessorSettings(Action<object> configurator)
        {
            NodeSettings settings = new NodeSettings(configurator);
            MeterFilterQuery = settings.TaskProcessorSettings.MeterFilterQuery;
            ProcessingThreadCount = settings.TaskProcessorSettings.ProcessingThreadCount;
        }

        private async Task NotifyEventEmailNode(int fileGroupID, int processingVersion)
        {
            Type nodeType = typeof(Email.EventEmailNode);
            string typeName = nodeType.FullName;
            int? result = QueryNodeID(typeName);

            if (result is null)
                return;

            void ConfigureRequest(HttpRequestMessage request)
            {
                int nodeID = result.GetValueOrDefault();
                string action = "TriggerForFileGroup";
                NameValueCollection queryParameters = new NameValueCollection();
                queryParameters.Add("fileGroupID", fileGroupID.ToString());
                queryParameters.Add("processingVersion", processingVersion.ToString());

                string url = Host.BuildURL(nodeID, action, queryParameters);
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(url);
            }

            using (HttpResponseMessage response = await Host.SendWebRequestAsync(ConfigureRequest))
            {
                if (!response.IsSuccessStatusCode)
                {
                    string body = await response.Content.ReadAsStringAsync();

                    string logMessage = new StringBuilder()
                        .AppendLine("Event email notification failed.")
                        .AppendLine($"Status: {response.StatusCode}")
                        .AppendLine("Body:")
                        .Append(body)
                        .ToString();

                    Log.Debug(logMessage);
                }
            }
        }

        private async Task NotifyEPRICapBankAnalysisNode(int fileGroupID, int processingVersion)
        {
            Type nodeType = typeof(EPRICapBankAnalysis.EPRICapBankAnalysisNode);
            string typeName = nodeType.FullName;
            int? result = QueryNodeID(typeName);

            if (result is null)
                return;

            void ConfigureRequest(HttpRequestMessage request)
            {
                int nodeID = result.GetValueOrDefault();
                string action = "Analyze";
                NameValueCollection queryParameters = new NameValueCollection();
                queryParameters.Add("fileGroupID", fileGroupID.ToString());
                queryParameters.Add("processingVersion", processingVersion.ToString());

                string url = Host.BuildURL(nodeID, action, queryParameters);
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(url);
            }

            using (HttpResponseMessage response = await Host.SendWebRequestAsync(ConfigureRequest))
            {
                if (!response.IsSuccessStatusCode)
                {
                    string body = await response.Content.ReadAsStringAsync();

                    string logMessage = new StringBuilder()
                        .AppendLine("EPRI Cap Bank Analysis notification failed.")
                        .AppendLine($"Status: {response.StatusCode}")
                        .AppendLine("Body:")
                        .Append(body)
                        .ToString();

                    Log.Debug(logMessage);
                }
            }
        }

        private int? QueryNodeID(string nodeTypeName)
        {
            using (AdoDataConnection connection = CreateDbConnection())
            {
                const string QueryFormat =
                    "SELECT Node.ID " +
                    "FROM " +
                    "    ActiveHost JOIN " +
                    "    Node ON Node.HostRegistrationID = ActiveHost.ID JOIN " +
                    "    NodeType ON Node.NodeTypeID = NodeType.ID " +
                    "WHERE NodeType.TypeName = {0}";

                return connection.ExecuteScalar<int?>(QueryFormat, nodeTypeName);
            }
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(AnalysisNode));

        // Static Methods
        private static void SetDataTimeRange(MeterDataSet meterDataSet)
        {
            DateTime dataStartTime = meterDataSet.DataSeries
                .Concat(meterDataSet.Digitals)
                .Where(dataSeries => dataSeries.DataPoints.Any())
                .Select(dataSeries => dataSeries.DataPoints.First().Time)
                .DefaultIfEmpty()
                .Min();

            DateTime dataEndTime = meterDataSet.DataSeries
                .Concat(meterDataSet.Digitals)
                .Where(dataSeries => dataSeries.DataPoints.Any())
                .Select(dataSeries => dataSeries.DataPoints.Last().Time)
                .DefaultIfEmpty()
                .Max();

            if (dataStartTime != default)
                meterDataSet.FileGroup.DataStartTime = dataStartTime;

            if (dataEndTime != default)
                meterDataSet.FileGroup.DataEndTime = dataEndTime;
        }

        #endregion
    }
}
