//******************************************************************************************************
//  Meter.cs - Gbtc
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
using GSF.ComponentModel.DataAnnotations;
using GSF.Data;
using GSF.Data.Model;
using Newtonsoft.Json;

namespace openXDA.Model
{
    public class Meter
    {
        #region [ Members ]

        // Fields
        private MeterLocation m_meterLocation;
        private List<MeterLine> m_meterLines;
        private List<Channel> m_channels;

        #endregion

        #region [ Properties ]

        [PrimaryKey(true)]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string AssetKey { get; set; }

        [Required]
        [Label("Location")]
        public int MeterLocationID { get; set; }

        [Required]
        [Searchable]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Alias { get; set; }

        [StringLength(12)]
        public string ShortName { get; set; }

        [Searchable]
        [Required]
        [StringLength(200)]
        public string Make { get; set; }

        [Searchable]
        [Required]
        [StringLength(200)]
        public string Model { get; set; }


        [StringLength(200)]
        public string TimeZone { get; set; }

        public string Description { get; set; }

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

        public IEnumerable<MeterLine> GetMeterLines(AdoDataConnection connection)
        {
            if ((object)connection == null)
                return null;

            TableOperations<MeterLine> meterLineTable = new TableOperations<MeterLine>(connection);
            return meterLineTable.QueryRecordsWhere("MeterID = {0}", ID);
        }

        public IEnumerable<Channel> GetChannels(AdoDataConnection connection)
        {
            if ((object)connection == null)
                return null;

            TableOperations<Channel> channelTable = new TableOperations<Channel>(connection);
            return channelTable.QueryRecordsWhere("MeterID = {0}", ID);
        }

        public TimeZoneInfo GetTimeZoneInfo(TimeZoneInfo defaultTimeZone)
        {
            if (!string.IsNullOrEmpty(TimeZone))
                return TimeZoneInfo.FindSystemTimeZoneById(TimeZone);

            return defaultTimeZone;
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
                    meterLine.Meter = this;
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
                channels = GetChannels(connection)?
                    .Select(LazyContext.GetChannel)
                    .ToList();
            }

            if ((object)channels != null)
            {
                foreach (Channel channel in channels)
                {
                    channel.Meter = this;
                    channel.LazyContext = LazyContext;
                }
            }

            return channels;
        }

        #endregion
    }

    public class MeterDetail : Meter
    {
        [Searchable]
        public string Location { get; set; }

        public string TimeZoneLabel
        {
            get
            {
                try
                {
                    if (TimeZone != "UTC")
                        return TimeZoneInfo.FindSystemTimeZoneById(TimeZone).ToString();
                }
                catch
                {
                    // Do not fail if the time zone cannot be found --
                    // instead, fall through to the logic below to
                    // find the label for UTC
                }

                return TimeZoneInfo.GetSystemTimeZones()
                    .Where(info => info.Id == "UTC")
                    .DefaultIfEmpty(TimeZoneInfo.Utc)
                    .First()
                    .ToString();
            }
        }
    }
}
