//******************************************************************************************************
//  LineImpedance.cs - Gbtc
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
//  08/29/2017 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using GSF.Data;
using GSF.Data.Model;
using Newtonsoft.Json;

namespace openXDA.Model
{
    public class LineImpedance
    {
        #region [ Members ]

        // Fields
        private Line m_line;

        #endregion

        #region [ Properties ]

        [PrimaryKey(true)]
        public int ID { get; set; }

        public int LineID { get; set; }

        public double R0 { get; set; }

        public double R1 { get; set; }

        public double X0 { get; set; }

        public double X1 { get; set; }

        [JsonIgnore]
        [NonRecordField]
        public Line Line
        {
            get
            {
                return m_line ?? (m_line = QueryLine());
            }
            set
            {
                m_line = value;
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

        public Line GetLine(AdoDataConnection connection)
        {
            if ((object)connection == null)
                return null;

            TableOperations<Line> lineTable = new TableOperations<Line>(connection);
            return lineTable.QueryRecordWhere("ID = {0}", LineID);
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

        #endregion
    }
}
