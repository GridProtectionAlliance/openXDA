//******************************************************************************************************
//  BreakerOpenResource.cs - Gbtc
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
using System.Linq;
using FaultData.Configuration;
using FaultData.DataAnalysis;
using FaultData.DataSets;
using GSF.Configuration;
using GSF.PQDIF.Logical;

namespace FaultData.DataResources
{
    public class BreakerOpenResource : DataResourceBase<MeterDataSet>
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

        #endregion

        #region [ Properties ]

        [Category]
        [SettingName(BreakerSettings.CategoryName)]
        public BreakerSettings BreakerSettings { get; } = new BreakerSettings();

        public Dictionary<DataGroup, List<Trip>> TripLookup { get; } = new Dictionary<DataGroup, List<Trip>>();

        #endregion

        #region [ Methods ]

        public override void Initialize(MeterDataSet meterDataSet)
        {
            CycleDataResource cycleDataResource = meterDataSet.GetResource<CycleDataResource>();

            for (int i = 0; i < cycleDataResource.DataGroups.Count; i++)
            {
                DataGroup dataGroup = cycleDataResource.DataGroups[i];
                VICycleDataGroup viCycleDataGroup = cycleDataResource.VICycleDataGroups[i];

                IEnumerable<Trip> iaTrips = FindBreakerTrips(viCycleDataGroup.IA?.Peak)
                    .Select(point => new Trip(Phase.AN, point.Time));

                IEnumerable<Trip> ibTrips = FindBreakerTrips(viCycleDataGroup.IB?.Peak)
                    .Select(point => new Trip(Phase.BN, point.Time));

                IEnumerable<Trip> icTrips = FindBreakerTrips(viCycleDataGroup.IC?.Peak)
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

        private IEnumerable<DataPoint> FindBreakerTrips(DataSeries rms)
        {
            if ((object)rms == null)
                return new List<DataPoint>();

            double threshold = BreakerSettings.OpenBreakerThreshold;

            return rms.DataPoints
                .Zip(rms.DataPoints.Skip(1), (P1, P2) => new { P1, P2 })
                .Where(obj => obj.P1.Value > threshold && obj.P2.Value <= threshold)
                .Select(obj => obj.P2);
        }

        #endregion
    }
}
