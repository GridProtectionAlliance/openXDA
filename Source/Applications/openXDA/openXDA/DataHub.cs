//******************************************************************************************************
//  DataHub.cs - Gbtc
//
//  Copyright © 2016, Grid Protection Alliance.  All Rights Reserved.
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
//  06/01/2016 - Billy Ernest
//       Generated original version of source code.
//  08/18/2016 - J. Ritchie Carroll
//       Updated to use record operations hub base class.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using GSF;
using GSF.Data.Model;
using GSF.Identity;
using GSF.PhasorProtocols.BPAPDCstream;
using GSF.Security.Model;
using GSF.Web.Hubs;
using GSF.Web.Model;
using GSF.Web.Security;
using openXDA.Model;
using openHistorian.XDALink;


namespace openXDA
{
    [AuthorizeHubRole]
    public class DataHub : RecordOperationsHub<DataHub>
    {
        // Client-side script functionality

        #region [ Setting Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Setting), RecordOperation.QueryRecordCount)]
        public int QuerySettingCount(string filterString)
        {
            return DataContext.Table<Setting>().QueryRecordCount();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Setting), RecordOperation.QueryRecords)]
        public IEnumerable<Setting> QuerySettings(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            return DataContext.Table<Setting>().QueryRecords(sortField, ascending, page, pageSize);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Setting), RecordOperation.DeleteRecord)]
        public void DeleteSetting(int id)
        {
            DataContext.Table<Setting>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Setting), RecordOperation.CreateNewRecord)]
        public Setting NewSetting()
        {
            return new Setting();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Setting), RecordOperation.AddNewRecord)]
        public void AddNewSetting(Setting record)
        {
            DataContext.Table<Setting>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(Setting), RecordOperation.UpdateRecord)]
        public void UpdateSetting(Setting record)
        {
            DataContext.Table<Setting>().UpdateRecord(record);
        }

        #endregion

        #region [ DashSettings Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DashSettings), RecordOperation.QueryRecordCount)]
        public int QueryDashSettingsCount(string filterString)
        {
            return DataContext.Table<DashSettings>().QueryRecordCount();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DashSettings), RecordOperation.QueryRecords)]
        public IEnumerable<DashSettings> QueryDashSettingss(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            return DataContext.Table<DashSettings>().QueryRecords(sortField, ascending, page, pageSize);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DashSettings), RecordOperation.DeleteRecord)]
        public void DeleteDashSettings(int id)
        {
            DataContext.Table<DashSettings>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DashSettings), RecordOperation.CreateNewRecord)]
        public DashSettings NewDashSettings()
        {
            return new DashSettings();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DashSettings), RecordOperation.AddNewRecord)]
        public void AddNewDashSetting(DashSettings record)
        {
            DataContext.Table<DashSettings>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(DashSettings), RecordOperation.UpdateRecord)]
        public void UpdateDashSetting(DashSettings record)
        {
            DataContext.Table<DashSettings>().UpdateRecord(record);
        }

        #endregion

        #region [ Meter Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Meter), RecordOperation.QueryRecordCount)]
        public int QueryMeterCount(int meterLocationID, string filterString)
        {
            filterString = (filterString ?? "").Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]").Replace("*", "%") + "%";

            if (meterLocationID > 0)
                return DataContext.Table<Meter>().QueryRecordCount(new RecordRestriction("MeterLocationID = {0} AND Name LIKE {1}", meterLocationID, filterString));
            else
                return DataContext.Table<Meter>().QueryRecordCount(new RecordRestriction("Name LIKE {0}", filterString));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Meter), RecordOperation.QueryRecords)]
        public IEnumerable<MeterDetail> QueryMeters(int meterLocationID, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            filterString = (filterString ?? "").Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]").Replace("*", "%") + "%";

            if (meterLocationID > 0)
                return DataContext.Table<MeterDetail>().QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("MeterLocationID = {0} AND Name LIKE {1}", meterLocationID, filterString));
            else
                return DataContext.Table<MeterDetail>().QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("Name LIKE {0}", filterString));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Meter), RecordOperation.DeleteRecord)]
        public void DeleteMeter(int id)
        {
            DataContext.Table<Meter>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Meter), RecordOperation.CreateNewRecord)]
        public MeterDetail NewMeter()
        {
            return new MeterDetail();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Meter), RecordOperation.AddNewRecord)]
        public void AddNewMeter(Meter record)
        {
            DataContext.Table<Meter>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(Meter), RecordOperation.UpdateRecord)]
        public void UpdateMeter(Meter record)
        {
            DataContext.Table<Meter>().UpdateRecord(record);
        }

        [AuthorizeHubRole("Administrator")]
        public IEnumerable<IDLabel> SearchMeters(string searchText, int limit = -1)
        {
            RecordRestriction restriction = new RecordRestriction("Name LIKE {0}", $"%{searchText}%");

            return DataContext.Table<Meter>().QueryRecords("Name", restriction, limit)
                .Select(meter => new IDLabel(meter.ID.ToString(), meter.Name));
        }

        [AuthorizeHubRole("Administrator")]
        public IEnumerable<IDLabel> SearchMetersByGroup(int groupID, string searchText, int limit = -1)
        {
            RecordRestriction restriction = new RecordRestriction("Name LIKE {0} AND ID NOT IN (SELECT MeterID FROM GroupMeter WHERE GroupID = {1})", $"%{searchText}%", groupID);

            return DataContext.Table<Meter>().QueryRecords("Name", restriction, limit)
                .Select(meter => new IDLabel(meter.ID.ToString(), meter.Name));
        }

        #endregion

        #region [ MeterLocation Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterLocation), RecordOperation.QueryRecordCount)]
        public int QueryMeterLocationCount(string filterString)
        {
            filterString = (filterString ?? "").Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]").Replace("*", "%") + "%";
            return DataContext.Table<MeterLocation>().QueryRecordCount(new RecordRestriction("Name LIKE {0}", filterString));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterLocation), RecordOperation.QueryRecords)]
        public IEnumerable<MeterLocation> QueryMeterLocations(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            filterString = (filterString ?? "").Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]").Replace("*", "%") + "%";
            return DataContext.Table<MeterLocation>().QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("Name LIKE {0}", filterString));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterLocation), RecordOperation.DeleteRecord)]
        public void DeleteMeterLocation(int id)
        {
            DataContext.Table<MeterLocation>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterLocation), RecordOperation.CreateNewRecord)]
        public MeterLocation NewMeterLocation()
        {
            return new MeterLocation();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterLocation), RecordOperation.AddNewRecord)]
        public void AddNewMeterLocation(MeterLocation record)
        {
            DataContext.Table<MeterLocation>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(MeterLocation), RecordOperation.UpdateRecord)]
        public void UpdateMeterLocation(MeterLocation record)
        {
            DataContext.Table<MeterLocation>().UpdateRecord(record);
        }

        [AuthorizeHubRole("Administrator")]
        public IEnumerable<IDLabel> SearchMeterLocations(string searchText, int limit = -1)
        {
            RecordRestriction restriction = new RecordRestriction("Name LIKE {0}", $"%{searchText}%");

            return DataContext.Table<MeterLocation>().QueryRecords("Name", restriction, limit)
                .Select(meterLocation => new IDLabel(meterLocation.ID.ToString(), meterLocation.Name));
        }

        #endregion

        #region [ Lines Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Line), RecordOperation.QueryRecordCount)]
        public int QueryLinesCount(string filterString)
        {
            filterString = (filterString ?? "").Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]").Replace("*", "%") + "%";
            return DataContext.Table<Line>().QueryRecordCount(new RecordRestriction("Name LIKE {0}", filterString));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Line), RecordOperation.QueryRecords)]
        public IEnumerable<Line> QueryLines(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            filterString = (filterString ?? "").Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]").Replace("*", "%") + "%";
            return DataContext.Table<Line>().QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("AssetKey LIKE {0}", filterString));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Line), RecordOperation.DeleteRecord)]
        public void DeleteLines(int id)
        {
            DataContext.Table<Line>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Line), RecordOperation.CreateNewRecord)]
        public Line NewLine()
        {
            return new Line();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Line), RecordOperation.AddNewRecord)]
        public void AddNewLines(Line record)
        {
            DataContext.Table<Line>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(Line), RecordOperation.UpdateRecord)]
        public void UpdateLines(Line record)
        {
            DataContext.Table<Line>().UpdateRecord(record);
        }

        [AuthorizeHubRole("Administrator")]
        public IEnumerable<IDLabel> SearchLines(string searchText, int limit = -1)
        {
            RecordRestriction restriction = new RecordRestriction("AssetKey LIKE {0}", $"%{searchText}%");

            return DataContext.Table<Line>().QueryRecords("AssetKey", restriction, limit)
                .Select(line => new IDLabel(line.ID.ToString(), line.AssetKey));
        }

        #endregion

        #region [ LineView Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(LineView), RecordOperation.QueryRecordCount)]
        public int QueryLineViewCount(string filterString)
        {
            filterString = (filterString ?? "").Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]").Replace("*", "%")+ "%";
            return DataContext.Table<LineView>().QueryRecordCount(new RecordRestriction("TopName LIKE {0}", filterString));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(LineView), RecordOperation.QueryRecords)]
        public IEnumerable<LineView> QueryLineView(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            filterString = (filterString ?? "").Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]").Replace("*", "%") + "%";
            return DataContext.Table<LineView>().QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("AssetKey LIKE {0}", filterString));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(LineView), RecordOperation.DeleteRecord)]
        public void DeleteLineView(int id)
        {
            int index = DataContext.Connection.ExecuteScalar<int>("Select ID FROM LineImpedance WHERE LineID = {0}", id);
            DataContext.Table<LineImpedance>().DeleteRecord(index);
            DataContext.Table<Line>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(LineView), RecordOperation.CreateNewRecord)]
        public LineView NewLineView()
        {
            return new LineView();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(LineView), RecordOperation.AddNewRecord)]
        public void AddNewLineView(LineView record)
        {
            DataContext.Table<Line>().AddNewRecord(CreateLine(record));
            int index = DataContext.Connection.ExecuteScalar<int?>("SELECT IDENT_CURRENT('Line')") ?? 0;
            record.ID = index;
            DataContext.Table<LineImpedance>().AddNewRecord(CreateLineImpedance(record));
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(LineView), RecordOperation.UpdateRecord)]
        public void UpdateLineView(LineView record)
        {
            DataContext.Table<Line>().UpdateRecord(CreateLine(record));
            DataContext.Table<LineImpedance>().UpdateRecord(CreateLineImpedance(record));
        }

        public Line CreateLine(LineView record)
        {
            Line line = NewLine();
            line.AssetKey = record.AssetKey;
            line.Description = record.Description;
            line.Length = record.Length;
            line.ThermalRating = record.ThermalRating;
            line.VoltageKV = record.VoltageKV;
            return line;
        }

        public LineImpedance CreateLineImpedance(LineView record)
        {
            LineImpedance li = new LineImpedance();
            li.R0 = record.R0;
            li.R1 = record.R1;
            li.X0 = record.X0;
            li.X1 = record.X1;
            li.LineID = record.ID;
            return li;
        }

        #endregion

        #region [ MeterLine Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterLine), RecordOperation.QueryRecordCount)]
        public int QueryMeterLineCount(int lineID, int meterID, string filterString)
        {
            string restrictionString = "";
            if(lineID == -1 && meterID != -1)
            {
                restrictionString = $"MeterID = {meterID}";
            }
            else if (meterID == -1 && lineID != -1)
            {
                restrictionString = $"LineID = {lineID}";
            }
            else if(meterID != -1 && lineID != -1)
            {
                restrictionString = $"MeterID = {meterID} AND LineID = {lineID}";
            }

            return DataContext.Table<MeterLine>().QueryRecordCount(new RecordRestriction(restrictionString));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterLine), RecordOperation.QueryRecords)]
        public IEnumerable<MeterLineDetail> QueryMeterLine(int lineID, int meterID, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            string restrictionString = "";
            if (lineID == -1 && meterID != -1)
            {
                restrictionString = $"MeterID = {meterID}";
            }
            else if (meterID == -1 && lineID != -1)
            {
                restrictionString = $"LineID = {lineID}";
            }
            else if (meterID != -1 && lineID != -1)
            {
                restrictionString = $"MeterID = {meterID} AND LineID = {lineID}";
            }

            return DataContext.Table<MeterLineDetail>().QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction(restrictionString));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterLine), RecordOperation.DeleteRecord)]
        public void DeleteMeterLine(int id)
        {
            DataContext.Table<MeterLine>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterLine), RecordOperation.CreateNewRecord)]
        public MeterLineDetail NewMeterLine()
        {
            return new MeterLineDetail();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterLine), RecordOperation.AddNewRecord)]
        public void AddNewMeterLine(MeterLine record)
        {
            DataContext.Table<MeterLine>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(MeterLine), RecordOperation.UpdateRecord)]
        public void UpdateMeterLine(MeterLine record)
        {
            DataContext.Table<MeterLine>().UpdateRecord(record);
        }

        #endregion

        #region [ Channel Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Channel), RecordOperation.QueryRecordCount)]
        public int QueryChannelCount(int meterID, int lineID, string filterString)
        {
            var filters = (filterString ?? "").Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]").Replace("*", "%").ParseKeyValuePairs()
                .Select(kvp => new { Field = kvp.Key, Operator = "LIKE", Value = kvp.Value.TrimEnd('%') + "%" })
                .ToList();

            filters.Add(new { Field = "MeterID", Operator = "=", Value = meterID.ToString() });
            filters.Add(new { Field = "LineID", Operator = "=", Value = lineID.ToString() });

            string expression = string.Join(" AND ", filters.Select((filter, index) => $"{filter.Field} {filter.Operator} {{{index}}}"));
            object[] parameters = filters.Select(filter => (object)filter.Value).ToArray();

            return DataContext.Table<ChannelDetail>().QueryRecordCount(new RecordRestriction(expression, parameters));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Channel), RecordOperation.QueryRecords)]
        public IEnumerable<ChannelDetail> QueryChannel(int meterID, int lineID, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            var filters = (filterString ?? "").Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]").Replace("*", "%").ParseKeyValuePairs()
                .Select(kvp => new { Field = kvp.Key, Operator = "LIKE", Value = kvp.Value.TrimEnd('%') + "%" })
                .ToList();

            filters.Add(new { Field = "MeterID", Operator = "=", Value = meterID.ToString() });
            filters.Add(new { Field = "LineID", Operator = "=", Value = lineID.ToString() });

            string expression = string.Join(" AND ", filters.Select((filter, index) => $"{filter.Field} {filter.Operator} {{{index}}}"));
            object[] parameters = filters.Select(filter => (object)filter.Value).ToArray();

            return DataContext.Table<ChannelDetail>().QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction(expression, parameters));
        }

        public IEnumerable<Channel> QueryChannelsForDropDown(string filterString)
        {
            return DataContext.Table<Channel>().QueryRecords(restriction: new RecordRestriction("Name LIKE {0}", filterString), limit: 50);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Channel), RecordOperation.DeleteRecord)]
        public void DeleteChannel(int id)
        {
            DataContext.Table<Channel>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Channel), RecordOperation.CreateNewRecord)]
        public ChannelDetail NewChannel()
        {
            return new ChannelDetail();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(Channel), RecordOperation.AddNewRecord)]
        public void AddNewChannel(ChannelDetail record)
        {
            DataContext.Table<Channel>().AddNewRecord(record);
            record.ID = DataContext.Connection.ExecuteScalar<int>("SELECT @@IDENTITY");

            if (!string.IsNullOrEmpty(record.Mapping))
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    bool seriesTypeExists = DataContext.Connection.ExecuteScalar<int>("SELECT COUNT(*) FROM SeriesType WHERE Name = 'Values'") > 0;

                    if (!seriesTypeExists)
                        DataContext.Connection.ExecuteNonQuery("INSERT INTO SeriesType VALUES('Values', 'Values')");

                    transaction.Complete();
                }

                int seriesTypeID = DataContext.Connection.ExecuteScalar<int>("SELECT ID FROM SeriesType WHERE Name = 'Values'");
                DataContext.Connection.ExecuteNonQuery("INSERT INTO Series VALUES({0}, {1}, {2})", record.ID, seriesTypeID, record.Mapping);
            }
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(Channel), RecordOperation.UpdateRecord)]
        public void UpdateChannel(ChannelDetail record)
        {
            DataContext.Table<Channel>().UpdateRecord(record);

            if (!string.IsNullOrEmpty(record.Mapping))
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    bool seriesTypeExists = DataContext.Connection.ExecuteScalar<int>("SELECT COUNT(*) FROM SeriesType WHERE Name = 'Values'") > 0;

                    if (!seriesTypeExists)
                        DataContext.Connection.ExecuteNonQuery("INSERT INTO SeriesType VALUES('Values', 'Values')");

                    transaction.Complete();
                }

                int seriesTypeID = DataContext.Connection.ExecuteScalar<int>("SELECT ID FROM SeriesType WHERE Name = 'Values'");

                using (TransactionScope transaction = new TransactionScope())
                {
                    bool seriesExists = DataContext.Connection.ExecuteScalar<int>("SELECT COUNT(*) FROM Series WHERE ChannelID = {0} AND SeriesTypeID = {1}", record.ID, seriesTypeID) > 0;

                    if (seriesExists)
                        DataContext.Connection.ExecuteNonQuery("UPDATE Series SET SourceIndexes = {0} WHERE ChannelID = {1} AND SeriesTypeID = {2}", record.Mapping, record.ID, seriesTypeID);
                    else
                        DataContext.Connection.ExecuteNonQuery("INSERT INTO Series VALUES({0}, {1}, {2})", record.ID, seriesTypeID, record.Mapping);

                    transaction.Complete();
                }
            }
            else
            {
                int seriesTypeID = DataContext.Connection.ExecuteScalar(defaultValue: -1, sqlFormat: "SELECT ID FROM SeriesType WHERE Name = 'Values'");
                DataContext.Connection.ExecuteNonQuery("UPDATE Series SET SourceIndexes = '' WHERE ChannelID = {0} AND SeriesTypeID = {1}", record.ID, seriesTypeID);
            }
        }

        [AuthorizeHubRole("Administrator")]
        public IEnumerable<IDLabel> SearchMeasurementTypes(string searchText, int limit = -1)
        {
            string limitText = (limit > 0) ? $"TOP {limit}" : "";

            return DataContext.Connection
                .RetrieveData($"SELECT {limitText} ID, Name FROM MeasurementType WHERE Name LIKE {{0}}", $"%{searchText}%")
                .Select()
                .Select(row => new IDLabel(row["ID"].ToString(), row["Name"].ToString()));
        }

        [AuthorizeHubRole("Administrator")]
        public IEnumerable<IDLabel> SearchMeasurementCharacteristics(string searchText, int limit = -1)
        {
            string limitText = (limit > 0) ? $"TOP {limit}" : "";

            return DataContext.Connection
                .RetrieveData($"SELECT {limitText} ID, Name FROM MeasurementCharacteristic WHERE Name LIKE {{0}}", $"%{searchText}%")
                .Select()
                .Select(row => new IDLabel(row["ID"].ToString(), row["Name"].ToString()));
        }

        [AuthorizeHubRole("Administrator")]
        public IEnumerable<IDLabel> SearchPhases(string searchText, int limit = -1)
        {
            string limitText = (limit > 0) ? $"TOP {limit}" : "";

            return DataContext.Connection
                .RetrieveData($"SELECT {limitText} ID, Name FROM Phase WHERE Name LIKE {{0}}", $"%{searchText}%")
                .Select()
                .Select(row => new IDLabel(row["ID"].ToString(), row["Name"].ToString()));
        }

        #endregion

        #region [ MeterGroup Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterGroup), RecordOperation.QueryRecordCount)]
        public int QueryGroupCount(string filterString)
        {
            filterString = (filterString ?? "%").EnsureEnd("%");
            return DataContext.Table<MeterGroup>().QueryRecordCount(new RecordRestriction("ID LIKE {0}", filterString));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterGroup), RecordOperation.QueryRecords)]
        public IEnumerable<MeterGroup> QueryGroups(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            filterString = (filterString ?? "%").EnsureEnd("%");
            return DataContext.Table<MeterGroup>().QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction($"ID LIKE '{filterString}'"));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterGroup), RecordOperation.DeleteRecord)]
        public void DeleteGroup(int id)
        {
            IEnumerable<MeterMeterGroup> table = DataContext.Table<MeterMeterGroup>().QueryRecords(restriction: new RecordRestriction("MeterGroupID = {0}", id));
            foreach (MeterMeterGroup gm in table)
            {
                DataContext.Table<MeterMeterGroup>().DeleteRecord(gm.ID);
            }

            IEnumerable<UserAccountMeterGroup> users = DataContext.Table<UserAccountMeterGroup>().QueryRecords(restriction: new RecordRestriction("MeterGroupID = {0}", id));
            foreach (UserAccountMeterGroup gm in users)
            {
                DataContext.Table<UserAccountMeterGroup>().DeleteRecord(gm.ID);
            }

            DataContext.Table<MeterGroup>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterGroup), RecordOperation.CreateNewRecord)]
        public MeterGroup NewGroup()
        {
            return new MeterGroup();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterGroup), RecordOperation.AddNewRecord)]
        public void AddNewGroup(MeterGroup record)
        {
            DataContext.Table<MeterGroup>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(MeterGroup), RecordOperation.UpdateRecord)]
        public void UpdateGroup(MeterGroup record)
        {
            DataContext.Table<MeterGroup>().UpdateRecord(record);
        }

        public int GetLastMeterGroupID()
        {
            return DataContext.Connection.ExecuteScalar<int?>("SELECT IDENT_CURRENT('MeterGroup')") ?? 0;
        }


        public void UpdateMeters(List<string> meters, int groupID )
        {
            IEnumerable<MeterMeterGroup> records = DataContext.Table<MeterMeterGroup>().QueryRecords(restriction: new RecordRestriction("MeterGroupID = {0}", groupID));
            foreach (MeterMeterGroup record in records)
            {
                if (!meters.Contains(record.MeterID.ToString()))
                    DataContext.Table<MeterMeterGroup>().DeleteRecord(record.ID);
            }

            foreach (string meter in meters)
            {
                if (!records.Any(record => record.MeterID == int.Parse(meter)))
                {
                    DataContext.Table<MeterMeterGroup>().AddNewRecord(new MeterMeterGroup() { MeterGroupID = groupID, MeterID = int.Parse(meter) });
                }
            }
        }

        public void UpdateUsers(List<string> users, int groupID)
        {
            IEnumerable<UserAccountMeterGroup> records = DataContext.Table<UserAccountMeterGroup>().QueryRecords(restriction: new RecordRestriction("MeterGroupID = {0}", groupID));
            foreach (UserAccountMeterGroup record in records)
            {
                if (!users.Contains(record.UserAccountID.ToString()))
                    DataContext.Table<UserAccountMeterGroup>().DeleteRecord(record.ID);
            }

            foreach (string user in users)
            {
                if (!records.Any(record => record.UserAccountID == Guid.Parse(user)))
                {
                    DataContext.Table<UserAccountMeterGroup>().AddNewRecord(new UserAccountMeterGroup() { MeterGroupID = groupID, UserAccountID = Guid.Parse(user) });
                }
            }
        }


        #endregion

        #region [ MeterMeterGroup Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterMeterGroup), RecordOperation.QueryRecordCount)]
        public int QueryGroupMeterViewCount(int groupID, string filterString)
        {
            return DataContext.Table<MeterMeterGroupView>().QueryRecordCount(new RecordRestriction("MeterGroupID = {0}", groupID));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterMeterGroup), RecordOperation.QueryRecords)]
        public IEnumerable<MeterMeterGroupView> QueryGroupMeterViews(int groupID, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            return DataContext.Table<MeterMeterGroupView>().QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("MeterGroupID = {0}", groupID));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterMeterGroup), RecordOperation.DeleteRecord)]
        public void DeleteGroupMeterView(int id)
        {
            DataContext.Table<MeterMeterGroup>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterMeterGroup), RecordOperation.CreateNewRecord)]
        public MeterMeterGroupView NewGroupMeterView()
        {
            return new MeterMeterGroupView();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterMeterGroup), RecordOperation.AddNewRecord)]
        public void AddNewGroupMeterView(MeterMeterGroup record)
        {
            DataContext.Table<MeterMeterGroup>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(MeterMeterGroup), RecordOperation.UpdateRecord)]
        public void UpdateGroupMeterView(MeterMeterGroup record)
        {
            DataContext.Table<MeterMeterGroup>().UpdateRecord(record);
        }

        #endregion

        #region [ UserAccountMeterGroup Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(UserAccountMeterGroup), RecordOperation.QueryRecordCount)]
        public int QueryUserAccountMeterGroupCount(int groupID, string filterString)
        {
            return DataContext.Table<UserAccountMeterGroupView>().QueryRecordCount(new RecordRestriction("MeterGroupID = {0}", groupID));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(UserAccountMeterGroup), RecordOperation.QueryRecords)]
        public IEnumerable<UserAccountMeterGroupView> QueryUserAccountMeterGroups(int groupID, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            return DataContext.Table<UserAccountMeterGroupView>().QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("MeterGroupID = {0}", groupID));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(UserAccountMeterGroupView), RecordOperation.DeleteRecord)]
        public void DeleteUserAccountMeterGroup(int id)
        {
            DataContext.Table<UserAccountMeterGroup>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(UserAccountMeterGroup), RecordOperation.CreateNewRecord)]
        public UserAccountMeterGroupView NewUserAccountMeterGroup()
        {
            return new UserAccountMeterGroupView();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(UserAccountMeterGroup), RecordOperation.AddNewRecord)]
        public void AddNewUserAccountMeterGroup(UserAccountMeterGroup record)
        {
            DataContext.Table<UserAccountMeterGroup>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(UserAccountMeterGroup), RecordOperation.UpdateRecord)]
        public void UpdateUserAccountMeterGroup(UserAccountMeterGroup record)
        {
            DataContext.Table<UserAccountMeterGroup>().UpdateRecord(record);
        }

        [AuthorizeHubRole("Administrator")]
        public IEnumerable<IDLabel> SearchUsersByGroup(int groupID, string searchText, int limit = -1)
        {
            RecordRestriction restriction = new RecordRestriction("ID NOT IN (SELECT UserAccountID FROM UserAccountMeterGroup WHERE MeterGroupID = {0})",  groupID);


            if (limit < 1)
                return DataContext
                    .Table<UserAccount>()
                    .QueryRecords(restriction: restriction)
                    .Select(record =>
                    {
                        record.Name = UserInfo.SIDToAccountName(record.Name ?? "");
                        return record;
                    })
                    .Where(record => record.Name?.ToLower().Contains(searchText.ToLower()) ?? false)
                    .Select(record => IDLabel.Create(record.ID.ToString(), record.Name));

            return DataContext
                .Table<UserAccount>()
                .QueryRecords(restriction: restriction)
                .Select(record =>
                {
                    record.Name = UserInfo.SIDToAccountName(record.Name ?? "");
                    return record;
                })
                .Where(record => record.Name?.ToLower().Contains(searchText.ToLower()) ?? false)
                .Take(limit)
                .Select(record => IDLabel.Create(record.ID.ToString(), record.Name));

        }

        #endregion

        #region [ User Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(User), RecordOperation.QueryRecordCount)]
        public int QueryUserCount(string filterString)
        {
            filterString = (filterString ?? "").TrimEnd('%').Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]").Replace("*", "%") + "%";
            return DataContext.Table<User>().QueryRecordCount(new RecordRestriction("Name LIKE {0}", filterString));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(User), RecordOperation.QueryRecords)]
        public IEnumerable<User> QueryUsers(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            filterString = (filterString ?? "").TrimEnd('%').Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]").Replace("*", "%") + "%";
            return DataContext.Table<User>().QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("Name LIKE {0}", filterString));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(User), RecordOperation.DeleteRecord)]
        public void DeleteUser(int id)
        {
            DataContext.Table<User>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(User), RecordOperation.CreateNewRecord)]
        public User NewUser()
        {
            return new User();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(User), RecordOperation.AddNewRecord)]
        public void AddNewUser(User record)
        {
            DataContext.Table<User>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(User), RecordOperation.UpdateRecord)]
        public void UpdateUser(User record)
        {
            DataContext.Table<User>().UpdateRecord(record);
        }

        public IEnumerable<User> GetUsers()
        {
            return DataContext.Table<User>().QueryRecords(restriction: new RecordRestriction("Active <> 0"));
        }
        #endregion

        #region [ AlarmRangeLimitView Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AlarmRangeLimitView), RecordOperation.QueryRecordCount)]
        public int QueryAlarmRangeLimitViewCount(int meterID, int lineID, string filterString)
        {
            string channelFilter = "%";
            string typeFilter = "%";
            string charFilter = "%";

            if (filterString != "%")
            {
                string[] filters = filterString.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]").Replace("*", "%").Split(';');
                if (filters.Length == 3)
                {
                    channelFilter = filters[0] + '%';
                    typeFilter = filters[1] += '%';
                    charFilter = filters[2] += '%';
                }
            }

            string MeterID = (meterID == -1 ? "%" : meterID.ToString());
            string LineID = (lineID == -1 ? "%" : lineID.ToString());

            return DataContext.Table<AlarmRangeLimitView>().QueryRecordCount(new RecordRestriction("Name LIKE {0} AND MeasurementType LIKE {1} AND MeasurementCharacteristic LIKE {2} AND MeterID = {3} AND LineID = {4}", channelFilter, typeFilter, charFilter, MeterID, LineID));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AlarmRangeLimitView), RecordOperation.QueryRecords)]
        public IEnumerable<AlarmRangeLimitView> QueryAlarmRangeLimitViews(int meterID, int lineID, string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            string channelFilter = "%";
            string typeFilter = "%";
            string charFilter = "%";

            if (filterString != "%")
            {
                string[] filters = filterString.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]").Replace("*", "%").Split(';');
                if (filters.Length == 3)
                {
                    channelFilter = filters[0] + '%';
                    typeFilter = filters[1] += '%';
                    charFilter = filters[2] += '%';
                }
            }


            return DataContext.Table<AlarmRangeLimitView>().QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("Name LIKE {0} AND MeasurementType LIKE {1} AND MeasurementCharacteristic LIKE {2} AND MeterID = {3} AND LineID = {4}", channelFilter, typeFilter, charFilter, meterID, lineID));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AlarmRangeLimitView), RecordOperation.DeleteRecord)]
        public void DeleteAlarmRangeLimitView(int id)
        {
            DataContext.Table<AlarmRangeLimit>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AlarmRangeLimitView), RecordOperation.CreateNewRecord)]
        public AlarmRangeLimitView NewAlarmRangeLimitView()
        {
            return new AlarmRangeLimitView();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AlarmRangeLimitView), RecordOperation.AddNewRecord)]
        public void AddNewAlarmRangeLimitView(AlarmRangeLimitView record)
        {
            if (record.IsDefault)
            {
                IEnumerable<DefaultAlarmRangeLimit> defaultLimits = DataContext.Table<DefaultAlarmRangeLimit>().QueryRecords(restriction: new RecordRestriction("MeasurementTypeID = {0} AND MeasurementCharacteristicID = {1}", record.MeasurementTypeID, record.MeasurementCharacteristicID));

                if (defaultLimits.Any())
                {
                    record.Severity = defaultLimits.First().Severity;
                    record.High = defaultLimits.First().High;
                    record.Low = defaultLimits.First().Low;
                    record.RangeInclusive = defaultLimits.First().RangeInclusive;
                    record.PerUnit = defaultLimits.First().PerUnit;
                }
            }

            DataContext.Table<AlarmRangeLimit>().AddNewRecord(CreateNewAlarmRangeLimit(record));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(AlarmRangeLimitView), RecordOperation.UpdateRecord)]
        public void UpdateAlarmRangeLimitView(AlarmRangeLimitView record)
        {
            if (record.IsDefault)
            {
                IEnumerable<DefaultAlarmRangeLimit> defaultLimits = DataContext.Table<DefaultAlarmRangeLimit>().QueryRecords(restriction: new RecordRestriction("MeasurementTypeID = {0} AND MeasurementCharacteristicID = {1}", record.MeasurementTypeID, record.MeasurementCharacteristicID));

                if (defaultLimits.Any())
                {
                    record.Severity = defaultLimits.First().Severity;
                    record.High = defaultLimits.First().High;
                    record.Low = defaultLimits.First().Low;
                    record.RangeInclusive = defaultLimits.First().RangeInclusive;
                    record.PerUnit = defaultLimits.First().PerUnit;
                }
            }

            DataContext.Table<AlarmRangeLimit>().UpdateRecord(CreateNewAlarmRangeLimit(record));
        }

        public AlarmRangeLimit CreateNewAlarmRangeLimit(AlarmRangeLimitView record)
        {
            AlarmRangeLimit arl = new AlarmRangeLimit();
            arl.ID = record.ID;
            arl.ChannelID = record.ChannelID;
            arl.AlarmTypeID = record.AlarmTypeID;
            arl.Enabled = record.Enabled;

            arl.Severity = record.Severity;
            arl.High = record.High;
            arl.Low = record.Low;
            arl.RangeInclusive = record.RangeInclusive;
            arl.PerUnit = record.PerUnit;
            arl.IsDefault = record.IsDefault;

            return arl;
        }

        [AuthorizeHubRole("Administrator")]
        public void ResetAlarmToDefault(AlarmRangeLimitView record)
        {
            IEnumerable<DefaultAlarmRangeLimit> defaultLimits = DataContext.Table<DefaultAlarmRangeLimit>().QueryRecords(restriction: new RecordRestriction("MeasurementTypeID = {0} AND MeasurementCharacteristicID = {1}", record.MeasurementTypeID, record.MeasurementCharacteristicID));

            if(defaultLimits.Any())
            {
                record.Severity = defaultLimits.First().Severity;
                record.High = defaultLimits.First().High;
                record.Low = defaultLimits.First().Low;
                record.RangeInclusive = defaultLimits.First().RangeInclusive;
                record.PerUnit = defaultLimits.First().PerUnit;
                record.IsDefault = true;

                DataContext.Table<AlarmRangeLimit>().UpdateRecord(CreateNewAlarmRangeLimit(record));
            }
        }

        [AuthorizeHubRole("Administrator")]
        public string SendAlarmTableToCSV()
        {
            string csv = "";
            string[] headers = DataContext.Table<AlarmRangeLimitView>().GetFieldNames();

            foreach (string h in headers)
            {
                if (csv == "")
                    csv = '[' + h + ']';
                else
                    csv += ",[" + h + ']';
            }

            csv += "\n";
             
            IEnumerable<AlarmRangeLimitView> limits = DataContext.Table<AlarmRangeLimitView>().QueryRecords();

            foreach (AlarmRangeLimitView limit in limits)
            {
                csv += limit.csvString() + '\n';
                //csv += limit.ID.ToString() + ',' + limit.ChannelID.ToString() + ',' + limit.Name.ToString() + ',' + limit.AlarmTypeID.ToString() + ',' + limit.Severity.ToString() + ',' + limit.High.ToString() + ',' 
                //    + limit.Low.ToString() + ',' + limit.RangeInclusive.ToString() + ',' + limit.PerUnit.ToString() + ',' + limit.Enabled.ToString() + ',' + limit.MeasurementType.ToString() + ','
                //    + limit.MeasurementTypeID.ToString() + ',' + limit.MeasurementCharacteristic.ToString() + ',' + limit.MeasurementCharacteristicID.ToString() + ',' + limit.Phase.ToString() + ','
                //    + limit.PhaseID.ToString() + ',' + limit.HarmonicGroup.ToString() + ','  + limit.IsDefault.ToString() + "\n";
            }

            return csv;
        }

        public void ImportAlarmTableCSV(string csv)
        {
            string[] csvRows = csv.Split('\n');
            string[] tableFields = csvRows[0].Split(',');

            TableOperations<AlarmRangeLimit> table = DataContext.Table<AlarmRangeLimit>();

            if (table.GetFieldNames() == tableFields)
            {
                for (int i = 1; i < csvRows.Length; ++i)
                {
                    string[] row = csvRows[i].Split(',');

                    AlarmRangeLimit newRecord = DataContext.Connection.ExecuteScalar<AlarmRangeLimit>("Select * FROM AlarmRangeLimit WHERE ID ={0}", row[0]);

                    newRecord.Severity = int.Parse(row[4]);
                    newRecord.High = float.Parse(row[5]);
                    newRecord.Low = float.Parse(row[6]);
                    newRecord.RangeInclusive = int.Parse(row[7]);
                    newRecord.PerUnit = int.Parse(row[8]);
                    newRecord.Enabled = int.Parse(row[9]);
                    newRecord.IsDefault = bool.Parse(row[17]);

                    table.UpdateRecord(newRecord);
                }
            }
        }

        #endregion

        #region [ DefaultAlarmRangeLimit Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DefaultAlarmRangeLimitView), RecordOperation.QueryRecordCount)]
        public int QueryDefaultAlarmRangeLimitViewCount(string filterString)
        {
            return DataContext.Table<DefaultAlarmRangeLimitView>().QueryRecordCount();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DefaultAlarmRangeLimitView), RecordOperation.QueryRecords)]
        public IEnumerable<DefaultAlarmRangeLimitView> QueryDefaultAlarmRangeLimitViews(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            return DataContext.Table<DefaultAlarmRangeLimitView>().QueryRecords(sortField, ascending, page, pageSize);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DefaultAlarmRangeLimitView), RecordOperation.DeleteRecord)]
        public void DeleteDefaultAlarmRangeLimitView(int id)
        {
            DataContext.Table<AlarmRangeLimit>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DefaultAlarmRangeLimitView), RecordOperation.CreateNewRecord)]
        public DefaultAlarmRangeLimitView NewDefaultAlarmRangeLimitView()
        {
            return new DefaultAlarmRangeLimitView();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DefaultAlarmRangeLimitView), RecordOperation.AddNewRecord)]
        public void AddNewDefaultAlarmRangeLimitView(DefaultAlarmRangeLimitView record)
        {
            DataContext.Table<DefaultAlarmRangeLimit>().AddNewRecord(CreateNewAlarmRangeLimit(record));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(DefaultAlarmRangeLimitView), RecordOperation.UpdateRecord)]
        public void UpdateDefaultAlarmRangeLimitView(DefaultAlarmRangeLimitView record)
        {
            DataContext.Table<DefaultAlarmRangeLimit>().UpdateRecord(CreateNewAlarmRangeLimit(record));
        }

        public DefaultAlarmRangeLimit CreateNewAlarmRangeLimit(DefaultAlarmRangeLimitView record)
        {
            DefaultAlarmRangeLimit arl = new DefaultAlarmRangeLimit();
            arl.ID = record.ID;
            arl.AlarmTypeID = record.AlarmTypeID;
            arl.MeasurementTypeID = record.MeasurementTypeID;
            arl.MeasurementCharacteristicID = record.MeasurementCharacteristicID;
            arl.Severity = record.Severity;
            arl.High = record.High;
            arl.Low = record.Low;
            arl.RangeInclusive = record.RangeInclusive;
            arl.PerUnit = record.PerUnit;
            return arl;
        }


        [AuthorizeHubRole("Administrator")]
        public void ResetDefaultLimits(DefaultAlarmRangeLimitView record)
        {
            IEnumerable<Channel> channels = DataContext.Table<Channel>().QueryRecords(restriction: new RecordRestriction("MeasurementTypeID = {0} AND MeasurementCharacteristicID = {1}", record.MeasurementTypeID, record.MeasurementCharacteristicID));
            string channelIDs = "";
            foreach (Channel channel in channels)
            {
                if(channelIDs == "")
                    channelIDs += channel.ID.ToString();
                else
                    channelIDs += ',' + channel.ID.ToString();
            }
            IEnumerable<AlarmRangeLimit> limits = DataContext.Table<AlarmRangeLimit>().QueryRecords(restriction: new RecordRestriction($"ChannelID IN ({channelIDs})"));
            foreach (AlarmRangeLimit limit in limits)
            {
                limit.IsDefault = true;
                limit.High = record.High;
                limit.Low = record.Low;
                limit.Severity = record.Severity;
                limit.RangeInclusive = record.RangeInclusive;
                limit.PerUnit = record.PerUnit;
                DataContext.Table<AlarmRangeLimit>().UpdateRecord(limit);
            }
        }

        #endregion

        #region [ EmailType Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailType), RecordOperation.QueryRecordCount)]
        public int QueryEmailTypeCount(string filterString)
        {
            filterString = (filterString ?? "").EnsureEnd("%");
            return DataContext.Table<EmailType>().QueryRecordCount(new RecordRestriction("ID LIKE {0}", filterString));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailType), RecordOperation.QueryRecords)]
        public IEnumerable<EmailType> QueryEmailType(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            filterString = (filterString ?? "").EnsureEnd("%");
            return DataContext.Table<EmailType>().QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("ID LIKE {0}", filterString));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailType), RecordOperation.DeleteRecord)]
        public void DeleteEmailType(int id)
        {
            DataContext.Table<EmailType>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailType), RecordOperation.CreateNewRecord)]
        public EmailType NewEmailType()
        {
            return new EmailType();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailType), RecordOperation.AddNewRecord)]
        public void AddNewEmailType(EmailType record)
        {
            DataContext.Table<EmailType>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(EmailType), RecordOperation.UpdateRecord)]
        public void UpdateEmailType(EmailType record)
        {
            DataContext.Table<EmailType>().UpdateRecord(record);
        }

        #endregion

        #region [ EmailGroup Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailGroup), RecordOperation.QueryRecordCount)]
        public int QueryEmailGroupCount(string filterString)
        {
            filterString = (filterString ?? "").EnsureEnd("%");
            return DataContext.Table<EmailGroup>().QueryRecordCount(new RecordRestriction("ID LIKE {0}", filterString));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailGroup), RecordOperation.QueryRecords)]
        public IEnumerable<EmailGroup> QueryEmailGroup(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            filterString = (filterString ?? "").EnsureEnd("%");
            return DataContext.Table<EmailGroup>().QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("ID LIKE {0}", filterString));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailGroup), RecordOperation.DeleteRecord)]
        public void DeleteEmailGroup(int id)
        {
            DataContext.Table<EmailGroup>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailGroup), RecordOperation.CreateNewRecord)]
        public EmailGroup NewEmailGroup()
        {
            return new EmailGroup();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailGroup), RecordOperation.AddNewRecord)]
        public void AddNewEmailGroup(EmailGroup record)
        {
            DataContext.Table<EmailGroup>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(EmailGroup), RecordOperation.UpdateRecord)]
        public void UpdateEmailGroup(EmailGroup record)
        {
            DataContext.Table<EmailGroup>().UpdateRecord(record);
        }


        #endregion

        #region [ EmailGroupMeterGroup Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailGroupMeterGroup), RecordOperation.QueryRecordCount)]
        public int QueryEmailGroupMeterGroupCount(string filterString)
        {
            string emailGroupFilter = "";
            string meterGroupFilter = "";

            if (filterString != "")
            {
                string[] filters = filterString.Split(';');
                if (filters.Length == 2)
                {
                    emailGroupFilter = filters[0] + '%';
                    meterGroupFilter = filters[1] + '%';
                }
            }


            return DataContext.Table<EmailGroupMeterGroup>().QueryRecordCount(new RecordRestriction("EmailGroupID LIKE {0} AND MeterGroupID LIKE {1}", emailGroupFilter, meterGroupFilter));
        }

        public int QueryEmailGroupMeterGroupCount(int EmailGroupID, string filterString)
        {
            return DataContext.Table<EmailGroupMeterGroup>().QueryRecordCount(new RecordRestriction("EmailGroupID = {0}", EmailGroupID));
        }


        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailGroupMeterGroup), RecordOperation.QueryRecords)]
        public IEnumerable<EmailGroupMeterGroup> QueryEmailGroupMeterGroup(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            string emailGroupFilter = "%";
            string meterGroupFilter = "%";

            if (filterString != "%")
            {
                string[] filters = filterString.Split(';');
                if (filters.Length == 2)
                {
                    emailGroupFilter = filters[0] + '%';
                    meterGroupFilter = filters[1] + '%';
                }
            }

            return DataContext.Table<EmailGroupMeterGroup>().QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("EmailGroupID LIKE {0} AND MeterGroupID LIKE {1}", emailGroupFilter, meterGroupFilter));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailGroupMeterGroup), RecordOperation.DeleteRecord)]
        public void DeleteEmailGroupMeterGroup(int id)
        {
            DataContext.Table<EmailGroupMeterGroup>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailGroupMeterGroup), RecordOperation.CreateNewRecord)]
        public EmailGroupMeterGroup NewEmailGroupMeterGroup()
        {
            return new EmailGroupMeterGroup();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailGroupMeterGroup), RecordOperation.AddNewRecord)]
        public void AddNewEmailGroupMeterGroup(EmailGroupMeterGroup record)
        {
            DataContext.Table<EmailGroupMeterGroup>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(EmailGroupMeterGroup), RecordOperation.UpdateRecord)]
        public void UpdateEmailGroupMeterGroup(EmailGroupMeterGroup record)
        {
            DataContext.Table<EmailGroupMeterGroup>().UpdateRecord(record);
        }


        #endregion

        #region [ EmailGroupUserAccount Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailGroupUserAccount), RecordOperation.QueryRecordCount)]
        public int QueryEmailGroupUserAccountCount(string filterString)
        {
            string emailGroupFilter = "";
            string userAccountFilter = "";

            if (filterString != "")
            {
                string[] filters = filterString.Split(';');
                if (filters.Length == 2)
                {
                    emailGroupFilter = filters[0] + '%';
                    userAccountFilter = filters[1] + '%';
                }
            }


            return DataContext.Table<EmailGroupUserAccount>().QueryRecordCount(new RecordRestriction("EmailGroupID LIKE {0} AND UserAccountID LIKE {1}", emailGroupFilter, userAccountFilter));
        }

        public int QueryEmailGroupUserAccountCount(int EmailGroupID, string filterString)
        {
            return DataContext.Table<EmailGroupUserAccount>().QueryRecordCount(new RecordRestriction("EmailGroupID = {0}", EmailGroupID));
        }


        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailGroupUserAccount), RecordOperation.QueryRecords)]
        public IEnumerable<EmailGroupUserAccount> QueryEmailGroupUserAccount(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            string emailGroupFilter = "%";
            string userAccountFilter = "%";

            if (filterString != "%")
            {
                string[] filters = filterString.Split(';');
                if (filters.Length == 2)
                {
                    emailGroupFilter = filters[0] + '%';
                    userAccountFilter = filters[1] + '%';
                }
            }

            return DataContext.Table<EmailGroupUserAccount>().QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("EmailGroupID LIKE {0} AND UserAccountID LIKE {1}", emailGroupFilter, userAccountFilter));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailGroupUserAccount), RecordOperation.DeleteRecord)]
        public void DeleteEmailGroupUserAccount(int id)
        {
            DataContext.Table<EmailGroupUserAccount>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailGroupUserAccount), RecordOperation.CreateNewRecord)]
        public EmailGroupUserAccount NewEmailGroupUserAccount()
        {
            return new EmailGroupUserAccount();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailGroupUserAccount), RecordOperation.AddNewRecord)]
        public void AddNewEmailGroupUserAccount(EmailGroupUserAccount record)
        {
            DataContext.Table<EmailGroupUserAccount>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(EmailGroupUserAccount), RecordOperation.UpdateRecord)]
        public void UpdateEmailGroupUserAccount(EmailGroupUserAccount record)
        {
            DataContext.Table<EmailGroupUserAccount>().UpdateRecord(record);
        }


        #endregion

        #region [ EmailGroupType Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailGroupType), RecordOperation.QueryRecordCount)]
        public int QueryEmailGroupTypeCount(string filterString)
        {
            filterString = (filterString ?? "").EnsureEnd("%");
            return DataContext.Table<EmailGroupType>().QueryRecordCount(new RecordRestriction("EmailGroupID LIKE {0}", filterString));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailGroupType), RecordOperation.QueryRecords)]
        public IEnumerable<EmailGroupType> QueryEmailGroupType(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            filterString = (filterString ?? "").EnsureEnd("%");
            return DataContext.Table<EmailGroupType>().QueryRecords(sortField, ascending, page, pageSize, new RecordRestriction("EmailGroupID LIKE {0}", filterString));
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailGroupType), RecordOperation.DeleteRecord)]
        public void DeleteEmailGroupType(int id)
        {
            DataContext.Table<EmailGroupType>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailGroupType), RecordOperation.CreateNewRecord)]
        public EmailGroupType NewEmailGroupType()
        {
            return new EmailGroupType();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(EmailGroupType), RecordOperation.AddNewRecord)]
        public void AddNewEmailGroupType(EmailGroupType record)
        {
            DataContext.Table<EmailGroupType>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(EmailGroupType), RecordOperation.UpdateRecord)]
        public void UpdateEmailGroupType(EmailGroupType record)
        {
            DataContext.Table<EmailGroupType>().UpdateRecord(record);
        }


        #endregion

        #region [ XSLTemplate Table Operations ]

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(XSLTemplate), RecordOperation.QueryRecordCount)]
        public int QueryXSLTemplateCount(string filterString)
        {
            filterString = (filterString ?? "").Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]").Replace("*", "%").EnsureEnd("%");
            return DataContext.Table<XSLTemplate>().QueryRecordCount();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(XSLTemplate), RecordOperation.QueryRecords)]
        public IEnumerable<XSLTemplate> QueryXSLTemplate(string sortField, bool ascending, int page, int pageSize, string filterString)
        {
            filterString = (filterString ?? "").Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]").Replace("*", "%").EnsureEnd("%");
            return DataContext.Table<XSLTemplate>().QueryRecords(sortField, ascending, page, pageSize);
        }

        public XSLTemplate QueryXSLTemplateByID(int id)
        {
            return DataContext.Table<XSLTemplate>().QueryRecords(restriction: new RecordRestriction("ID = {0}", id)).First();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(XSLTemplate), RecordOperation.DeleteRecord)]
        public void DeleteXSLTemplate(int id)
        {
            DataContext.Table<XSLTemplate>().DeleteRecord(id);
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(XSLTemplate), RecordOperation.CreateNewRecord)]
        public XSLTemplate NewXSLTemplate()
        {
            return new XSLTemplate();
        }

        [AuthorizeHubRole("Administrator")]
        [RecordOperation(typeof(XSLTemplate), RecordOperation.AddNewRecord)]
        public void AddNewXSLTemplate(XSLTemplate record)
        {
            DataContext.Table<XSLTemplate>().AddNewRecord(record);
        }

        [AuthorizeHubRole("Administrator, Owner")]
        [RecordOperation(typeof(XSLTemplate), RecordOperation.UpdateRecord)]
        public void UpdateXSLTemplate(XSLTemplate record)
        {
            DataContext.Table<XSLTemplate>().UpdateRecord(record);
        }


        #endregion

        #region [Event Criterion]

        public class EventCriterion
        {
            public bool five;
            public bool four;
            public bool three;
            public bool two;
            public bool one;
            public bool zero;
            public bool disturbances;
            public bool fault;
        }

        public EventCriterion GetEventCriterion(int EmailGroupID)
        {
            EventCriterion ec = new EventCriterion();

            ec.five = DataContext.Connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 5", EmailGroupID) != null;
            ec.four = DataContext.Connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 4", EmailGroupID) != null;
            ec.three = DataContext.Connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 3", EmailGroupID) != null;
            ec.two = DataContext.Connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 2", EmailGroupID) != null;
            ec.one = DataContext.Connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 1", EmailGroupID) != null;
            ec.zero = DataContext.Connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 0", EmailGroupID) != null;
            ec.fault = DataContext.Connection.ExecuteScalar("SELECT ID FROM FaultEmailCriterion WHERE EmailGroupID = {0}", EmailGroupID) != null;
            ec.disturbances = ec.five || ec.four || ec.three || ec.two || ec.one || ec.zero;


            return ec;
        }

        public void UpdateEventCriterion(EventCriterion record, int EmailGroupID)
        {
            if (record.fault && DataContext.Connection.ExecuteScalar("SELECT ID FROM FaultEmailCriterion WHERE EmailGroupID = {0}", EmailGroupID) == null)
                DataContext.Table<FaultEmailCriterion>().AddNewRecord(new FaultEmailCriterion() { EmailGroupID = EmailGroupID });
            else if(!record.fault && DataContext.Connection.ExecuteScalar("SELECT ID FROM FaultEmailCriterion WHERE EmailGroupID = {0}", EmailGroupID) != null)
            {
                int index = DataContext.Connection.ExecuteScalar<int>("SELECT ID FROM FaultEmailCriterion WHERE EmailGroupID ={0}", EmailGroupID);
                DataContext.Table<FaultEmailCriterion>().DeleteRecord(index);
            }

            if (record.five && DataContext.Connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 5", EmailGroupID) == null)
                DataContext.Table<DisturbanceEmailCriterion>().AddNewRecord(new DisturbanceEmailCriterion() { EmailGroupID = EmailGroupID, SeverityCode = 5});
            else if (!record.five && DataContext.Connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 5", EmailGroupID) != null)
            {
                int index = DataContext.Connection.ExecuteScalar<int>("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 5", EmailGroupID);
                DataContext.Table<DisturbanceEmailCriterion>().DeleteRecord(index);
            }

            if (record.four && DataContext.Connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 4", EmailGroupID) == null)
                DataContext.Table<DisturbanceEmailCriterion>().AddNewRecord(new DisturbanceEmailCriterion() { EmailGroupID = EmailGroupID, SeverityCode = 4 });
            else if (!record.five && DataContext.Connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 4", EmailGroupID) != null)
            {
                int index = DataContext.Connection.ExecuteScalar<int>("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 4", EmailGroupID);
                DataContext.Table<DisturbanceEmailCriterion>().DeleteRecord(index);
            }

            if (record.three && DataContext.Connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 3", EmailGroupID) == null)
                DataContext.Table<DisturbanceEmailCriterion>().AddNewRecord(new DisturbanceEmailCriterion() { EmailGroupID = EmailGroupID, SeverityCode = 3 });
            else if (!record.five && DataContext.Connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 3", EmailGroupID) != null)
            {
                int index = DataContext.Connection.ExecuteScalar<int>("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 3", EmailGroupID);
                DataContext.Table<DisturbanceEmailCriterion>().DeleteRecord(index);
            }

            if (record.two && DataContext.Connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 2", EmailGroupID) == null)
                DataContext.Table<DisturbanceEmailCriterion>().AddNewRecord(new DisturbanceEmailCriterion() { EmailGroupID = EmailGroupID, SeverityCode = 2 });
            else if (!record.five && DataContext.Connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 2", EmailGroupID) != null)
            {
                int index = DataContext.Connection.ExecuteScalar<int>("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 2", EmailGroupID);
                DataContext.Table<DisturbanceEmailCriterion>().DeleteRecord(index);
            }

            if (record.one && DataContext.Connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 1", EmailGroupID) == null)
                DataContext.Table<DisturbanceEmailCriterion>().AddNewRecord(new DisturbanceEmailCriterion() { EmailGroupID = EmailGroupID, SeverityCode = 1 });
            else if (!record.five && DataContext.Connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 1", EmailGroupID) != null)
            {
                int index = DataContext.Connection.ExecuteScalar<int>("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 1", EmailGroupID);
                DataContext.Table<DisturbanceEmailCriterion>().DeleteRecord(index);
            }

            if (record.zero && DataContext.Connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 0", EmailGroupID) == null)
                DataContext.Table<DisturbanceEmailCriterion>().AddNewRecord(new DisturbanceEmailCriterion() { EmailGroupID = EmailGroupID, SeverityCode = 0 });
            else if (!record.five && DataContext.Connection.ExecuteScalar("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 0", EmailGroupID) != null)
            {
                int index = DataContext.Connection.ExecuteScalar<int>("SELECT ID FROM DisturbanceEmailCriterion WHERE EmailGroupID = {0} AND SeverityCode = 0", EmailGroupID);
                DataContext.Table<DisturbanceEmailCriterion>().DeleteRecord(index);
            }

        }

        #endregion

        #region [ Stored Procedure Operations ]

        public List<TrendingData> GetTrendingData(DateTime startDate, DateTime endDate, int channelId)
        {
            List<TrendingData> trendingData = new List<TrendingData>();
            //using (IDbCommand cmd = DataContext.Connection.Connection.CreateCommand())
            //{
            //    cmd.CommandText = "dbo.selectAlarmDataByChannelByDate";
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.CommandTimeout = 30000;
            //    IDbDataParameter StartDate = cmd.CreateParameter();
            //    IDbDataParameter EndDate = cmd.CreateParameter();
            //    IDbDataParameter ChannelID = cmd.CreateParameter();

            //    StartDate.ParameterName = "@StartDate";
            //    EndDate.ParameterName = "@EndDate";
            //    ChannelID.ParameterName = "@ChannelID";

            //    StartDate.Value = startDate;
            //    EndDate.Value = endDate;
            //    ChannelID.Value = channelId;

            //    cmd.Parameters.Add(StartDate);
            //    cmd.Parameters.Add(EndDate);
            //    cmd.Parameters.Add(ChannelID);

            //    using( IDataReader reader = cmd.ExecuteReader())
            //    {

            //        while (reader.Read())
            //        {
            //            TrendingData obj = new TrendingData();

            //            obj.ChannelID = Convert.ToInt32(reader["ChannelID"]);
            //            obj.SeriesType = Convert.ToString(reader["SeriesType"]);
            //            obj.Time = Convert.ToDateTime(reader["Time"]);
            //            obj.Value = Convert.ToDouble(reader["Value"]);
            //            trendingData.Add(obj);
            //        }

            //    }
            //}

            string historianServer = DataContext.Connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Server'") ?? "127.0.0.1";
            string historianInstance = DataContext.Connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Instance'") ?? "XDA";
            IEnumerable<int> channelIDs = new List<int> { channelId };

            
            using (Historian historian = new Historian(historianServer, historianInstance))
            {
                foreach (TrendingDataPoint point in historian.Read(channelIDs, startDate, endDate))
                {
                    TrendingData obj = new TrendingData();

                    obj.ChannelID = point.ChannelID;
                    obj.SeriesType = point.SeriesID.ToString();
                    obj.Time = point.Timestamp;
                    obj.Value = point.Value;
                    trendingData.Add(obj);
                }
            }


            return trendingData;
           
        }

        #endregion

        #region [ Misc ]

        public IEnumerable<IDLabel> SearchTimeZones(string searchText , int limit)
        {
            IReadOnlyCollection<TimeZoneInfo> tzi = TimeZoneInfo.GetSystemTimeZones();

            return tzi
                .Select(row => new IDLabel(row.Id, row.ToString()))
                .Where(row => row.label.ToLower().Contains(searchText.ToLower()));
        }

        #endregion
    }
}