//******************************************************************************************************
//  CycleDataGroup.cs - Gbtc
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

using openXDA.Model;
using System;

namespace FaultData.DataAnalysis
{
    public class CycleDataGroup
    {
        #region [ Members ]

        // Constants
        private const int RMSIndex = 0;
        private const int PhaseIndex = 1;
        private const int PeakIndex = 2;
        private const int ErrorIndex = 3;
        private Asset m_asset;
        // Fields
        private DataGroup m_dataGroup;

        #endregion

        #region [ Constructors ]

        public CycleDataGroup(DataGroup dataGroup, Asset asset)
        {
            m_dataGroup = dataGroup;
            m_asset = asset;
        }

        #endregion

        #region [ Properties ]

        public DataSeries RMS
        {
            get
            {
                return m_dataGroup[RMSIndex];
            }
        }

        public DataSeries Phase
        {
            get
            {
                return m_dataGroup[PhaseIndex];
            }
        }

        public DataSeries Peak
        {
            get
            {
                return m_dataGroup[PeakIndex];
            }
        }

        public DataSeries Error
        {
            get
            {
                return m_dataGroup[ErrorIndex];
            }
        }

        public Asset Asset
        {
            get
            {
                return m_asset;
            }
        }
        #endregion

        #region [ Methods ]

        public DataGroup ToDataGroup()
        {
            return m_dataGroup;
        }

        public CycleDataGroup ToSubGroup(int startIndex, int endIndex)
        {
            return new CycleDataGroup(m_dataGroup.ToSubGroup(startIndex, endIndex), m_asset);
        }

        public CycleDataGroup ToSubGroup(DateTime startTime, DateTime endTime)
        {
            return new CycleDataGroup(m_dataGroup.ToSubGroup(startTime, endTime), m_asset);
        }

        #endregion
    }
}
