//******************************************************************************************************
//  JSONApiController.cs - Gbtc
//
//  Copyright © 2017, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  06/08/2017 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web.Http;
using FaultData.DataAnalysis;
using GSF.Data;
using GSF.Data.Model;
using openXDA.Model;
using Disturbance = openXDA.Model.Disturbance;
using Fault = openXDA.Model.Fault;

namespace openXDA.Adapters
{
    public class JSONApiController : ApiController
    {
        #region [ Constructors ]

        public JSONApiController(Func<AdoDataConnection> connectionFactory) =>
            ConnectionFactory = connectionFactory;

        #endregion

        #region [ Properties ]

        private Func<AdoDataConnection> ConnectionFactory { get; }

        #endregion

        #region [ Query Classes ]

        public class ConfigJSON
        {
            public string AssetKey { get; set; }
            public string ID { get; set; } 
            public string Name { get; set; }
        }

        public class EventJSON
        {
            public string MeterIDList { get; set; }
            public string MeterAssetKeyList { get; set; }
            public string EventIDList { get; set; }
            public string AssetIDList { get; set; }
            public string AssetKeyList { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
        }

        public class EventDataJSON
        {
            public string EventID { get; set; }
        }

        #endregion

        #region [ Config Calls ]

        [HttpPost]
        public IEnumerable<Meter> GetMeters(ConfigJSON json)
        {
            string assetKey = json?.AssetKey ?? "%";
            string name = json?.Name ?? "%";

            int id = int.TryParse(json?.ID, out int jsonID)
                ? jsonID
                : -1;

            string idFilter = (id != -1)
                ? "ID = {1}"
                : "1=1";

            string queryFormat =
                $"SELECT * " +
                $"FROM Meter " +
                $"WHERE " +
                $"    AssetKey LIKE {{0}} AND " +
                $"    {idFilter} AND " +
                $"    Name LIKE {{2}}";

            using (AdoDataConnection connection = ConnectionFactory())
            using (DataTable result = connection.RetrieveData(Timeout.Infinite, queryFormat, assetKey, id, name))
            {
                TableOperations<Meter> meterTable = new TableOperations<Meter>(connection);

                return result
                    .AsEnumerable()
                    .Select(meterTable.LoadRecord)
                    .ToList();
            }
        }

        [HttpPost]
        public IEnumerable<Line> GetLines(ConfigJSON json)
        {
            string assetKey = json?.AssetKey ?? "%";

            int id = int.TryParse(json?.ID, out int jsonID)
                ? jsonID
                : -1;

            string idFilter = (id != -1)
                ? "ID = {1}"
                : "1=1";

            string queryFormat =
                $"SELECT * " +
                $"FROM Line " +
                $"WHERE " +
                $"    AssetKey LIKE {{0}} AND " +
                $"    {idFilter}";

            using (AdoDataConnection connection = ConnectionFactory())
            using (DataTable result = connection.RetrieveData(Timeout.Infinite, queryFormat, assetKey, id))
            {
                TableOperations<Line> lineTable = new TableOperations<Line>(connection);

                return result
                    .AsEnumerable()
                    .Select(lineTable.LoadRecord)
                    .ToList();
            }
        }

        [HttpPost]
        public IEnumerable<Location> GetStations(ConfigJSON json)
        {
            string assetKey = json?.AssetKey ?? "%";
            string name = json?.Name ?? "%";

            int id = int.TryParse(json?.ID, out int jsonID)
                ? jsonID
                : -1;

            string idFilter = (id != -1)
                ? "ID = {1}"
                : "1=1";

            string queryFormat =
                $"SELECT * " +
                $"FROM Location " +
                $"WHERE " +
                $"    LocationKey LIKE {{0}} AND " +
                $"    {idFilter} AND " +
                $"    Name LIKE {{2}}";

            using (AdoDataConnection connection = ConnectionFactory())
            using (DataTable result = connection.RetrieveData(Timeout.Infinite, queryFormat, assetKey, id, name))
            {
                TableOperations<Location> locationTable = new TableOperations<Location>(connection);

                return result
                    .AsEnumerable()
                    .Select(locationTable.LoadRecord)
                    .ToList();
            }
        }

        [HttpPost]
        public IEnumerable<Channel> GetChannelsByMeter(ConfigJSON json)
        {
            string assetKey = json?.AssetKey ?? "%";
            string name = json?.Name ?? "%";

            int id = int.TryParse(json?.ID, out int jsonID)
                ? jsonID
                : -1;

            string idFilter = (id != -1)
                ? "ID = {1}"
                : "1=1";

            string queryFormat =
                $"SELECT * " +
                $"FROM Channel " +
                $"WHERE MeterID IN " +
                $"( " +
                $"    SELECT ID " +
                $"    FROM Meter " +
                $"    WHERE " +
                $"        AssetKey LIKE {{0}} AND " +
                $"        {idFilter} AND " +
                $"        Name LIKE {{2}} " +
                $")";

            using (AdoDataConnection connection = ConnectionFactory())
            using (DataTable result = connection.RetrieveData(Timeout.Infinite, queryFormat, assetKey, id, name))
            {
                TableOperations<Channel> channelTable = new TableOperations<Channel>(connection);

                return result
                    .AsEnumerable()
                    .Select(channelTable.LoadRecord)
                    .ToList();
            }
        }

        #endregion

        #region [ Event Calls ]

        [HttpPost]
        public IEnumerable<Event> GetEvents(EventJSON json)
        {
            RecordRestriction ToRecordRestriction(string filterFormat, string itemList)
            {
                object[] parameters = itemList
                    .Split(',')
                    .Select(item => int.TryParse(item, out int id) ? id : (object)item)
                    .ToArray();

                string paramList = string.Join(",", parameters.Select((_, index) => $"{{{index}}}"));
                string filter = string.Format(filterFormat, paramList);
                return new RecordRestriction(filter, parameters);
            }

            DateTime startTime = json?.StartDate ?? DateTime.MinValue;
            DateTime endTime = json?.EndDate ?? DateTime.Now;

            RecordRestriction restriction =
                new RecordRestriction("StartTime >= {0}", startTime) &
                new RecordRestriction("EndTime <= {0}", endTime);

            if (json.MeterIDList != null)
                restriction &= ToRecordRestriction("MeterID IN ({0})", json.MeterIDList);

            if (json.MeterAssetKeyList != null)
                restriction &= ToRecordRestriction("MeterID IN (SELECT ID FROM Meter WHERE AssetKey IN ({0}))", json.MeterAssetKeyList);

            if (json.AssetIDList != null)
                restriction &= ToRecordRestriction("AssetID IN ({0})", json.AssetIDList);

            if (json.AssetKeyList != null)
                restriction &= ToRecordRestriction("AssetID IN (SELECT ID FROM Asset WHERE AssetKey IN ({0}))", json.AssetKeyList);

            if (json.EventIDList != null)
                restriction &= ToRecordRestriction("ID IN ({0})", json.EventIDList);

            string queryFormat = $"SELECT * FROM Event WHERE {restriction.FilterExpression}";

            using (AdoDataConnection connection = ConnectionFactory())
            using (DataTable result = connection.RetrieveData(Timeout.Infinite, queryFormat, restriction.Parameters))
            {
                TableOperations<Event> eventTable = new TableOperations<Event>(connection);

                return result
                    .AsEnumerable()
                    .Select(eventTable.LoadRecord)
                    .ToList();
            }
        }

        [HttpPost]
        public IEnumerable<Fault> GetFaults(EventJSON json)
        {
            RecordRestriction ToRecordRestriction(string filterFormat, string itemList)
            {
                object[] parameters = itemList
                    .Split(',')
                    .Select(item => int.TryParse(item, out int id) ? id : (object)item)
                    .ToArray();

                string paramList = string.Join(",", parameters.Select((_, index) => $"{{{index}}}"));
                string filter = string.Format(filterFormat, paramList);
                return new RecordRestriction(filter, parameters);
            }

            DateTime startTime = json?.StartDate ?? DateTime.MinValue;
            DateTime endTime = json?.EndDate ?? DateTime.Now;

            RecordRestriction restriction =
                new RecordRestriction("Inception >= {0}", startTime) &
                new RecordRestriction("Inception <= {0}", endTime);

            if (json.MeterIDList != null)
                restriction &= ToRecordRestriction("EventID IN (SELECT ID FROM Event WHERE MeterID IN ({0}))", json.MeterIDList);

            if (json.MeterAssetKeyList != null)
                restriction &= ToRecordRestriction("EventID IN (SELECT ID FROM Event WHERE MeterID IN (SELECT ID FROM Meter WHERE AssetKey IN ({0})))", json.MeterAssetKeyList);

            if (json.AssetIDList != null)
                restriction &= ToRecordRestriction("EventID IN (SELECT ID FROM Event WHERE AssetID IN ({0}))", json.AssetIDList);

            if (json.AssetKeyList != null)
                restriction &= ToRecordRestriction("EventID IN (SELECT ID FROM Event WHERE AssetID IN (SELECT ID FROM Asset WHERE AssetKey IN ({0})))", json.AssetKeyList);

            if (json.EventIDList != null)
                restriction &= ToRecordRestriction("EventID IN ({0})", json.EventIDList);

            string queryFormat = $"SELECT * FROM FaultSummary WHERE {restriction.FilterExpression}";

            using (AdoDataConnection connection = ConnectionFactory())
            using (DataTable result = connection.RetrieveData(Timeout.Infinite, queryFormat, restriction.Parameters))
            {
                TableOperations<Fault> faultTable = new TableOperations<Fault>(connection);

                return result
                    .AsEnumerable()
                    .Select(faultTable.LoadRecord)
                    .ToList();
            }
        }

        [HttpPost]
        public IEnumerable<Disturbance> GetDisturbances(EventJSON json)
        {
            RecordRestriction ToRecordRestriction(string filterFormat, string itemList)
            {
                object[] parameters = itemList
                    .Split(',')
                    .Select(item => int.TryParse(item, out int id) ? id : (object)item)
                    .ToArray();

                string paramList = string.Join(",", parameters.Select((_, index) => $"{{{index}}}"));
                string filter = string.Format(filterFormat, paramList);
                return new RecordRestriction(filter, parameters);
            }

            DateTime startTime = json?.StartDate ?? DateTime.MinValue;
            DateTime endTime = json?.EndDate ?? DateTime.Now;

            RecordRestriction restriction =
                new RecordRestriction("StartTime >= {0}", startTime) &
                new RecordRestriction("EndTime <= {0}", endTime);

            if (json.MeterIDList != null)
                restriction &= ToRecordRestriction("EventID IN (SELECT ID FROM Event WHERE MeterID IN ({0}))", json.MeterIDList);

            if (json.MeterAssetKeyList != null)
                restriction &= ToRecordRestriction("EventID IN (SELECT ID FROM Event WHERE MeterID IN (SELECT ID FROM Meter WHERE AssetKey IN ({0})))", json.MeterAssetKeyList);

            if (json.AssetIDList != null)
                restriction &= ToRecordRestriction("EventID IN (SELECT ID FROM Event WHERE AssetID IN ({0}))", json.AssetIDList);

            if (json.AssetKeyList != null)
                restriction &= ToRecordRestriction("EventID IN (SELECT ID FROM Event WHERE AssetID IN (SELECT ID FROM Asset WHERE AssetKey IN ({0})))", json.AssetKeyList);

            if (json.EventIDList != null)
                restriction &= ToRecordRestriction("EventID IN ({0})", json.EventIDList);

            string queryFormat = $"SELECT * FROM Disturbance WHERE {restriction.FilterExpression}";

            using (AdoDataConnection connection = ConnectionFactory())
            using (DataTable result = connection.RetrieveData(Timeout.Infinite, queryFormat, restriction.Parameters))
            {
                TableOperations<Disturbance> disturbanceTable = new TableOperations<Disturbance>(connection);

                return result
                    .AsEnumerable()
                    .Select(disturbanceTable.LoadRecord)
                    .ToList();
            }
        }

        [HttpPost]
        public IEnumerable<BreakerOperation> GetBreakerOperations(EventJSON json)
        {
            RecordRestriction ToRecordRestriction(string filterFormat, string itemList)
            {
                object[] parameters = itemList
                    .Split(',')
                    .Select(item => int.TryParse(item, out int id) ? id : (object)item)
                    .ToArray();

                string paramList = string.Join(",", parameters.Select((_, index) => $"{{{index}}}"));
                string filter = string.Format(filterFormat, paramList);
                return new RecordRestriction(filter, parameters);
            }

            DateTime startTime = json?.StartDate ?? DateTime.MinValue;
            DateTime endTime = json?.EndDate ?? DateTime.Now;

            RecordRestriction restriction =
                new RecordRestriction("TripCoilEnergized >= {0}", startTime) &
                new RecordRestriction("TripCoilEnergized <= {0}", endTime);

            if (json.MeterIDList != null)
                restriction &= ToRecordRestriction("EventID IN (SELECT ID FROM Event WHERE MeterID IN ({0}))", json.MeterIDList);

            if (json.MeterAssetKeyList != null)
                restriction &= ToRecordRestriction("EventID IN (SELECT ID FROM Event WHERE MeterID IN (SELECT ID FROM Meter WHERE AssetKey IN ({0})))", json.MeterAssetKeyList);

            if (json.AssetIDList != null)
                restriction &= ToRecordRestriction("EventID IN (SELECT ID FROM Event WHERE AssetID IN ({0}))", json.AssetIDList);

            if (json.AssetKeyList != null)
                restriction &= ToRecordRestriction("EventID IN (SELECT ID FROM Event WHERE AssetID IN (SELECT ID FROM Asset WHERE AssetKey IN ({0})))", json.AssetKeyList);

            if (json.EventIDList != null)
                restriction &= ToRecordRestriction("EventID IN ({0})", json.EventIDList);

            string queryFormat = $"SELECT * FROM BreakerOperation WHERE {restriction.FilterExpression}";

            using (AdoDataConnection connection = ConnectionFactory())
            using (DataTable result = connection.RetrieveData(Timeout.Infinite, queryFormat, restriction.Parameters))
            {
                TableOperations<BreakerOperation> breakerOperationTable = new TableOperations<BreakerOperation>(connection);

                return result
                    .AsEnumerable()
                    .Select(breakerOperationTable.LoadRecord)
                    .ToList();
            }
        }

        #endregion

        #region [ Event Data Calls ]

        [HttpPost]
        public IHttpActionResult GetEventWaveformData(EventDataJSON json)
        {
            if(json != null && json.EventID != null)
            {
                try
                {
                    int eventID = int.Parse(json.EventID);
                    using (AdoDataConnection connection = ConnectionFactory())
                    {
                        Event evt = new TableOperations<Event>(connection).QueryRecordWhere("ID = {0}", eventID);
                        Meter meter = new TableOperations<Meter>(connection).QueryRecordWhere("ID = {0}", evt.MeterID);
                        meter.ConnectionFactory = ConnectionFactory;
                        List<byte[]> frequencyDomainData = ChannelData.DataFromEvent(eventID, ConnectionFactory);
                        
                        DataGroup dataGroup = new DataGroup();
                        dataGroup.FromData(meter, frequencyDomainData);
                        VIDataGroup vIDataGroup = new VIDataGroup(dataGroup);
                        dataGroup = vIDataGroup.ToDataGroup();
                        DataTable table = new DataTable();

                        table.Columns.Add("Timestamp", typeof(DateTime));
                        foreach(var series in dataGroup.DataSeries)
                            table.Columns.Add(series.SeriesInfo.Channel.MeasurementType.Name + "(" + series.SeriesInfo.Channel.Phase.Name + ")", typeof(double));
                        
                        for(int i = 0; i < dataGroup.DataSeries[0].DataPoints.Count(); ++i)
                        {
                            DataRow row = table.NewRow();
                            row["Timestamp"] = dataGroup.DataSeries[0].DataPoints[i].Time;
                            for (int j = 1; j < table.Columns.Count; ++j)
                            {
                                row[table.Columns[j].ColumnName] = dataGroup.DataSeries[j-1].DataPoints[i].Value;
                            }

                            table.Rows.Add(row);

                        }
                        return Ok(table);
                    }
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }
            else
                return BadRequest("Please provide event id");
        }

        [HttpPost]
        public IEnumerable<EventData> GetEventFrequencyDomainData(EventDataJSON json)
        {
            if (json != null && json.EventID != null)
            {
                try
                {
                    const string QueryFormat =
                        "SELECT * " +
                        "FROM " +
                        "    GetEventData({0}) AS GottenEventData JOIN " +
                        "    Series ON GottenEventData.SeriesID = Series.ID JOIN " +
                        "    Channel ON Series.ChannelID = Channel.ID " +
                        "WHERE Characteristic <> 'Instantaneous'";

                    int eventID = int.Parse(json.EventID);

                    using (AdoDataConnection connection = ConnectionFactory())
                    using (DataTable result = connection.RetrieveData(Timeout.Infinite, QueryFormat, eventID))
                    {
                        TableOperations<EventData> eventDataTable = new TableOperations<EventData>(connection);

                        return result
                            .AsEnumerable()
                            .Select(eventDataTable.LoadRecord)
                            .ToList();
                    }
                }
                catch
                {
                    return null;
                }
            }
            else
                return null;
        }

        #endregion
    }
}