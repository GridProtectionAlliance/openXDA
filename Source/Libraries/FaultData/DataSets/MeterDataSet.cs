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
using FaultData.DataAnalysis;
using FaultData.Database;
using FaultData.DataResources;
using GSF.Configuration;

namespace FaultData.DataSets
{
    public class MeterDataSet : IDataSet
    {
        #region [ Members ]

        // Fields
        private string m_connectionString;
        private Dictionary<Type, object> m_resources;

        private string m_filePath;
        private FileGroup m_fileGroup;
        private Meter m_meter;
        private List<DataSeries> m_dataSeries;

        #endregion

        #region [ Constructors ]

        public MeterDataSet()
        {
            m_resources = new Dictionary<Type, object>();
            m_dataSeries = new List<DataSeries>();
        }

        #endregion

        #region [ Properties ]

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

            ConnectionStringParser connectionStringParser;

            type = typeof(T);

            if (m_resources.TryGetValue(type, out obj))
            {
                resource = (T)obj;
            }
            else
            {
                connectionStringParser = new ConnectionStringParser();
                connectionStringParser.SerializeUnspecifiedProperties = true;

                resource = resourceFactory();
                connectionStringParser.ParseConnectionString(m_connectionString, resource);
                resource.Initialize(this);
                m_resources.Add(type, resource);
            }

            return resource;
        }

        #endregion
    }
}
