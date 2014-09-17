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

        // Fields
        private DataGroup m_dataGroup;

        #endregion

        #region [ Constructors ]

        public VIDataGroup(DataGroup dataGroup)
        {
            m_dataGroup = dataGroup;
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

        #endregion
    }
}
