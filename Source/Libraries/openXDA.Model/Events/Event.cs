//******************************************************************************************************
//  Event.cs - Gbtc
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
//  08/29/2017 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using GSF.Data;
using GSF.Data.Model;
using Newtonsoft.Json;

namespace openXDA.Model
{
    [TableName("Event")]
    public class Event
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        public int FileGroupID { get; set; }

        public int MeterID { get; set; }

        public int AssetID { get; set; }

        public int EventTypeID { get; set; }

        public int? EventDataID { get; set; }

        public string Name { get; set; }

        public string Alias { get; set; }

        public string ShortName { get; set; }

        [FieldDataType(DbType.DateTime2, DatabaseType.SQLServer)]
        public DateTime StartTime { get; set; }

        [FieldDataType(DbType.DateTime2, DatabaseType.SQLServer)]
        public DateTime EndTime { get; set; }

        public int Samples { get; set; }

        public int TimeZoneOffset { get; set; }

        public int SamplesPerSecond { get; set; }

        public int SamplesPerCycle { get; set; }

        public string Description { get; set; }

        public string UpdatedBy { get; set; }

        [JsonIgnore]
        [NonRecordField]
        public List<ChannelData> EventData { get; set; }

        [JsonIgnore]
        [NonRecordField]
        public List<Disturbance> Disturbances { get; set; } = new List<Disturbance>();
    }

    [TableName("EventView")]
    public class EventForDate : EventView { }

    [TableName("EventView")]
    public class EventForDay : EventView { }

    [TableName("EventView")]
    public class EventForMeter : EventView { }

    [TableName("EventView")]
    public class SingleEvent : EventView { }

    [TableName("EventView")]
    public class EventView : Event
    {
        [Searchable(SearchType.LikeExpression)]
        [PrimaryKey(true)]
        public new int ID
        {
            get => base.ID;
            set => base.ID = value;
        }

        [Searchable]
        public string AssetName { get; set; }

        [Searchable]
        public string MeterName { get; set; }

        public string StationName { get; set; }

        [Searchable]
        public string EventTypeName { get; set; }
    }

    public class MeterEventsByLine
    {
        public int thelineid { get; set; }

        public int theeventid { get; set; }

        public string theeventtype { get; set; }

        public DateTime theinceptiontime { get; set; }

        public string thelinename { get; set; }

        public int voltage { get; set; }

        public string thefaulttype { get; set; }

        public string thecurrentdistance { get; set; }

        public bool pqiexists { get; set; }
    }

    public static partial class TableOperationsExtensions
    {
        public static List<Event> GetSystemEvent(this TableOperations<Event> eventTable, DateTime startTime, DateTime endTime, double timeTolerance)
        {
            AdoDataConnection connection = eventTable.Connection;

            using (IDbCommand command = connection.Connection.CreateCommand())
            {
                command.CommandText = "GetSystemEvent";
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = connection.DefaultTimeout;
                AddParameter(command, "startTime", startTime, DbType.DateTime2);
                AddParameter(command, "endTime", endTime, DbType.DateTime2);
                AddParameter(command, "timeTolerance", timeTolerance, DbType.Double);

                IDataAdapter adapter = (IDataAdapter)Activator.CreateInstance(connection.AdapterType, command);

                using (adapter as IDisposable)
                {
                    DataSet dataSet = new DataSet();

                    adapter.Fill(dataSet);

                    return dataSet.Tables[0].Rows
                        .Cast<DataRow>()
                        .Select(row => eventTable.LoadRecord(row))
                        .ToList();
                }
            }
        }

        public static List<Event> GetLineEvent(this TableOperations<Event> eventTable, int assetID, DateTime startTime, DateTime endTime, double timeTolerance)
        {
            AdoDataConnection connection = eventTable.Connection;

            using (IDbCommand command = connection.Connection.CreateCommand())
            {
                command.CommandText = "GetLineEvent";
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = connection.DefaultTimeout;
                AddParameter(command, "assetID", assetID, DbType.Int32);
                AddParameter(command, "startTime", startTime, DbType.DateTime2);
                AddParameter(command, "endTime", endTime, DbType.DateTime2);
                AddParameter(command, "timeTolerance", timeTolerance, DbType.Double);

                IDataAdapter adapter = (IDataAdapter)Activator.CreateInstance(connection.AdapterType, command);

                using (adapter as IDisposable)
                {
                    DataSet dataSet = new DataSet();

                    adapter.Fill(dataSet);

                    return dataSet.Tables[0].Rows
                        .Cast<DataRow>()
                        .Select(row => eventTable.LoadRecord(row))
                        .ToList();
                }
            }
        }

        private static void AddParameter(IDbCommand command, string name, object value, DbType dbType)
        {
            IDbDataParameter parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;
            parameter.DbType = dbType;
            command.Parameters.Add(parameter);
        }
    }
}