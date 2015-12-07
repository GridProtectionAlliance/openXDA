//******************************************************************************************************
//  VIDataGroup.cs - Gbtc
//
//  Copyright © 2014, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the Eclipse Public License -v 1.0 (the "License"); you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/eclipse-1.0.php
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  08/29/2014 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using FaultData.Database;
using ChannelKey = System.Tuple<int, int, string, string, string, string>;
using SeriesKey = System.Tuple<int, int, string, string, string, string, string>;

namespace FaultData.DataAnalysis
{
    public class VIDataGroup
    {
        #region [ Members ]

        // Constants
        private const int VAIndex = 0;
        private const int VBIndex = 1;
        private const int VCIndex = 2;
        private const int IAIndex = 3;
        private const int IBIndex = 4;
        private const int ICIndex = 5;
        private const int IRIndex = 6;

        // Fields
        private DataGroup m_dataGroup;

        #endregion

        #region [ Constructors ]

        public VIDataGroup(DataGroup dataGroup)
        {
            // Get all necessary voltage and current channels in the proper order
            List<DataSeries> viSeriesList = Enumerable.Range(0, 7)
                .GroupJoin(dataGroup.DataSeries.Where(IsInstantaneous), i => i, GetIndex, (i, series) => series.FirstOrDefault())
                .ToList();

            // Validate that no channels are missing
            if (viSeriesList.Any(series => (object)series == null))
                throw new InvalidOperationException("Cannot create VIDataGroup from an incomplete set of voltage/current channels");

            // Create the data group from the complete
            // set of voltage/current channels
            m_dataGroup = new DataGroup(viSeriesList);
        }

        #endregion

        #region [ Properties ]

        public DataSeries VA
        {
            get
            {
                return m_dataGroup[VAIndex];
            }
        }

        public DataSeries VB
        {
            get
            {
                return m_dataGroup[VBIndex];
            }
        }

        public DataSeries VC
        {
            get
            {
                return m_dataGroup[VCIndex];
            }
        }

        public DataSeries IA
        {
            get
            {
                return m_dataGroup[IAIndex];
            }
        }

        public DataSeries IB
        {
            get
            {
                return m_dataGroup[IBIndex];
            }
        }

        public DataSeries IC
        {
            get
            {
                return m_dataGroup[ICIndex];
            }
        }

        public DataSeries IR
        {
            get
            {
                return m_dataGroup[IRIndex];
            }
        }

        #endregion

        #region [ Methods ]

        public DataGroup ToDataGroup()
        {
            return m_dataGroup;
        }

        public VIDataGroup ToSubGroup(int startIndex, int endIndex)
        {
            return new VIDataGroup(m_dataGroup.ToSubGroup(startIndex, endIndex));
        }

        #endregion

        #region [ Static ]

        // Static Methods
        public static DataSeries AddMissingCurrentSeries(MeterInfoDataContext meterInfo, Meter meter, DataGroup dataGroup)
        {
            DataSeries missingSeries = null;

            // Get all necessary voltage and current channels in the proper order
            List<DataSeries> viSeriesList = Enumerable.Range(VAIndex, IRIndex + 1)
                .GroupJoin(dataGroup.DataSeries.Where(IsInstantaneous), i => i, GetIndex, (i, series) => series.FirstOrDefault())
                .ToList();

            // Validate that no more than one current channel is missing
            if (viSeriesList.Count(series => (object)series == null) > 1)
                return null;

            // Attempt to fill in missing current channels
            // based on the relation IR = IA + IB + IC
            if ((object)viSeriesList[IAIndex] == null)
            {
                missingSeries = viSeriesList[IRIndex];
                missingSeries = missingSeries.Add(viSeriesList[IBIndex].Negate());
                missingSeries = missingSeries.Add(viSeriesList[ICIndex].Negate());

                missingSeries.SeriesInfo = GetSeriesInfo(meterInfo, meter, dataGroup, IAIndex);
                missingSeries.SeriesInfo.Channel.Line = viSeriesList[IBIndex].SeriesInfo.Channel.Line;

                viSeriesList[IAIndex] = missingSeries;
            }
            else if ((object)viSeriesList[IBIndex] == null)
            {
                missingSeries = viSeriesList[IRIndex];
                missingSeries = missingSeries.Add(viSeriesList[IAIndex].Negate());
                missingSeries = missingSeries.Add(viSeriesList[ICIndex].Negate());

                missingSeries.SeriesInfo = GetSeriesInfo(meterInfo, meter, dataGroup, IBIndex);
                missingSeries.SeriesInfo.Channel.Line = viSeriesList[IAIndex].SeriesInfo.Channel.Line;

                viSeriesList[IBIndex] = missingSeries;
            }
            else if ((object)viSeriesList[ICIndex] == null)
            {
                missingSeries = viSeriesList[IRIndex];
                missingSeries = missingSeries.Add(viSeriesList[IAIndex].Negate());
                missingSeries = missingSeries.Add(viSeriesList[IBIndex].Negate());

                missingSeries.SeriesInfo = GetSeriesInfo(meterInfo, meter, dataGroup, ICIndex);
                missingSeries.SeriesInfo.Channel.Line = viSeriesList[IAIndex].SeriesInfo.Channel.Line;

                viSeriesList[ICIndex] = missingSeries;
            }
            else if ((object)viSeriesList[IRIndex] == null)
            {
                missingSeries = viSeriesList[IAIndex];
                missingSeries = missingSeries.Add(viSeriesList[IBIndex]);
                missingSeries = missingSeries.Add(viSeriesList[ICIndex]);

                missingSeries.SeriesInfo = GetSeriesInfo(meterInfo, meter, dataGroup, IRIndex);
                missingSeries.SeriesInfo.Channel.Line = viSeriesList[IAIndex].SeriesInfo.Channel.Line;

                viSeriesList[IRIndex] = missingSeries;
            }

            if ((object)missingSeries != null)
                dataGroup.Add(missingSeries);

            return missingSeries;
        }

        private static Series GetSeriesInfo(MeterInfoDataContext meterInfo, Meter meter, DataGroup dataGroup, int index)
        {
            Dictionary<int, string> measurementTypeNameLookup = new Dictionary<int, string>()
            {
                { VAIndex, "Voltage" },
                { VBIndex, "Voltage" },
                { VCIndex, "Voltage" },
                { IAIndex, "Current" },
                { IBIndex, "Current" },
                { ICIndex, "Current" },
                { IRIndex, "Current" }
            };

            Dictionary<int, string> phaseNameLookup = new Dictionary<int, string>()
            {
                { VAIndex, "AN" },
                { VBIndex, "BN" },
                { VCIndex, "CN" },
                { IAIndex, "AN" },
                { IBIndex, "BN" },
                { ICIndex, "CN" },
                { IRIndex, "RES" }
            };

            int lineID;
            string measurementTypeName;
            string measurementCharacteristicName;
            string phaseName;
            string seriesTypeName;
            string channelName;

            DataContextLookup<ChannelKey, Channel> channelLookup;
            DataContextLookup<SeriesKey, Series> seriesLookup;
            DataContextLookup<string, MeasurementType> measurementTypeLookup;
            DataContextLookup<string, MeasurementCharacteristic> measurementCharacteristicLookup;
            DataContextLookup<string, Phase> phaseLookup;
            DataContextLookup<string, SeriesType> seriesTypeLookup;

            SeriesKey seriesKey;
            ChannelKey channelKey;

            lineID = dataGroup.Line.ID;
            measurementTypeName = measurementTypeNameLookup[index];
            measurementCharacteristicName = "Instantaneous";
            phaseName = phaseNameLookup[index];
            seriesTypeName = "Values";
            channelName = string.Concat(measurementTypeName, " ", phaseName);

            channelLookup = new DataContextLookup<ChannelKey, Channel>(meterInfo, GetChannelKey)
                .WithFilterExpression(channel => channel.MeterID == meter.ID)
                .WithFilterExpression(channel => channel.LineID == dataGroup.Line.ID);

            seriesLookup = new DataContextLookup<SeriesKey, Series>(meterInfo, GetSeriesKey)
                .WithFilterExpression(series => series.Channel.Meter.ID == meter.ID)
                .WithFilterExpression(series => series.Channel.Line.ID == dataGroup.Line.ID);

            measurementTypeLookup = new DataContextLookup<string, MeasurementType>(meterInfo, measurementType => measurementType.Name);
            measurementCharacteristicLookup = new DataContextLookup<string, MeasurementCharacteristic>(meterInfo, measurementCharacteristic => measurementCharacteristic.Name);
            phaseLookup = new DataContextLookup<string, Phase>(meterInfo, phase => phase.Name);
            seriesTypeLookup = new DataContextLookup<string, SeriesType>(meterInfo, seriesType => seriesType.Name);

            seriesKey = Tuple.Create(lineID, 0, channelName, measurementTypeName, measurementCharacteristicName, phaseName, seriesTypeName);
            channelKey = Tuple.Create(lineID, 0, channelName, measurementTypeName, measurementCharacteristicName, phaseName);

            return seriesLookup.GetOrAdd(seriesKey, key =>
            {
                SeriesType seriesType = seriesTypeLookup.GetOrAdd(seriesTypeName, name => new SeriesType() { Name = name, Description = name });

                Channel channel = channelLookup.GetOrAdd(channelKey, chKey =>
                {
                    MeasurementType measurementType = measurementTypeLookup.GetOrAdd(measurementTypeName, name => new MeasurementType() { Name = name, Description = name });
                    MeasurementCharacteristic measurementCharacteristic = measurementCharacteristicLookup.GetOrAdd(measurementCharacteristicName, name => new MeasurementCharacteristic() { Name = name, Description = name });
                    Phase phase = phaseLookup.GetOrAdd(phaseName, name => new Phase() { Name = name, Description = name });

                    return new Channel()
                    {
                        Meter = meter,
                        Line = dataGroup.Line,
                        MeasurementType = measurementType,
                        MeasurementCharacteristic = measurementCharacteristic,
                        Phase = phase,
                        Name = string.Concat(measurementType.Name, " ", phase.Name),
                        SamplesPerHour = dataGroup.SamplesPerHour,
                        PerUnitValue = 0,
                        HarmonicGroup = 0,
                        Description = string.Concat(measurementCharacteristic.Name, " ", measurementType.Name, " ", phase.Name),
                        Enabled = 1
                    };
                });

                return new Series()
                {
                    SeriesType = seriesType,
                    Channel = channel,
                    SourceIndexes = string.Empty
                };
            });
        }

        private static bool IsInstantaneous(DataSeries dataSeries)
        {
            string characteristicName = dataSeries.SeriesInfo.Channel.MeasurementCharacteristic.Name;
            string seriesTypeName = dataSeries.SeriesInfo.SeriesType.Name;

            return (characteristicName == "Instantaneous") &&
                   (seriesTypeName == "Values" || seriesTypeName == "Instantaneous");
        }

        private static int GetIndex(DataSeries dataSeries)
        {
            Dictionary<string, int> indexLookup = new Dictionary<string, int>()
            {
                { "Voltage AN", VAIndex },
                { "Voltage BN", VBIndex },
                { "Voltage CN", VCIndex },
                { "Current AN", IAIndex },
                { "Current BN", IBIndex },
                { "Current CN", ICIndex },
                { "Current RES", IRIndex }
            };

            int index;
            string measurementType = dataSeries.SeriesInfo.Channel.MeasurementType.Name;
            string phase = dataSeries.SeriesInfo.Channel.Phase.Name;
            string key = string.Format("{0} {1}", measurementType, phase);

            return indexLookup.TryGetValue(key, out index) ? index : -1;
        }

        private static ChannelKey GetChannelKey(Channel channel)
        {
            return Tuple.Create(
                channel.LineID,
                channel.HarmonicGroup,
                channel.Name,
                channel.MeasurementType.Name,
                channel.MeasurementCharacteristic.Name,
                channel.Phase.Name);
        }

        private static SeriesKey GetSeriesKey(Series series)
        {
            Channel channel = series.Channel;

            return Tuple.Create(
                channel.LineID,
                channel.HarmonicGroup,
                channel.Name,
                channel.MeasurementType.Name,
                channel.MeasurementCharacteristic.Name,
                channel.Phase.Name,
                series.SeriesType.Name);
        }

        #endregion
    }
}
