//******************************************************************************************************
//  MeterDataSet.cs - Gbtc
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
//  05/13/2014 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using FaultData.DataAnalysis;
using FaultData.DataResources;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using openXDA.Model;

namespace FaultData.DataSets
{
    public class MeterDataSet : IDataSet
    {
        #region [ Members ]

        // Fields
        private Func<AdoDataConnection> m_createDbConnection;
        private string m_connectionString;
        private Dictionary<Type, object> m_resources;

        private string m_filePath;
        private FileGroup m_fileGroup;
        private Meter m_meter;
        private ConfigurationDataSet m_configuration;
        private List<DataSeries> m_dataSeries;
        private List<DataSeries> m_digitals;
        private List<ReportedDisturbance> m_disturbanceStatistics;

        #endregion

        #region [ Constructors ]

        public MeterDataSet()
        {
            m_resources = new Dictionary<Type, object>();
            m_configuration = new ConfigurationDataSet();
            m_dataSeries = new List<DataSeries>();
            m_digitals = new List<DataSeries>();
            m_disturbanceStatistics = new List<ReportedDisturbance>();
        }

        public MeterDataSet(Event evt)
        {
            m_resources = new Dictionary<Type, object>();
            m_configuration = new ConfigurationDataSet();
            m_dataSeries = new List<DataSeries>();
            m_digitals = new List<DataSeries>();
            m_disturbanceStatistics = new List<ReportedDisturbance>();

            using (AdoDataConnection connection = new AdoDataConnection("systemSettings")) {
                Meter = (new TableOperations<Meter>(connection)).QueryRecordWhere("ID = {0}", evt.MeterID);
                Meter.ConnectionFactory = () => new AdoDataConnection("systemSettings");
                CreateDbConnection = () => new AdoDataConnection("systemSettings");
                ConnectionString = string.Empty;
                FilePath = (new TableOperations<DataFile>(connection)).QueryRecordWhere("FileGroupID = {0}", evt.FileGroupID).FilePath;
                FileGroup = (new TableOperations<FileGroup>(connection)).QueryRecordWhere("ID = {0}", evt.FileGroupID);
                Configuration.LineLength = (new TableOperations<Line>(connection)).QueryRecordWhere("ID = {0}", evt.LineID).Length;
                Configuration.X0 = (new TableOperations<LineImpedance>(connection)).QueryRecordWhere("LineID = {0}", evt.LineID)?.X0;
                Configuration.X1 = (new TableOperations<LineImpedance>(connection)).QueryRecordWhere("LineID = {0}", evt.LineID)?.X1;
                Configuration.R0 = (new TableOperations<LineImpedance>(connection)).QueryRecordWhere("LineID = {0}", evt.LineID)?.R0;
                Configuration.R1 = (new TableOperations<LineImpedance>(connection)).QueryRecordWhere("LineID = {0}", evt.LineID)?.R1;
                DataGroup dataGroup = ToDataGroup((new TableOperations<EventData>(connection)).QueryRecordWhere("ID = {0}", evt.EventDataID).TimeDomainData);
                DataSeries = dataGroup.DataSeries.Where(ds=> ds.SeriesInfo.Channel.MeasurementType.Name != "Digital").ToList();
                Digitals = dataGroup.DataSeries.Where(ds => ds.SeriesInfo.Channel.MeasurementType.Name == "Digital").ToList();
            }
        }

        #endregion

        #region [ Properties ]

        public Func<AdoDataConnection> CreateDbConnection
        {
            get
            {
                return m_createDbConnection;
            }
            set
            {
                m_createDbConnection = value;
            }
        }

        public string ConnectionString
        {
            get
            {
                return m_connectionString;
            }
            set
            {
                m_connectionString = value;
            }
        }

        public string FilePath
        {
            get
            {
                return m_filePath;
            }
            set
            {
                m_filePath = value;
            }
        }

        public FileGroup FileGroup
        {
            get
            {
                return m_fileGroup;
            }
            set
            {
                m_fileGroup = value;
            }
        }

        public Meter Meter
        {
            get
            {
                return m_meter;
            }
            set
            {
                m_meter = value;
            }
        }

        public ConfigurationDataSet Configuration
        {
            get
            {
                return m_configuration;
            }
        }

        public List<DataSeries> DataSeries
        {
            get
            {
                return m_dataSeries;
            }
            set
            {
                m_dataSeries = value;
            }
        }

        public List<DataSeries> Digitals
        {
            get
            {
                return m_digitals;
            }
            set
            {
                m_digitals = value;
            }
        }

        public List<ReportedDisturbance> ReportedDisturbances
        {
            get
            {
                return m_disturbanceStatistics;
            }
            set
            {
                m_disturbanceStatistics = value;
            }
        }

        #endregion

        #region [ Methods ]

        public T GetResource<T>() where T : class, IDataResource, new()
        {
            return GetResource(() => new T());
        }

        public T GetResource<T>(Func<T> resourceFactory) where T : class, IDataResource
        {
            Type type;
            object obj;
            T resource;

            type = typeof(T);

            if (m_resources.TryGetValue(type, out obj))
            {
                resource = (T)obj;
            }
            else
            {
                resource = resourceFactory();
                ConnectionStringParser.ParseConnectionString(m_connectionString, resource);
                resource.Initialize(this);
                m_resources.Add(type, resource);
            }

            return resource;
        }

        private DataGroup ToDataGroup(byte[] data)
        {
            DataGroup dataGroup = new DataGroup();
            dataGroup.FromData(Meter, data);
            return dataGroup;
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ConnectionStringParser<SettingAttribute, CategoryAttribute> ConnectionStringParser = new ConnectionStringParser<SettingAttribute, CategoryAttribute>();

        #endregion
    }
}
