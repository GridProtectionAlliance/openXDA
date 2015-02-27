//******************************************************************************************************
//  DbAdapterContainer.cs - Gbtc
//
//  Copyright © 2015, Grid Protection Alliance.  All Rights Reserved.
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
//  02/24/2015 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Data.SqlClient;
using FaultData.Database.AlarmDataTableAdapters;
using FaultData.Database.DataQualityTableAdapters;
using FaultData.Database.FaultLocationDataTableAdapters;
using FaultData.Database.MeterDataTableAdapters;
using CycleDataTableAdapter = FaultData.Database.MeterDataTableAdapters.CycleDataTableAdapter;

namespace FaultData.Database
{
    public sealed class DbAdapterContainer : IDisposable
    {
        #region [ Members ]

        // Fields
        private SqlConnection m_connection;

        // Linq-to-SQL
        private SystemInfoDataContext m_systemInfoAdapter;
        private FileInfoDataContext m_fileInfoAdapter;
        private MeterInfoDataContext m_meterInfoAdapter;
        private FaultLocationInfoDataContext m_faultLocationInfoAdapter;

        // MeterData
        private EventTypeTableAdapter m_eventTypeAdapter;
        private EventTableAdapter m_eventAdapter;
        private HourlyTrendingSummaryTableAdapter m_hourlyTrendingSummaryAdapter;
        private DailyTrendingSummaryTableAdapter m_dailyTrendingSummaryAdapter;
        private ChannelNormalTableAdapter m_channelNormalAdapter;

        // FaultLocationData
        private CycleDataTableAdapter m_cycleDataAdapter;
        private FaultCurveTableAdapter m_faultCurveAdapter;
        private FaultSummaryTableAdapter m_faultSummaryAdapter;

        // DataQuality
        private DefaultDataQualityRangeLimitTableAdapter m_defaultDataQualityRangeLimitAdapter;
        private DataQualityRangeLimitTableAdapter m_dataQualityRangeLimitAdapter;

        // AlarmData
        private AlarmTypeTableAdapter m_alarmTypeAdapter;
        private DefaultAlarmRangeLimitTableAdapter m_defaultAlarmRangeLimitAdapter;
        private AlarmRangeLimitTableAdapter m_alarmRangeLimitAdapter;
        private HourOfWeekLimitTableAdapter m_hourOfWeekLimitAdapter;
        private AlarmLogTableAdapter m_alarmLogAdapter;

        private bool m_disposed;

        #endregion

        #region [ Constructors ]

        public DbAdapterContainer(string connectionString)
        {
            m_connection = new SqlConnection(connectionString);
            m_connection.Open();
        }

        public DbAdapterContainer(SqlConnection connection)
        {
            m_connection = connection;
        }

        #endregion

        #region [ Properties ]

        public SqlConnection Connection
        {
            get
            {
                return m_connection;
            }
        }

        public SystemInfoDataContext SystemInfoAdapter
        {
            get
            {
                if (!m_disposed && (object)m_systemInfoAdapter == null)
                    m_systemInfoAdapter = new SystemInfoDataContext(m_connection);

                return m_systemInfoAdapter;
            }
        }

        public FileInfoDataContext FileInfoAdapter
        {
            get
            {
                if (!m_disposed && (object)m_fileInfoAdapter == null)
                    m_fileInfoAdapter = new FileInfoDataContext(m_connection);

                return m_fileInfoAdapter;
            }
        }

        public MeterInfoDataContext MeterInfoAdapter
        {
            get
            {
                if (!m_disposed && (object)m_meterInfoAdapter == null)
                    m_meterInfoAdapter = new MeterInfoDataContext(m_connection);

                return m_meterInfoAdapter;
            }
        }

        public FaultLocationInfoDataContext FaultLocationInfoAdapter
        {
            get
            {
                if (!m_disposed && (object)m_faultLocationInfoAdapter == null)
                    m_faultLocationInfoAdapter = new FaultLocationInfoDataContext(m_connection);

                return m_faultLocationInfoAdapter;
            }
        }

        public EventTypeTableAdapter EventTypeAdapter
        {
            get
            {
                if (!m_disposed && (object)m_eventTypeAdapter == null)
                {
                    m_eventTypeAdapter = new EventTypeTableAdapter();
                    m_eventTypeAdapter.Connection = m_connection;
                }

                return m_eventTypeAdapter;
            }
        }

        public EventTableAdapter EventAdapter
        {
            get
            {
                if (!m_disposed && (object)m_eventAdapter == null)
                {
                    m_eventAdapter = new EventTableAdapter();
                    m_eventAdapter.Connection = m_connection;
                }

                return m_eventAdapter;
            }
        }

        public HourlyTrendingSummaryTableAdapter HourlyTrendingSummaryAdapter
        {
            get
            {
                if (!m_disposed && (object)m_hourlyTrendingSummaryAdapter == null)
                {
                    m_hourlyTrendingSummaryAdapter = new HourlyTrendingSummaryTableAdapter();
                    m_hourlyTrendingSummaryAdapter.Connection = m_connection;
                }

                return m_hourlyTrendingSummaryAdapter;
            }
        }

        public DailyTrendingSummaryTableAdapter DailyTrendingSummaryAdapter
        {
            get
            {
                if (!m_disposed && (object)m_dailyTrendingSummaryAdapter == null)
                {
                    m_dailyTrendingSummaryAdapter = new DailyTrendingSummaryTableAdapter();
                    m_dailyTrendingSummaryAdapter.Connection = m_connection;
                }

                return m_dailyTrendingSummaryAdapter;
            }
        }

        public ChannelNormalTableAdapter ChannelNormalAdapter
        {
            get
            {
                if (!m_disposed && (object)m_channelNormalAdapter == null)
                {
                    m_channelNormalAdapter = new ChannelNormalTableAdapter();
                    m_channelNormalAdapter.Connection = m_connection;
                }

                return m_channelNormalAdapter;
            }
        }

        public CycleDataTableAdapter CycleDataAdapter
        {
            get
            {
                if (!m_disposed && (object)m_cycleDataAdapter == null)
                {
                    m_cycleDataAdapter = new CycleDataTableAdapter();
                    m_cycleDataAdapter.Connection = m_connection;
                }

                return m_cycleDataAdapter;
            }
        }

        public FaultCurveTableAdapter FaultCurveAdapter
        {
            get
            {
                if (!m_disposed && (object)m_faultCurveAdapter == null)
                {
                    m_faultCurveAdapter = new FaultCurveTableAdapter();
                    m_faultCurveAdapter.Connection = m_connection;
                }

                return m_faultCurveAdapter;
            }
        }

        public FaultSummaryTableAdapter FaultSummaryAdapter
        {
            get
            {
                if (!m_disposed && (object)m_faultSummaryAdapter == null)
                {
                    m_faultSummaryAdapter = new FaultSummaryTableAdapter();
                    m_faultSummaryAdapter.Connection = m_connection;
                }

                return m_faultSummaryAdapter;
            }
        }

        public DefaultDataQualityRangeLimitTableAdapter DefaultDataQualityRangeLimitAdapter
        {
            get
            {
                if (!m_disposed && (object)m_defaultDataQualityRangeLimitAdapter == null)
                {
                    m_defaultDataQualityRangeLimitAdapter = new DefaultDataQualityRangeLimitTableAdapter();
                    m_defaultDataQualityRangeLimitAdapter.Connection = m_connection;
                }

                return m_defaultDataQualityRangeLimitAdapter;
            }
        }

        public DataQualityRangeLimitTableAdapter DataQualityRangeLimitAdapter
        {
            get
            {
                if (!m_disposed && (object)m_dataQualityRangeLimitAdapter == null)
                {
                    m_dataQualityRangeLimitAdapter = new DataQualityRangeLimitTableAdapter();
                    m_dataQualityRangeLimitAdapter.Connection = m_connection;
                }

                return m_dataQualityRangeLimitAdapter;
            }
        }

        public AlarmTypeTableAdapter AlarmTypeAdapter
        {
            get
            {
                if (!m_disposed && (object)m_alarmTypeAdapter == null)
                {
                    m_alarmTypeAdapter = new AlarmTypeTableAdapter();
                    m_alarmTypeAdapter.Connection = m_connection;
                }

                return m_alarmTypeAdapter;
            }
        }

        public DefaultAlarmRangeLimitTableAdapter DefaultAlarmRangeLimitAdapter
        {
            get
            {
                if (!m_disposed && (object)m_defaultAlarmRangeLimitAdapter == null)
                {
                    m_defaultAlarmRangeLimitAdapter = new DefaultAlarmRangeLimitTableAdapter();
                    m_defaultAlarmRangeLimitAdapter.Connection = m_connection;
                }

                return m_defaultAlarmRangeLimitAdapter;
            }
        }

        public AlarmRangeLimitTableAdapter AlarmRangeLimitAdapter
        {
            get
            {
                if (!m_disposed && (object)m_alarmRangeLimitAdapter == null)
                {
                    m_alarmRangeLimitAdapter = new AlarmRangeLimitTableAdapter();
                    m_alarmRangeLimitAdapter.Connection = m_connection;
                }

                return m_alarmRangeLimitAdapter;
            }
        }

        public HourOfWeekLimitTableAdapter HourOfWeekLimitAdapter
        {
            get
            {
                if (!m_disposed && (object)m_hourOfWeekLimitAdapter == null)
                {
                    m_hourOfWeekLimitAdapter = new HourOfWeekLimitTableAdapter();
                    m_hourOfWeekLimitAdapter.Connection = m_connection;
                }

                return m_hourOfWeekLimitAdapter;
            }
        }

        public AlarmLogTableAdapter AlarmLogAdapter
        {
            get
            {
                if (!m_disposed && (object)m_alarmLogAdapter == null)
                {
                    m_alarmLogAdapter = new AlarmLogTableAdapter();
                    m_alarmLogAdapter.Connection = m_connection;
                }

                return m_alarmLogAdapter;
            }
        }

        #endregion

        #region [ Methods ]

        public void Dispose()
        {
            if (!m_disposed)
            {
                Dispose(m_connection);
                Dispose(m_systemInfoAdapter);
                Dispose(m_fileInfoAdapter);
                Dispose(m_meterInfoAdapter);
                Dispose(m_faultLocationInfoAdapter);
                Dispose(m_eventTypeAdapter);
                Dispose(m_eventAdapter);
                Dispose(m_hourlyTrendingSummaryAdapter);
                Dispose(m_dailyTrendingSummaryAdapter);
                Dispose(m_channelNormalAdapter);
                Dispose(m_cycleDataAdapter);
                Dispose(m_faultCurveAdapter);
                Dispose(m_faultSummaryAdapter);
                Dispose(m_defaultDataQualityRangeLimitAdapter);
                Dispose(m_dataQualityRangeLimitAdapter);
                Dispose(m_alarmTypeAdapter);
                Dispose(m_defaultAlarmRangeLimitAdapter);
                Dispose(m_alarmRangeLimitAdapter);
                Dispose(m_hourOfWeekLimitAdapter);
                Dispose(m_alarmLogAdapter);
                m_disposed = true;
            }
        }

        private void Dispose(IDisposable obj)
        {
            if ((object)obj != null)
                obj.Dispose();
        }

        #endregion
    }
}
