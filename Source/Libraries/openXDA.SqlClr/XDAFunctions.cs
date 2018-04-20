//******************************************************************************************************
//  XDAFunctions.cs - Gbtc
//
//  Copyright © 2016, Grid Protection Alliance.  All Rights Reserved.
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
//  09/01/2016 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Microsoft.SqlServer.Server;

namespace openXDA.SqlClr
{
    public class XDAFunctions
    {
        private class DataPoint
        {
            public int SeriesID;
            public string Characteristic;
            public DateTime Time;
            public double Value;
        }

        private class FaultDataPoint
        {
            public string Algorithm;
            public DateTime Time;
            public double Value;
        }

        [SqlFunction(
            DataAccess = DataAccessKind.Read,
            FillRowMethodName = "GetEventData_FillRow",
            TableDefinition = "[SeriesID] INT, [Characteristic] NVARCHAR(200), [Time] DATETIME2, [Value] FLOAT")
        ]
        public static IEnumerable GetEventData(SqlInt32 eventID)
        {
            const string query =
                "SELECT " +
                "    TimeDomainData, " +
                "FROM EventData " +
                "WHERE ID = " +
                "(" +
                "    SELECT EventDataID " +
                "    FROM Event " +
                "    WHERE ID = @id " +
                ")";

            DataSet eventDataSet = new DataSet();

            using (SqlConnection connection = new SqlConnection("context connection=true"))
            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                connection.Open();
                command.Parameters.AddWithValue("@id", eventID);
                adapter.Fill(eventDataSet);
            }

            DataRow row = eventDataSet.Tables[0].Rows[0];

            return ReadFrom(Inflate((byte[])row["TimeDomainData"]))
                .ToArray();
        }
        
        public static void GetEventData_FillRow(object source, out SqlInt32 seriesID, out SqlString characteristic, out DateTime time, out SqlDouble value)
        {
            DataPoint dataPoint = source as DataPoint;

            if ((object)dataPoint == null)
                throw new InvalidOperationException("FillRow source is not a DataPoint");

            seriesID = dataPoint.SeriesID;
            characteristic = dataPoint.Characteristic;
            time = dataPoint.Time;
            value = ToSqlDouble(dataPoint.Value);
        }

        [SqlFunction(
            DataAccess = DataAccessKind.Read,
            FillRowMethodName = "GetFaultData_FillRow",
            TableDefinition = "[Algorithm] NVARCHAR(80), [Time] DATETIME2, [Value] FLOAT")
        ]
        public static IEnumerable GetFaultData(SqlInt32 eventID)
        {
            const string query =
                "SELECT " +
                "    Algorithm, " +
                "    Data " +
                "FROM FaultCurve " +
                "WHERE EventID = @id";

            DataSet faultDataSet = new DataSet();

            using (SqlConnection connection = new SqlConnection("context connection=true"))
            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                connection.Open();
                command.Parameters.AddWithValue("@id", eventID);
                adapter.Fill(faultDataSet);
            }

            return faultDataSet.Tables[0].Select()
                .SelectMany(row => ReadFrom(Inflate((byte[])row["Data"])).Select(dataPoint => new FaultDataPoint()
                {
                    Algorithm = row["Algorithm"].ToString(),
                    Time = dataPoint.Time,
                    Value = dataPoint.Value
                }))
                .ToArray();
        }

        public static void GetFaultData_FillRow(object source, out SqlString algorithm, out DateTime time, out SqlDouble value)
        {
            FaultDataPoint dataPoint = source as FaultDataPoint;

            if ((object)dataPoint == null)
                throw new InvalidOperationException("FillRow source is not a DataPoint");

            algorithm = dataPoint.Algorithm;
            time = dataPoint.Time;
            value = ToSqlDouble(dataPoint.Value);
        }

        private static IEnumerable<DataPoint> ReadFrom(byte[] data, bool freq = false)
        {
            int offset = 0;
            int samples = BitConverter.ToInt32(data, offset);
            DateTime[] times = new DateTime[samples];

            offset += sizeof(int);

            for (int i = 0; i < samples; i++)
            {
                times[i] = new DateTime(BitConverter.ToInt64(data, offset));
                offset += sizeof(long);
            }

            int seriesIndex = 0;

            while (offset < data.Length)
            {
                int seriesID = BitConverter.ToInt32(data, offset);

                offset += sizeof(int);

                for (int i = 0; i < samples; i++)
                {
                    yield return new DataPoint()
                    {
                        SeriesID = seriesID,
                        Characteristic = freq ? GetCharacteristic(seriesIndex) : "Instantaneous",
                        Time = times[i],
                        Value = BitConverter.ToDouble(data, offset)
                    };

                    offset += sizeof(double);
                }

                seriesIndex++;
            }
        }

        private static byte[] Inflate(byte[] data)
        {
            using (MemoryStream stream = new MemoryStream(data))
            using (GZipStream inflater = new GZipStream(stream, CompressionMode.Decompress))
            using (MemoryStream result = new MemoryStream())
            {
                inflater.CopyTo(result);
                return result.ToArray();
            }
        }

        private static string GetCharacteristic(int index)
        {
            switch (index % 4)
            {
                case 0: return "RMS";
                case 1: return "AngleFund";
                case 2: return "WaveAmplitude";
                case 3: return "WaveError";
                default: return "Unknown";
            }
        }

        private static SqlDouble ToSqlDouble(double num)
        {
            const double Sentinel = -1.0e308;

            return (double.IsNaN(num) || double.IsInfinity(num))
                ? Sentinel
                : num;
        }
    }
}
