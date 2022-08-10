//******************************************************************************************************
//  Series.cs - Gbtc
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
//  06/20/2017 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using GSF.Data;
using GSF.Data.Model;
using Newtonsoft.Json;

namespace openXDA.Model
{
    public class SeriesKey : IEquatable<SeriesKey>
    {
        #region [ Constructors ]

        public SeriesKey(ChannelKey channelKey, string seriesType)
        {
            ChannelKey = channelKey;
            SeriesType = seriesType;
        }

        public SeriesKey(Series series)
            : this(new ChannelKey(series.Channel), series.SeriesType.Name)
        {
        }

        #endregion

        #region [ Properties ]

        public ChannelKey ChannelKey { get; }
        public string SeriesType { get; }

        #endregion

        #region [ Methods ]

        public override int GetHashCode()
        {
            int hash = 1009;
            hash = 9176 * hash + ChannelKey.GetHashCode();
            hash = 9176 * hash + SeriesType.GetHashCode();
            return hash;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SeriesKey);
        }

        public bool Equals(SeriesKey other)
        {
            if (other is null)
                return false;

            return
                ChannelKey.Equals(other.ChannelKey) &&
                SeriesType.Equals(other.SeriesType);
        }

        #endregion
    }

    public class Series
    {
        #region [ Members ]

        // Fields
        private SeriesType m_seriesType;
        private Channel m_channel;

        #endregion

        #region [ Properties ]

        [PrimaryKey(true)]
        public int ID { get; set; }

        public int ChannelID { get; set; }

        public int SeriesTypeID { get; set; }

        public string SourceIndexes { get; set; }

        [JsonIgnore]
        [NonRecordField]
        public SeriesType SeriesType
        {
            get
            {
                return m_seriesType ?? (m_seriesType = QuerySeriesType());
            }
            set
            {
                m_seriesType = value;
            }
        }

        [JsonIgnore]
        [NonRecordField]
        public Channel Channel
        {
            get
            {
                return m_channel ?? (m_channel = QueryChannel());
            }
            set
            {
                m_channel = value;
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

        public SeriesType GetSeriesType(AdoDataConnection connection)
        {
            if ((object)connection == null)
                return null;

            TableOperations<SeriesType> seriesTypeTable = new TableOperations<SeriesType>(connection);
            return seriesTypeTable.QueryRecordWhere("ID = {0}", SeriesTypeID);
        }

        public Channel GetChannel(AdoDataConnection connection)
        {
            if ((object)connection == null)
                return null;

            TableOperations<Channel> channelTable = new TableOperations<Channel>(connection);
            return channelTable.QueryRecordWhere("ID = {0}", ChannelID);
        }

        private SeriesType QuerySeriesType()
        {
            SeriesType seriesType;

            using (AdoDataConnection connection = ConnectionFactory?.Invoke())
            {
                seriesType = GetSeriesType(connection);
            }

            return LazyContext.GetSeriesType(seriesType);
        }

        private Channel QueryChannel()
        {
            Channel channel;

            using (AdoDataConnection connection = ConnectionFactory?.Invoke())
            {
                channel = GetChannel(connection);
            }

            if ((object)channel != null)
                channel.LazyContext = LazyContext;

            return LazyContext.GetChannel(channel);
        }

        #endregion

        #region [Static]
        public static Series GetInfo(Meter meter, int seriesID) {
            List<Channel> channels = meter.Channels;

            // Disable lazy loading
            var connectionFactory = meter.ConnectionFactory;
            meter.ConnectionFactory = null;

            // Search for a series that has already been loaded
            Series seriesInfo = channels
                .Where(channel => (object)channel.Series != null)
                .SelectMany(channel => channel.Series)
                .FirstOrDefault(series => series.ID == seriesID);

            // Restore lazy loading
            meter.ConnectionFactory = connectionFactory;

            // If the series was found without
            // lazy loading, return that series
            if ((object)seriesInfo != null)
                return seriesInfo;

            int channelID;

            using (AdoDataConnection connection = meter.ConnectionFactory())
            {
                // Get the channel ID of the series so we can skip lazy loading series collections for irrelevant channels
                channelID = connection.ExecuteScalar<int>("SELECT ChannelID FROM Series WHERE ID = {0}", seriesID);
            }

            // Now filter to the appropriate channel and search its series collection
            return channels
                .Where(channel => channel.ID == channelID)
                .SelectMany(channel => channel.Series)
                .FirstOrDefault(series => series.ID == seriesID);

        }

        #endregion
    }
}
