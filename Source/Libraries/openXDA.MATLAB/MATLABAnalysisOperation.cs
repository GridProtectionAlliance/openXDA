//******************************************************************************************************
//  MATLABAnalysisOperation.cs - Gbtc
//
//  Copyright © 2023, Grid Protection Alliance.  All Rights Reserved.
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
//  03/27/2023 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using FaultData.DataAnalysis;
using FaultData.DataOperations;
using FaultData.DataResources;
using FaultData.DataSets;
using GSF;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using log4net;
using openXDA.Model;

namespace openXDA.MATLAB
{
    public class MATLABAnalysisOperation : DataOperationBase<MeterDataSet>
    {
        public class Settings
        {
            [Setting]
            [DefaultValue("")]
            public string Analytics { get; set; }
        }

        [Category]
        [SettingName("MATLAB")]
        public Settings MATLABSettings { get; }
            = new Settings();

        public override void Execute(MeterDataSet meterDataSet)
        {
            CycleDataResource cycleDataResource = meterDataSet.GetResource<CycleDataResource>();
            List<DataGroup> dataGroups = cycleDataResource.DataGroups;
            List<VIDataGroup> viDataGroups = cycleDataResource.VIDataGroups;

            List<MATLABAnalyticSetup> analyticSetupList = MATLABSettings.Analytics
                .ParseKeyValuePairs()
                .Select(kvp => MATLABAnalyticSetup.Parse(kvp.Value))
                .ToList();

            using (AdoDataConnection connection = meterDataSet.CreateDbConnection())
            {
                TableOperations<Event> eventTable = new TableOperations<Event>(connection);
                TableOperations<EventTag> eventTagTable = new TableOperations<EventTag>(connection);
                TableOperations<EventEventTag> eventEventTagTable = new TableOperations<EventEventTag>(connection);

                for (int i = 0; i < dataGroups.Count; i++)
                {
                    DataGroup dataGroup = dataGroups[i];
                    VIDataGroup viDataGroup = viDataGroups[i];
                    Event evt = eventTable.GetEvent(meterDataSet.FileGroup, dataGroup);
                    List<MATLABAnalyticTag> allTags = new List<MATLABAnalyticTag>();

                    foreach (MATLABAnalyticSetup setup in analyticSetupList)
                    {
                        try
                        {
                            MATLABAnalytic analytic = ToAnalytic(setup);
                            List<MATLABAnalyticSettingField> settingFields = QuerySettingFields(connection, setup.SettingSQL, evt.ID);
                            List<MATLABAnalyticTag> analyticTags = analytic.Execute(viDataGroup, settingFields);
                            allTags.AddRange(analyticTags);
                        }
                        catch (Exception ex)
                        {
                            Log.Error($"Error occurred while executing MATLAB analytic {analyticModel.MethodName}: {ex.Message}", ex);
                        }
                    }

                    foreach (MATLABAnalyticTag tag in allTags)
                    {
                        EventTag eventTag = eventTagTable.GetOrAdd(tag.Name);
                        EventEventTag eventEventTag = eventEventTagTable.NewRecord();
                        eventEventTag.EventID = evt.ID;
                        eventEventTag.EventTagID = eventTag.ID;
                        eventEventTag.TagData = tag.JSONData;
                        eventEventTagTable.AddNewRecord(eventEventTag);
                    }
                }
            }
        }

        private MATLABAnalytic ToAnalytic(MATLABAnalyticSetup setup)
        {
            string assemblyName = setup.AssemblyName;
            string methodName = setup.MethodName;
            MATLABAnalysisFunctionInvokerFactory invokerFactory = AnalysisFunctionFactory.GetAnalysisFunctionInvokerFactory(assemblyName, methodName);
            return new MATLABAnalytic(invokerFactory);
        }

        private List<MATLABAnalyticSettingField> QuerySettingFields(AdoDataConnection connection, string sql, int eventID)
        {
            MATLABAnalyticSettingField ToSettingField(DataColumn column, DataRow row)
            {
                string name = column.ColumnName;
                object value = row[column];
                return new MATLABAnalyticSettingField(name, value);
            }

            using (DataTable table = connection.RetrieveData(sql, eventID))
            {
                if (table.Rows.Count == 0)
                    return new List<MATLABAnalyticSettingField>();

                DataRow row = table.Rows[0];

                return table.Columns
                    .Cast<DataColumn>()
                    .Select(column => ToSettingField(column, row))
                    .ToList();
            }
        }

        private static MATLABAnalysisFunctionFactory AnalysisFunctionFactory { get; }
            = new MATLABAnalysisFunctionFactory();

        private static readonly ILog Log = LogManager.GetLogger(typeof(MATLABAnalysisOperation));
    }
}
