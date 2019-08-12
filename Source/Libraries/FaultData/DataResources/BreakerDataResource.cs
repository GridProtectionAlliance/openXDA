//******************************************************************************************************
//  BreakerDataResource.cs - Gbtc
//
//  Copyright © 2019, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may not use this
//  file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  07/19/2019 - Stephen C. Wills
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
using GSF.Collections;
using GSF.Configuration;
using GSF.PQDIF.Logical;

namespace FaultData.DataResources
{
    public class BreakerDataResource : DataResourceBase<MeterDataSet>
    {
        #region [ Members ]

        // Nested Types
        public class Trip
        {
            public Trip(Phase phase, DateTime timestamp)
            {
                Phase = phase;
                Timestamp = timestamp;
            }

            public Phase Phase { get; }
            public DateTime Timestamp { get; }
        }

        public class Restrike
        {
            public Restrike(Phase phase, DateTime timestamp)
            {
                Phase = phase;
                Timestamp = timestamp;
            }

            public Phase Phase { get; }
            public DateTime Timestamp { get; }
        }

        #endregion

        #region [ Properties ]

        [Setting]
        public double SystemFrequency { get; set; }

        [Category]
        [SettingName(BreakerSettings.CategoryName)]
        public BreakerSettings BreakerSettings { get; } = new BreakerSettings();

        public Dictionary<DataGroup, List<Trip>> TripLookup { get; } = new Dictionary<DataGroup, List<Trip>>();

        public Dictionary<DataGroup, List<Restrike>> RestrikeLookup { get; } = new Dictionary<DataGroup, List<Restrike>>();

        #endregion

        #region [ Methods ]

        public override void Initialize(MeterDataSet meterDataSet)
        {
            CycleDataResource cycleDataResource = meterDataSet.GetResource<CycleDataResource>();
            FindAllTrips(cycleDataResource);
            FindAllRestrikes(cycleDataResource);
        }

        private void FindAllTrips(CycleDataResource cycleDataResource)
        {
            for (int i = 0; i < cycleDataResource.DataGroups.Count; i++)
            {
                DataGroup dataGroup = cycleDataResource.DataGroups[i];
                VICycleDataGroup viCycleDataGroup = cycleDataResource.VICycleDataGroups[i];

                IEnumerable<Trip> iaTrips = FindTrips(viCycleDataGroup.IA?.Peak)
                    .Select(point => new Trip(Phase.AN, point.Time));

                IEnumerable<Trip> ibTrips = FindTrips(viCycleDataGroup.IB?.Peak)
                    .Select(point => new Trip(Phase.BN, point.Time));

                IEnumerable<Trip> icTrips = FindTrips(viCycleDataGroup.IC?.Peak)
                    .Select(point => new Trip(Phase.CN, point.Time));

                List<Trip> allTrips = Enumerable.Empty<Trip>()
                    .Concat(iaTrips)
                    .Concat(ibTrips)
                    .Concat(icTrips)
                    .ToList();

                if (allTrips.Any())
                    TripLookup.Add(dataGroup, allTrips);
            }
        }

        private IEnumerable<DataPoint> FindTrips(DataSeries peak)
        {
            if ((object)peak == null)
                return new List<DataPoint>();

            double threshold = BreakerSettings.OpenBreakerThreshold;

            return peak.DataPoints
                .Zip(peak.DataPoints.Skip(1), (P1, P2) => new { P1, P2 })
                .Where(obj => obj.P1.Value > threshold && obj.P2.Value <= threshold)
                .Select(obj => obj.P2);
        }

        private void FindAllRestrikes(CycleDataResource cycleDataResource)
        {
            for (int i = 0; i < cycleDataResource.DataGroups.Count; i++)
            {
                DataGroup dataGroup = cycleDataResource.DataGroups[i];
                VIDataGroup viDataGroup = cycleDataResource.VIDataGroups[i];

                IEnumerable<Restrike> iaRestrikes = FindRestrikes(viDataGroup.IA)
                    .Select(point => new Restrike(Phase.AN, point.Time));

                IEnumerable<Restrike> ibRestrikes = FindRestrikes(viDataGroup.IB)
                    .Select(point => new Restrike(Phase.BN, point.Time));

                IEnumerable<Restrike> icRestrikes = FindRestrikes(viDataGroup.IC)
                    .Select(point => new Restrike(Phase.CN, point.Time));

                List<Restrike> allRestrikes = Enumerable.Empty<Restrike>()
                    .Concat(iaRestrikes)
                    .Concat(ibRestrikes)
                    .Concat(icRestrikes)
                    .ToList();

                if (allRestrikes.Any())
                    RestrikeLookup.Add(dataGroup, allRestrikes);
            }
        }

        private List<DataPoint> FindRestrikes(DataSeries waveform)
        {
            List<DataPoint> restrikes = new List<DataPoint>();

            if (waveform == null)
                return restrikes;

            double minSecondsBeforeRestrike = BreakerSettings.MinCyclesBeforeRestrike / SystemFrequency;
            double maxSecondsBeforeRestrike = BreakerSettings.MaxCyclesBeforeRestrike / SystemFrequency;
            TimeSpan minTimeBeforeRestrike = TimeSpan.FromSeconds(minSecondsBeforeRestrike);
            TimeSpan maxTimeBeforeRestrike = TimeSpan.FromSeconds(maxSecondsBeforeRestrike);
            int startIndex = -1;

            for (int i = 0; i < waveform.DataPoints.Count; i++)
            {
                DataPoint dataPoint = waveform[i];
                bool closeToZero = Math.Abs(dataPoint.Value) < BreakerSettings.OpenBreakerThreshold;

                if (closeToZero && startIndex < 0)
                    startIndex = i;

                if (!closeToZero)
                {
                    // If the start of the region is the start of the waveform,
                    // we can't tell when the region actually started so it is
                    // therefore not a valid candidate for breaker restrike
                    if (startIndex > 0)
                    {
                        DataPoint start = waveform[startIndex];
                        DataPoint end = waveform[i - 1];
                        TimeSpan duration = end.Time - start.Time;

                        if (minTimeBeforeRestrike <= duration && duration <= maxTimeBeforeRestrike)
                            restrikes.Add(waveform[i]);
                    }

                    startIndex = -1;
                }
            }

            return restrikes;
        }

        #endregion
    }
}
