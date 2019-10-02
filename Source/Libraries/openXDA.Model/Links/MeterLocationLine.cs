//******************************************************************************************************
//  MeterLocationLine.cs - Gbtc
//
//  Copyright © 2017, Grid Protection Alliance.  All Rights Reserved.
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
//  06/19/2017 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using GSF.Data;
using GSF.Data.Model;
using Newtonsoft.Json;

namespace openXDA.Model
{
    public class MeterLocationLine
    {
        #region [ Members ]

        // Fields
        private MeterLocation m_meterLocation;
        private Line m_line;
        private SourceImpedance m_sourceImpedance;

        #endregion

        #region [ Properties ]

        [PrimaryKey(true)]
        public int ID { get; set; }

        public int MeterLocationID { get; set; }

        public int LineID { get; set; }

        [JsonIgnore]
        [NonRecordField]
        public MeterLocation MeterLocation
        {
            get
            {
                return m_meterLocation ?? (m_meterLocation = QueryMeterLocation());
            }
            set
            {
                m_meterLocation = value;
            }
        }

        [JsonIgnore]
        [NonRecordField]
        public Line Line
        {
            get
            {
                return m_line ?? (m_line ?? QueryLine());
            }
            set
            {
                m_line = value;
            }
        }

        [JsonIgnore]
        [NonRecordField]
        public SourceImpedance SourceImpedance
        {
            get
            {
                return m_sourceImpedance ?? (m_sourceImpedance ?? QuerySourceImpedance());
            }
            set
            {
                m_sourceImpedance = value;
            }
        }

        [JsonIgnore]
        [NonRecordField]
        public Func<AdoDataConnection> ConnectionFactory
        {
            get
            {
                return LazyContext.ConnectionFactory;
            }
            set
            {
                LazyContext.ConnectionFactory = value;
            }
        }

        [JsonIgnore]
        [NonRecordField]
        internal LazyContext LazyContext { get; set; } = new LazyContext();

        #endregion

        #region [ Methods ]

        public MeterLocation GetMeterLocation(AdoDataConnection connection)
        {
            if ((object)connection == null)
                return null;

            TableOperations<MeterLocation> meterLocationTable = new TableOperations<MeterLocation>(connection);
            return meterLocationTable.QueryRecordWhere("ID = {0}", MeterLocationID);
        }

        public Line GetLine(AdoDataConnection connection)
        {
            if ((object)connection == null)
                return null;

            TableOperations<Line> lineTable = new TableOperations<Line>(connection);
            return lineTable.QueryRecordWhere("ID = {0}", LineID);
        }

        public SourceImpedance GetSourceImpedance(AdoDataConnection connection)
        {
            if ((object)connection == null)
                return null;

            TableOperations<SourceImpedance> sourceImpedanceTable = new TableOperations<SourceImpedance>(connection);
            return sourceImpedanceTable.QueryRecordWhere("MeterLocationLineID = {0}", ID);
        }

        private MeterLocation QueryMeterLocation()
        {
            MeterLocation meterLocation;

            using (AdoDataConnection connection = ConnectionFactory?.Invoke())
            {
                meterLocation = GetMeterLocation(connection);
            }

            if ((object)meterLocation != null)
                meterLocation.LazyContext = LazyContext;

            return LazyContext.GetMeterLocation(meterLocation);
        }

        private Line QueryLine()
        {
            Line line;

            using (AdoDataConnection connection = ConnectionFactory?.Invoke())
            {
                line = GetLine(connection);
            }

            if ((object)line != null)
                line.LazyContext = LazyContext;

            return LazyContext.GetLine(line);
        }

        private SourceImpedance QuerySourceImpedance()
        {
            SourceImpedance sourceImpedance;

            using (AdoDataConnection connection = ConnectionFactory?.Invoke())
            {
                sourceImpedance = GetSourceImpedance(connection);
            }

            if ((object)sourceImpedance != null)
                sourceImpedance.LazyContext = LazyContext;

            return LazyContext.GetSourceImpedance(sourceImpedance);
        }

        #endregion
    }
}
