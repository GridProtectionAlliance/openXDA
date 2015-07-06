//******************************************************************************************************
//  DataGroups.cs - Gbtc
//
//  Copyright © 2014, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the Eclipse Public License -v 1.0 (the "License"); you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/eclipse-1.0.php
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  07/21/2014 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using FaultData.DataAnalysis;
using FaultData.DataSets;
using Line = FaultData.Database.Line;

namespace FaultData.DataResources
{
    public class DataGroupsResource : DataResourceBase<MeterDataSet>
    {
        #region [ Members ]

        // Fields
        private List<DataGroup> m_dataGroups;

        #endregion

        #region [ Properties ]

        public List<DataGroup> DataGroups
        {
            get
            {
                return m_dataGroups;
            }
        }

        #endregion

        #region [ Methods ]

        public override void Initialize(MeterDataSet meterDataSet)
        {
            List<DataGroup> dataGroups;
            DataGroup dataGroup;

            dataGroups = new List<DataGroup>();

            foreach (IGrouping<Line, DataSeries> lineGroup in meterDataSet.DataSeries.Concat(meterDataSet.Digitals).GroupBy(GetLine))
            {
                foreach (IGrouping<DateTime, DataSeries> startTimeGroup in lineGroup.GroupBy(dataSeries => dataSeries.DataPoints[0].Time))
                {
                    foreach (IGrouping<DateTime, DataSeries> endTimeGroup in startTimeGroup.GroupBy(dataSeries => dataSeries.DataPoints[dataSeries.DataPoints.Count - 1].Time))
                    {
                        foreach (IGrouping<int, DataSeries> sampleCountGroup in endTimeGroup.GroupBy(dataSeries => dataSeries.DataPoints.Count))
                        {
                            dataGroup = new DataGroup();

                            foreach (DataSeries dataSeries in sampleCountGroup)
                                dataGroup.Add(dataSeries);

                            dataGroups.Add(dataGroup);
                        }
                    }
                }
            }

            m_dataGroups = dataGroups;
        }

        private Line GetLine(DataSeries dataSeries)
        {
            if ((object)dataSeries.SeriesInfo != null)
                return dataSeries.SeriesInfo.Channel.Line;

            return null;
        }

        #endregion
    }
}
