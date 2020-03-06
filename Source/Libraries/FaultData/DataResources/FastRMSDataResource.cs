//******************************************************************************************************
//  FastRMSDataResource.cs - Gbtc
//
//  Copyright © 2020, Grid Protection Alliance.  All Rights Reserved.
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
//  03/06/2020 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using FaultData.DataAnalysis;
using FaultData.DataSets;
using GSF;
using GSF.Collections;

namespace FaultData.DataResources
{
    public class FastRMSDataResource : DataResourceBase<MeterDataSet>
    {
        public Dictionary<DataGroup, DataGroup> FastRMSLookup { get; private set; }

        public FastRMSDataResource() =>
            FastRMSLookup = new Dictionary<DataGroup, DataGroup>();

        public override void Initialize(MeterDataSet meterDataSet)
        {
            DataGroupsResource dataGroupsResource = meterDataSet.GetResource<DataGroupsResource>();

            DataGroup GetFastRMS(DataGroup dataGroup) => dataGroupsResource.DataGroups
                .Where(fastRMS => fastRMS.Classification == DataClassification.FastRMS)
                .Where(fastRMS => Overlaps(dataGroup, fastRMS))
                .MinBy(fastRMS => fastRMS.StartTime);

            FastRMSLookup = dataGroupsResource.DataGroups
                .Where(dataGroup => dataGroup.Classification == DataClassification.Event)
                .Select(DataGroup => new { DataGroup, FastRMS = GetFastRMS(DataGroup) })
                .Where(mapping => mapping.FastRMS != null)
                .ToDictionary(mapping => mapping.DataGroup, mapping => mapping.FastRMS);
        }

        private static bool Overlaps(DataGroup dataGroup1, DataGroup dataGroup2)
        {
            Range<DateTime> range1 = new Range<DateTime>(dataGroup1.StartTime, dataGroup1.EndTime);
            Range<DateTime> range2 = new Range<DateTime>(dataGroup2.StartTime, dataGroup2.EndTime);
            return range1.Overlaps(range2);
        }
    }
}
