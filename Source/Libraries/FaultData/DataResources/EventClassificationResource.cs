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
        Other
    }

    public class EventClassificationResource : DataResourceBase<MeterDataSet>
    {
        #region [ Members ]

        // Fields
        private DbAdapterContainer m_dbAdapterContainer;
        private Dictionary<DataGroup, EventClassification> m_classifications;

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
            CycleDataResource cycleDataResource = meterDataSet.GetResource<CycleDataResource>();
            FaultDataResource faultDataResource = meterDataSet.GetResource(() => new FaultDataResource(m_dbAdapterContainer));
            FaultGroup faultGroup;

            DataGroup dataGroup;
            VICycleDataGroup viCycleDataGroup;

            for (int i = 0; i < cycleDataResource.DataGroups.Count; i++)
            {
                dataGroup = cycleDataResource.DataGroups[i];
                viCycleDataGroup = cycleDataResource.VICycleDataGroups[i];

                if (!faultDataResource.FaultLookup.TryGetValue(dataGroup, out faultGroup))
                    faultGroup = null;

                m_classifications.Add(dataGroup, Classify(meterDataSet, dataGroup, viCycleDataGroup, faultGroup));
            }
        }

        private EventClassification Classify(MeterDataSet meterDataSet, DataGroup dataGroup, VICycleDataGroup viCycleDataGroup, FaultGroup faultGroup)
        {
            double nominalVoltage;
            DataSeries va;
            DataSeries vb;
            DataSeries vc;

            if ((object)faultGroup != null && (faultGroup.FaultDetectionLogicResult ?? faultGroup.FaultValidationLogicResult))
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

            foreach (DataSeries series in seriesList)
            {
                values = series.DataPoints.Select(dataPoint => dataPoint.Value);

                if (values.Any(value => value <= m_interruptionThreshold))
                    return true;
            }

            return false;
        }

        private bool HasSwell(params DataSeries[] seriesList)
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

        private bool HasSag(params DataSeries[] seriesList)
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

        #endregion
    }
}
