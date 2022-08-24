﻿//******************************************************************************************************
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
//  12/13/2019 - C. Lackner
//       Updated to fit in new Asset based model structure.
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
    [AllowSearch]
    [SettingsCategory("systemSettings")]
    [PostRoles("Administrator, Transmission SME")]
    [DeleteRoles("Administrator, Transmission SME")]
    [PatchRoles("Administrator, Transmission SME")]
    public class Meter
    {
        #region [ Members ]

        // Fields
        private Location m_location;
        private List<MeterAsset> m_meterAssets;
        private List<Channel> m_channels;

        #endregion

        #region [ Properties ]

        [PrimaryKey(true)]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        [DefaultSortOrder]
        public string AssetKey { get; set; }

        [Required]
        [Label("Location")]
        public int LocationID { get; set; }

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
        public Location Location
        {
            get
            {
                return m_location ?? (m_location = QueryLocation());
            }
            set
            {
                m_location = value;
            }
        }

        [JsonIgnore]
        [NonRecordField]
        public List<MeterAsset> MeterAssets
        {
            get
            {
                return m_meterAssets ?? (m_meterAssets = QueryMeterAssets());
            }
            set
            {
                m_meterAssets = value;
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

        public List<Series> Series
        {
            get
            {
                List<Channel> channels = Channels;

                if (channels is null)
                    return null;

                bool IsQueryRequired()
                {
                    var connectionFactory = ConnectionFactory;

                    try
                    {
                        // Don't trigger individual queries for each channel
                        ConnectionFactory = null;
                        return channels.Any(channel => channel.Series is null);
                    }
                    finally
                    {
                        ConnectionFactory = connectionFactory;
                    }
                }

                if (IsQueryRequired())
                    return QuerySeries();

                return channels
                    .SelectMany(channel => channel.Series)
                    .ToList();
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

        public Location GetLocation(AdoDataConnection connection)
        {
            if ((object)connection == null)
                return null;

            TableOperations<Location> locationTable = new TableOperations<Location>(connection);
            return locationTable.QueryRecordWhere("ID = {0}", LocationID);
        }

        public IEnumerable<MeterAsset> GetMeterAssets(AdoDataConnection connection)
        {
            if ((object)connection == null)
                return null;

            TableOperations<MeterAsset> meterAssetTable = new TableOperations<MeterAsset>(connection);
            return meterAssetTable.QueryRecordsWhere("MeterID = {0}", ID);
        }

        public IEnumerable<Channel> GetChannels(AdoDataConnection connection)
        {
            if ((object)connection == null)
                return null;

            TableOperations<Channel> channelTable = new TableOperations<Channel>(connection);
            return channelTable.QueryRecordsWhere("MeterID = {0}", ID);
        }

        public IEnumerable<Series> GetSeries(AdoDataConnection connection)
        {
            if ((object)connection == null)
                return null;

            TableOperations<Series> seriesTable = new TableOperations<Series>(connection);
            return seriesTable.QueryRecordsWhere("ChannelID IN (SELECT ID FROM Channel WHERE MeterID = {0})", ID);
        }

        public TimeZoneInfo GetTimeZoneInfo(TimeZoneInfo defaultTimeZone)
        {
            if (!string.IsNullOrEmpty(TimeZone))
                return TimeZoneInfo.FindSystemTimeZoneById(TimeZone);

            return defaultTimeZone;
        }

        private Location QueryLocation()
        {
            Location location;

            using (AdoDataConnection connection = ConnectionFactory?.Invoke())
            {
                location = GetLocation(connection);
            }

            if ((object)location != null)
                location.LazyContext = LazyContext;

            return LazyContext.GetLocation(location);
        }

        private List<MeterAsset> QueryMeterAssets()
        {
            List<MeterAsset> meterAssets;

            using (AdoDataConnection connection = ConnectionFactory?.Invoke())
            {
                meterAssets = GetMeterAssets(connection)?
                    .Select(LazyContext.GetMeterAsset)
                    .ToList();
            }

            if ((object)meterAssets != null)
            {
                foreach (MeterAsset meterAsset in meterAssets)
                {
                    meterAsset.Meter = this;
                    meterAsset.LazyContext = LazyContext;
                }
            }

            return meterAssets;
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

        private List<Series> QuerySeries()
        {
            List<Series> seriesList;

            using (AdoDataConnection connection = ConnectionFactory?.Invoke())
            {
                seriesList = GetSeries(connection)?
                    .Select(LazyContext.GetSeries)
                    .ToList();
            }

            if (!(seriesList is null))
            {
                ILookup<int, Series> seriesLookup = seriesList.ToLookup(series => series.ChannelID);

                foreach (Channel channel in Channels)
                {
                    channel.Series = seriesLookup[channel.ID].ToList();

                    foreach (Series series in channel.Series)
                    {
                        series.Channel = channel;
                        series.LazyContext = LazyContext;
                    }
                }
            }

            return seriesList;
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
