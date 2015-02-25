//******************************************************************************************************
//  VIDataGroup.cs - Gbtc
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
//  08/29/2014 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;

namespace FaultData.DataAnalysis
{
    public class VIDataGroup
    {
        #region [ Members ]

        // Constants
        private const int VAIndex = 0;
        private const int VBIndex = 1;
        private const int VCIndex = 2;
        private const int IAIndex = 3;
        private const int IBIndex = 4;
        private const int ICIndex = 5;
        private const int IRIndex = 6;

        // Fields
        private DataGroup m_dataGroup;

        #endregion

        #region [ Constructors ]

        public VIDataGroup(DataGroup dataGroup)
        {
            DataSeries missingSeries;

            // Get all necessary voltage and current channels in the proper order
            List<DataSeries> viSeriesList = Enumerable.Range(0, 7)
                .GroupJoin(dataGroup.DataSeries.Where(IsInstantaneous), i => i, GetIndex, (i, series) => series.FirstOrDefault())
                .ToList();

            // Validate that no more than one channel is missing
            if (viSeriesList.Count(series => (object)series == null) > 1)
                throw new InvalidOperationException("Cannot create VIDataGroup from an incomplete set of voltage/current channels");

            // Attempt to fill in missing current channels
            // based on the relation IR = IA + IB + IC
            if ((object)viSeriesList[IAIndex] == null)
            {
                missingSeries = viSeriesList[IRIndex];
                missingSeries.Add(viSeriesList[IBIndex].Negate());
                missingSeries.Add(viSeriesList[ICIndex].Negate());
                viSeriesList[IAIndex] = missingSeries;
            }
            else if ((object)viSeriesList[IBIndex] == null)
            {
                missingSeries = viSeriesList[IRIndex];
                missingSeries.Add(viSeriesList[IAIndex].Negate());
                missingSeries.Add(viSeriesList[ICIndex].Negate());
                viSeriesList[IBIndex] = missingSeries;
            }
            else if ((object)viSeriesList[ICIndex] == null)
            {
                missingSeries = viSeriesList[IRIndex];
                missingSeries.Add(viSeriesList[IAIndex].Negate());
                missingSeries.Add(viSeriesList[IBIndex].Negate());
                viSeriesList[ICIndex] = missingSeries;
            }
            else if ((object)viSeriesList[IRIndex] == null)
            {
                missingSeries = viSeriesList[IAIndex];
                missingSeries.Add(viSeriesList[IBIndex]);
                missingSeries.Add(viSeriesList[ICIndex]);
                viSeriesList[IRIndex] = missingSeries;
            }

            // Validate that no channels are missing
            if (viSeriesList.Any(series => (object)series == null))
                throw new InvalidOperationException("Cannot create VIDataGroup from an incomplete set of voltage/current channels");

            // Create the data group from the complete
            // set of voltage/current channels
            m_dataGroup = new DataGroup(viSeriesList);
        }

        #endregion

        #region [ Properties ]

        public DataSeries VA
        {
            get
            {
                return m_dataGroup[VAIndex];
            }
        }

        public DataSeries VB
        {
            get
            {
                return m_dataGroup[VBIndex];
            }
        }

        public DataSeries VC
        {
            get
            {
                return m_dataGroup[VCIndex];
            }
        }

        public DataSeries IA
        {
            get
            {
                return m_dataGroup[IAIndex];
            }
        }

        public DataSeries IB
        {
            get
            {
                return m_dataGroup[IBIndex];
            }
        }

        public DataSeries IC
        {
            get
            {
                return m_dataGroup[ICIndex];
            }
        }

        public DataSeries IR
        {
            get
            {
                return m_dataGroup[IRIndex];
            }
        }

        #endregion

        #region [ Methods ]

        public DataGroup ToDataGroup()
        {
            return m_dataGroup;
        }

        public VIDataGroup ToSubGroup(int startIndex, int endIndex)
        {
            return new VIDataGroup(m_dataGroup.ToSubGroup(startIndex, endIndex));
        }

        private bool IsInstantaneous(DataSeries dataSeries)
        {
            string characteristicName = dataSeries.SeriesInfo.Channel.MeasurementCharacteristic.Name;
            string seriesTypeName = dataSeries.SeriesInfo.SeriesType.Name;

            return (characteristicName == "Instantaneous") &&
                   (seriesTypeName == "Values" || seriesTypeName == "Instantaneous");
        }

        private int GetIndex(DataSeries dataSeries)
        {
            Dictionary<string, int> indexLookup = new Dictionary<string, int>()
            {
                { "Voltage AN", VAIndex },
                { "Voltage BN", VBIndex },
                { "Voltage CN", VCIndex },
                { "Current AN", IAIndex },
                { "Current BN", IBIndex },
                { "Current CN", ICIndex },
                { "Current RES", IRIndex }
            };

            int index;
            string measurementType = dataSeries.SeriesInfo.Channel.MeasurementType.Name;
            string phase = dataSeries.SeriesInfo.Channel.Phase.Name;
            string key = string.Format("{0} {1}", measurementType, phase);

            return indexLookup.TryGetValue(key, out index) ? index : -1;
        }

        #endregion
    }
}
