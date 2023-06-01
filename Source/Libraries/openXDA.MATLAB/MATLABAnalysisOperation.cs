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
using System.Data;
using System.Linq;
using FaultData.DataAnalysis;
using FaultData.DataOperations;
using FaultData.DataResources;
using FaultData.DataSets;
using GSF.Data;
using GSF.Data.Model;
using log4net;
using openXDA.Model;
using AnalyticModel = openXDA.Model.MATLABAnalytic;

namespace openXDA.MATLAB
{
    public class MATLABAnalysisOperation : DataOperationBase<MeterDataSet>
    {
        public override void Execute(MeterDataSet meterDataSet)
        {
            CycleDataResource cycleDataResource = meterDataSet.GetResource<CycleDataResource>();
            List<DataGroup> dataGroups = cycleDataResource.DataGroups;
            List<VIDataGroup> viDataGroups = cycleDataResource.VIDataGroups;

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
                    List<AnalyticModel> analyticModelList = QueryAnalytics(connection, evt.ID);
                    List<MATLABAnalyticTag> allTags = new List<MATLABAnalyticTag>();

                    foreach (AnalyticModel analyticModel in analyticModelList)
                    {
                        try
                        {
                            MATLABAnalytic analytic = ToAnalytic(analyticModel);
                            List<MATLABAnalyticSettingField> settingFields = QuerySettingFields(connection, analyticModel.SettingSQL, evt.ID);
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

        private List<AnalyticModel> QueryAnalytics(AdoDataConnection connection, int eventID)
        {
            const string MATLABAnalyticQueryFormat =
                "SELECT DISTINCT MATLABAnalytic.* " +
                "FROM " +
                "    Event JOIN " +
                "    Asset ON Event.AssetID = Asset.ID CROSS JOIN " +
                "    MATLABAnalytic LEFT OUTER JOIN " +
                "    MATLABAnalyticAssetType ON MATLABAnalyticAssetType.MATLABAnalyticID = MATLABAnalytic.ID LEFT OUTER JOIN " +
                "    MATLABAnalyticEventType ON MATLABAnalyticEventType.MATLABAnalyticID = MATLABAnalytic.ID " +
                "WHERE " +
                "    Event.ID = {0} AND " +
                "    (MATLABAnalyticAssetType.AssetTypeID IS NULL OR MATLABAnalyticAssetType.AssetTypeID = Asset.AssetTypeID) AND " +
                "    (MATLABAnalyticEventType.EventTypeID IS NULL OR MATLABAnalyticEventType.EventTypeID = Event.EventTypeID) " +
                "ORDER BY MATLABAnalytic.LoadOrder";

            TableOperations<AnalyticModel> matlabAnalyticTable = new TableOperations<AnalyticModel>(connection);

            using (DataTable table = connection.RetrieveData(MATLABAnalyticQueryFormat, eventID))
            {
                return table
                    .AsEnumerable()
                    .Select(matlabAnalyticTable.LoadRecord)
                    .ToList();
            }
        }

        private MATLABAnalytic ToAnalytic(AnalyticModel model)
        {
            string assemblyName = model.AssemblyName;
            string methodName = model.MethodName;
            MATLABAnalysisFunctionInvokerFactory invokerFactory = AnalysisFunctionFactory.GetAnalysisFunctionInvokerFactory(assemblyName, methodName);
            return new MATLABAnalytic(invokerFactory);
        }

        private List<MATLABAnalyticSettingField> QuerySettingFields(AdoDataConnection connection, string sql, int eventID)
        {
            if (string.IsNullOrEmpty(sql))
                return new List<MATLABAnalyticSettingField>(0);

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
