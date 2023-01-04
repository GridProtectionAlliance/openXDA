﻿//******************************************************************************************************
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

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using FaultData.DataAnalysis;
using FaultData.DataSets;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using openXDA.Configuration;
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
        Snapshot,
        MeterDataQuality
    }

    public class EventClassificationResource : DataResourceBase<MeterDataSet>
    {
        #region [ Properties ]

        public Dictionary<DataGroup, EventClassification> Classifications { get; }
            = new Dictionary<DataGroup, EventClassification>();

        [Category]
        [SettingName(BreakerSection.CategoryName)]
        public BreakerSection BreakerSettings { get; }
            = new BreakerSection();

        [Category]
        [SettingName(FaultLocationSection.CategoryName)]
        public FaultLocationSection FaultLocationSettings { get; }
            = new FaultLocationSection();

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

                EventClassification classification = Classify(meterDataSet, dataGroup, viDataGroup, faultGroup);

                if (!ValidateEventType(classification, dataGroup.Asset))
                    classification = EventClassification.Other;

                Classifications.Add(dataGroup, classification);
            }

            foreach (DataGroup dataGroup in dataGroupsResource.DataGroups)
            {
                if (dataGroup.DataSeries.Count > 0)
                    continue;

                EventClassification classification = Classify(meterDataSet, dataGroup);

                if (!(dataGroup.Asset is null) && !ValidateEventType(classification, dataGroup.Asset))
                    classification = EventClassification.Other;

                Classifications.Add(dataGroup, classification);
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

                if (!faultGroup.FaultDetectionLogicResult.HasValue && FaultLocationSettings.UseDefaultFaultDetectionLogic)
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

        private static bool ValidateEventType(EventClassification eventClassification, Asset asset)
        {
            if (eventClassification == EventClassification.Other)
                return true;

            using (AdoDataConnection connection = asset.ConnectionFactory())
            {
                EventType evtType = new TableOperations<EventType>(connection).QueryRecordWhere("Name = {0}", eventClassification.ToString());
                if (evtType is null)
                    return false;

                EventTypeAssetType evtAssetType = new TableOperations<EventTypeAssetType>(connection).QueryRecordWhere("EventTypeID = {0} AND AssetTypeID = {1}", evtType.ID, asset.AssetTypeID);
                return !(evtAssetType is null);
            }        
        }

        private static bool HasBreakerChannels(DataGroup dataGroup)
        {
            using (AdoDataConnection connection = dataGroup.Asset.ConnectionFactory())
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
