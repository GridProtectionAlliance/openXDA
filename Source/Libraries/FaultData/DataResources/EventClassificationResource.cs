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
        Other
    }

    public class EventClassificationResource : DataResourceBase<MeterDataSet>
    {
        #region [ Members ]

        // Nested Types
        public class Factory
        {
            public DbAdapterContainer DbAdapterContainer;

            public double MaxVoltage;
            public double MaxCurrent;
            public double LowVoltageThreshold;
            public double MaxLowVoltageCurrent;
            public double MaxTimeOffset;
            public double MinTimeOffset;

            public double ResidualCurrentTrigger;
            public double PhaseCurrentTrigger;
            public double PrefaultTrigger;
            public double FaultSuppressionTrigger;
            public double MaxFaultDistanceMultiplier;
            public double MinFaultDistanceMultiplier;

            public EventClassificationResource Create()
            {
                return new EventClassificationResource()
                {
                    m_dbAdapterContainer = DbAdapterContainer,
                    m_maxVoltage = MaxVoltage,
                    m_maxCurrent = MaxCurrent,
                    m_lowVoltageThreshold = LowVoltageThreshold,
                    m_maxLowVoltageCurrent = MaxLowVoltageCurrent,
                    m_maxTimeOffset = MaxTimeOffset,
                    m_minTimeOffset = MinTimeOffset,
                    m_residualCurrentTrigger = ResidualCurrentTrigger,
                    m_phaseCurrentTrigger = PhaseCurrentTrigger,
                    m_prefaultTrigger = PrefaultTrigger,
                    m_faultSuppressionTrigger = FaultSuppressionTrigger,
                    m_maxFaultDistanceMultiplier = MaxFaultDistanceMultiplier,
                    m_minFaultDistanceMultiplier = MinFaultDistanceMultiplier
                };
            }
        }

        // Fields
        private DbAdapterContainer m_dbAdapterContainer;

        private double m_maxVoltage;
        private double m_maxCurrent;
        private double m_lowVoltageThreshold;
        private double m_maxLowVoltageCurrent;
        private double m_maxTimeOffset;
        private double m_minTimeOffset;

        private double m_residualCurrentTrigger;
        private double m_phaseCurrentTrigger;
        private double m_prefaultTrigger;
        private double m_faultSuppressionTrigger;
        private double m_maxFaultDistanceMultiplier;
        private double m_minFaultDistanceMultiplier;

        private Dictionary<DataGroup, EventClassification> m_classifications;

        #endregion

        #region [ Constructors ]

        private EventClassificationResource()
        {
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

        #endregion

        #region [ Methods ]

        public override void Initialize(MeterDataSet meterDataSet)
        {
            FaultDataResource.Factory faultDataResourceFactory = new FaultDataResource.Factory()
            {
                DbAdapterContainer = m_dbAdapterContainer,
                MaxVoltage = m_maxVoltage,
                MaxCurrent = m_maxCurrent,
                LowVoltageThreshold = m_lowVoltageThreshold,
                MaxLowVoltageCurrent = m_maxLowVoltageCurrent,
                MaxTimeOffset = m_maxTimeOffset,
                MinTimeOffset = m_minTimeOffset,
                FaultSuppressionTrigger = m_faultSuppressionTrigger,
                PhaseCurrentTrigger = m_phaseCurrentTrigger,
                PrefaultTrigger = m_prefaultTrigger,
                ResidualCurrentTrigger = m_residualCurrentTrigger,
                MaxFaultDistanceMultiplier = m_maxFaultDistanceMultiplier,
                MinFaultDistanceMultiplier = m_minFaultDistanceMultiplier
            };

            CycleDataResource cycleDataResource = meterDataSet.GetResource<CycleDataResource>();
            FaultDataResource faultDataResource = meterDataSet.GetResource(faultDataResourceFactory.Create);
            List<Fault> faults;

            DataGroup dataGroup;
            VICycleDataGroup viCycleDataGroup;

            for (int i = 0; i < cycleDataResource.DataGroups.Count; i++)
            {
                dataGroup = cycleDataResource.DataGroups[i];
                viCycleDataGroup = cycleDataResource.VICycleDataGroups[i];

                if (!faultDataResource.FaultLookup.TryGetValue(dataGroup, out faults))
                    faults = new List<Fault>();

                m_classifications.Add(dataGroup, Classify(dataGroup, viCycleDataGroup, faults));
            }
        }

        private EventClassification Classify(DataGroup dataGroup, VICycleDataGroup viCycleDataGroup, List<Fault> faults)
        {
            double nominalVoltage;
            DataSeries va;
            DataSeries vb;
            DataSeries vc;

            if (faults.Any(fault => fault.Info.NumberOfValidDistances > 0))
                return EventClassification.Fault;

            // Get the line-to-neutral nominal voltage in volts
            nominalVoltage = dataGroup.Line.VoltageKV * 1000.0D / Math.Sqrt(3.0D);

            // Per-unit voltage waveforms based on nominal voltage
            va = viCycleDataGroup.VA.RMS.Multiply(1.0D / nominalVoltage);
            vb = viCycleDataGroup.VB.RMS.Multiply(1.0D / nominalVoltage);
            vc = viCycleDataGroup.VC.RMS.Multiply(1.0D / nominalVoltage);

            if (HasInterruption(va, vb, vc))
                return EventClassification.Interruption;

            if (HasSwell(va, vb, vc))
                return EventClassification.Swell;

            if (HasSag(va, vb, vc))
                return EventClassification.Sag;

            return EventClassification.Other;
        }

        private bool HasInterruption(params DataSeries[] seriesList)
        {
            IEnumerable<double> values;

            foreach (var series in seriesList)
            {
                values = series.DataPoints.Select(dataPoint => dataPoint.Value);

                if (values.Any(value => value <= 0.1D))
                    return true;
            }

            return false;
        }

        private bool HasSwell(params DataSeries[] seriesList)
        {
            IEnumerable<double> values;

            foreach (var series in seriesList)
            {
                values = series.DataPoints.Select(dataPoint => dataPoint.Value);

                if (values.Any(value => value >= 1.1D))
                    return true;
            }

            return false;
        }

        private bool HasSag(params DataSeries[] seriesList)
        {
            IEnumerable<double> values;

            foreach (var series in seriesList)
            {
                values = series.DataPoints.Select(dataPoint => dataPoint.Value);

                if (values.Any(value => value <= 0.9D))
                    return true;
            }

            return false;
        }

        #endregion
    }
}
