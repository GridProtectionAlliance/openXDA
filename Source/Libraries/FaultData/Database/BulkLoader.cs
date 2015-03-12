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

using System;
using System.Data;
using System.Data.SqlClient;

namespace FaultData.Database
{
    public class BulkLoader
    {
        #region [ Members ]

        // Fields
        private SqlConnection m_connection;
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
            string tempTableName;

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(m_connection))
            using (SqlCommand command = m_connection.CreateCommand())
            {
                // Set the timeout to infinite
                bulkCopy.BulkCopyTimeout = 0;

                // Create the temp table where data will be loaded directly
                tempTableName = CreateTempTable(table, command);

                // Bulk load the data to the temp table
                bulkCopy.DestinationTableName = tempTableName;
                bulkCopy.WriteToServer(table);

                // Migrate data from the temp table to the target table
                if ((object)m_mergeTableFormat == null)
                    DumpTempTable(table, command, tempTableName);
                else
                    MergeTempTable(table, command, tempTableName);

                // Drop the temp table
                command.CommandText = string.Format("DROP TABLE {0}", tempTableName);
                command.ExecuteNonQuery();
            }
        }

        private string CreateTempTable(DataTable table, SqlCommand command)
        {
            string guid = Guid.NewGuid().ToString("N");
            string variableName = "@var" + guid;
            string tempTableName = "#temp" + guid;

            // Populate the variable with CREATE TABLE syntax
            command.CommandText = string.Format("SELECT {0} = COALESCE({0} + ', ', 'CREATE TABLE {1} (') + c.name + ' ' + t.name + " +
                                                "CASE WHEN t.name = 'varchar' OR t.name = 'varbinary' THEN '(' + CAST(c.max_length AS VARCHAR(3)) + ')' ELSE '' END " +
                                                "FROM sys.columns c INNER JOIN sys.types AS t ON c.system_type_id = t.system_type_id " +
                                                "WHERE object_id = (SELECT object_id from sys.objects where name = '{2}') " +
                                                "ORDER BY c.column_id", variableName, tempTableName, table.TableName);

            command.Parameters.Add(variableName, SqlDbType.VarChar, -1);
            command.Parameters[0].Direction = ParameterDirection.Output;
            command.ExecuteNonQuery();

            // Execute the CREATE TABLE statement
            command.CommandText = command.Parameters[0].Value.ToString().Replace("(-1)", "(max)") + ")";
            command.Parameters.Clear();
            command.ExecuteNonQuery();

            return tempTableName;
        }

        private void DumpTempTable(DataTable table, SqlCommand command, string tempTableName)
        {
            // Drop the ID column which, by convention,
            // is an autoincrementing integer primary key field
            if (table.Columns.Contains("ID"))
            {
                command.CommandText = string.Format("ALTER TABLE {0} DROP COLUMN ID", tempTableName);
                command.ExecuteNonQuery();
            }

            // Execute the CREATE TABLE statement
            command.CommandText = string.Format("INSERT INTO {0} SELECT * FROM {1}", table.TableName, tempTableName);
            command.ExecuteNonQuery();
        }

        private void MergeTempTable(DataTable table, SqlCommand command, string tempTableName)
        {
            // Merge the data from the temp table into the destination table
            command.CommandText = string.Format(m_mergeTableFormat, table.TableName, tempTableName);
            command.ExecuteNonQuery();
        }

        #endregion
    }
}
