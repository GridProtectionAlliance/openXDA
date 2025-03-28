﻿//******************************************************************************************************
//  ScheduledDataSourceFactory.cs - Gbtc
//
//  Copyright © 2024, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may not use this
//  file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  05/15/2024 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GSF.Data;
using GSF.Data.Model;
using openXDA.Model;

namespace FaultData.DataWriters.Emails
{
    public class ScheduledDataSourceFactory
    {
        private Func<AdoDataConnection> ConnectionFactory { get; }

        public ScheduledDataSourceFactory(Func<AdoDataConnection> connectionFactory) =>
            ConnectionFactory = connectionFactory;

        public List<ScheduledDataSourceDefinition> LoadDataSourceDefinitions(ScheduledEmailType emailType)
        {
            using (AdoDataConnection connection = ConnectionFactory())
            {
                TableOperations<ScheduledEmailDataSource> dataSourceTable = new TableOperations<ScheduledEmailDataSource>(connection);
                TableOperations<ScheduledEmailDataSourceEmailType> dataSourceEmailTypeTable = new TableOperations<ScheduledEmailDataSourceEmailType>(connection);

                ScheduledDataSourceDefinition ToDefinition(ScheduledEmailDataSourceEmailType mapping) => new ScheduledDataSourceDefinition()
                {
                    EmailType = emailType,
                    Mapping = mapping,
                    DataSource = dataSourceTable.QueryRecordWhere("ID = {0}", mapping.ScheduledEmailDataSourceID)
                };

                return dataSourceEmailTypeTable
                    .QueryRecordsWhere("ScheduledEmailTypeID = {0}", emailType.ID)
                    .Select(ToDefinition)
                    .Where(definition => !(definition.DataSource is null))
                    .ToList();
            }
        }

        public ScheduledDataSource CreateDataSource(ScheduledDataSourceDefinition definition)
        {
            string assemblyName = definition.DataSource.AssemblyName;
            string typeName = definition.DataSource.TypeName;
            PluginFactory<IScheduledDataSource> pluginFactory = new PluginFactory<IScheduledDataSource>();
            Type pluginType = pluginFactory.GetPluginType(assemblyName, typeName);
            Type dbFactoryType = typeof(Func<AdoDataConnection>);
            ConstructorInfo constructor = pluginType.GetConstructor(new[] { dbFactoryType });
            object[] parameters = (constructor is null) ? Array.Empty<object>() : new object[] { ConnectionFactory };
            IScheduledDataSource dataSource = pluginFactory.Create(assemblyName, typeName, parameters);
            ConfigurationLoader<ScheduledEmailDataSourceSetting> configurationLoader = new ConfigurationLoader<ScheduledEmailDataSourceSetting>(definition.Mapping.ID, ConnectionFactory);
            dataSource.Configure(configurationLoader.Configure);
            return new ScheduledDataSource(definition.DataSource.Name, dataSource);
        }
    }
}
