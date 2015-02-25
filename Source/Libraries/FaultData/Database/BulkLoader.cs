//******************************************************************************************************
//  BulkLoader.cs - Gbtc
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
//  07/22/2014 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System.Data;
using System.Data.SqlClient;

namespace FaultData.Database
{
    public class BulkLoader
    {
        #region [ Members ]

        // Fields
        private SqlConnection m_connection;
        private string m_createTableFormat;
        private string m_mergeTableFormat;

        #endregion

        #region [ Properties ]

        public SqlConnection Connection
        {
            get
            {
                return m_connection;
            }
            set
            {
                m_connection = value;
            }
        }

        public string CreateTableFormat
        {
            get
            {
                return m_createTableFormat;
            }
            set
            {
                m_createTableFormat = value;
            }
        }

        public string MergeTableFormat
        {
            get
            {
                return m_mergeTableFormat;
            }
            set
            {
                m_mergeTableFormat = value;
            }
        }

        #endregion

        #region [ Methods ]

        public void Load(DataTable table)
        {
            string tableName;
            string tempTableName;

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(m_connection))
            using (SqlCommand command = m_connection.CreateCommand())
            {
                // Set the timeout to infinite
                bulkCopy.BulkCopyTimeout = 0;

                // Get the name of the table to load data into
                tableName = table.TableName;
                tempTableName = "#temp" + tableName;

                // Create the temp table where data will be loaded directly
                command.CommandText = string.Format(m_createTableFormat, tempTableName);
                command.ExecuteNonQuery();

                // Bulk load the data to the temp table
                bulkCopy.DestinationTableName = tempTableName;
                bulkCopy.WriteToServer(table);

                // Merge the data from the temp table into the destination table
                command.CommandText = string.Format(m_mergeTableFormat, tableName, tempTableName);
                command.ExecuteNonQuery();

                // Drop the temp table
                command.CommandText = string.Format("DROP TABLE {0}", tempTableName);
                command.ExecuteNonQuery();
            }
        }

        #endregion
    }
}
