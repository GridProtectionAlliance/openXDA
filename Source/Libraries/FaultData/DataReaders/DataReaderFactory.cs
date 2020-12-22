//******************************************************************************************************
//  DataReaderFactory.cs - Gbtc
//
//  Copyright © 2021, Grid Protection Alliance.  All Rights Reserved.
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
//  01/11/2021 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GSF.Data;
using GSF.Data.Model;
using GSF.IO;
using openXDA.Model;

namespace FaultData.DataReaders
{
    public class DataReaderFactory
    {
        private Func<AdoDataConnection> ConnectionFactory { get; }
        private PluginFactory<IDataReader> PluginFactory { get; }

        public DataReaderFactory(Func<AdoDataConnection> connectionFactory)
        {
            if (connectionFactory is null)
                throw new ArgumentNullException(nameof(connectionFactory));

            ConnectionFactory = connectionFactory;
            PluginFactory = new PluginFactory<IDataReader>();
        }

        public IDataReader CreateDataReader(FileInfo[] fileList)
        {
            IEnumerable<string> filePaths = fileList
                .Select(fileInfo => fileInfo.FullName);

            return CreateDataReader(filePaths);
        }

        public IDataReader CreateDataReader(FileGroup fileGroup)
        {
            IEnumerable<string> filePaths = fileGroup.DataFiles
                .Select(dataFile => dataFile.FilePath);

            return CreateDataReader(filePaths);
        }

        public IDataReader CreateDataReader(IEnumerable<string> filePaths)
        {
            List<DataReader> QueryDataReaderDefinitions()
            {
                using (AdoDataConnection connection = ConnectionFactory())
                {
                    TableOperations<DataReader> dataReaderTable = new TableOperations<DataReader>(connection);

                    return dataReaderTable
                        .QueryRecords("LoadOrder")
                        .ToList();
                }
            }

            List<DataReader> dataReaderDefinitions = QueryDataReaderDefinitions();

            foreach (string filePath in filePaths)
            {
                DataReader definition = dataReaderDefinitions
                    .FirstOrDefault(reader => FilePath.IsFilePatternMatch(reader.FilePattern, filePath, true));

                if (definition is null)
                    continue;

                string assemblyName = definition.AssemblyName;
                string typeName = definition.TypeName;
                return PluginFactory.Create(assemblyName, typeName);
            }

            throw new Exception($"No data reader could be found to process the file group.");
        }
    }
}
