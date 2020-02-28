//******************************************************************************************************
//  StructureQueryOperation.cs - Gbtc
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
//  08/16/2019 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using FaultData.DataAnalysis;
using FaultData.DataResources;
using FaultData.DataSets;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using HtmlAgilityPack;
using openXDA.Model;

namespace FaultData.DataOperations.TVA
{
    public class StructureQueryOperation : DataOperationBase<MeterDataSet>
    {
        #region [ Members ]

        // Nested Types
        public class StructureQuerySettings
        {
            [Setting]
            [DefaultValue("http://server/site/neareststructure?station={0}&line={1}&distance={2}")]
            public string URLFormat { get; set; }

            [Setting]
            [DefaultValue("StrNumber:AssetKey,Latitude:Latitude,Longitude:Longitude")]
            public string FieldMappings { get; set; }
        }

        // Constants

        private const string FaultSummaryQuery =
            "SELECT ID " +
            "FROM FaultSummary " +
            "WHERE " +
            "    EventID = {0} AND " +
            "    FaultNumber = {1} AND " +
            "    IsSelectedAlgorithm <> 0";

        #endregion

        #region [ Properties ]

        [Category]
        [SettingName("StructureQuery")]
        public StructureQuerySettings Settings { get; } = new StructureQuerySettings();

        private Func<string, string> FieldMappingLookup
        {
            get
            {
                Dictionary<string, string> fieldMappings = Settings.FieldMappings.Split(',')
                    .Select(mapping => mapping.Split(':'))
                    .ToDictionary(mapping => mapping[1].Trim(), mapping => mapping[0].Trim());

                return inputField => fieldMappings.TryGetValue(inputField, out string outputField) ? outputField : inputField;
            }
        }

        #endregion

        #region [ Methods ]

        public override void Execute(MeterDataSet meterDataSet)
        {
            FaultDataResource faultDataResource = meterDataSet.GetResource<FaultDataResource>();
            string stationKey = meterDataSet.Meter.MeterLocation.AssetKey;

            foreach (var kvp in faultDataResource.FaultLookup)
            {
                DataGroup dataGroup = kvp.Key;
                DataAnalysis.FaultGroup faultGroup = kvp.Value;
                string lineKey = dataGroup.Line.AssetKey;

                for (int i = 0; i < faultGroup.Faults.Count; i++)
                {
                    int faultNumber = i + 1;
                    DataAnalysis.Fault fault = faultGroup.Faults[i];

                    if (fault.IsSuppressed)
                        continue;

                    string distance = fault.Summaries
                        .Where(summary => summary.IsValid)
                        .Where(summary => summary.IsSelectedAlgorithm)
                        .Select(summary => summary.Distance.ToString("0.###"))
                        .FirstOrDefault();

                    if (distance == null)
                        return;

                    string url = string.Format(Settings.URLFormat, stationKey, lineKey, distance);
                    string structureInfo = GetStructureInfo(url);
                    DataTable structureData = ToDataTable(structureInfo);

                    if (structureData.Rows.Count == 0)
                        return;

                    Func<string, string> fieldMappingLookup = FieldMappingLookup;
                    string assetKeyField = fieldMappingLookup("AssetKey");
                    string latitudeKeyField = fieldMappingLookup("Latitude");
                    string longitudeKeyField = fieldMappingLookup("Longitude");

                    if (!structureData.Columns.Contains(assetKeyField))
                        return;

                    using (AdoDataConnection connection = meterDataSet.CreateDbConnection())
                    {
                        TableOperations<Event> eventTable = new TableOperations<Event>(connection);
                        Event evt = eventTable.GetEvent(meterDataSet.FileGroup, dataGroup);
                        int faultSummaryID = connection.ExecuteScalar<int>(FaultSummaryQuery, evt.ID, faultNumber);

                        TableOperations<Structure> structureTable = new TableOperations<Structure>(connection);

                        foreach (DataRow row in structureData.Rows)
                        {
                            string assetKey = row.Field<string>(assetKeyField);
                            string latitude = null;
                            string longitude = null;

                            if (structureData.Columns.Contains(latitudeKeyField))
                                latitude = row.Field<string>(latitudeKeyField);

                            if (structureData.Columns.Contains(longitudeKeyField))
                                longitude = row.Field<string>(longitudeKeyField);

                            Structure structure = structureTable.QueryRecordWhere("AssetKey = {0}", assetKey)
                                ?? new Structure() { AssetKey = assetKey };

                            structure.LineID = dataGroup.Line.ID;

                            if (double.TryParse(latitude, out double lat))
                                structure.Latitude = lat;

                            if (double.TryParse(longitude, out double lon))
                                structure.Longitude = lon;

                            structureTable.AddNewOrUpdateRecord(structure);

                            if (structure.ID == 0)
                                structure.ID = connection.ExecuteScalar<int>("SELECT @@IDENTITY");

                            if (faultSummaryID != 0)
                                connection.ExecuteNonQuery("INSERT INTO NearestStructure(FaultSummaryID, StructureID, Deviation) VALUES({0}, {1}, 0)", faultSummaryID, structure.ID);
                        }
                    }
                }
            }
        }

        private static string GetStructureInfo(string url)
        {
            bool HandlePreRequest(HttpWebRequest request)
            {
                request.UseDefaultCredentials = true;
                return true;
            }

            HtmlWeb webClient = new HtmlWeb();
            webClient.PreRequest += HandlePreRequest;
            HtmlDocument doc = webClient.Load(url);
            return doc.DocumentNode.InnerText.Trim();
        }

        private DataTable ToDataTable(string csvInput)
        {
            char[] newLineChars = new[] { '\r', '\n' };

            string[] lines = csvInput.Trim()
                .Split(newLineChars, StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.Trim())
                .Where(line => !string.IsNullOrEmpty(line))
                .ToArray();

            string[] fields = lines[0].Split(',');

            DataTable table = new DataTable();

            foreach (string field in fields)
                table.Columns.Add(field);

            foreach (string row in lines.Skip(1))
            {
                string[] values = row.Split(',');
                table.Rows.Add(values);
            }

            return table;
        }

        private double ToDouble(string csvValue) =>
            double.TryParse(csvValue, out double num)
                ? num
                : 0.0D;

        #endregion
    }
}
