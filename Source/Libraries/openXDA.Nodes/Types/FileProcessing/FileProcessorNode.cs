//******************************************************************************************************
//  FileProcessorNode.cs - Gbtc
//
//  Copyright © 2020, Grid Protection Alliance.  All Rights Reserved.
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
//  12/15/2020 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using GSF.IO;
using GSF.Threading;
using log4net;
using openXDA.Configuration;
using openXDA.Model;
using openXDA.Nodes.Types.Analysis;
using AnalysisTask = openXDA.Nodes.Types.Analysis.AnalysisTask;
using FileShare = openXDA.Configuration.FileWatcher.FileShare;

namespace openXDA.Nodes.Types.FileProcessing
{
    public class FileProcessorNode : NodeBase, IDisposable
    {
        #region [ Members ]

        // Nested Types
        private class Settings
        {
            public Settings(Action<object> configure) =>
                configure(this);

            [Category]
            [SettingName(FileWatcherSection.CategoryName)]
            public FileWatcherSection FileWatcherSettings { get; } = new FileWatcherSection();

            [Category]
            [SettingName(FileEnumeratorSection.CategoryName)]
            public FileEnumeratorSection FileEnumeratorSettings { get; } = new FileEnumeratorSection();

            [Category]
            [SettingName(FileProcessorSection.CategoryName)]
            public FileProcessorSection FileProcessorSettings { get; } = new FileProcessorSection();
        }

        private class FileProcessorWebController : ApiController
        {
            private FileProcessorNode Node { get; }

            public FileProcessorWebController(FileProcessorNode node) =>
                Node = node;

            [HttpPost]
            public void Enumerate() =>
                Node.FileProcessor.EnumerateWatchDirectories();

            [HttpPost]
            public void FlushAndEnumerate()
            {
                Node.FileProcessor.ResetIndexAndStatistics();
                Node.FileProcessor.EnumerateWatchDirectories();
            }

            [HttpPost]
            public void StopEnumeration() =>
                Node.FileProcessor.StopEnumeration();

            [HttpGet]
            public HttpResponseMessage Status()
            {
                string status = Node.GetStatus();
                HttpResponseMessage response = new HttpResponseMessage();
                response.Content = new StringContent(status);
                return response;
            }
        }

        private class WorkItem
        {
            public string FilePath { get; }
            public int Priority { get; }
            public int RetryCount => MutableRetryCount;
            private int MutableRetryCount { get; set; }

            public WorkItem(string filePath, int priority)
            {
                FilePath = filePath;
                Priority = priority;
            }

            public void IncrementRetryCount() => MutableRetryCount++;
        }

        // Fields
        private int m_processedFileCount;

        #endregion

        #region [ Constructors ]

        public FileProcessorNode(Host host, Node definition, NodeType type)
            : base(host, definition, type)
        {
            FileProcessor = new FileProcessor();
            WorkQueue = new ConcurrentQueue<WorkItem>();

            Action pollingFunction = GetPollingFunction();
            void LogException(Exception ex) => Log.Error(ex.Message, ex);
            PollOperation = new ShortSynchronizedOperation(pollingFunction, LogException);
            NotifyOperation = new TaskSynchronizedOperation(NotifyAnalysisNodesAsync, LogException);

            Action<object> configurator = GetConfigurator();
            Configure(configurator);
        }

        #endregion

        #region [ Properties ]

        private FileProcessor FileProcessor { get; }
        private ConcurrentQueue<WorkItem> WorkQueue { get; }
        private ISynchronizedOperation PollOperation { get; }
        private TaskSynchronizedOperation NotifyOperation { get; }
        private IEnumerable<FileShare> FileShares { get; set; }
        private int ProcessingThreadCount { get; set; }

        private int ProcessedFileCount =>
            Interlocked.CompareExchange(ref m_processedFileCount, 0, 0);

        private bool IsDisposed { get; set; }

        #endregion

        #region [ Methods ]

        public override IHttpController CreateWebController() =>
            new FileProcessorWebController(this);

        public void Dispose()
        {
            if (IsDisposed)
                return;

            try { FileProcessor.Dispose(); }
            finally { IsDisposed = true; }
        }

        protected override void OnReconfigure(Action<object> configurator) =>
            Configure(configurator);

        private Action GetPollingFunction()
        {
            // This is used to track the processing thread
            // count so resource usage can be throttled
            int threadCount = 0;
            int GetThreadCount() => Interlocked.CompareExchange(ref threadCount, 0, 0);

            return () =>
            {
                if (IsDisposed)
                    return;

                int maxThreadCount = ProcessingThreadCount;

                // If there are no threads available to process,
                // active threads will poll again when they are finished
                if (GetThreadCount() >= maxThreadCount)
                    return;

                // It's not necessary to keep
                // polling if the queue is empty
                if (!WorkQueue.TryDequeue(out WorkItem task))
                    return;

                // Add another processing thread for the polled file
                Interlocked.Increment(ref threadCount);

                _ = Task.Run(() =>
                {
                    try
                    {
                        TryProcess(task);
                    }
                    finally
                    {
                        Interlocked.Decrement(ref threadCount);

                        // Ensure there are no more tasks in the queue
                        PollOperation.RunOnceAsync();
                    }
                });

                // Keep polling until the queue is empty
                PollOperation.RunOnceAsync();
            };
        }

        private void TryProcess(WorkItem workItem)
        {
            if (IsDisposed)
                return;

            string filePath = workItem.FilePath;
            int priority = workItem.Priority;
            int retryCount = workItem.RetryCount;

            int GetDelay()
            {
                // 8 * 250 ms = 2 sec (cumulative: 2 sec)
                const int FastRetryLimit = 8;
                const int FastRetryDelay = 250;

                if (retryCount < FastRetryLimit)
                    return FastRetryDelay;

                // 13 * 1000 ms = 13 sec (cumulative: 15 sec)
                const int QuickRetryLimit = 13 + FastRetryLimit;
                const int QuickRetryDelay = 1000;

                if (retryCount < QuickRetryLimit)
                    return QuickRetryDelay;

                // 9 * 5000 ms = 45 sec (cumulative: 60 sec)
                const int RelaxedRetryLimit = 9 + QuickRetryLimit;
                const int RelaxedRetryDelay = 5000;

                if (retryCount < RelaxedRetryLimit)
                    return RelaxedRetryDelay;

                // After 60 seconds, continue with the slow retry delay
                const int SlowRetryDelay = 60000;
                return SlowRetryDelay;
            }

            async Task RetryAsync()
            {
                int delay = GetDelay();
                await Task.Delay(delay);
                workItem.IncrementRetryCount();
                WorkQueue.Enqueue(workItem);
                PollOperation.RunOnceAsync();
            }

            try
            {
                Action<object> configurator = GetConfigurator();
                FileProcessingTask fileProcessingTask = new FileProcessingTask(filePath, priority, CreateDbConnection, configurator);
                fileProcessingTask.Execute();
                NotifyOperation.RunOnceAsync();
            }
            catch (FileSkippedException ex)
            {
                if (ex.Requeue && retryCount < 30)
                {
                    _ = RetryAsync();
                    return;
                }

                Log.Warn(ex.Message, ex);
            }
            catch (Exception ex)
            {
                string message = $"Exception occurred processing file \"{filePath}\": {ex.Message}";
                Log.Error(message, ex);
            }

            Interlocked.Increment(ref m_processedFileCount);
        }

        private void Configure(Action<object> configurator)
        {
            Settings settings = new Settings(configurator);
            ProcessingThreadCount = settings.FileProcessorSettings.ProcessingThreadCount;
            AuthenticateFileShares(settings);
            ConfigureFileProcessor(settings);
        }

        private void AuthenticateFileShares(Settings settings)
        {
            void HandleException(Exception ex) => Log.Error(ex.Message, ex);

            FileShares = settings.FileWatcherSettings.FileShareList;

            foreach (FileShare fileShare in FileShares)
            {
                if (!fileShare.TryAuthenticate())
                    HandleException(fileShare.AuthenticationException);
            }
        }

        private void ConfigureFileProcessor(Settings settings)
        {
            string QueryFilter()
            {
                const string Query = "SELECT FilePattern FROM DataReader";

                // Get the list of file extensions to be processed by openXDA
                using (AdoDataConnection connection = CreateDbConnection())
                using (DataTable result = connection.RetrieveData(Query))
                {
                    IEnumerable<string> filterPatterns = result
                        .AsEnumerable()
                        .Select(row => row.ConvertField<string>("FilePattern"));

                    return string.Join(Path.PathSeparator.ToString(), filterPatterns);
                }
            }

            FileProcessor.Filter = QueryFilter();
            FileProcessor.FolderExclusion = settings.FileEnumeratorSettings.FolderExclusion;
            FileProcessor.InternalBufferSize = settings.FileWatcherSettings.BufferSize;
            FileProcessor.EnumerationStrategy = settings.FileEnumeratorSettings.Strategy;
            FileProcessor.MaxThreadCount = settings.FileWatcherSettings.InternalThreadCount;
            FileProcessor.TrackChanges = true;
            FileProcessor.Processing += FileProcessor_Processing;
            FileProcessor.Error += FileProcessor_Error;

            IReadOnlyCollection<string> watchDirectories = settings.FileWatcherSettings.WatchDirectoryList;

            foreach (string path in FileProcessor.TrackedDirectories.ToList())
            {
                if (!watchDirectories.Contains(path, StringComparer.OrdinalIgnoreCase))
                    FileProcessor.RemoveTrackedDirectory(path);
            }

            foreach (string path in watchDirectories)
            {
                try
                {
                    Directory.CreateDirectory(path);
                    FileProcessor.AddTrackedDirectory(path);
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message, ex);
                }
            }

            FileProcessor.EnumerateWatchDirectories();
        }

        private async Task NotifyAnalysisNodesAsync()
        {
            async Task NotifyAsync(string url)
            {
                void ConfigureRequest(HttpRequestMessage request)
                {
                    request.Method = HttpMethod.Post;
                    request.RequestUri = new Uri(url);
                }

                using (HttpResponseMessage response = await Host.SendWebRequestAsync(ConfigureRequest))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        string body = await response.Content.ReadAsStringAsync();

                        string logMessage = new StringBuilder()
                            .AppendLine("Analysis node notification failed.")
                            .AppendLine($"Status: {response.StatusCode}")
                            .AppendLine("Body:")
                            .Append(body)
                            .ToString();

                        Log.Debug(logMessage);
                    }
                }
            }

            const string QueryFormat =
                "SELECT Node.ID NodeID " +
                "FROM " +
                "    ActiveHost JOIN " +
                "    Node ON Node.HostRegistrationID = ActiveHost.ID JOIN " +
                "    NodeType ON Node.NodeTypeID = NodeType.ID " +
                "WHERE NodeType.TypeName = {0}";

            Type analysisNodeType = typeof(AnalysisNode);
            string analysisNodeTypeName = analysisNodeType.FullName;

            using (AdoDataConnection connection = CreateDbConnection())
            using (DataTable result = connection.RetrieveData(QueryFormat, analysisNodeTypeName))
            {
                List<Task> notifyTasks = result
                    .AsEnumerable()
                    .Select(row =>
                    {
                        int nodeID = row.ConvertField<int>("NodeID");
                        string url = Host.BuildURL(nodeID, "PollTaskQueue");
                        return NotifyAsync(url);
                    })
                    .ToList();

                try { await Task.WhenAll(notifyTasks); }
                catch (Exception ex) { Log.Error(ex.Message, ex); }

                // This method runs as the action of a synchronized operation
                // so this limits notifications to at most every 5 seconds
                TimeSpan delay = TimeSpan.FromSeconds(5);
                await Task.Delay(delay);
            }
        }

        private string GetStatus()
        {
            StringBuilder statusBuilder = new StringBuilder();
            statusBuilder.AppendLine($"                 Filter: {FileProcessor.Filter}");

            if (!string.IsNullOrEmpty(FileProcessor.FolderExclusion))
                statusBuilder.AppendLine($"       Folder Exclusion: {FileProcessor.FolderExclusion}");

            int processedFileCount = ProcessedFileCount;
            int skippedFileCount = FileProcessor.SkippedFileCount;
            int scannedFileCount = FileProcessor.ProcessedFileCount + skippedFileCount;
            statusBuilder.AppendLine($"   Internal buffer size: {FileProcessor.InternalBufferSize}");
            statusBuilder.AppendLine($"   Max thread pool size: {FileProcessor.MaxThreadCount}");
            statusBuilder.AppendLine($"   Enumeration strategy: {FileProcessor.EnumerationStrategy}");
            statusBuilder.AppendLine($"         Is Enumerating: {FileProcessor.IsEnumerating}");
            statusBuilder.AppendLine($"          Scanned files: {scannedFileCount}");
            statusBuilder.AppendLine($"        Processed files: {processedFileCount}");
            statusBuilder.AppendLine($"          Skipped files: {skippedFileCount}");
            statusBuilder.AppendLine();

            if (FileProcessor.IsEnumerating)
            {
                IList<string> activelyEnumeratedPaths = FileProcessor.ActivelyEnumeratedPaths;

                statusBuilder.AppendLine("  Actively enumerated paths:");

                foreach (string path in activelyEnumeratedPaths.Take(5))
                    statusBuilder.AppendLine($"    {path}");

                if (activelyEnumeratedPaths.Count > 5)
                    statusBuilder.AppendLine($"    {activelyEnumeratedPaths.Count - 5} more...");

                statusBuilder.AppendLine();
            }

            statusBuilder.AppendLine("  Watch directories:");

            foreach (string path in FileProcessor.TrackedDirectories)
                statusBuilder.AppendLine($"    {path}");

            if (FileShares.Any())
            {
                statusBuilder.AppendLine("  File shares:");

                foreach (FileShare fileShare in FileShares)
                {
                    if (fileShare.AuthenticationException is null)
                        statusBuilder.AppendLine($"    {fileShare.Name}");
                    else
                        statusBuilder.AppendLine($"    {fileShare.Name} [Exception: {fileShare.AuthenticationException.Message}]");
                }

                statusBuilder.AppendLine();
            }

            return statusBuilder
                .ToString()
                .TrimEnd();
        }

        private void FileProcessor_Processing(object sender, FileProcessorEventArgs fileProcessorEventArgs)
        {
            if (IsDisposed)
                return;

            string filePath = fileProcessorEventArgs.FullPath;

            int priority = fileProcessorEventArgs.RaisedByFileWatcher
                ? AnalysisTask.FileWatcherPriority
                : AnalysisTask.FileEnumerationPriority;

            WorkItem workItem = new WorkItem(filePath, priority);
            WorkQueue.Enqueue(workItem);
            PollOperation.RunOnceAsync();
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(FileProcessorNode));

        // Static Methods
        private static void FileProcessor_Error(object sender, ErrorEventArgs args)
        {
            Exception ex = args.GetException();
            Log.Error(ex.Message, ex);
        }

        #endregion
    }
}
