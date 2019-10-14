//******************************************************************************************************
//  MeterLocation.cs - Gbtc
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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GSF.Data;
using GSF.Data.Model;
using Newtonsoft.Json;

namespace openXDA.Model
{
    public class MeterLocation
    {
        #region [ Members ]

        // Fields
        private List<Meter> m_meters;
        private List<MeterLocationLine> m_meterLocationLines;

        #endregion

        #region [ Properties ]

        [PrimaryKey(true)]
        public int ID { get; set; }

        [StringLength(50)]
        [Searchable]
        [Required]
        public string AssetKey { get; set; }

        [StringLength(200)]
        [Searchable]
        [Required]
        public string Name { get; set; }

        [StringLength(200)]
        [Searchable]
        public string Alias { get; set; }

        [StringLength(50)]
        [Searchable]
        public string ShortName { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [JsonIgnore]
        [NonRecordField]
        public List<Meter> Meters
        {
            get
            {
                return m_meters ?? (m_meters = QueryMeters());
            }
            set
            {
                m_meters = value;
            }
        }

        [JsonIgnore]
        [NonRecordField]
        public List<MeterLocationLine> MeterLocationLines
        {
            get
            {
                return m_meterLocationLines ?? (m_meterLocationLines = QueryMeterLocationLines());
            }
            set
            {
                m_meterLocationLines = value;
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

        public IEnumerable<Meter> GetMeters(AdoDataConnection connection)
        {
            if ((object)connection == null)
                return null;

            TableOperations<Meter> meterTable = new TableOperations<Meter>(connection);
            return meterTable.QueryRecordsWhere("MeterLocationID = {0}", ID);
        }

        public IEnumerable<MeterLocationLine> GetMeterLocationLines(AdoDataConnection connection)
        {
            if ((object)connection == null)
                return null;

            TableOperations<MeterLocationLine> meterLocationLineTable = new TableOperations<MeterLocationLine>(connection);
            return meterLocationLineTable.QueryRecordsWhere("MeterLocationID = {0}", ID);
        }

        private List<Meter> QueryMeters()
        {
            List<Meter> meters;

            using (AdoDataConnection connection = ConnectionFactory?.Invoke())
            {
                meters = GetMeters(connection)?
                    .Select(LazyContext.GetMeter)
                    .ToList();
            }

            if ((object)meters != null)
            {
                foreach (Meter meter in meters)
                {
                    meter.MeterLocation = this;
                    meter.LazyContext = LazyContext;
                }
            }

            return meters;
        }

        private List<MeterLocationLine> QueryMeterLocationLines()
        {
            List<MeterLocationLine> meterLocationLines;

            using (AdoDataConnection connection = ConnectionFactory?.Invoke())
            {
                meterLocationLines = GetMeterLocationLines(connection)?
                    .Select(LazyContext.GetMeterLocationLine)
                    .ToList();
            }

            if ((object)meterLocationLines != null)
            {
                foreach (MeterLocationLine meterLocationLine in meterLocationLines)
                {
                    meterLocationLine.MeterLocation = this;
                    meterLocationLine.LazyContext = LazyContext;
                }
            }

            return meterLocationLines;
        }

        #endregion
    }
}
