//******************************************************************************************************
//  SignalCode.cs - Gbtc
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
//  09/08/2017 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using FaultData.DataAnalysis;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using openXDA.Model;

namespace openXDA.Hubs
{
    public class SignalCode
    {
        #region [ Members ]
        private const double MaxFaultDistanceMultiplier = 1.25D;
        private const double MinFaultDistanceMultiplier = -0.1D;
        private double m_systemFrequency;

        public class eventSet
        {
            public string Yaxis0name;
            public string Yaxis1name;
            public string[] xAxis;
            public List<signalDetail> data;
            public List<faultSegmentDetail> detail;
        }

        public class signalDetail
        {
            public bool visible = true;
            public bool showInLegend = true;
            public bool showInTooltip = true;
            public string name;
            public string type;
            public int yAxis;
            public double[] data;
        }


        public class faultSegmentDetail
        {
            public string type;
            public int StartSample;
            public int EndSample;
        }

        public enum FlotSeriesType
        {
            Waveform,
            Cycle,
            Fault
        }

        public class FlotSeries
        {
            public FlotSeriesType FlotType;
            public int SeriesID;
            public string ChannelName;
            public string ChannelDescription;
            public string MeasurementType;
            public string MeasurementCharacteristic;
            public string Phase;
            public string SeriesType;
            public List<double[]> DataPoints = new List<double[]>();

            public FlotSeries Clone()
            {
                FlotSeries clone = (FlotSeries)MemberwiseClone();
                clone.DataPoints = new List<double[]>();
                return clone;
            }
        }
        #endregion

        #region [ Constructors ]

        public SignalCode()
        {
            int i = 1;
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                m_systemFrequency = connection.ExecuteScalar<double?>("SELECT Value FROM Setting WHERE Name = 'SystemFrequency'") ?? 60.0D;
            }
            //Uncomment the following line if using designed components 
            //InitializeComponent(); 
        }

        #endregion

        #region [ Properties ]
        public double SystemFrequency
        {
            get
            {
                return m_systemFrequency;
            }
        }
        #endregion




        private static string ConnectionString = ConfigurationFile.Current.Settings["systemSettings"]["ConnectionString"].Value;

        private static readonly List<FlotSeries> CycleDataInfo = new List<FlotSeries>();

        static SignalCode()
        {
            foreach (string phase in new string[] { "AN", "BN", "CN", "AB", "BC", "CA" })
            {
                int seriesID = 0;

                foreach (string measurementCharacteristic in new string[] { "RMS", "AngleFund", "WaveAmplitude", "WaveError" })
                {
                    string measurementType = "Voltage";
                    char measurementTypeDesignation = 'V';

                    CycleDataInfo.Add(new FlotSeries()
                    {
                        FlotType = FlotSeriesType.Cycle,
                        SeriesID = seriesID++,
                        ChannelName = string.Concat(measurementTypeDesignation, phase, " ", measurementCharacteristic),
                        ChannelDescription = string.Concat(phase, " ", measurementType, " ", measurementCharacteristic),
                        MeasurementType = measurementType,
                        MeasurementCharacteristic = measurementCharacteristic,
                        Phase = phase,
                        SeriesType = "Values"
                    });
                }
            }

            foreach (string phase in new string[] { "AN", "BN", "CN", "RES" })
            {
                int seriesID = 0;

                foreach (string measurementCharacteristic in new string[] { "RMS", "AngleFund", "WaveAmplitude", "WaveError" })
                {
                    string measurementType = "Current";
                    char measurementTypeDesignation = 'I';

                    CycleDataInfo.Add(new FlotSeries()
                    {
                        FlotType = FlotSeriesType.Cycle,
                        SeriesID = seriesID++,
                        ChannelName = string.Concat(measurementTypeDesignation, phase, " ", measurementCharacteristic),
                        ChannelDescription = string.Concat(phase, " ", measurementType, " ", measurementCharacteristic),
                        MeasurementType = measurementType,
                        MeasurementCharacteristic = measurementCharacteristic,
                        Phase = phase,
                        SeriesType = "Values"
                    });
                }
            }
        }

        private static List<Series> GetWaveformInfo(AdoDataConnection connection, int meterID, int lineID)
        {
            TableOperations<Series> seriesTable = new TableOperations<Series>(connection);

            List<Series> seriesList = seriesTable
                .QueryRecords("ID", new RecordRestriction("ChannelID IN (SELECT ID FROM Channel WHERE MeterID = {0} AND LineID = {1})", meterID, lineID))
                .ToList();

            foreach (Series series in seriesList)
                series.ConnectionFactory = () => new AdoDataConnection(connection.Connection, typeof(SqlDataAdapter), false);

            return seriesList;
        }

        private static List<string> GetFaultCurveInfo(AdoDataConnection connection, int eventID)
        {
            const string query =
                "SELECT Algorithm " +
                "FROM FaultCurve LEFT OUTER JOIN FaultLocationAlgorithm ON Algorithm = MethodName " +
                "WHERE EventID = {0} " +
                "ORDER BY CASE WHEN ExecutionOrder IS NULL THEN 1 ELSE 0 END, ExecutionOrder";

            DataTable table = connection.RetrieveData(query, eventID);

            return table.Rows
                .Cast<DataRow>()
                .Select(row => row.Field<string>("Algorithm"))
                .ToList();
        }

        public static List<FlotSeries> GetFlotInfo(int eventID)
        {
            using (AdoDataConnection connection = CreateDbConnection())
            {
                TableOperations<Event> eventTable = new TableOperations<Event>(connection);

                Event evt = eventTable.QueryRecordWhere("ID = {0}", eventID);

                if ((object)evt == null)
                    return new List<FlotSeries>();

                List<Series> waveformInfo = GetWaveformInfo(connection, evt.MeterID, evt.LineID);

                var lookup = waveformInfo
                    .Where(info => info.Channel.MeasurementCharacteristic.Name == "Instantaneous")
                    .Where(info => new string[] { "Instantaneous", "Values" }.Contains(info.SeriesType.Name))
                    .Select(info => new { MeasurementType = info.Channel.MeasurementType.Name, Phase = info.Channel.Phase.Name })
                    .Distinct()
                    .ToDictionary(info => info);

                IEnumerable<FlotSeries> cycleDataInfo = CycleDataInfo
                    .Where(info => lookup.ContainsKey(new { info.MeasurementType, info.Phase }))
                    .Select(info => info.Clone());

                return waveformInfo
                    .Select(ToFlotSeries)
                    .Concat(cycleDataInfo)
                    .Concat(GetFaultCurveInfo(connection, eventID).Select(ToFlotSeries))
                    .ToList();
            }
        }

        private static FlotSeries ToFlotSeries(Series series)
        {
            return new FlotSeries()
            {
                FlotType = FlotSeriesType.Waveform,
                SeriesID = series.ID,
                ChannelName = series.Channel.Name,
                ChannelDescription = series.Channel.Description,
                MeasurementType = series.Channel.MeasurementType.Name,
                MeasurementCharacteristic = series.Channel.MeasurementCharacteristic.Name,
                Phase = series.Channel.Phase.Name,
                SeriesType = series.SeriesType.Name
            };
        }

        private static FlotSeries ToFlotSeries(string faultLocationAlgorithm)
        {
            return new FlotSeries()
            {
                FlotType = FlotSeriesType.Fault,
                ChannelName = faultLocationAlgorithm,
                ChannelDescription = faultLocationAlgorithm,
                MeasurementType = "Distance",
                MeasurementCharacteristic = "FaultDistance",
                Phase = "None",
                SeriesType = "Values"
            };
        }

        public List<FlotSeries> GetFlotData(int eventID, List<int> seriesIndexes)
        {
            List<FlotSeries> flotSeriesList = new List<FlotSeries>();

            using (AdoDataConnection connection = CreateDbConnection())
            {
                TableOperations<Meter> meterTable = new TableOperations<Meter>(connection);
                TableOperations<Event> eventTable = new TableOperations<Event>(connection);
                TableOperations<FaultCurve> faultCurveTable = new TableOperations<FaultCurve>(connection);

                Event evt = eventTable.QueryRecordWhere("ID = {0}", eventID);
                Meter meter = meterTable.QueryRecordWhere("ID = {0}", evt.MeterID);
                meter.ConnectionFactory = () => new AdoDataConnection(connection.Connection, connection.AdapterType, false);

                List<FlotSeries> flotInfo = GetFlotInfo(eventID);
                DateTime epoch = new DateTime(1970, 1, 1);

                byte[] timeDomainData = connection.ExecuteScalar<byte[]>("SELECT TimeDomainData FROM EventData WHERE ID = {0}", evt.EventDataID);

                Lazy<DataGroup> dataGroup = new Lazy<DataGroup>(() => ToDataGroup(meter, timeDomainData));
                Dictionary<int, DataSeries> waveformData = dataGroup.Value.DataSeries.ToDictionary(dataSeries => dataSeries.SeriesInfo.ID);
      

                Lazy<DataGroup> cycleData = new Lazy<DataGroup>(() => (Transform.ToVICycleDataGroup(new VIDataGroup(dataGroup.Value), SystemFrequency).ToDataGroup()));

                Lazy<Dictionary<string, DataSeries>> faultCurveData = new Lazy<Dictionary<string, DataSeries>>(() =>
                {
                    return faultCurveTable
                        .QueryRecordsWhere("EventID = {0}", evt.ID)
                        .Select(faultCurve => new
                        {
                            Algorithm = faultCurve.Algorithm,
                            DataGroup = ToDataGroup(meter, faultCurve.Data)
                        })
                        .Where(obj => obj.DataGroup.DataSeries.Count > 0)
                        .ToDictionary(obj => obj.Algorithm, obj => obj.DataGroup[0]);
                });

                foreach (int index in seriesIndexes)
                {
                    DataSeries dataSeries = null;
                    FlotSeries flotSeries;

                    if (index >= flotInfo.Count)
                        continue;

                    flotSeries = flotInfo[index];

                    if (flotSeries.FlotType == FlotSeriesType.Waveform)
                    {
                        if (!waveformData.TryGetValue(flotSeries.SeriesID, out dataSeries))
                            continue;
                    }
                    else if (flotSeries.FlotType == FlotSeriesType.Cycle)
                    {
                        dataSeries = cycleData.Value.DataSeries
                            .Where(series => series.SeriesInfo.Channel.MeasurementType.Name == flotSeries.MeasurementType)
                            .Where(series => series.SeriesInfo.Channel.Phase.Name == flotSeries.Phase)
                            .Skip(flotSeries.SeriesID)
                            .FirstOrDefault();

                        if ((object)dataSeries == null)
                            continue;
                    }
                    else if (flotSeries.FlotType == FlotSeriesType.Fault)
                    {
                        string algorithm = flotSeries.ChannelName;

                        if (!faultCurveData.Value.TryGetValue(algorithm, out dataSeries))
                            continue;
                    }
                    else
                    {
                        continue;
                    }

                    foreach (DataPoint dataPoint in dataSeries.DataPoints)
                    {
                        if (!double.IsNaN(dataPoint.Value))
                            flotSeries.DataPoints.Add(new double[] { dataPoint.Time.Subtract(epoch).TotalMilliseconds, dataPoint.Value });
                    }

                    flotSeriesList.Add(flotSeries);
                }
            }

            return flotSeriesList;
        }

        public DataGroup ToDataGroup(Meter meter, byte[] data)
        {
            DataGroup dataGroup = new DataGroup();
            dataGroup.FromData(meter, data);
            return dataGroup;
        }

        /// <summary>
        /// FixFaultCurve
        /// </summary>
        /// <param name="faultCurve"></param>
        /// <param name="line"></param>
        private void FixFaultCurve(DataSeries faultCurve, Line line)
        {
            double maxFaultDistance = MaxFaultDistanceMultiplier * line.Length;
            double minFaultDistance = MinFaultDistanceMultiplier * line.Length;

            foreach (DataPoint dataPoint in faultCurve.DataPoints)
            {
                if (double.IsNaN(dataPoint.Value))
                    dataPoint.Value = 0.0D;
                else if (dataPoint.Value > maxFaultDistance)
                    dataPoint.Value = maxFaultDistance;
                else if (dataPoint.Value < minFaultDistance)
                    dataPoint.Value = minFaultDistance;
            }
        }

        private static AdoDataConnection CreateDbConnection()
        {
            return new AdoDataConnection(ConnectionString, typeof(SqlConnection), typeof(SqlDataAdapter));
        }
    }
}