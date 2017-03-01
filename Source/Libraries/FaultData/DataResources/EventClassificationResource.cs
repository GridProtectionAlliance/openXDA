//******************************************************************************************************
//  DataGroupClassificationResource.cs - Gbtc
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
//  02/20/2015 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using FaultData.DataAnalysis;
using FaultData.Database;
using FaultData.DataSets;

namespace FaultData.DataResources
{
    public enum EventClassification
    {
        Fault,
        Interruption,
        Sag,
        Swell,
        Transient,
        Other,
        Breaker
    }

    public class EventClassificationResource : DataResourceBase<MeterDataSet>
    {
        #region [ Members ]

        // Fields
        private DbAdapterContainer m_dbAdapterContainer;
        private Dictionary<DataGroup, EventClassification> m_classifications;

        private double m_systemFrequency;
        private double m_sagThreshold;
        private double m_swellThreshold;
        private double m_interruptionThreshold;

        #endregion

        #region [ Constructors ]

        public EventClassificationResource(DbAdapterContainer dbAdapterContainer)
        {
            m_dbAdapterContainer = dbAdapterContainer;
            m_classifications = new Dictionary<DataGroup, EventClassification>();
        }

        #endregion

        #region [ Properties ]

        public Dictionary<DataGroup, EventClassification> Classifications
        {
            get
            {
                return m_classifications;
            }
        }

        [Setting]
        public double SystemFrequency
        {
            get
            {
                return m_systemFrequency;
            }
            set
            {
                m_systemFrequency = value;
            }
        }

        [Setting]
        public double SagThreshold
        {
            get
            {
                return m_sagThreshold;
            }
            set
            {
                m_sagThreshold = value;
            }
        }

        [Setting]
        public double SwellThreshold
        {
            get
            {
                return m_swellThreshold;
            }
            set
            {
                m_swellThreshold = value;
            }
        }

        [Setting]
        public double InterruptionThreshold
        {
            get
            {
                return m_interruptionThreshold;
            }
            set
            {
                m_interruptionThreshold = value;
            }
        }

        #endregion

        #region [ Methods ]

        public override void Initialize(MeterDataSet meterDataSet)
        {
            CycleDataResource cycleDataResource = CycleDataResource.GetResource(meterDataSet, m_dbAdapterContainer);
            FaultDataResource faultDataResource = meterDataSet.GetResource(() => new FaultDataResource(m_dbAdapterContainer));
            FaultGroup faultGroup;

            DataGroup dataGroup;
            VIDataGroup viDataGroup;
            VICycleDataGroup viCycleDataGroup;

            for (int i = 0; i < cycleDataResource.DataGroups.Count; i++)
            {
                dataGroup = cycleDataResource.DataGroups[i];
                viDataGroup = cycleDataResource.VIDataGroups[i];
                viCycleDataGroup = cycleDataResource.VICycleDataGroups[i];

                if (!faultDataResource.FaultLookup.TryGetValue(dataGroup, out faultGroup))
                    faultGroup = null;

                m_classifications.Add(dataGroup, Classify(meterDataSet, dataGroup, viDataGroup, viCycleDataGroup, faultGroup));
            }
        }

        private EventClassification Classify(MeterDataSet meterDataSet, DataGroup dataGroup, VIDataGroup viDataGroup, VICycleDataGroup viCycleDataGroup, FaultGroup faultGroup)
        {
            if (viDataGroup.DefinedNeutralVoltages == 0 && viDataGroup.DefinedLineVoltages == 0 && dataGroup.DataSeries.SelectMany(series => series.SeriesInfo.Channel.BreakerChannels).Any())
                return EventClassification.Breaker;

            if ((object)faultGroup != null && (faultGroup.FaultDetectionLogicResult ?? faultGroup.FaultValidationLogicResult))
                return EventClassification.Fault;

            DataSeries[] rms =
            {
                viCycleDataGroup.VA?.RMS,
                viCycleDataGroup.VB?.RMS,
                viCycleDataGroup.VC?.RMS,
                viCycleDataGroup.VAB?.RMS,
                viCycleDataGroup.VBC?.RMS,
                viCycleDataGroup.VCA?.RMS
            };

            List<DataSeries> perUnitRMS = rms
                .Where(dataSeries => (object)dataSeries != null)
                .Where(dataSeries => GetPerUnitValue(dataSeries) != 0.0D)
                .Select(dataSeries => dataSeries.Multiply(1.0D / GetPerUnitValue(dataSeries)))
                .ToList();

            if (HasInterruption(perUnitRMS))
                return EventClassification.Interruption;

            if (HasSag(perUnitRMS))
                return EventClassification.Sag;

            if (HasSwell(perUnitRMS))
                return EventClassification.Swell;

            return EventClassification.Other;
        }

        private bool HasInterruption(IEnumerable<DataSeries> seriesList)
        {
            IEnumerable<double> values;

            foreach (DataSeries series in seriesList)
            {
                values = series.DataPoints.Select(dataPoint => dataPoint.Value);

                if (values.Any(value => value <= m_interruptionThreshold))
                    return true;
            }

            return false;
        }

        private bool HasSag(IEnumerable<DataSeries> seriesList)
        {
            IEnumerable<double> values;

            foreach (DataSeries series in seriesList)
            {
                values = series.DataPoints.Select(dataPoint => dataPoint.Value);

                if (values.Any(value => value <= m_sagThreshold))
                    return true;
            }

            return false;
        }

        private bool HasSwell(IEnumerable<DataSeries> seriesList)
        {
            IEnumerable<double> values;

            foreach (DataSeries series in seriesList)
            {
                values = series.DataPoints.Select(dataPoint => dataPoint.Value);

                if (values.Any(value => value >= m_swellThreshold))
                    return true;
            }

            return false;
        }

        private double GetPerUnitValue(DataSeries dataSeries)
        {
            if ((object)dataSeries.SeriesInfo.Channel.PerUnitValue != null)
                return dataSeries.SeriesInfo.Channel.PerUnitValue.GetValueOrDefault();

            if (dataSeries.SeriesInfo.Channel.MeasurementType.Name != "Voltage")
                return 0.0D;

            if (!IsLineToNeutral(dataSeries.SeriesInfo.Channel) && !IsLineToLine(dataSeries.SeriesInfo.Channel))
                return 0.0D;

            double voltageKV = dataSeries.SeriesInfo.Channel.Line.VoltageKV * 1000.0D;

            double divisor = IsLineToNeutral(dataSeries.SeriesInfo.Channel)
                ? Math.Sqrt(3.0D)
                : 1.0D;

            return voltageKV / divisor;
        }

        private static bool IsLineToNeutral(Channel channel)
        {
            return channel.Phase.Name == "AN" ||
                   channel.Phase.Name == "BN" ||
                   channel.Phase.Name == "CN" ||
                   channel.Phase.Name == "RES" ||
                   channel.Phase.Name == "NG" ||
                   channel.Phase.Name == "LineToNeutralAverage";
        }

        private static bool IsLineToLine(Channel channel)
        {
            return channel.Phase.Name == "AB" ||
                   channel.Phase.Name == "BC" ||
                   channel.Phase.Name == "CA" ||
                   channel.Phase.Name == "LineToLineAverage";
        }

        #endregion
    }
}
