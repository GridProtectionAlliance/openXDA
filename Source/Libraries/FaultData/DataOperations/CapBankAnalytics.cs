//******************************************************************************************************
//  CapBankAnalytics.cs - Gbtc
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
//  07/29/2020 - Christoph Lackner
//       Generated original version of source code.
//
//******************************************************************************************************


using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using FaultData.DataAnalysis;
using FaultData.DataResources;
using FaultData.DataOperations;
using FaultData.DataSets;
using GSF.Data;
using GSF.Data.Model;
using openXDA.Model;
using log4net;
using System.IO;

namespace FaultData.DataOperations
{
    public class CapBankAnalytics : DataOperationBase<MeterDataSet>
    {
        public override void Execute(MeterDataSet meterDataSet)
        {
            // Make an Assumption that what is actually Triggering this is data for the CapBank.
            // This is going to be an issue if there is data from a capBank but the relay data does not exist yet.
            // We will need to deal with that issue later.

            using (AdoDataConnection connection = new AdoDataConnection("systemSetting"))
            {
                TableOperations<Event> evtTbl = new TableOperations<Event>(connection);
                int capBankId = connection.ExecuteScalar<int>("SELECT ID FROM AssetType WHERE Name = 'CapacitorBank'");

                List<Event> events = evtTbl.QueryRecordsWhere("FileGroupID = {0}", meterDataSet.FileGroup.ID).ToList();

                foreach (IGrouping<int, Event> group in events.GroupBy<Event,int>(item => item.AssetID))
                {
                    Asset asset = new TableOperations<Asset>(connection).QueryRecordWhere("ID = {0}", group.Key);
                    if (asset.AssetTypeID != capBankId)
                        continue;

                    Dictionary<string, Event> eventMapping = new Dictionary<string, Event>();

                    // Create TxT Input File

                    foreach (Event evt in group)
                    {
                        // Create Data input File
                    }

                    if (eventMapping.Count == 0)
                        continue;
                    // Run ML code

                    // Read Output Files
                    Log.Info("Processing CapBank Analytic Results...");
                    ParseOutputs(eventMapping);


                }
                
            }
            
        }

        private void ParseOutputs(Dictionary<string,Event> eventMapping)
        {
            string resultFolder = "./";

            using (AdoDataConnection connection = new AdoDataConnection("systemSetting"))
            {
                resultFolder = connection.ExecuteScalar<string>("SELECT Value from Setting Where Name='EPRICapBankAnalytic.FileLocation'");
            }

            resultFolder = Path.GetDirectoryName(resultFolder) ?? string.Empty;

            string[] csvFiles = Directory.GetFiles(resultFolder, "*.csv", SearchOption.AllDirectories);
            string switchingAnalysis;
            string restrikeAnalysis;
            string preInsertionAnalytics;
            string capBankAnalytics;

            switchingAnalysis = csvFiles.Where(item => item.EndsWith("_GenCapSwitch.csv", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            Log.Info($"Processing Capacitor Switching Analytic File");
            restrikeAnalysis = csvFiles.Where(item => item.EndsWith("_Restrike.csv", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            Log.Info($"Processing Restrike Analytic Files");
            preInsertionAnalytics = csvFiles.Where(item => item.EndsWith("_PreInsHealth.csv", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            Log.Info($"Processing Pre-Insertion Health Analytic Files");
            capBankAnalytics = csvFiles.Where(item => item.EndsWith("_RelayHealth.csv", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            Log.Info($"Processing CapBank and Relay Health Analytic Files");

            List<CBAnalyticResult> results;

            if (!string.IsNullOrEmpty(switchingAnalysis))
            {
                Log.Info($"Processing Capacitor Switching Analytic File");
                results = ReadSwitchingAnalytic(switchingAnalysis, eventMapping);

            }
            else
            {
                Log.Warn($"Capacitor Switching Analytic Results not found");
                return;
            }

        }

        private List<CBAnalyticResult> ReadSwitchingAnalytic(string filename, Dictionary<string, Event> eventMapping)
        {
            List<string> lines = new List<string>();

            // Open the file and read each line
            using (StreamReader fileReader = new StreamReader(File.OpenRead(filename)))
            {
                while (!fileReader.EndOfStream)
                {
                    lines.Add(fileReader.ReadLine().Trim());
                }
            }

            List<CBAnalyticResult> result = new List<CBAnalyticResult>();
            string fileName = "";

          
            using (AdoDataConnection connection = new AdoDataConnection("systemSetting"))
            {
              
                foreach (string line in lines.Skip(2))
                {
                    List<string> fields = line.Split(',').ToList();

                    if (!string.IsNullOrEmpty(fields[0].Trim()))
                        fileName = fields[0].Trim();

                    string evtLabel = fields[1].Trim();

                    CBAnalyticResult row = new CBAnalyticResult();

                    if (evtLabel.EndsWith("a", StringComparison.OrdinalIgnoreCase))
                        row.PhaseID = connection.ExecuteScalar<int>("SELECT ID FROM Phase WHERE Name = 'AN'");
                    else if (evtLabel.EndsWith("b", StringComparison.OrdinalIgnoreCase))
                        row.PhaseID = connection.ExecuteScalar<int>("SELECT ID FROM Phase WHERE Name = 'BN'");
                    else if (evtLabel.EndsWith("c", StringComparison.OrdinalIgnoreCase))
                        row.PhaseID = connection.ExecuteScalar<int>("SELECT ID FROM Phase WHERE Name = 'CN'");
                    else
                        row.PhaseID = connection.ExecuteScalar<int>("SELECT ID FROM Phase WHERE Name = 'None'");

                    row.DataErrorID = Convert.ToInt32(fields[2].Trim());
                    if (Convert.ToInt32(fields[3].Trim()) == 1)
                    {
                        Log.Warn("No Relay Data Found");
                    }

                    row.CBStatusID = Convert.ToInt32(fields[4].Trim());
                    row.CBOperationID = Convert.ToInt32(fields[5].Trim());
                    if (Convert.ToInt32(fields[6].Trim()) < 1 && Convert.ToInt32(fields[7].Trim()) < 1)
                    {
                        Log.Warn("Capacitor Bank Analysis not completed");
                        continue;
                    }
                    row.EnergizedBanks = Convert.ToInt32(fields[6].Trim());
                    row.DeEnergizedBanks = Convert.ToInt32(fields[7].Trim());

                    row.InServiceBank = Convert.ToInt32(fields[8].Trim());
                    row.DeltaQ = Convert.ToDouble(fields[9].Trim());
                    row.Ipre = Convert.ToDouble(fields[10].Trim());
                    row.Ipost = Convert.ToDouble(fields[11].Trim());
                    row.Vpre = Convert.ToDouble(fields[13].Trim());
                    row.Vpost = Convert.ToDouble(fields[14].Trim());
                    row.MVAsc = Convert.ToDouble(fields[16].Trim());

                    row.IsRes = Convert.ToInt32(fields[17].Trim()) == 1;
                    row.ResFreq = Convert.ToInt32(fields[18].Trim());

                    row.THDpre = Convert.ToDouble(fields[19].Trim());
                    row.THDpost = Convert.ToDouble(fields[20].Trim());
                    row.THDVpre = Convert.ToDouble(fields[22].Trim());
                    row.THDVpost = Convert.ToDouble(fields[23].Trim());

                    row.StepPre = Convert.ToInt32(fields[25].Trim());
                    row.StepPost = Convert.ToInt32(fields[26].Trim());
                    row.SwitchingFreq = Convert.ToDouble(fields[27].Trim());
                    row.Vpeak = Convert.ToDouble(fields[28].Trim());
                    row.Xpre = Convert.ToDouble(fields[29].Trim());
                    row.Xpost = Convert.ToDouble(fields[30].Trim());

                    int year = Convert.ToInt32(fields[31].Trim());
                    int month = Convert.ToInt32(fields[32].Trim());
                    int day = Convert.ToInt32(fields[33].Trim());

                    int hour = Convert.ToInt32(fields[34].Trim());
                    int minute = Convert.ToInt32(fields[35].Trim());
                    int second = Convert.ToInt32(fields[36].Trim());

                    row.Time = new DateTime(year, month, day, hour, minute, second);
                    row.Toffset = Convert.ToDouble(fields[37].Trim());

                    Event evt = null;
                    if (eventMapping.TryGetValue(fileName, out evt))
                    {
                        row.EventID = evt.ID;
                        result.Add(row);
                    }
                    
                }
            }

            return result;
        }

        #region[ Static ]

       
        private static List<DataGroup> FindDataGroups(MeterDataSet meterDataSet)
        {
            DataGroupsResource dataGroupsResource = meterDataSet.GetResource<DataGroupsResource>();
            return dataGroupsResource.DataGroups.ToList();
        }

        private static Event FindEvent(MeterDataSet meterDataSet, Channel channel)
        {
            List<DataGroup> dataGroups = FindDataGroups(meterDataSet);

            dataGroups = dataGroups.Where(
                dataGroup => 
                dataGroup.DataSeries.Select(
                    dataSeries => 
                    dataSeries.SeriesInfo.Channel.ID)
                    .ToList().Contains(channel.ID)
                ).ToList();

            if (dataGroups.Count ==0)
            {
                return null;
            }

            Event evt;

            const string Filter =
                    "FileGroupID = {0} AND " +
                    "AssetID = {1} AND " +
                    "StartTime = {2} AND " +
                    "EndTime = {3}";
            
            int fileGroupID = meterDataSet.FileGroup.ID;
            int lineID = dataGroups[0].Asset.ID;

            using (AdoDataConnection connection = meterDataSet.CreateDbConnection())
            {


                IDbDataParameter startTime = ToDateTime2(connection, dataGroups[0].StartTime);
                IDbDataParameter endTime = ToDateTime2(connection, dataGroups[0].EndTime);

                TableOperations<Event> eventTable = new TableOperations<Event>(connection);
                evt = eventTable.QueryRecordWhere(Filter, fileGroupID, lineID, startTime, endTime);

            }
            return evt;
        }

        private static IDbDataParameter ToDateTime2(AdoDataConnection connection, DateTime dateTime)
        {
            using (IDbCommand command = connection.Connection.CreateCommand())
            {
                IDbDataParameter parameter = command.CreateParameter();
                parameter.DbType = DbType.DateTime2;
                parameter.Value = dateTime;
                return parameter;
            }
        }

        private static readonly ILog Log = LogManager.GetLogger(typeof(RelayEnergization));

        #endregion
    }

}
