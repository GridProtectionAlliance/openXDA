//******************************************************************************************************
//  SqlFunctions.cs - Gbtc
//
//  Copyright © 2015, Grid Protection Alliance.  All Rights Reserved.
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
//  05/04/2015 - J. Ritchie Carroll
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections;
using System.Data.SqlTypes;
using System.Text;
using GSF;
using Microsoft.SqlServer.Server;

/// <summary>
/// openXDA SQL functions used to query trending data from openHistorian.
/// </summary>
// ReSharper disable once CheckNamespace
public class SqlFunctions
{
    // No need to make this a structure since it will just get boxed when passed to FillRow function anyway...
    private class SeriesMeasurement
    {
        public readonly uint ChannelID;
        public readonly uint SeriesID;
        public readonly DateTime Time;
        public readonly float Value;

        public SeriesMeasurement(SqlProcedures.Measurement measurement)
        {
            ChannelID = measurement.ID.HighDoubleWord();
            SeriesID = measurement.ID.LowDoubleWord();
            Time = measurement.Time;
            Value = measurement.Value;
        }
    }

    /// <summary>
    /// Queries trending data from the openHistorian.
    /// </summary>
    /// <param name="historianServer">Historian server host IP or DNS name. Can be optionally suffixed with port number, e.g.: historian:38402.</param>
    /// <param name="instanceName">Instance name of the historian.</param>
    /// <param name="startTime">Start time of desired data range.</param>
    /// <param name="stopTime">End time of desired data range.</param>
    /// <param name="channelIDs">Comma separated list of channels ID values; set to <c>null</c>to retrieve values for all channels.</param>
    /// <param name="seriesCount">Number of series to return for each channel; defaults to 3 for min, max and average.</param>
    /// <returns>
    /// Enumerable trending data results for specified time range and points.
    /// </returns>
    [SqlFunction(
        DataAccess = DataAccessKind.Read,
        FillRowMethodName = "GetTrendingData_FillRow",
        TableDefinition = "[ChannelID] int, [SeriesID] int, [Time] datetime2, [Value] real")
    ]
    public static IEnumerable GetTrendingData(SqlString historianServer, SqlString instanceName, DateTime startTime, DateTime stopTime, SqlString channelIDs, SqlInt32 seriesCount)
    {
        StringBuilder measurementIDs = null;

        // Combine each channel ID with an ID for each series to create a historian measurement ID
        if (!channelIDs.IsNull && !string.IsNullOrEmpty(channelIDs.Value))
        {
            string[] channels = channelIDs.Value.Split(',');
            measurementIDs = new StringBuilder();

            for (int i = 0; i < channels.Length; i++)
                for (int j = 0; j < seriesCount.Value; j++)
                    measurementIDs.AppendFormat("{0}{1}", measurementIDs.Length == 0 ? "" : ",", Word.MakeQuadWord(uint.Parse(channels[i]), (uint)j));
        }

        foreach (SqlProcedures.Measurement measurement in SqlProcedures.GetHistorianData(historianServer, instanceName, startTime, stopTime, measurementIDs == null ? SqlString.Null : measurementIDs.ToString()))
            yield return new SeriesMeasurement(measurement);
    }

    /// <summary>
    /// Used to fill table columns with enumerable data returned from <see cref="GetTrendingData"/>.
    /// </summary>
    /// <param name="source">Source data, i.e., a SeriesMeasurement.</param>
    /// <param name="channelID">SeriesMeasurement Channel ID</param>
    /// <param name="seriesID">SeriesMeasurement Series ID</param>
    /// <param name="time">SeriesMeasurement Timestamp</param>
    /// <param name="value">SeriesMeasurement value</param>
    public static void GetTrendingData_FillRow(object source, out SqlInt32 channelID, out SqlInt32 seriesID, out DateTime time, out SqlSingle value)
    {
        SeriesMeasurement seriesMeasurement = source as SeriesMeasurement;

        if ((object)seriesMeasurement == null)
            throw new InvalidOperationException("FillRow source is not a SeriesMeasurement");

        channelID = (int)seriesMeasurement.ChannelID;
        seriesID = (int)seriesMeasurement.SeriesID;
        time = seriesMeasurement.Time;
        value = seriesMeasurement.Value;
    }
}
