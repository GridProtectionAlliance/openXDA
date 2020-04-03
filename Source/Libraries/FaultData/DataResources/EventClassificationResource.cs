//******************************************************************************************************
//  EventClassificationResource.cs - Gbtc
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
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using FaultData.Configuration;
using FaultData.DataAnalysis;
using FaultData.DataSets;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using openXDA.Model;
using FaultGroup = FaultData.DataAnalysis.FaultGroup;

namespace FaultData.DataResources
{
    public enum EventClassification
    {
        Fault,
        RecloseIntoFault,
        BreakerOpen,
        Interruption,
        Sag,
        Swell,
        Transient,
        Other,
        Breaker,
        Snapshot
    }

    public class EventClassificationResource : DataResourceBase<MeterDataSet>
    {
        #region [ Members ]

        // Fields
        private Dictionary<DataGroup, EventClassification> m_classifications;

        private double m_systemFrequency;
        private double m_sagThreshold;
        private double m_swellThreshold;
        private double m_interruptionThreshold;
        private BreakerSettings m_breakerSettings;
        private FaultLocationSettings m_faultLocationSettings;

        #endregion

        #region [ Constructors ]

        public EventClassificationResource()
        {
            m_classifications = new Dictionary<DataGroup, EventClassification>();
            m_breakerSettings = new BreakerSettings();
            m_faultLocationSettings = new FaultLocationSettings();
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

        [Category]
        [SettingName(BreakerSettings.CategoryName)]
        public BreakerSettings BreakerSettings
        {
            get
            {
                return m_breakerSettings;
            }
        }

        [Category]
        [SettingName(FaultLocationSettings.CategoryName)]
        public FaultLocationSettings FaultLocationSettings
        {
            get
            {
                return m_faultLocationSettings;
            }
        }

        #endregion

        #region [ Methods ]

        public override void Initialize(MeterDataSet meterDataSet)
        {
            DataGroupsResource dataGroupsResource = meterDataSet.GetResource<DataGroupsResource>();
            CycleDataResource cycleDataResource = meterDataSet.GetResource<CycleDataResource>();
            FaultDataResource faultDataResource = meterDataSet.GetResource<FaultDataResource>();

            for (int i = 0; i < cycleDataResource.DataGroups.Count; i++)
            {
                DataGroup dataGroup = cycleDataResource.DataGroups[i];
                VIDataGroup viDataGroup = cycleDataResource.VIDataGroups[i];
                FaultGroup faultGroup;

                if (!faultDataResource.FaultLookup.TryGetValue(dataGroup, out faultGroup))
                    faultGroup = null;

                m_classifications.Add(dataGroup, Classify(meterDataSet, dataGroup, viDataGroup, faultGroup));
            }

            foreach (DataGroup dataGroup in dataGroupsResource.DataGroups)
            {
                if (dataGroup.DataSeries.Count > 0)
                    continue;

                m_classifications.Add(dataGroup, Classify(meterDataSet, dataGroup));
            }
        }

        private EventClassification Classify(MeterDataSet meterDataSet, DataGroup dataGroup, VIDataGroup viDataGroup = null, FaultGroup faultGroup = null)
        {
            SnapshotDataResource snapshotDataResource = meterDataSet.GetResource<SnapshotDataResource>();
            if (snapshotDataResource.IsSnapshot)
                return EventClassification.Snapshot;

            if ((object)viDataGroup != null)
            {
                if (viDataGroup.DefinedNeutralVoltages == 0 && viDataGroup.DefinedLineVoltages == 0 && HasBreakerChannels(dataGroup))
                    return EventClassification.Breaker;
            }

            if ((object)faultGroup != null)
            {
                if (faultGroup.FaultDetectionLogicResult ?? false)
                    return EventClassification.Fault;

                if (!faultGroup.FaultDetectionLogicResult.HasValue && m_faultLocationSettings.UseDefaultFaultDetectionLogic)
                {
                    if (faultGroup.FaultValidationLogicResult && faultGroup.Faults.Any(fault => !fault.IsSuppressed && !fault.IsReclose))
                        return EventClassification.Fault;

                    if (faultGroup.FaultValidationLogicResult && faultGroup.Faults.Any(fault => !fault.IsSuppressed && fault.IsReclose))
                        return EventClassification.RecloseIntoFault;
                }
            }

            if (BreakerSettings.BreakerOpenEventTypeEnabled)
            {
                BreakerDataResource breakerOpenResource = meterDataSet.GetResource<BreakerDataResource>();

                if (breakerOpenResource.TripLookup.TryGetValue(dataGroup, out List<BreakerDataResource.Trip> trips))
                    return EventClassification.BreakerOpen;
            }

            InterruptionDataResource interruptionDataResource = meterDataSet.GetResource<InterruptionDataResource>();
            SagDataResource sagDataResource = meterDataSet.GetResource<SagDataResource>();
            SwellDataResource swellDataResource = meterDataSet.GetResource<SwellDataResource>();
            TransientDataResource transientDataResource = meterDataSet.GetResource<TransientDataResource>();

            if (interruptionDataResource.Interruptions.ContainsKey(dataGroup))
                return EventClassification.Interruption;

            if (sagDataResource.Sags.ContainsKey(dataGroup))
                return EventClassification.Sag;

            if (swellDataResource.Swells.ContainsKey(dataGroup))
                return EventClassification.Swell;

            if (transientDataResource.Transients.ContainsKey(dataGroup))
                return EventClassification.Transient;

            return EventClassification.Other;
        }

        private static bool HasBreakerChannels(DataGroup dataGroup)
        {
            using (AdoDataConnection connection = dataGroup.Line.ConnectionFactory())
            {
                TableOperations<BreakerChannel> breakerChannelTable = new TableOperations<BreakerChannel>(connection);

                List<int> channelIDs = dataGroup.DataSeries
                    .Select(dataSeries => dataSeries.SeriesInfo.Channel.ID)
                    .ToList();

                int count = breakerChannelTable.QueryRecordCountWhere($"ChannelID IN ({string.Join(",", channelIDs)})");

                if (count > 0)
                    return true;
            }

            return false;
        }

        #endregion
    }
}
