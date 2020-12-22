//******************************************************************************************************
//  EPRICapBankAnalysisNode.cs - Gbtc
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
//  08/04/2020 - C. Lackner
//       Generated original version of source code.
//  02/14/2021 - Stephen C. Wills
//       Refactored from CapBankAnalyticEngine.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using FaultData.DataAnalysis;
using FaultData.DataOperations;
using GSF.Collections;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using log4net;
using openXDA.Configuration;
using openXDA.Model;

namespace openXDA.Nodes.Types.EPRICapBankAnalysis
{
    public class EPRICapBankAnalysisNode : NodeBase
    {
        #region [ Members ]

        // Nested Types
        private delegate void CapBankAnalysisRoutine(double[,] kFactors, string inputParameterFile);

        private class Settings
        {
            public Settings(Action<object> configure) =>
                configure(this);

            [Category]
            [SettingName(DataAnalysisSection.CategoryName)]
            public DataAnalysisSection DataAnalysisSettings { get; } = new DataAnalysisSection();

            [Category]
            [SettingName(EPRICapBankAnalyticSection.CategoryName)]
            public EPRICapBankAnalyticSection AnalyticSettings { get; }
                = new EPRICapBankAnalyticSection();
        }

        private class EPRICapBankAnalysisWebController : ApiController
        {
            private EPRICapBankAnalysisNode Node { get; }

            public EPRICapBankAnalysisWebController(EPRICapBankAnalysisNode node) =>
                Node = node;

            [HttpGet]
            public void Analyze(int fileGroupID, int processingVersion) =>
                Node.Process(fileGroupID, processingVersion);
        }

        #endregion

        #region [ Constructors ]

        public EPRICapBankAnalysisNode(Host host, Node definition, NodeType type)
            : base(host, definition, type)
        {
            Configurator = GetConfigurator();
        }

        #endregion

        #region [ Properties ]

        private string AnalysisRoutineAssembly { get; set; }
        private string AnalysisRoutineMethod { get; set; }
        private CapBankAnalysisRoutine AnalysisRoutine { get; set; }
        private Action<object> Configurator { get; set; }

        #endregion

        #region [ Methods ]

        public void Process(int fileGroupID, int fileVersion)
        {
            Settings settings = new Settings(Configure);

            if (!settings.AnalyticSettings.Enabled)
                return;

            Log.Info($"Adding file group (ID={fileGroupID}) to CapBank Analytic Queue");
            
            using (AdoDataConnection connection = CreateDbConnection())
            {
                TableOperations<Event> evtTbl = new TableOperations<Event>(connection);
                int capBankId = connection.ExecuteScalar<int>("SELECT ID FROM AssetType WHERE Name = 'CapacitorBank'");

                List<Event> events = evtTbl
                    .QueryRecordsWhere("FileGroupID = {0} AND FileVersion = {1}", fileGroupID, fileVersion)
                    .ToList();

                foreach (IGrouping<int, Event> group in events.GroupBy<Event, int>(item => item.AssetID))
                {
                    Asset asset = new TableOperations<Asset>(connection).QueryRecordWhere("ID = {0}", group.Key);
                    if (asset.AssetTypeID != capBankId)
                        continue;

                    EPRICapBankAnalytics analytic = new EPRICapBankAnalytics(RunAnalytic, settings.AnalyticSettings.Delay);
                    analytic.Process(group.ToList(), fileGroupID);
                    analytic.StartTimer();

                    lock (s_eventFileLock)
                    {
                        using (FileBackedDictionary<int, List<int>> dictionary = new FileBackedDictionary<int, List<int>>("./CapBankAnalytic.bin"))
                            dictionary.Add(fileGroupID, group.Select(item => item.ID).ToList());
                    }
                }
            }
        }

        protected override void OnReconfigure(Action<object> configurator) =>
            Configurator = configurator;

        public override IHttpController CreateWebController() =>
            new EPRICapBankAnalysisWebController(this);

        private void RunAnalytic(List<Event> events, int fileGroupID)
        {
            // First step is to remove entry from Dictionary to avoid reprocessing if anything fails
            lock (s_eventFileLock)
            {
                using (FileBackedDictionary<int, List<int>> dictionary = new FileBackedDictionary<int, List<int>>("./CapBankAnalytic.bin"))
                    dictionary.Remove(fileGroupID);
            }

            Log.Info("Starting CapBank Analytic...");

            // Then Run the EPRI Analytic
            using (AdoDataConnection connection = CreateDbConnection())
            {
                TableOperations<Event> evtTbl = new TableOperations<Event>(connection);

                Asset asset = new TableOperations<Asset>(connection).QueryRecordWhere("ID = {0}", events.First().AssetID);

                Dictionary<string, Event> eventMapping = new Dictionary<string, Event>();
                Dictionary<string, Event> relayEventMapping = new Dictionary<string, Event>();

                CapBank capBank;
                List<CapBankRelay> relays;

                capBank = new TableOperations<CapBank>(connection).QueryRecordWhere("ID = {0}", asset.ID);
                capBank.ConnectionFactory = CreateDbConnection;
                relays = capBank.ConnectedRelays;

                lock (s_folderLock)
                {
                    // Create TxT Input File
                    GenerateParameterFile(events.First());

                    foreach (Event evt in events)
                    {
                        string fname = GenerateCapBankDataFile(evt);
                        if (fname == "")
                            continue;

                        eventMapping.Add(fname, evt);

                        List<Event> relayEvents = GetRelayEvents(evt.StartTime, evt.EndTime, relays);
                        relayEvents.ForEach(relayEvt => { relayEventMapping.Add(GenerateRelayDataFile(relayEvt), evt);  });
                    }

                    if (eventMapping.Count == 0)
                        return;

                    // Run ML code
                    Settings settings = new Settings(Configure);
                    string dstFolder = settings.AnalyticSettings.ParameterFileLocation;
                    dstFolder = Path.GetFullPath(Path.GetDirectoryName(dstFolder));

                    // Compute Initial K

                    double[,] kFactors = GetKFactors(connection, events.First(), capBank.NumberOfBanks, ComputeKInitial(capBank));
                    string inputParameterFile = Path.Combine(dstFolder, $"{capBank.AssetKey}.txt");
                    CapBankAnalysisRoutine analysisRoutine = GetAnalysisRoutine(connection);
                    analysisRoutine(kFactors, inputParameterFile);

                    // Read Output Files
                    Log.Info("Processing CapBank Analytic Results...");
                    ParseOutputs(eventMapping, relayEventMapping);

                    // Trigger cap bank emails
                    _ = NotifyEventEmailNodeAsync(events);
                }
            }
        }

        private double ComputeKInitial(CapBank capBank)
        {
            double K = 1.0D;
            if (capBank.Fused)
            {
                double M = capBank.LowerXFRRatio;
                double B = capBank.VTratioBus;
                double S = capBank.Nseries;
                double T = (double)capBank.NLowerGroups;

                K = (M/B)* (S/T);
            }
            else if (capBank.Compensated)
            {
                double B = capBank.VTratioBus;
                double N = ((double)capBank.RelayPTRatioPrimary / (double)capBank.RelayPTRatioSecondary);

                double Vu = capBank.UnitKV* 1000.0D;
                double Qu = capBank.UnitKVAr* 1000.0D;
                double S = capBank.Nseries;
                double P = capBank.Nparalell;

                double Qr = capBank.LVKVAr * 1000.0D;
                double Vr = capBank.LVKV * 1000.0D; ;
                double R = capBank.NumberLVCaps;


                double Xp = Vu*Vu / Qu * S / P;
                double Xb = Vr*Vr / Qr / R;

                K = (Xb + Xp) * N / (Xp * B);
            }
            return K;
        }
        private double[,] GetKFactors(AdoDataConnection connection, Event evt, int numBanks, double Kinitial)
        {
            int GetPhaseIndex(string phaseName)
            {
                switch (phaseName)
                {
                    case "AN": return 0;
                    case "BN": return 1;
                    case "CN": return 2;
                    default: return -1;
                }
            }

            string query =
                "SELECT " +
                "    Event.StartTime EventTime, " +
                "    CBCapBankResult.BankInService, " +
                "    Phase.Name Phase, " +
                "    CBCapBankResult.Kfactor " +
                "INTO #kfactors " +
                "FROM " +
                "    CBCapBankResult JOIN " +
                "    CBAnalyticResult ON CBCapBankResult.CBResultID = CBAnalyticResult.ID JOIN " +
                "    Event ON CBAnalyticResult.EventID = Event.ID JOIN " +
                "    Phase ON CBAnalyticResult.PhaseID = Phase.ID " +
                "WHERE MeterID = {0} " +
                " " +
                "SELECT " +
                "    Bank.Number BankNumber, " +
                "    KRecord.Phase, " +
                "    KRecord.Kfactor " +
                "FROM " +
                "    (SELECT DISTINCT BankInService Number FROM #kfactors) Bank CROSS APPLY " +
                "    (SELECT TOP 1 * FROM #kfactors WHERE BankInService = Bank.Number ORDER BY EventTime DESC) KRecord";

            int phases = 3;
            int banks = numBanks;
            double[,] kFactors = new double[phases, banks];

            for (int i = 0; i < phases; i++)
            {
                for (int j = 0; j < banks; j++)
                    kFactors[i, j] = Kinitial;
            }

            using (DataTable kRecords = connection.RetrieveData(query, evt.MeterID))
            {
                foreach (DataRow row in kRecords.Rows)
                {
                    string phaseName = row.ConvertField<string>("Phase");
                    int phase = GetPhaseIndex(phaseName);
                    int bank = row.ConvertField<int>("BankNumber");

                    if (phase < 0 || phase >= phases)
                        continue;

                    if (bank < 0 || bank >= banks)
                        continue;

                    kFactors[phase, bank] = row.ConvertField<double>("Kfactor");
                }
            }

            return kFactors;
        }

        private void ParseOutputs(Dictionary<string, Event> eventMapping, Dictionary<string, Event> relayEventMapping)
        {
            Settings settings = new Settings(Configure);
            string resultFolder = settings.AnalyticSettings.ResultFileLocation;
            resultFolder = Path.GetDirectoryName(resultFolder) ?? string.Empty;

            string[] csvFiles = Directory.GetFiles(resultFolder, "*.csv", SearchOption.AllDirectories);
            string switchingAnalysis;
            string restrikeAnalysis;
            string preInsertionAnalytics;
            string capBankAnalytics;

            switchingAnalysis = csvFiles.Where(item => item.EndsWith("_GenCapSwitch.csv", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            restrikeAnalysis = csvFiles.Where(item => item.EndsWith("_Restrike.csv", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            preInsertionAnalytics = csvFiles.Where(item => item.EndsWith("_PreInsHealth.csv", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            capBankAnalytics = csvFiles.Where(item => item.EndsWith("_RelayHealth.csv", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

            if (!string.IsNullOrEmpty(switchingAnalysis))
            {
                Log.Info($"Processing Capacitor Switching Analytic File");
                ReadSwitchingAnalytic(switchingAnalysis, eventMapping);

                Log.Info($"Processing Pre-Insertion Health Analytic Files");
                ReadPreInsertionAnalytic(preInsertionAnalytics, relayEventMapping);
                
                Log.Info($"Processing Restrike Analytic Files");
                ReadRestrikeAnalytic(restrikeAnalysis, eventMapping);

                Log.Info($"Processing CapBank and Relay Health Analytic Files");
                ReadCapBankAnalytic(capBankAnalytics, eventMapping);

                Log.Info("Finished Processing CapBank Analytic Results");
            }
            else
            {
                Log.Warn($"Capacitor Switching Analytic Results not found");
                return;
            }
        }

        private CapBankAnalysisRoutine GetAnalysisRoutine(AdoDataConnection parentConnection)
        {
            Settings settings = new Settings(Configure);
            string analysisRoutineAssembly = settings.AnalyticSettings.AnalysisRoutineAssembly;
            string analysisRoutineMethod = settings.AnalyticSettings.AnalysisRoutineMethod;

            if (string.IsNullOrEmpty(analysisRoutineAssembly) || string.IsNullOrEmpty(analysisRoutineMethod))
            {
                AnalysisRoutine = (_, __) => { };
                return AnalysisRoutine;
            }

            if (analysisRoutineAssembly == AnalysisRoutineAssembly && analysisRoutineMethod == AnalysisRoutineMethod)
                return AnalysisRoutine;

            AnalysisRoutineAssembly = analysisRoutineAssembly;
            AnalysisRoutineMethod = analysisRoutineMethod;

            try
            {
                int index = analysisRoutineMethod.LastIndexOf('.');

                if (index < 0)
                    throw new IndexOutOfRangeException($"Type information for cap bank analysis method {AnalysisRoutineMethod} not found.");

                string assemblyPath = analysisRoutineAssembly;
                string typeName = analysisRoutineMethod.Substring(0, index);
                string methodName = analysisRoutineMethod.Substring(index + 1);

                Type analysisRoutineType = typeof(CapBankAnalysisRoutine);
                MethodInfo invokeMethod = analysisRoutineType.GetMethod("Invoke");

                Type[] parameterTypes = invokeMethod
                    .GetParameters()
                    .Select(parameter => parameter.ParameterType)
                    .ToArray();

                Assembly analysisAssembly = Assembly.LoadFrom(assemblyPath);
                Type analyzerType = analysisAssembly.GetType(typeName);
                object analyzer = Activator.CreateInstance(analyzerType);

                MethodInfo analysisMethod = analyzerType.GetMethod(methodName, parameterTypes);
                Delegate analysisDelegate = analysisMethod.CreateDelegate(analysisRoutineType, analyzer);
                CapBankAnalysisRoutine analysisRoutine = (CapBankAnalysisRoutine)analysisDelegate;

                AnalysisRoutine = (kFactors, inputParameterFile) =>
                {
                    Settings engineSettings = new Settings(Configure);
                    string analyzerSettings = engineSettings.AnalyticSettings.Analyzer;
                    ConnectionStringParser.ParseConnectionString(analyzerSettings, analyzer);
                    analysisRoutine(kFactors, inputParameterFile);
                };
            }
            catch (Exception ex)
            {
                Log.Error("Unable to resolve the cap bank analysis routine.", ex);
                AnalysisRoutine = (_, __) => { };
            }

            return AnalysisRoutine;
        }

        private void ReadSwitchingAnalytic(string filename, Dictionary<string, Event> eventMapping)
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

            string fileName = "";

            using (AdoDataConnection connection = CreateDbConnection())
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

                    row.DataErrorID = ConvertToInt(fields[2]);
                    if (ConvertToInt(fields[3]) == 1)
                    {
                        Log.Warn("No Relay Data Found");
                    }

                    row.CBStatusID = ConvertToInt(fields[4]);
                    row.CBOperationID = ConvertToInt(fields[5]);
                    if (ConvertToInt(fields[6]) < 1 && ConvertToInt(fields[7]) < 1)
                    {
                        Log.Warn("Unable to identify target CapBank");
                    }
                    row.EnergizedBanks = ConvertToInt(fields[6]);
                    row.DeEnergizedBanks = ConvertToInt(fields[7]);

                    row.InServiceBank = ConvertToInt(fields[8]);
                    row.DeltaQ = ConvertToDouble(fields[9]);
                    row.Ipre = ConvertToDouble(fields[10]);
                    row.Ipost = ConvertToDouble(fields[11]);
                    row.Vpre = ConvertToDouble(fields[13]);
                    row.Vpost = ConvertToDouble(fields[14]);
                    row.MVAsc = ConvertToDouble(fields[16]);

                    row.IsRes = ConvertToInt(fields[17]) == 1;
                    row.ResFreq = ConvertToInt(fields[18]);

                    row.THDpre = ConvertToDouble(fields[19]);
                    row.THDpost = ConvertToDouble(fields[20]);
                    row.THDVpre = ConvertToDouble(fields[22]);
                    row.THDVpost = ConvertToDouble(fields[23]);

                    row.StepPre = ConvertToInt(fields[25]);
                    row.StepPost = ConvertToInt(fields[26]);
                    row.SwitchingFreq = ConvertToDouble(fields[27]);
                    row.Vpeak = ConvertToDouble(fields[28]);
                    row.Xpre = ConvertToDouble(fields[29]);
                    row.Xpost = ConvertToDouble(fields[30]);

                    int year = ConvertToInt(fields[31]) ?? 2020;
                    int month = ConvertToInt(fields[32]) ?? 1;
                    int day = ConvertToInt(fields[33]) ?? 1;

                    int hour = ConvertToInt(fields[34]) ?? 0;
                    int minute = ConvertToInt(fields[35]) ?? 0;
                    int second = ConvertToInt(fields[36]) ?? 0;

                    row.Time = new DateTime(year, month, day, hour, minute, second);
                    row.Toffset = ConvertToDouble(fields[37]);

                    row.XshortedThld = ConvertToDouble(fields[39]);
                    row.XblownThld = ConvertToDouble(fields[40]);
                    row.dVThld = ConvertToDouble(fields[41]);
                    row.dVThldLG = ConvertToDouble(fields[42]);

                    if (eventMapping.TryGetValue(fileName, out Event evt))
                    {
                        row.EventID = evt.ID;
                        new TableOperations<CBAnalyticResult>(connection).AddNewRecord(row);
                        row.ID = connection.ExecuteScalar<int>("SELECT @@Identity");
                    }
                }
            }
        }

        private void ReadPreInsertionAnalytic(string filename, Dictionary<string, Event> eventMapping)
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

            string fileName = "";

            using (AdoDataConnection connection = CreateDbConnection())
            {
                foreach (string line in lines.Skip(2))
                {
                    List<string> fields = line.Split(',').ToList();

                    if (!string.IsNullOrEmpty(fields[0].Trim()))
                        fileName = fields[0].Trim();

                    string evtLabel = fields[1].Trim();

                    CBSwitchHealthAnalytic row = new CBSwitchHealthAnalytic();
                    int PhaseID = 0;

                    if (evtLabel.EndsWith("a", StringComparison.OrdinalIgnoreCase))
                        PhaseID = connection.ExecuteScalar<int>("SELECT ID FROM Phase WHERE Name = 'AN'");
                    else if (evtLabel.EndsWith("b", StringComparison.OrdinalIgnoreCase))
                        PhaseID = connection.ExecuteScalar<int>("SELECT ID FROM Phase WHERE Name = 'BN'");
                    else if (evtLabel.EndsWith("c", StringComparison.OrdinalIgnoreCase))
                        PhaseID = connection.ExecuteScalar<int>("SELECT ID FROM Phase WHERE Name = 'CN'");
                    else
                        PhaseID = connection.ExecuteScalar<int>("SELECT ID FROM Phase WHERE Name = 'None'");

                    row.CBSwitchingConditionID = ConvertToInt(fields[3]) ?? -1;
                    row.R = ConvertToDouble(fields[7]);
                    row.X = ConvertToDouble(fields[8]);
                    row.Duration = ConvertToDouble(fields[6]);
                    row.I = ConvertToDouble(fields[9]);

                    if (row.CBSwitchingConditionID == -1)
                        continue;

                    Event evt;
                    if (eventMapping.TryGetValue(fileName, out evt))
                    {
                        if (new TableOperations<CBAnalyticResult>(connection).QueryRecordCountWhere("PhaseID = {0} AND EventID = {1}", PhaseID, evt.ID) != 1)
                            continue;

                        row.CBResultID = new TableOperations<CBAnalyticResult>(connection).QueryRecordWhere("PhaseID = {0} AND EventID = {1}", PhaseID, evt.ID).ID;

                        new TableOperations<CBSwitchHealthAnalytic>(connection).AddNewRecord(row);
                    }
                }
            }
        }

        private void ReadRestrikeAnalytic(string filename, Dictionary<string, Event> eventMapping)
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

            string fileName = "";

            using (AdoDataConnection connection = CreateDbConnection())
            {
                foreach (string line in lines.Skip(2))
                {
                    List<string> fields = line.Split(',').ToList();

                    if (!string.IsNullOrEmpty(fields[0].Trim()))
                        fileName = fields[0].Trim();

                    string evtLabel = fields[1].Trim();

                    CBRestrikeResult row = new CBRestrikeResult();
                    int PhaseID = 0;

                    if (evtLabel.EndsWith("a", StringComparison.OrdinalIgnoreCase))
                        PhaseID = connection.ExecuteScalar<int>("SELECT ID FROM Phase WHERE Name = 'AN'");
                    else if (evtLabel.EndsWith("b", StringComparison.OrdinalIgnoreCase))
                        PhaseID = connection.ExecuteScalar<int>("SELECT ID FROM Phase WHERE Name = 'BN'");
                    else if (evtLabel.EndsWith("c", StringComparison.OrdinalIgnoreCase))
                        PhaseID = connection.ExecuteScalar<int>("SELECT ID FROM Phase WHERE Name = 'CN'");
                    else
                        PhaseID = connection.ExecuteScalar<int>("SELECT ID FROM Phase WHERE Name = 'None'");

                    row.CBRestrikeTypeID = ConvertToInt(fields[3]) ?? -1;
                    row.Text = ConvertToDouble(fields[6]);
                    row.Trest = ConvertToDouble(fields[7]);
                    row.Text2 = ConvertToDouble(fields[8]);
                    row.Drest = ConvertToDouble(fields[9]);
                    row.Imax = ConvertToDouble(fields[10]);
                    row.Vmax = ConvertToDouble(fields[11]);

                    if (row.CBRestrikeTypeID < 0)
                        continue;

                    if (eventMapping.TryGetValue(fileName, out Event evt))
                    {
                        if (new TableOperations<CBAnalyticResult>(connection).QueryRecordCountWhere("PhaseID = {0} AND EventID = {1}", PhaseID, evt.ID) != 1)
                            continue;

                        row.CBResultID = new TableOperations<CBAnalyticResult>(connection).QueryRecordWhere("PhaseID = {0} AND EventID = {1}", PhaseID, evt.ID).ID;

                        new TableOperations<CBRestrikeResult>(connection).AddNewRecord(row);
                    }
                }
            }
        }

        private void ReadCapBankAnalytic(string filename, Dictionary<string, Event> eventMapping)
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

            string fileName = "";

            using (AdoDataConnection connection = CreateDbConnection())
            {
                foreach (string line in lines.Skip(2))
                {
                    List<string> fields = line.Split(',').ToList();

                    if (!string.IsNullOrEmpty(fields[0].Trim()))
                        fileName = fields[0].Trim();

                    string evtLabel = fields[1].Trim();

                    CBCapBankResult row = new CBCapBankResult();
                    int PhaseID = 0;

                    if (evtLabel.EndsWith("a", StringComparison.OrdinalIgnoreCase))
                        PhaseID = connection.ExecuteScalar<int>("SELECT ID FROM Phase WHERE Name = 'AN'");
                    else if (evtLabel.EndsWith("b", StringComparison.OrdinalIgnoreCase))
                        PhaseID = connection.ExecuteScalar<int>("SELECT ID FROM Phase WHERE Name = 'BN'");
                    else if (evtLabel.EndsWith("c", StringComparison.OrdinalIgnoreCase))
                        PhaseID = connection.ExecuteScalar<int>("SELECT ID FROM Phase WHERE Name = 'CN'");
                    else
                        PhaseID = connection.ExecuteScalar<int>("SELECT ID FROM Phase WHERE Name = 'None'");

                    Event evt = null;
                    if (eventMapping.TryGetValue(fileName, out evt))
                    {
                        if (new TableOperations<CBAnalyticResult>(connection).QueryRecordCountWhere("PhaseID = {0} AND EventID = {1}", PhaseID, evt.ID) != 1)
                            continue;

                        row.CBResultID = new TableOperations<CBAnalyticResult>(connection).QueryRecordWhere("PhaseID = {0} AND EventID = {1}", PhaseID, evt.ID).ID;
                    }
                    if (evt == null)
                        continue;

                    CapBank capBank = new TableOperations<CapBank>(connection).QueryRecordWhere("Id = {0}", evt.AssetID);
                    int offset = 3;
                    CBAnalyticResult result = new TableOperations<CBAnalyticResult>(connection).QueryRecordWhere("Id = {0}", row.CBResultID);

                    row.CBBankHealthID = ConvertToInt(fields[offset]) ?? -1;
                    offset++;
                    row.CBOperationID = ConvertToInt(fields[offset]) ?? -1;
                    offset++;

                    if (capBank.Compensated && !capBank.Fused)
                    {
                        row.Kfactor = ConvertToDouble(fields[offset + (int)result.InServiceBank]);
                        offset = offset + capBank.NumberOfBanks;

                        row.dV = ConvertToDouble(fields[offset + (int)result.InServiceBank]);
                        offset = offset + capBank.NumberOfBanks;

                        row.XLV = ConvertToDouble(fields[offset + (int)result.InServiceBank]);
                        offset = offset + capBank.NumberOfBanks;

                        row.X = ConvertToDouble(fields[offset + (int)result.InServiceBank]);
                        offset = offset + capBank.NumberOfBanks;

                        row.XVmiss = ConvertToDouble(fields[offset + (int)result.InServiceBank]);
                        offset = offset + capBank.NumberOfBanks;

                        row.VUIEC = ConvertToDouble(fields[offset + (int)result.InServiceBank]);
                        offset = offset + capBank.NumberOfBanks;

                        row.VUIEEE = ConvertToDouble(fields[offset + (int)result.InServiceBank]);
                    }
                    else if (!capBank.Fused && !capBank.Compensated)
                    {
                        row.Vrelay = ConvertToDouble(fields[offset + (int)result.InServiceBank]);
                        offset = offset + capBank.NumberOfBanks;

                        row.Ineutral = ConvertToDouble(fields[offset + (int)result.InServiceBank]);
                        offset = offset + capBank.NumberOfBanks;

                        row.V0 = ConvertToDouble(fields[offset + (int)result.InServiceBank]) ;
                        offset = offset + capBank.NumberOfBanks;

                        row.Z0 = ConvertToDouble(fields[offset + (int)result.InServiceBank]);
                        offset = offset + capBank.NumberOfBanks;

                        row.XLV = ConvertToDouble(fields[offset + (int)result.InServiceBank]);
                        offset = offset + capBank.NumberOfBanks;

                        row.X = ConvertToDouble(fields[offset + (int)result.InServiceBank]);
                        offset = offset + capBank.NumberOfBanks;

                        row.VUIEC = ConvertToDouble(fields[offset + (int)result.InServiceBank]);
                        offset = offset + capBank.NumberOfBanks;

                        row.VUIEEE = ConvertToDouble(fields[offset + (int)result.InServiceBank]);
                    }
                    else
                    {
                        row.Kfactor = ConvertToDouble(fields[offset + (int)result.InServiceBank]);
                        offset = offset + capBank.NumberOfBanks;

                        row.dV = ConvertToDouble(fields[offset + (int)result.InServiceBank]);
                        offset = offset + capBank.NumberOfBanks;

                        row.XUG = ConvertToDouble(fields[offset + (int)result.InServiceBank]);
                        offset = offset + capBank.NumberOfBanks;

                        row.XLG = ConvertToDouble(fields[offset + (int)result.InServiceBank]);
                        offset = offset + capBank.NumberOfBanks;

                        row.X = ConvertToDouble(fields[offset + (int)result.InServiceBank]);
                        offset = offset + capBank.NumberOfBanks;

                        row.XVmiss = ConvertToDouble(fields[offset + (int)result.InServiceBank]);
                        offset = offset + capBank.NumberOfBanks;

                        row.VUIEC = ConvertToDouble(fields[offset + (int)result.InServiceBank]);
                        offset = offset + capBank.NumberOfBanks;

                        row.VUIEEE = ConvertToDouble(fields[offset + (int)result.InServiceBank]);
                    }

                    if (row.CBOperationID < 0)
                        continue;

                    row.BankInService = (int)result.InServiceBank;

                    new TableOperations<CBCapBankResult>(connection).AddNewRecord(row);
                }
            }
        }

        private int? ConvertToInt(string value)
        {
            double? v = ConvertToDouble(value);
            if (v == null)
                return null;

            return (int?)v;
        }

        private double? ConvertToDouble(string value)
        {
            double v = Convert.ToDouble(value);
            if (double.IsNaN(v))
                return null;
            return v;
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


            using (AdoDataConnection connection = CreateDbConnection())
            {
                capBank = new TableOperations<CapBank>(connection).QueryRecordWhere("ID = {0}", evt.AssetID);
                capBank.ConnectionFactory = CreateDbConnection;
                relays = capBank.ConnectedRelays;

                if (vSamples == -1 || iSamples == -1)
                {
                    Log.Error($"Voltage or Current Measurment not available for {capBank.AssetName}");
                    return;
                }

                Settings settings = new Settings(Configure);
                double systemFreq = settings.DataAnalysisSettings.SystemFrequency;
                double vThreshold = settings.AnalyticSettings.VThreshhold;
                double iThreshold = settings.AnalyticSettings.IThreshhold;
                double thdLimit = settings.AnalyticSettings.THDLimit;
                double tOffset = settings.AnalyticSettings.Toffset;
                bool enablePreInsertion = settings.AnalyticSettings.EvalPreInsertion;

                iSamples = Math.Round(iSamples / systemFreq);
                vSamples = Math.Round(vSamples / systemFreq);

                string dstFolder = settings.AnalyticSettings.ParameterFileLocation;
                dstFolder = Path.GetDirectoryName(dstFolder);

                string datafolder = settings.AnalyticSettings.DataFileLocation;
                string resultFolder = settings.AnalyticSettings.ResultFileLocation;
                datafolder = Path.GetFullPath(Path.GetDirectoryName(datafolder));
                resultFolder = Path.GetFullPath(Path.GetDirectoryName(resultFolder));

                Directory.CreateDirectory(datafolder);
                Directory.CreateDirectory(resultFolder);
                Directory.CreateDirectory(dstFolder);

                foreach (FileInfo file in new DirectoryInfo(datafolder).EnumerateFiles())
                {
                    file.Delete();
                }
                foreach (FileInfo file in new DirectoryInfo(resultFolder).EnumerateFiles())
                {
                    file.Delete();
                }
                foreach (FileInfo file in new DirectoryInfo(dstFolder).EnumerateFiles())
                {
                    file.Delete();
                }

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
                lines.Add($"13 no - voltage threshold for voltage waveforms in V; noBusVoltage = {vThreshold}");
                lines.Add($"14 no - current threshold for current waveforms in A; noBusCurrent = {iThreshold}");
                lines.Add($"15 the upper limit(in percent) to detect harmonic resonance; iTHDLimit = {thdLimit}");
                lines.Add("");
                lines.Add("Specify cap bank configuration data");
                lines.Add($"17 number of banks; numBanks = {capBank.NumberOfBanks}");
                lines.Add($"18 nominal bus voltage in kV line-to - line; nominalBuskVLL = {capBank.VoltageKV}");
                lines.Add($"19 capacitor step size; StepSizeQ3kvar = {capBank.CapacitancePerBank}");
                lines.Add($"20 type of the circuit switcher(0 for no control, 1 for pre - ins, 2 for sync closing) ; capSwitcherTypeMultBanks = [{capBank.CktSwitcher}]");
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
                lines.Add($"33 Offset time between cap bank and relay time stamps; dToffset = {tOffset}");
              
                if (relays.Count == 0)
                {
                    lines.Add($"34 Rated relay voltage in V; ratedRelayVoltage = {161}");
                    lines.Add($"35 No voltage for relay or threshold of ON voltage; relayOnVoltageThreshold = {0.075}");
                    Log.Error($"No connectd Relays found for {capBank.AssetName}");

                }
                else
                {
                    lines.Add($"34 Rated relay voltage in V; ratedRelayVoltage = {relays.First().VoltageKV}");
                    lines.Add($"35 No voltage for relay or threshold of ON voltage; relayOnVoltageThreshold = {relays.First().OnVoltageThreshhold}");
                }
                
                lines.Add("");
                lines.Add("Specify relay capacitor configuration for fuseless configurations");
                lines.Add($"37 Bus VT ratio; busVT = {capBank.VTratioBus}");
                lines.Add($"38 the number of relay capacitor; NLVcapUnit = {capBank.NumberLVCaps}");
                lines.Add($"39 the number of elements; NLVcapE = {capBank.NumberLVUnits}");
                lines.Add($"40 low - voltage capacitor size for capacitor bank relaying; LVcapUnitRatedkvar = {capBank.LVKVAr}");
                lines.Add($"41 rated voltage of the low - voltage capacitor; LVcapUnitRatedV = {capBank.LVKV}");
                lines.Add($"42 the reactance tolerance of LV cap., negative tolerance in percent; nLVcapTolpct = {capBank.LVNegReactanceTol}");
                lines.Add($"43 the reactance tolerance of LV cap., positive tolerance in percent; pLVcapTolpct = {capBank.LVPosReactanceTol}");
                lines.Add($"44 relay PT ratio, high to low; relayPTR = [{capBank.RelayPTRatioPrimary + " " + capBank.RelayPTRatioSecondary}]");
                lines.Add($"45 the output resistor of the voltage divider circuit; Rv = {capBank.Rv}");
                lines.Add($"46 the input resistor of the voltage divider circuit; Rh = {capBank.Rh}");
                lines.Add("");
                lines.Add("Specify relay capacitor configuration for fused configurations");
                lines.Add($"48 upper group voltage transformer ratio; UVTR = {capBank.VTratioBus}");
                lines.Add($"49 lower group voltage transformer ratio; LVTR = {capBank.LowerXFRRatio}");
                lines.Add($"50 number of lower groups below the tap point; nLGbTap = {capBank.NLowerGroups}");
                lines.Add("");
                lines.Add("Specify capacitor protection scheme");
                lines.Add("Set pCapDesign to 1 for compensated design");
                lines.Add("Set pCapDesign to 2 for uncompensated design");
                lines.Add("Set pCapDesign to 3 for fused design");
                lines.Add($"55 pCapDesign = {(capBank.Compensated ? "1" : (capBank.Fused ? "3" : "2"))}");
                lines.Add("");
                lines.Add("Specify the assumed initial numbers of shorted or blown fuses in cap banks.");
                lines.Add("They are used to set thresholds for declaring banks having shorted units or blown fuses");
                lines.Add("Numbers do not need to be whole(integers, i.e., 1). They can be fractional(real), i.e., 0.75");
                lines.Add("For the fuseless compensated and uncompensated designs, specify the following:");
                lines.Add($"60 the assumed initial total number of shorted capacitor elements, use 1 catch more; NFesg_init = {capBank.Nshorted}");
                lines.Add("For the fused compensated design, specify the following:");
                lines.Add($"62 the assumed initial number of blown fuses per group; use 1 to catch more; nBFG_init = {capBank.BlownFuses}");
                lines.Add($"63 the assumed initial number of groups with blown fuses; use 1 to catch more; nGBF_init = {capBank.BlownGroups}");
                lines.Add($"64 the assumed initial number of groups shorted; use 0.5 to to catch more; NsS_init = {capBank.ShortedGroups}");
                lines.Add("");
                lines.Add("Specify relay keywords");
                int n = 1;
                foreach (CapBankRelay relay in relays)
                {
                    lines.Add($"{n + 65} fileKeyRelay4Cap{{{n}}} = Relay{relay.ID}");
                    n++;
                }
                lines.Add("");
                lines.Add("Evaluation of pre-insertion takes a longer time.  Set evalPreIns to 1 to evaluate; 0 to skip");
                lines.Add($"{n + 66} evalPreIns = {(enablePreInsertion ? 1 : 0)}");

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

        private string GenerateCapBankDataFile(Event evt)
        {
            CapBank capBank;
            VIDataGroup data = QueryDataGroup(evt.ID, evt.MeterID);

            using (AdoDataConnection connection = CreateDbConnection())
            {
                capBank = new TableOperations<CapBank>(connection).QueryRecordWhere("ID = {0}", evt.AssetID);
                capBank.ConnectionFactory = CreateDbConnection;

                Settings settings = new Settings(Configure);
                string datafolder = settings.AnalyticSettings.DataFileLocation;
                datafolder = Path.GetDirectoryName(datafolder);

                string dstFile = $"{capBank.AssetKey}-{evt.StartTime:yyyyMMddTHHmmss}-{evt.ID}.csv";
                List<string> lines = new List<string>();

                lines.Add($"\"{capBank.AssetKey} - {evt.StartTime:MM/dd/yyyy HH:mm:ss.ffff} \"");
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
                    return "";

                lines.Add(header);
                lines.AddRange(ToCsV(series));

                // Open the file and write in each line
                using (StreamWriter fileWriter = new StreamWriter(File.OpenWrite(datafolder + "\\" + dstFile)))
                {
                    for (int i = 0; i < lines.Count(); i++)
                    {
                        fileWriter.WriteLine(lines[i]);
                    }
                }

                return dstFile;
            }
        }

        private string GenerateRelayDataFile(Event evt)
        {
            CapBankRelay relay;
            VIDataGroup data = QueryDataGroup(evt.ID, evt.MeterID);

            using (AdoDataConnection connection = CreateDbConnection())
            {
                relay = new TableOperations<CapBankRelay>(connection).QueryRecordWhere("ID = {0}", evt.AssetID);
                relay.ConnectionFactory = CreateDbConnection;

                Settings settings = new Settings(Configure);
                string datafolder = settings.AnalyticSettings.DataFileLocation;
                datafolder = Path.GetDirectoryName(datafolder);

                string dstFile = $"Relay{relay.ID}-{evt.StartTime:yyyyMMddTHHmmss}-{evt.ID}.csv";
                List<string> lines = new List<string>();

                lines.Add($"\"{relay.AssetKey} - {evt.StartTime:MM/dd/yyyy HH:mm:ss.ffff} \"");
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
                    return dstFile;

                lines.Add(header);
                lines.AddRange(ToCsV(series));

                // Open the file and write in each line
                using (StreamWriter fileWriter = new StreamWriter(File.OpenWrite(datafolder + "\\" + dstFile)))
                {
                    for (int i = 0; i < lines.Count(); i++)
                    {
                        fileWriter.WriteLine(lines[i]);
                    }
                }

                return dstFile;
            }
        }

        private List<Event> GetRelayEvents(DateTime startTime, DateTime endTime, List<CapBankRelay> relays)
        {
            List<Event> result = new List<Event>();

            using (AdoDataConnection connection = CreateDbConnection())
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

        private VIDataGroup QueryDataGroup(int eventID, int meterId)
        {
            using (AdoDataConnection connection = CreateDbConnection())
            {
                List<byte[]> data = ChannelData.DataFromEvent(eventID, CreateDbConnection);
                Meter meter = new TableOperations<Meter>(connection).QueryRecordWhere("ID = {0}", meterId);
                meter.ConnectionFactory = CreateDbConnection;
                return ToDataGroup(meter, data);
            }
        }

        private async Task NotifyEventEmailNodeAsync(List<Event> events)
        {
            const string QueryFormat =
                "SELECT Node.ID " +
                "FROM " +
                "    ActiveHost JOIN " +
                "    Node ON Node.HostRegistrationID = ActiveHost.ID JOIN " +
                "    NodeType ON Node.NodeTypeID = NodeType.ID " +
                "WHERE NodeType.TypeName = {0}";

            Type nodeType = typeof(Email.EventEmailNode);
            string typeName = nodeType.FullName;
            int? result;

            using (AdoDataConnection connection = CreateDbConnection())
                result = connection.ExecuteScalar<int?>(QueryFormat, typeName);

            if (result is null)
                return;

            void ConfigureRequest(HttpRequestMessage request)
            {
                int nodeID = result.GetValueOrDefault();
                string action = "TriggerForEvents";
                NameValueCollection queryParameters = new NameValueCollection();
                queryParameters.Add("triggerSource", "CapBankAnalyticEngine");

                foreach (Event evt in events)
                    queryParameters.Add("eventIDs", evt.ID.ToString());

                string baseURL = Host.BuildURL(nodeID, action, queryParameters);
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

        private void Configure(object obj) => Configurator(obj);

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ConnectionStringParser<SettingAttribute, CategoryAttribute> ConnectionStringParser = new ConnectionStringParser<SettingAttribute, CategoryAttribute>();
        private static readonly ILog Log = LogManager.GetLogger(typeof(EPRICapBankAnalysisNode));
        private static readonly object s_eventFileLock = new object();
        private static readonly object s_folderLock = new object();

        // Static Methods
        private static List<string> ToCsV(List<DataSeries> data)
        {

            List<string> result = new List<string>();
            DateTime startingTime = data.First().DataPoints[0].Time;

            for (int i = 0; i < data.First().DataPoints.Count; ++i)
            {
                double TS = (data.First().DataPoints[i].Time - startingTime).TotalSeconds;

                IEnumerable<string> row = new List<string>() { TS.ToString("0.#######") };

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

        #endregion
    }
}
