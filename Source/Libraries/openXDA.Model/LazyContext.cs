//******************************************************************************************************
//  LazyContext.cs - Gbtc
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
//  09/04/2017 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using GSF.Data;

namespace openXDA.Model
{
    internal class LazyContext
    {
        #region [ Members ]

        // Fields
        private Dictionary<int, MeterLocation> m_meterLocations;
        private Dictionary<int, Meter> m_meters;
        private Dictionary<int, Line> m_lines;
        private Dictionary<int, MeterLocationLine> m_meterLocationLines;
        private Dictionary<int, MeterLine> m_meterLines;
        private Dictionary<int, Channel> m_channels;
        private Dictionary<int, Series> m_series;
        private Dictionary<int, MeasurementType> m_measurementTypes;
        private Dictionary<int, MeasurementCharacteristic> m_measurementCharacteristics;
        private Dictionary<int, Phase> m_phases;
        private Dictionary<int, SeriesType> m_seriesTypes;

        #endregion

        #region [ Constructors ]

        public LazyContext()
        {
            m_meterLocations = new Dictionary<int, MeterLocation>();
            m_meters = new Dictionary<int, Meter>();
            m_lines = new Dictionary<int, Line>();
            m_meterLocationLines = new Dictionary<int, MeterLocationLine>();
            m_meterLines = new Dictionary<int, MeterLine>();
            m_channels = new Dictionary<int, Channel>();
            m_series = new Dictionary<int, Series>();
            m_measurementTypes = new Dictionary<int, MeasurementType>();
            m_measurementCharacteristics = new Dictionary<int, MeasurementCharacteristic>();
            m_phases = new Dictionary<int, Phase>();
            m_seriesTypes = new Dictionary<int, SeriesType>();
        }

        #endregion

        #region [ Properties ]

        public Func<AdoDataConnection> ConnectionFactory { get; set; }

        #endregion

        #region [ Methods ]

        public MeterLocation GetMeterLocation(MeterLocation meterLocation)
        {
            MeterLocation cachedMeterLocation;

            if ((object)meterLocation == null)
                return null;

            if (meterLocation.ID == 0)
                return meterLocation;

            if (m_meterLocations.TryGetValue(meterLocation.ID, out cachedMeterLocation))
                return cachedMeterLocation;

            m_meterLocations.Add(meterLocation.ID, meterLocation);
            return meterLocation;
        }

        public Meter GetMeter(Meter meter)
        {
            Meter cachedMeter;

            if ((object)meter == null)
                return null;

            if (meter.ID == 0)
                return meter;

            if (m_meters.TryGetValue(meter.ID, out cachedMeter))
                return cachedMeter;

            m_meters.Add(meter.ID, meter);
            return meter;
        }

        public Line GetLine(Line line)
        {
            Line cachedLine;

            if ((object)line == null)
                return null;

            if (line.ID == 0)
                return line;

            if (m_lines.TryGetValue(line.ID, out cachedLine))
                return cachedLine;

            m_lines.Add(line.ID, line);
            return line;
        }

        public MeterLocationLine GetMeterLocationLine(MeterLocationLine meterLocationLine)
        {
            MeterLocationLine cachedMeterLocationLine;

            if ((object)meterLocationLine == null)
                return null;

            if (meterLocationLine.ID == 0)
                return meterLocationLine;

            if (m_meterLocationLines.TryGetValue(meterLocationLine.ID, out cachedMeterLocationLine))
                return cachedMeterLocationLine;

            m_meterLocationLines.Add(meterLocationLine.ID, meterLocationLine);
            return meterLocationLine;
        }

        public MeterLine GetMeterLine(MeterLine meterLine)
        {
            MeterLine cachedMeterLine;

            if ((object)meterLine == null)
                return null;

            if (meterLine.ID == 0)
                return meterLine;

            if (m_meterLines.TryGetValue(meterLine.ID, out cachedMeterLine))
                return cachedMeterLine;

            m_meterLines.Add(meterLine.ID, meterLine);
            return meterLine;
        }

        public Channel GetChannel(Channel channel)
        {
            Channel cachedChannelLine;

            if ((object)channel == null)
                return null;

            if (channel.ID == 0)
                return channel;

            if (m_channels.TryGetValue(channel.ID, out cachedChannelLine))
                return cachedChannelLine;

            m_channels.Add(channel.ID, channel);
            return channel;
        }

        public Series GetSeries(Series series)
        {
            Series cachedSeriesLine;

            if ((object)series == null)
                return null;

            if (series.ID == 0)
                return series;

            if (m_series.TryGetValue(series.ID, out cachedSeriesLine))
                return cachedSeriesLine;

            m_series.Add(series.ID, series);
            return series;
        }

        public MeasurementType GetMeasurementType(MeasurementType measurementType)
        {
            MeasurementType cachedMeasurementTypeLine;

            if ((object)measurementType == null)
                return null;

            if (measurementType.ID == 0)
                return measurementType;

            if (m_measurementTypes.TryGetValue(measurementType.ID, out cachedMeasurementTypeLine))
                return cachedMeasurementTypeLine;

            m_measurementTypes.Add(measurementType.ID, measurementType);
            return measurementType;
        }

        public MeasurementCharacteristic GetMeasurementCharacteristic(MeasurementCharacteristic measurementCharacteristic)
        {
            MeasurementCharacteristic cachedMeasurementCharacteristicLine;

            if ((object)measurementCharacteristic == null)
                return null;

            if (measurementCharacteristic.ID == 0)
                return measurementCharacteristic;

            if (m_measurementCharacteristics.TryGetValue(measurementCharacteristic.ID, out cachedMeasurementCharacteristicLine))
                return cachedMeasurementCharacteristicLine;

            m_measurementCharacteristics.Add(measurementCharacteristic.ID, measurementCharacteristic);
            return measurementCharacteristic;
        }

        public Phase GetPhase(Phase phase)
        {
            Phase cachedPhaseLine;

            if ((object)phase == null)
                return null;

            if (phase.ID == 0)
                return phase;

            if (m_phases.TryGetValue(phase.ID, out cachedPhaseLine))
                return cachedPhaseLine;

            m_phases.Add(phase.ID, phase);
            return phase;
        }

        public SeriesType GetSeriesType(SeriesType seriesType)
        {
            SeriesType cachedSeriesTypeLine;

            if ((object)seriesType == null)
                return null;

            if (seriesType.ID == 0)
                return seriesType;

            if (m_seriesTypes.TryGetValue(seriesType.ID, out cachedSeriesTypeLine))
                return cachedSeriesTypeLine;

            m_seriesTypes.Add(seriesType.ID, seriesType);
            return seriesType;
        }

        #endregion
    }
}
