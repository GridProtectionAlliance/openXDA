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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
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
        }

        private class NotifySynchronizedOperation : SynchronizedOperationBase
        {
            private FileProcessorNode Node { get; }

            public NotifySynchronizedOperation(FileProcessorNode node)
                : base(() => { })
            {
                Node = node;
            }

            protected override void ExecuteActionAsync()
            {
                Task.Run(async () =>
                {
                    try { Node.NotifyAnalysisNodes(); }
                    catch (Exception ex) { Log.Error(ex.Message, ex); }

                    TimeSpan delay = TimeSpan.FromSeconds(15);
                    await Task.Delay(delay);

                    if (ExecuteAction())
                        ExecuteActionAsync();
                });
            }
        }

        private class FileProcessorWebController : ApiController
        {
            private FileProcessorNode Node { get; }

            public FileProcessorWebController(FileProcessorNode node) =>
                Node = node;

            [HttpGet]
            public void Enumerate() =>
                Node.FileProcessor.EnumerateWatchDirectories();

            [HttpGet]
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

        // Constants
        private const int FileEnumerationPriority = 1;
        private const int FileWatcherPriority = 2;
        private const int RequeuePriority = 3;

        #endregion

        #region [ Constructors ]

        public FileProcessorNode(Host host, Node definition, NodeType type)
            : base(host, definition, type)
        {
            FileProcessor = new FileProcessor();
            NotifyOperation = new NotifySynchronizedOperation(this);

            Action<object> configurator = GetConfigurator();
            ConfigureFileProcessor(configurator);
        }

        #endregion

        #region [ Properties ]

        private FileProcessor FileProcessor { get; }
        private NotifySynchronizedOperation NotifyOperation { get; }
        private IEnumerable<FileShare> FileShares { get; set; }
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
            ConfigureFileProcessor(configurator);

        private void ConfigureFileProcessor(Action<object> configurator)
        {
            Settings settings = new Settings(configurator);
            FileProcessor.Filter = QueryFileProcessorFilter();
            FileProcessor.FolderExclusion = settings.FileEnumeratorSettings.FolderExclusion;
            FileProcessor.InternalBufferSize = settings.FileWatcherSettings.BufferSize;
            FileProcessor.EnumerationStrategy = settings.FileEnumeratorSettings.Strategy;
            FileProcessor.MaxThreadCount = settings.FileWatcherSettings.InternalThreadCount;
            FileProcessor.TrackChanges = true;
            FileProcessor.Processing += FileProcessor_Processing;
            FileProcessor.Error += FileProcessor_Error;

            // Attempt to authenticate to configured file shares
            void HandleException(Exception ex) => Log.Error(ex.Message, ex);

            FileShares = settings.FileWatcherSettings.FileShareList;

            foreach (FileShare fileShare in FileShares)
            {
                if (!fileShare.TryAuthenticate())
                    HandleException(fileShare.AuthenticationException);
            }

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
        }

        private string QueryFileProcessorFilter()
        {
            List<string> filterPatterns;

            // Get the list of file extensions to be processed by openXDA
            using (AdoDataConnection connection = CreateDbConnection())
            {
                TableOperations<DataReader> dataReaderTable = new TableOperations<DataReader>(connection);

                filterPatterns = dataReaderTable
                    .QueryRecords()
                    .Select(reader => reader.FilePattern)
                    .ToList();
            }

            return string.Join(Path.PathSeparator.ToString(), filterPatterns);
        }

        private void NotifyAnalysisNodes()
        {
            async Task NotifyAsync(string url)
            {
                void ConfigureRequest(HttpRequestMessage request) =>
                    request.RequestUri = new Uri(url);

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

                _ = Task.Run(async () =>
                {
                    try { await Task.WhenAll(notifyTasks); }
                    catch (Exception ex) { Log.Error(ex.Message, ex); }
                });
            }
        }

        private void FileProcessor_Processing(object sender, FileProcessorEventArgs fileProcessorEventArgs)
        {
            if (IsDisposed)
                return;

            try
            {
                string filePath = fileProcessorEventArgs.FullPath;

                int priority = fileProcessorEventArgs.RaisedByFileWatcher
                    ? FileWatcherPriority
                    : FileEnumerationPriority;

                Action<object> configurator = GetConfigurator();
                FileProcessingTask task = new FileProcessingTask(filePath, priority, CreateDbConnection, configurator);
                task.Execute();

                NotifyOperation.RunOnceAsync();
            }
            catch (FileSkippedException ex)
            {
                if (ex.Requeue && fileProcessorEventArgs.RetryCount < 30)
                {
                    fileProcessorEventArgs.Requeue = true;
                    return;
                }

                // Do not wrap FileSkippedExceptions because
                // these only generate warning messages
                throw;
            }
            catch (Exception ex)
            {
                // Wrap all other exceptions to include the file path in the message
                string message = $"Exception occurred processing file \"{fileProcessorEventArgs.FullPath}\": {ex.Message}";
                throw new Exception(message, ex);
            }
        }

        private string GetStatus()
        {
            StringBuilder statusBuilder = new StringBuilder();
            statusBuilder.AppendLine($"                 Filter: {FileProcessor.Filter}");

            if (!string.IsNullOrEmpty(FileProcessor.FolderExclusion))
                statusBuilder.AppendLine($"       Folder Exclusion: {FileProcessor.FolderExclusion}");

            statusBuilder.AppendLine($"   Internal buffer size: {FileProcessor.InternalBufferSize}");
            statusBuilder.AppendLine($"   Max thread pool size: {FileProcessor.MaxThreadCount}");
            statusBuilder.AppendLine($"   Enumeration strategy: {FileProcessor.EnumerationStrategy}");
            statusBuilder.AppendLine($"         Is Enumerating: {FileProcessor.IsEnumerating}");
            statusBuilder.AppendLine($"        Processed files: {FileProcessor.ProcessedFileCount}");
            statusBuilder.AppendLine($"          Skipped files: {FileProcessor.SkippedFileCount}");
            statusBuilder.AppendLine($"         Requeued files: {FileProcessor.RequeuedFileCount}");
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
                    if ((object)fileShare.AuthenticationException == null)
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

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(FileProcessorNode));

        // Static Methods
        private static void FileProcessor_Error(object sender, ErrorEventArgs args)
        {
            Exception ex = args.GetException();

            if (ex is FileSkippedException)
                Log.Warn(ex.Message, ex);
            else
                Log.Error(ex.Message, ex);
        }

        #endregion
    }
}
