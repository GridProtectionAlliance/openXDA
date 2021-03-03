//******************************************************************************************************
//  SystemEventResource.cs - Gbtc
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
//  04/16/2015 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using FaultData.DataAnalysis;
using FaultData.DataSets;
using GSF;
using GSF.Configuration;
using openXDA.Configuration;

namespace FaultData.DataResources
{
    public class SystemEventResource : DataResourceBase<MeterDataSet>
    {
        #region [ Members ]

        // Nested Types
        public class SystemEvent
        {
            public DateTime StartTime;
            public DateTime EndTime;

            public bool Overlaps(SystemEvent systemEvent)
            {
                return (StartTime <= systemEvent.StartTime && systemEvent.EndTime <= EndTime) ||
                       (systemEvent.StartTime <= StartTime && StartTime <= systemEvent.EndTime);
            }

            public void MergeWith(SystemEvent systemEvent)
            {
                StartTime = Common.Min(StartTime, systemEvent.StartTime);
                EndTime = Common.Max(EndTime, systemEvent.EndTime);
            }
        }

        #endregion

        #region [ Properties ]

        [Category]
        [SettingName(DataAnalysisSection.CategoryName)]
        public DataAnalysisSection DataAnalysisSettings { get; }
            = new DataAnalysisSection();

        public List<SystemEvent> SystemEvents { get; private set; }

        #endregion

        #region [ Methods ]

        public override void Initialize(MeterDataSet meterDataSet)
        {
            CycleDataResource cycleDataResource = meterDataSet.GetResource<CycleDataResource>();
            SystemEvents = GetSystemEvents(cycleDataResource.DataGroups);
        }

        private List<SystemEvent> GetSystemEvents(IEnumerable<DataGroup> dataGroups)
        {
            double timeTolerance = DataAnalysisSettings.TimeTolerance;

            return dataGroups
                .OrderBy(dataGroup => dataGroup.StartTime)
                .Select(dataGroup => new SystemEvent()
                {
                    StartTime = dataGroup.StartTime.AddSeconds(-timeTolerance),
                    EndTime = dataGroup.EndTime.AddSeconds(timeTolerance),
                })
                .Aggregate(new List<SystemEvent>(), (list, systemEvent) =>
                {
                    SystemEvent last = list.LastOrDefault();

                    if ((object)last != null && last.Overlaps(systemEvent))
                        last.MergeWith(systemEvent);
                    else
                        list.Add(systemEvent);

                    return list;
                });
        }

        #endregion
    }
}
