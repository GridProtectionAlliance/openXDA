//******************************************************************************************************
//  SmartFawgOperation.cs - Gbtc
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
//  10/18/2019 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using FaultData.DataAnalysis;
using FaultData.DataResources;
using FaultData.DataSets;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using openXDA.Model;

namespace FaultData.DataOperations.TVA
{
    public class SmartFawgOperation : DataOperationBase<MeterDataSet>
    {
        public class SmartFawgSettings
        {
            [Setting]
            [DefaultValue("Data Source=server; Initial Catalog=smartFAWG; Integrated Security=SSPI")]
            public string ConnectionString { get; set; }

            [Setting]
            [DefaultValue("Fawg One")]
            public string Branch { get; set; }
        }

        [Setting]
        [SettingName("XDATimeZone")]
        public string XDATimeZoneID
        {
            get
            {
                return XDATimeZone.Id;
            }
            set
            {
                XDATimeZone = TimeZoneInfo.FindSystemTimeZoneById(value);
            }
        }

        [Category]
        [SettingName("SmartFawg")]
        public SmartFawgSettings Settings { get; }

        private TimeZoneInfo XDATimeZone { get; set; }

        public override void Execute(MeterDataSet meterDataSet)
        {
            if (meterDataSet.LoadHistoricConfiguration)
                return;

            const string QueryResource = "FaultData.DataOperations.TVA.SmartFawg.sql";
            Assembly faultData = Assembly.GetExecutingAssembly();
            string query;

            using (Stream queryStream = faultData.GetManifestResourceStream(QueryResource))
            using (TextReader reader = new StreamReader(queryStream))
            {
                query = reader.ReadToEnd();
            }

            DataGroupsResource dataGroupsResource = meterDataSet.GetResource<DataGroupsResource>();
            DateTime xdaNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, XDATimeZone);
            DateTime thisMonth = new DateTime(xdaNow.Year, xdaNow.Month, xdaNow.Day);
            string monthString = thisMonth.ToString("yyyyMM");
            string branch = Settings.Branch;

            using (AdoDataConnection xdaConnection = meterDataSet.CreateDbConnection())
            using (AdoDataConnection smartFawgConnection = new AdoDataConnection(Settings.ConnectionString, typeof(SqlConnection), typeof(SqlDataAdapter)))
            {
                foreach (DataGroup dataGroup in dataGroupsResource.DataGroups)
                {
                    if (dataGroup.Line == null)
                        continue;

                    DateTime updatedOn = xdaConnection.ExecuteScalar<DateTime>("SELECT UpdatedOn FROM Line WHERE ID = {0}", dataGroup.Line.ID);

                    if (updatedOn >= thisMonth)
                        continue;

                    string transLineNumber = dataGroup.Line.AssetKey;

                    if (transLineNumber.StartsWith("L"))
                        transLineNumber = transLineNumber.Substring(1);

                    using (DataTable table = smartFawgConnection.RetrieveData(query, transLineNumber, branch, monthString))
                    {
                        if (table.Rows.Count == 0)
                            continue;

                        DataRow row = table.Rows[0];
                        double lineLength = row.ConvertField<double>("LengthMiles");
                        double r0 = row.ConvertField<double>("ZeroSeqResistance");
                        double x0 = row.ConvertField<double>("ZeroSeqReactance");
                        double r1 = row.ConvertField<double>("PosSeqResistance");
                        double x1 = row.ConvertField<double>("PosSeqReactance");

                        if (lineLength != dataGroup.Line.Length)
                        {
                            TableOperations<Line> lineTable = new TableOperations<Line>(xdaConnection);
                            dataGroup.Line.Length = lineLength;
                            lineTable.UpdateRecord(dataGroup.Line);
                        }

                        LineImpedance lineImpedance = dataGroup.Line.LineImpedance;

                        bool impedanceChanged =
                            lineImpedance == null ||
                            r0 != lineImpedance.R0 ||
                            x0 != lineImpedance.X0 ||
                            r1 != lineImpedance.R1 ||
                            x1 != lineImpedance.X1;

                        if (lineImpedance == null)
                            lineImpedance = new LineImpedance() { LineID = dataGroup.Line.ID };

                        if (impedanceChanged)
                        {
                            TableOperations<LineImpedance> lineImpedanceTable = new TableOperations<LineImpedance>(xdaConnection);
                            dataGroup.Line.LineImpedance.R0 = r0;
                            dataGroup.Line.LineImpedance.X0 = x0;
                            dataGroup.Line.LineImpedance.R1 = r1;
                            dataGroup.Line.LineImpedance.X1 = x1;
                            lineImpedanceTable.UpdateRecord(dataGroup.Line.LineImpedance);
                        }
                    }
                }
            }
        }
    }
}
