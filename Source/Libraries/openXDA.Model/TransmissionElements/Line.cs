//******************************************************************************************************
//  Line.cs - Gbtc
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
    public class Line
    {
        #region [ Members ]

        // Fields
        private List<MeterLocationLine> m_meterLocationLines;
        private List<MeterLine> m_meterLines;
        private List<Channel> m_channels;
        private LineImpedance m_lineImpedance;

        #endregion

        #region [ Properties ]

        [PrimaryKey(true)]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        [Searchable]
        public string AssetKey { get; set; }

        [Required]
        public double VoltageKV { get; set; }

        [Required]
        public double ThermalRating { get; set; }

        [Required]
        public double Length { get; set; }

        public double? MaxFaultDistance { get; set; }

        public double? MinFaultDistance { get; set; }

        public string Description { get; set; }

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
        public List<MeterLine> MeterLines
        {
            get
            {
                return m_meterLines ?? (m_meterLines = QueryMeterLines());
            }
            set
            {
                m_meterLines = value;
            }
        }

        [JsonIgnore]
        [NonRecordField]
        public List<Channel> Channels
        {
            get
            {
                return m_channels ?? (m_channels = QueryChannels());
            }
            set
            {
                m_channels = value;
            }
        }

        [JsonIgnore]
        [NonRecordField]
        public LineImpedance LineImpedance
        {
            get
            {
                return m_lineImpedance ?? (m_lineImpedance ?? QueryLineImpedance());
            }
            set
            {
                m_lineImpedance = value;
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

        public IEnumerable<MeterLocationLine> GetMeterLocationLines(AdoDataConnection connection)
        {
            if ((object)connection == null)
                return null;

            TableOperations<MeterLocationLine> meterLocationLineTable = new TableOperations<MeterLocationLine>(connection);
            return meterLocationLineTable.QueryRecordsWhere("LineID = {0}", ID);
        }

        public IEnumerable<MeterLine> GetMeterLines(AdoDataConnection connection)
        {
            if ((object)connection == null)
                return null;

            TableOperations<MeterLine> meterLineTable = new TableOperations<MeterLine>(connection);
            return meterLineTable.QueryRecordsWhere("LineID = {0}", ID);
        }

        public IEnumerable<Channel> GetChannel(AdoDataConnection connection)
        {
            if ((object)connection == null)
                return null;

            TableOperations<Channel> channelTable = new TableOperations<Channel>(connection);
            return channelTable.QueryRecordsWhere("LineID = {0}", ID);
        }

        public LineImpedance GetLineImpedance(AdoDataConnection connection)
        {
            if ((object)connection == null)
                return null;

            TableOperations<LineImpedance> lineImpedanceTable = new TableOperations<LineImpedance>(connection);
            return lineImpedanceTable.QueryRecordWhere("LineID = {0}", ID);
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
                    meterLocationLine.Line = this;
                    meterLocationLine.LazyContext = LazyContext;
                }
            }

            return meterLocationLines;
        }

        private List<MeterLine> QueryMeterLines()
        {
            List<MeterLine> meterLines;

            using (AdoDataConnection connection = ConnectionFactory?.Invoke())
            {
                meterLines = GetMeterLines(connection)?
                    .Select(LazyContext.GetMeterLine)
                    .ToList();
            }

            if ((object)meterLines != null)
            {
                foreach (MeterLine meterLine in meterLines)
                {
                    meterLine.Line = this;
                    meterLine.LazyContext = LazyContext;
                }
            }

            return meterLines;
        }

        private List<Channel> QueryChannels()
        {
            List<Channel> channels;

            using (AdoDataConnection connection = ConnectionFactory?.Invoke())
            {
                channels = GetChannel(connection)?
                    .Select(LazyContext.GetChannel)
                    .ToList();
            }

            if ((object)channels != null)
            {
                foreach (Channel channel in channels)
                {
                    channel.Line = this;
                    channel.LazyContext = LazyContext;
                }
            }

            return channels;
        }

        private LineImpedance QueryLineImpedance()
        {
            LineImpedance lineImpedance;

            using (AdoDataConnection connection = ConnectionFactory?.Invoke())
            {
                lineImpedance = GetLineImpedance(connection);
            }

            if ((object)lineImpedance != null)
                lineImpedance.LazyContext = LazyContext;

            return LazyContext.GetLineImpedance(lineImpedance);
        }

        #endregion
    }
}
