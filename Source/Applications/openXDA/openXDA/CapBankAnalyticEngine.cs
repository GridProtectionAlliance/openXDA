//******************************************************************************************************
//  CapBankAnalyticEngine.cs - Gbtc
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
//  08/04/2020 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using FaultData.DataSets;
using FaultData.DataWriters;
using GSF.Data;
using GSF.Data.Model;
using GSF.Threading;
using log4net;
using openXDA.Model;
using FaultData.DataOperations;
using System.IO;
using FaultData.DataAnalysis;
using GSF.Collections;

namespace openXDA
{
    public class CapBankAnalyticEngine
    {
        #region [ Members ]

        #endregion

        #region [ Constructors ]

        public CapBankAnalyticEngine()
        {
            lock (s_eventFileLock)
            {
                using(AdoDataConnection connection = new AdoDataConnection("SystemSettings"))
                using (FileBackedDictionary<int, List<int>> dictionary = new FileBackedDictionary<int, List<int>>("./CapBankAnalytic.bin"))
                {
                    foreach (int fileGroupID in dictionary.Keys)
                    {
                        EPRICapBankAnalytics analytic = new EPRICapBankAnalytics(RunAnalytic);
                        TableOperations<Event> evtTbl = new TableOperations<Event>(connection);
                        List<Event> evts = dictionary[fileGroupID].Select(item => evtTbl.QueryRecordWhere("Id = {0}", item)).ToList();
                        analytic.Process(evts, fileGroupID);
                        analytic.StartTimer();
                    }
                }
                    
            }

        }

        #endregion

        #region [ Properties ]

        #endregion

        #region [ Methods ]

        public void Process(MeterDataSet meterDataSet)
        {

            using (AdoDataConnection connection = meterDataSet.CreateDbConnection())
            {
                bool enabled = connection.ExecuteScalar<bool?>("SELECT Value FROM Setting WHERE Name = 'EPRICapBankAnalytic.Enabled'") ?? false;
                if (!enabled)
                    return;

                TableOperations<Event> evtTbl = new TableOperations<Event>(connection);
                int capBankId = connection.ExecuteScalar<int>("SELECT ID FROM AssetType WHERE Name = 'CapacitorBank'");

                List<Event> events = evtTbl.QueryRecordsWhere("FileGroupID = {0}", meterDataSet.FileGroup.ID).ToList();

                foreach (IGrouping<int, Event> group in events.GroupBy<Event, int>(item => item.AssetID))
                {
                    Asset asset = new TableOperations<Asset>(connection).QueryRecordWhere("ID = {0}", group.Key);
                    if (asset.AssetTypeID != capBankId)
                        continue;

                    EPRICapBankAnalytics analytic = new EPRICapBankAnalytics(RunAnalytic);
                    analytic.Process(group.ToList(), meterDataSet.FileGroup.ID);
                    analytic.StartTimer();
                    lock(s_eventFileLock)
                    {
                        using (FileBackedDictionary<int, List<int>> dictionary = new FileBackedDictionary<int, List<int>>("./CapBankAnalytic.bin"))
                            dictionary.Add(meterDataSet.FileGroup.ID, group.Select(item => item.ID).ToList());
                    }
                    
                }
            }
        }

        private void RunAnalytic(List <Event> events, int fileGroupID)
        {

            // First step is to remove entry from Dictionary to avoid reprocessing if anything fails
            lock (s_eventFileLock)
            {
                using (FileBackedDictionary<int, List<int>> dictionary = new FileBackedDictionary<int, List<int>>("./CapBankAnalytic.bin"))
                    dictionary.Remove(fileGroupID);
            }

            // Then Run the EPRI Analytic
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                TableOperations<Event> evtTbl = new TableOperations<Event>(connection);

                Asset asset = new TableOperations<Asset>(connection).QueryRecordWhere("ID = {0}", events.First().AssetID);
                   
                Dictionary<string, Event> eventMapping = new Dictionary<string, Event>();

                CapBank capBank;
                List<CapBankRelay> relays;

                capBank = new TableOperations<CapBank>(connection).QueryRecordWhere("ID = {0}", asset.ID);
                capBank.ConnectionFactory = () => { return new AdoDataConnection("systemSettings"); };
                relays = capBank.ConnectedRelays;

                // Create TxT Input File
                GenerateParameterFile(events.First());

                foreach (Event evt in events)
                {
                    GenerateCapBankDataFile(evt);

                    List<Event> relayEvents = GetRelayEvents(evt.StartTime, evt.EndTime, relays);
                    relayEvents.ForEach(relayEvt => GenerateRelayDataFile(relayEvt));
                }

                if (eventMapping.Count == 0)
                    return;
                // Run ML code

                // Read Output Files
                Log.Info("Processing CapBank Analytic Results...");
                ParseOutputs(eventMapping);


            }

        }

        private void ParseOutputs(Dictionary<string, Event> eventMapping)
        {
            string resultFolder = "./";

            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                resultFolder = connection.ExecuteScalar<string>("SELECT Value from Setting Where Name='EPRICapBankAnalytic.ResultFileLocation'");
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

        private void GenerateParameterFile(Event evt)
        {

            CapBank capBank;
            List<CapBankRelay> relays;
            VIDataGroup datagroup = QueryDataGroup(evt.ID, evt.MeterID);

            double iSamples = datagroup.Data.Where(item =>
                item.SeriesInfo.Channel.AssetID == evt.AssetID &&
                item.SeriesInfo.Channel.MeasurementType.Name == "Current" &&
                item.SeriesInfo.Channel.MeasurementCharacteristic.Name == "Instantaneous").First()?.SampleRate ?? -1.0;

            double vSamples = datagroup.Data.Where(item =>
                item.SeriesInfo.Channel.AssetID == evt.AssetID &&
                item.SeriesInfo.Channel.MeasurementType.Name == "Voltage" &&
                item.SeriesInfo.Channel.MeasurementCharacteristic.Name == "Instantaneous").First()?.SampleRate ?? -1.0;


            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                capBank = new TableOperations<CapBank>(connection).QueryRecordWhere("ID = {0}", evt.AssetID);
                capBank.ConnectionFactory = () => { return new AdoDataConnection("systemSettings"); };
                relays = capBank.ConnectedRelays;

                if (vSamples == -1 || iSamples == -1)
                {
                    Log.Error($"Voltage or Current Measurment not available for {capBank.AssetName}");
                    return;
                }

                double systemFreq = connection.ExecuteScalar<double?>("SELECT Value FROM Setting WHERE Name = 'SystemFrequency'") ?? 60.0D;

                iSamples = Math.Round(iSamples / systemFreq);
                vSamples = Math.Round(vSamples / systemFreq);

                string dstFolder = connection.ExecuteScalar<string>("SELECT Value from Setting Where Name='EPRICapBankAnalytic.ParameterFileLocation'");
                dstFolder = Path.GetDirectoryName(dstFolder) ?? string.Empty;

                string datafolder = connection.ExecuteScalar<string>("SELECT Value from Setting Where Name='EPRICapBankAnalytic.DataFileLocation'");
                string resultFolder = connection.ExecuteScalar<string>("SELECT Value from Setting Where Name='EPRICapBankAnalytic.ResultFileLocation'");
                datafolder = Path.GetDirectoryName(datafolder) ?? string.Empty;
                resultFolder = Path.GetDirectoryName(resultFolder) ?? string.Empty;

                string dstFile = capBank.AssetKey + ".txt";
                List<string> lines = new List<string>();

                lines.Add("Please enter capacitor bank data; Do not change the variable names; Change the values only");
                lines.Add("The equal sign character is used as a delimiter, do not change or use it as part of text");
                lines.Add($"User note: Input parameters for {capBank.AssetName} capacitor banks");
                lines.Add("User note: File Generated by OpenXDA for auto processing");
                lines.Add("");
                lines.Add("Specify the directory where cap bank events are stored");
                lines.Add($"1 dataDir = {datafolder}");
                lines.Add("Specify the directory where cap.health analysis results will be stored.It cannot be in dataDir");
                lines.Add($"3 resultDirFname = {resultFolder}");
                lines.Add("Specify the capacitor bank keyword used in the cap bank file names");
                lines.Add($"5 fileKeyCapEvents = {capBank.AssetKey}");
                lines.Add("");
                lines.Add("Specify the analysis period");
                lines.Add("7 start date; fDateStartInputStr = 1990-01-01");
                lines.Add("8 end date; fDateEndInputStr = 2099-01-01");
                lines.Add("");
                lines.Add("Specify measurement requirements");
                lines.Add($"10 fundamental frequency; fundf = {systemFreq}");
                lines.Add($"11 sampling rate for voltage; svNSPC = {vSamples}");
                lines.Add($"12 sampling rate for current; siNSPC = {iSamples}");
                lines.Add("13 no - voltage threshold for voltage waveforms in V; noBusVoltage = 500");
                lines.Add("14 no - current threshold for current waveforms in A; noBusCurrent = 4.0");
                lines.Add("15 the upper limit(in percent) to detect harmonic resonance; iTHDLimit = 5.5");
                lines.Add("");
                lines.Add("Specify cap bank configuration data");
                lines.Add($"17 number of banks; numBanks = {capBank.NumberOfBanks}");
                lines.Add($"18 nominal bus voltage in kV line-to - line; nominalBuskVLL = {capBank.VoltageKV}");
                lines.Add($"19 capacitor step size; StepSizeQ3kvar = {capBank.CapacitancePerBank}");
                lines.Add($"20 type of the circuit switcher(0 for no control, 1 for pre - ins, 2 for sync closing) ; capSwitcherTypeMultBanks = [{string.Join(", ", Enumerable.Repeat((capBank.CktSwitcher ? "1" : "0"), capBank.NumberOfBanks))}]");
                lines.Add($"21 bank max operating voltage in kV; bankMCOV = {capBank.MaxKV}");
                lines.Add($"22 rated kvar of a capacitor unit; capUnitRatedkvar = {capBank.UnitKVAr}");
                lines.Add($"23 rated kV of a capacitor unit; capUnitRatedkV = {capBank.UnitKV}");
                lines.Add($"24 the reactance tolerance of cap.unit, negative tolerance in percent; ncapUnitTolpct = {capBank.NegReactanceTol}");
                lines.Add($"25 the reactance tolerance of cap.unit, positive tolerance in percent; pcapUnitTolpct = {capBank.PosReactanceTol}");
                lines.Add($"26 for fuseless design; the number of parallel strings of capacitor units connected in series; Nps = {capBank.Nparalell}");
                lines.Add($"27 for fuseless design; the number of capacitor units connected in series in each parallel string; Nus = {capBank.Nseries}");
                lines.Add($"28 for fuseless design; the number of series groups of capacitor elements connected in parallel in each capacitor unit; Nesg = {capBank.NSeriesGroup}");
                lines.Add($"29 for fuseless design; the number of capacitor elements connected in parallel in each series group in each capacitor unit; Nepg = {capBank.NParalellGroup}");
                lines.Add($"30 for fused design; the number of series groups per phase; each group is in series; Ns = {capBank.Nseries}");
                lines.Add($"31 for fused design; the number of capacitors per group; each cap is in parallel; Np = {capBank.Nparalell}");
                lines.Add("");
                lines.Add("Specify relay data inputs and requirements");
                lines.Add($"33 Offset time between cap bank and relay time stamps; dToffset = 1");
                lines.Add($"34 Rated relay voltage in V; ratedRelayVoltage = {relays.First().VoltageKV}");
                lines.Add($"35 No voltage for relay or threshold of ON voltage; relayOnVoltageThreshold = {relays.First().OnVoltageThreshhold}");
                lines.Add("");
                lines.Add("Specify relay capacitor configuration for fuseless configurations");
                lines.Add($"37 Bus VT ratio; busVT = {capBank.VTratioBus}");
                lines.Add($"38 the number of relay capacitor; NLVcapUnit = {capBank.NumberLVCaps}");
                lines.Add($"39 the number of elements; NLVcapE = {capBank.NumberLVUnits}");
                lines.Add($"40 low - voltage capacitor size for capacitor bank relaying; LVcapUnitRatedkvar = {capBank.LVKVAr}");
                lines.Add($"41 rated voltage of the low - voltage capacitor; LVcapUnitRatedV = {capBank.LVKV}");
                lines.Add($"42 the reactance tolerance of LV cap., negative tolerance in percent; nLVcapTolpct = {capBank.LVNegReactanceTol}");
                lines.Add($"43 the reactance tolerance of LV cap., positive tolerance in percent; pLVcapTolpct = {capBank.LVPosReactanceTol}");
                lines.Add($"44 relay PT ratio, high to low; relayPTR = {relays.First().RelayPTRatio}");
                lines.Add($"45 the output resistor of the voltage divider circuit; Rv = {relays.First().Rv}");
                lines.Add($"46 the input resistor of the voltage divider circuit; Rh = {relays.First().Rh}");
                lines.Add("");
                lines.Add("Specify relay capacitor configuration for fused configurations");
                lines.Add($"48 upper group voltage transformer ratio; UVTR = {capBank.UpperXFRRatio}");
                lines.Add($"49 lower group voltage transformer ratio; LVTR = {capBank.LowerXFRRatio}");
                lines.Add($"50 number of lower groups below the tap point; nLGbTap = 3");
                lines.Add("");
                lines.Add("Specify capacitor protection scheme");
                lines.Add("Set pCapDesign to 1 for compensated design");
                lines.Add("Set pCapDesign to 2 for uncompensated design");
                lines.Add("Set pCapDesign to 3 for fused design");
                lines.Add($"55 pCapDesign = {(relays.First().Compensated ? "1" : (capBank.Fused ? "3" : "2"))}");
                lines.Add("");
                lines.Add("Specify the assumed initial numbers of shorted or blown fuses in cap banks.");
                lines.Add("They are used to set thresholds for declaring banks having shorted units or blown fuses");
                lines.Add("Numbers do not need to be whole(integers, i.e., 1). They can be fractional(real), i.e., 0.75");
                lines.Add("For the fuseless compensated and uncompensated designs, specify the following:");
                lines.Add($"60 the assumed initial total number of shorted capacitor elements, use 1 catch more; NFesg_init = {capBank.Nshorted}");
                lines.Add("For the fused compensated design, specify the following:");
                lines.Add($"62 the assumed initial number of blown fuses per group; use 1 to catch more; nBFG_init = {capBank.BlownFuses}");
                lines.Add($"63 the assumed initial number of groups with blown fuses; use 1 to catch more; nGBF_init = {capBank.BlownGroups}");
                lines.Add($"64 the assumed initial number of groups shorted; use 0.5 to to catch more; NsS_init = {capBank.Nshorted}");
                lines.Add("");
                lines.Add("Specify relay keywords");
                int n = 1;
                foreach (CapBankRelay relay in relays)
                {
                    lines.Add($"{n + 65} fileKeyRelay4Cap{{{n}}} = {relay.AssetKey}");
                    n++;
                }
                lines.Add("");
                lines.Add("Evaluation of pre-insertion takes a longer time.  Set evalPreIns to 1 to evaluate; 0 to skip");
                lines.Add($"{n + 66} evalPreIns = 1");

                Directory.CreateDirectory(dstFolder);
                // Open the file and write in each line
                using (StreamWriter fileWriter = new StreamWriter(File.OpenWrite(dstFolder + "\\" + dstFile)))
                {
                    for (int i = 0; i < lines.Count(); i++)
                    {
                        fileWriter.WriteLine(lines[i]);
                    }
                }
            }

        }

        private void GenerateCapBankDataFile(Event evt)
        {
            CapBank capBank;
            VIDataGroup data = QueryDataGroup(evt.ID, evt.MeterID);

            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                capBank = new TableOperations<CapBank>(connection).QueryRecordWhere("ID = {0}", evt.AssetID);
                capBank.ConnectionFactory = () => { return new AdoDataConnection("systemSettings"); };

                string datafolder = connection.ExecuteScalar<string>("SELECT Value from Setting Where Name='EPRICapBankAnalytic.DataFileLocation'");
                datafolder = Path.GetDirectoryName(datafolder) ?? string.Empty;


                string dstFile = capBank.AssetKey + $"-{evt.ID}.csv";
                List<string> lines = new List<string>();

                lines.Add($"\"{capBank.AssetKey} - {evt.StartTime.ToString("%M/%d/%yyyy")} {evt.StartTime.ToString("%HH:%mm:%ss.ffff")} \"");
                lines.Add("\"\"");
                string header = "Time (s),";
                List<DataSeries> series = new List<DataSeries>();
                if (data.VA != null)
                {
                    header = header + " Va";
                    series.Add(data.VA);
                }

                if (data.VB != null)
                {
                    header = header + ", Vb";
                    series.Add(data.VB);
                }
                if (data.VC != null)
                {
                    header = header + ", Vc";
                    series.Add(data.VC);
                }
                if (data.IA != null)
                {
                    header = header + ", Ia";
                    series.Add(data.IA);
                }
                if (data.IB != null)
                {
                    header = header + ", Ib";
                    series.Add(data.IB);
                }
                if (data.IC != null)
                {
                    header = header + ", Ic";
                    series.Add(data.IC);
                }
                // #ToDo: Switch to use In from VI Datagroup
                if (data.IR != null)
                {
                    header = header + ", In";
                    series.Add(data.IR);
                }

                if (header == "Time (s),")
                    return;

                lines.Add(header);
                lines.AddRange(ToCsV(series));

                Directory.CreateDirectory(datafolder);
                // Open the file and write in each line
                using (StreamWriter fileWriter = new StreamWriter(File.OpenWrite(datafolder + "\\" + dstFile)))
                {
                    for (int i = 0; i < lines.Count(); i++)
                    {
                        fileWriter.WriteLine(lines[i]);
                    }
                }
            }

        }

        private void GenerateRelayDataFile(Event evt)
        {
            CapBankRelay relay;
            VIDataGroup data = QueryDataGroup(evt.ID, evt.MeterID);

            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                relay = new TableOperations<CapBankRelay>(connection).QueryRecordWhere("ID = {0}", evt.AssetID);
                relay.ConnectionFactory = () => { return new AdoDataConnection("systemSettings"); };

                string datafolder = connection.ExecuteScalar<string>("SELECT Value from Setting Where Name='EPRICapBankAnalytic.DataFileLocation'");
                datafolder = Path.GetDirectoryName(datafolder) ?? string.Empty;


                string dstFile = relay.AssetKey + $"-{evt.ID}.csv";
                List<string> lines = new List<string>();

                lines.Add($"\"{relay.AssetKey} - {evt.StartTime.ToString("%M/%d/%yyyy")} {evt.StartTime.ToString("%HH:%mm:%ss.ffff")} \"");
                lines.Add("\"\"");
                string header = "Time (s),";
                List<DataSeries> series = new List<DataSeries>();
                if (data.VA != null)
                {
                    header = header + " Va";
                    series.Add(data.VA);
                }

                if (data.VB != null)
                {
                    header = header + ", Vb";
                    series.Add(data.VB);
                }
                if (data.VC != null)
                {
                    header = header + ", Vc";
                    series.Add(data.VC);
                }
                if (header == "Time (s),")
                    return;

                lines.Add(header);
                lines.AddRange(ToCsV(series));

                Directory.CreateDirectory(datafolder);
                // Open the file and write in each line
                using (StreamWriter fileWriter = new StreamWriter(File.OpenWrite(datafolder + "\\" + dstFile)))
                {
                    for (int i = 0; i < lines.Count(); i++)
                    {
                        fileWriter.WriteLine(lines[i]);
                    }
                }
            }

        }

        private List<Event> GetRelayEvents(DateTime startTime, DateTime endTime, List<CapBankRelay> relays)
        {
            List<Event> result = new List<Event>();

            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                TableOperations<Event> evtTbl = new TableOperations<Event>(connection);

                DataTable dataTable = connection.RetrieveData($@"
                    SELECT
	                    DISTINCT
	                    Event.ID as EventID
                    FROM
	                    Event 
                    WHERE
	                    Event.StartTime <= {{1}} AND
	                    Event.EndTime >= {{0}} AND
                        Event.AssetID in ({string.Join(",", relays.Select(item => item.ID))})
                    ", ToDateTime2(connection, startTime), ToDateTime2(connection, endTime));

                foreach (DataRow row in dataTable.Rows)
                {
                    result.Add(evtTbl.QueryRecordWhere("ID = {0}", int.Parse(row["EventID"].ToString())));
                }

                return result;
            }

        }

        #endregion

        #region[ Static ]

        private static List<string> ToCsV(List<DataSeries> data)
        {

            List<string> result = new List<string>();
            DateTime startingTime = data.First().DataPoints[0].Time;

            for (int i = 0; i < data.First().DataPoints.Count; ++i)
            {
                double TS = (data.First().DataPoints[i].Time - startingTime).TotalSeconds;

                IEnumerable<string> row = new List<string>() { TS.ToString() };

                row = row.Concat(data.Select(x =>
                {
                    if (x.DataPoints.Count > i)
                        return x.DataPoints[i].Value.ToString();
                    else
                        return string.Empty;
                }));

                result.Add(string.Join(",", row));
            }

            return result;
        }

        private static readonly ILog Log = LogManager.GetLogger(typeof(CapBankAnalyticEngine));

        private static VIDataGroup QueryDataGroup(int eventID, int meterId)
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                List<byte[]> data = ChannelData.DataFromEvent(eventID, connection);
                Meter meter = new TableOperations<Meter>(connection).QueryRecordWhere("ID = {0}", meterId);
                meter.ConnectionFactory = () => { return new AdoDataConnection("systemSettings"); };
                return ToDataGroup(meter, data);
            }

        }

        private static VIDataGroup ToDataGroup(Meter meter, List<byte[]> data)
        {
            DataGroup dataGroup = new DataGroup();
            dataGroup.FromData(meter, data);
            return new VIDataGroup(dataGroup);
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

        private static readonly object s_eventFileLock = new object();
        #endregion
    }
}