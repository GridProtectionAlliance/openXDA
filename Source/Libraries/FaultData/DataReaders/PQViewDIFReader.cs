//******************************************************************************************************
//  PQViewDIFReader.cs - Gbtc
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
//  09/27/2017 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using FaultData.DataAnalysis;
using FaultData.DataSets;
using GSF.Data;
using GSF.Data.Model;
using openXDA.Model;

namespace FaultData.DataReaders
{
    public class PQViewDIFReader : IDataReader, IDisposable
    {
        #region [ Members ]

        // Fields
        private string m_dbConnectionString;
        private int m_dbTimeout;
        private string m_filePattern;
        private PQDIFReader m_pqdifReader;
        private bool m_disposed;

        #endregion

        #region [ Constructors ]

        public PQViewDIFReader()
        {
            m_pqdifReader = new PQDIFReader();
        }

        #endregion

        #region [ Properties ]

        [Setting]
        public string DbConnectionString
        {
            get
            {
                return m_dbConnectionString;
            }
            set
            {
                m_dbConnectionString = value;
            }
        }

        [Setting]
        public int DbTimeout
        {
            get
            {
                return m_dbTimeout;
            }
            set
            {
                m_dbTimeout = value;
            }
        }

        [Setting]
        public string FilePattern
        {
            get
            {
                return m_filePattern;
            }
            set
            {
                m_filePattern = value;
            }
        }

        public MeterDataSet MeterDataSet
        {
            get
            {
                return m_pqdifReader.MeterDataSet;
            }
        }

        #endregion

        #region [ Methods ]

        public bool CanParse(string filePath, DateTime fileCreationTime)
        {
            return m_pqdifReader.CanParse(filePath, fileCreationTime);
        }

        public void Parse(string filePath)
        {
            m_pqdifReader.Parse(filePath);

            using (AdoDataConnection connection = CreateDbConnection())
            {
                Func<DateTime, IDbDataParameter> toDateTime2 = dateTime => ToDateTime2(connection, dateTime);
                TableOperations<Event> eventTable = new TableOperations<Event>(connection);

                string meterKey = GetMeterKey(filePath, m_filePattern);
                int meterID = connection.ExecuteScalar<int>("SELECT ID FROM Meter WHERE AssetKey = {0}", meterKey);

                Predicate<DataSeries> isDuplicate = dataSeries =>
                {
                    int samples = dataSeries.DataPoints.Count;

                    if (samples == 0)
                        return false;

                    DateTime startTime = dataSeries.DataPoints[0].Time;
                    DateTime endTime = dataSeries.DataPoints[dataSeries.DataPoints.Count - 1].Time;

                    RecordRestriction recordRestriction =
                        new RecordRestriction("MeterID = {0}", meterID) &
                        new RecordRestriction("StartTime = {0}", toDateTime2(startTime)) &
                        new RecordRestriction("EndTime = {0}", toDateTime2(endTime)) &
                        new RecordRestriction("Samples = {0}", samples);

                    int eventCount = eventTable.QueryRecordCount(recordRestriction);

                    return eventCount > 0;
                };

                MeterDataSet meterDataSet = m_pqdifReader.MeterDataSet;
                meterDataSet.DataSeries.RemoveAll(isDuplicate);
                meterDataSet.Digitals.RemoveAll(isDuplicate);
            }
        }

        public void Dispose()
        {
            if (!m_disposed)
            {
                try
                {
                    if ((object)m_pqdifReader != null)
                    {
                        m_pqdifReader.Dispose();
                        m_pqdifReader = null;
                    }
                }
                finally
                {
                    m_disposed = true;
                }
            }
        }

        private AdoDataConnection CreateDbConnection()
        {
            return CreateDbConnection(m_dbConnectionString, m_dbTimeout);
        }

        #endregion

        #region [ Static ]

        // Static Methods

        // Creates a new database connection based on the given system configuration.
        private static AdoDataConnection CreateDbConnection(string connectionString, int timeout)
        {
            Type connectionType = typeof(SqlConnection);
            Type adapterType = typeof(SqlDataAdapter);

            return new AdoDataConnection(connectionString, connectionType, adapterType)
            {
                DefaultTimeout = timeout
            };
        }

        // Uses regular expressions to read the meter's asset key from the file path.
        private static string GetMeterKey(string filePath, string filePattern)
        {
            Match match = Regex.Match(filePath, filePattern, RegexOptions.IgnoreCase);
            Group meterKeyGroup;

            if (!match.Success)
                return null;

            meterKeyGroup = match.Groups["AssetKey"];

            if ((object)meterKeyGroup == null)
                return null;

            return meterKeyGroup.Value;
        }

        private static IDbDataParameter ToDateTime2(AdoDataConnection connection, DateTime dateTime)
        {
            using (IDbCommand command = connection.Connection.CreateCommand())
            {
                IDbDataParameter parameter = command.CreateParameter();
                parameter.DbType = DbType.DateTime2;
                parameter.Value = dateTime;
                return parameter;
            }
        }

        #endregion
    }
}
