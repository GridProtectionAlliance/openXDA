//******************************************************************************************************
//  FilePrunerNode.cs - Gbtc
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
//  02/08/2021 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
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

namespace openXDA.Nodes.Types.FilePruning
{
    public class FilePrunerNode : NodeBase
    {
        #region [ Members ]

        // Nested Types
        private class Settings
        {
            public Settings(Action<object> configure) =>
                configure(this);

            [Category]
            [SettingName(FilePrunerSection.CategoryName)]
            public FilePrunerSection FilePrunerSettings { get; } = new FilePrunerSection();

            [Category]
            [SettingName(FileWatcherSection.CategoryName)]
            public FileWatcherSection FileWatcherSettings { get; } = new FileWatcherSection();
        }

        private class FilePruningWebController : ApiController
        {
            private FilePrunerNode Node { get; }

            public FilePruningWebController(FilePrunerNode node) =>
                Node = node;

            [HttpGet]
            public int CountEvents(DateTime maxDate) =>
                Node.CountEvents(maxDate);

            [HttpPost]
            public void PurgeFileGroups(DateTime maxDate) =>
                Node.PurgeFileGroups(maxDate);

            [HttpPost]
            public void PurgeOrphanData() =>
                Node.PurgeOrphanDataOperation.TryRunOnceAsync();
        }

        #endregion

        #region [ Constructors ]

        public FilePrunerNode(Host host, Node definition, NodeType type)
            : base(host, definition, type)
        {
            PurgeOrphanDataOperation = new LongSynchronizedOperation(PurgeOrphanData, LogException) { IsBackground = true };
        }

        #endregion

        #region [ Properties ]

        private ISynchronizedOperation PurgeOrphanDataOperation { get; }

        #endregion

        #region [ Methods ]

        public override IHttpController CreateWebController() =>
            new FilePruningWebController(this);

        protected override void OnReconfigure(Action<object> configurator) =>
            ScheduleAutoPrune(configurator);

        private void ScheduleAutoPrune(Action<object> configurator)
        {
            Settings settings = new Settings(configurator);
            string name = nameof(AutoPrune);
            string schedule = settings.FilePrunerSettings.Schedule;
            Host.RegisterScheduledProcess(this, AutoPrune, name, schedule);
        }

        private void AutoPrune()
        {
            Action<object> configurator = GetConfigurator();
            Settings settings = new Settings(configurator);
            TimeSpan retentionPeriod = settings.FilePrunerSettings.RetentionPeriod;

            if (retentionPeriod <= TimeSpan.Zero)
                return;

            DateTime threshold = DateTime.UtcNow - retentionPeriod;

            foreach (string path in settings.FileWatcherSettings.WatchDirectoryList)
            {
                foreach (string filePath in FilePath.EnumerateFiles(path))
                {
                    FileInfo fileInfo = new FileInfo(filePath);

                    List<DateTime> list = new List<DateTime>(3)
                    {
                        fileInfo.CreationTimeUtc,
                        fileInfo.LastAccessTimeUtc,
                        fileInfo.LastWriteTimeUtc
                    };

                    DateTime max = list.Max();

                    if (max < threshold)
                    {
                        Log.Info($"Deleting file - {fileInfo.Name}");
                        fileInfo.Delete();
                    }
                }
            }
        }

        private int CountEvents(DateTime maxDate)
        {
            using (AdoDataConnection connection = CreateDbConnection())
            {
                TableOperations<Event> eventTable = new TableOperations<Event>(connection);
                string filter = "FileGroupID IN (SELECT ID FROM FileGroup WHERE ProcessingStartTime < {0})";
                return eventTable.QueryRecordCountWhere(filter, maxDate);
            }
        }

        private void PurgeFileGroups(DateTime maxDate)
        {
            using (AdoDataConnection connection = CreateDbConnection())
            using (IDbCommand command = connection.Connection.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "UniversalCascadeDelete";
                command.AddParameterWithValue("@tableName", "FileGroup");
                command.AddParameterWithValue("@baseCriteria", $"ProcessingStartTime < '{maxDate:yyyy-MM-dd}'");
                command.ExecuteNonQuery();
            }

            PurgeOrphanDataOperation.TryRunOnceAsync();
        }

        private void PurgeOrphanData()
        {
            void PurgeChannelData()
            {
                const string Query =
                    "DECLARE @count INT = 1 " +
                    "" +
                    "WHILE @count > 0 " +
                    "BEGIN " +
                    "    SELECT TOP 50 ID " +
                    "    INTO #channelDataIDs " +
                    "    FROM ChannelData " +
                    "    WHERE NOT EXISTS " +
                    "    ( " +
                    "        SELECT * " +
                    "        FROM Event " +
                    "        WHERE ID = ChannelData.EventID " +
                    "    ) " +
                    "" +
                    "    DELETE FROM ChannelData " +
                    "    WHERE ID IN (SELECT * FROM #channelDataIDs) " +
                    "" +
                    "    SET @count = @@ROWCOUNT " +
                    "END ";

                using (AdoDataConnection connection = CreateDbConnection())
                    connection.ExecuteNonQuery(Query);
            }

            void PurgeEventData()
            {
                const string Query =
                    "DECLARE @count INT = 1 " +
                    "" +
                    "WHILE @count > 0 " +
                    "BEGIN " +
                    "    SELECT TOP 50 ID " +
                    "    INTO #eventDataIDs " +
                    "    FROM EventData " +
                    "    WHERE NOT EXISTS " +
                    "    ( " +
                    "        SELECT * " +
                    "        FROM Event " +
                    "        WHERE EventDataID = EventData.ID " +
                    "    ) " +
                    "" +
                    "    DELETE FROM EventData " +
                    "    WHERE ID IN (SELECT * FROM #eventDataIDs) " +
                    "" +
                    "    SET @count = @@ROWCOUNT " +
                    "END ";

                using (AdoDataConnection connection = CreateDbConnection())
                    connection.ExecuteNonQuery(Query);
            }

            Task channelDataTask = Task.Run(PurgeChannelData);
            Task eventDataTask = Task.Run(PurgeEventData);
            Task all = Task.WhenAll(channelDataTask, eventDataTask);
            all.GetAwaiter().GetResult();
        }

        private void LogException(Exception ex) =>
            Log.Error(ex.Message, ex);

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(FilePrunerNode));

        #endregion
    }
}
