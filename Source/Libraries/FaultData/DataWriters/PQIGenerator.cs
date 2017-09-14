//******************************************************************************************************
//  ChartGenerator.cs - Gbtc
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
//  09/05/2017 - Stephen A. Jenks
//       Generated original version of source code.
//
//******************************************************************************************************

using GSF.Data;
using System;
using System.Data;
using System.Linq;
using System.Xml.Linq;

namespace FaultData.DataWriters
{
    /// <summary>
    /// Class containing logic to query the PQI database 
    /// </summary>
    public class PQIGenerator
    {
        #region [ Static ]

        public static XElement GetPqiInformation(AdoDataConnection connection, XElement element)
        {
            string queryType = ((string)element.Attribute("type") ?? "").ToLower();

            return queryType == "customerequipment" ? GetCustomerEquipmentAffected(connection, element) : element;
        }

        private static XElement GetCustomerEquipmentAffected(AdoDataConnection connection, XElement element)
        {
            int eventID;

            XElement returnTable = new XElement("table");
            returnTable.SetAttributeValue("class", "left");
            string returnTableContent = "";

            try
            {
                eventID = Convert.ToInt32((string)element.Attribute("eventID") ?? "-1");
            }
            catch
            {
                return element;
            }

            DataTable table = new DataTable();
            using (IDbCommand command = connection.Connection.CreateCommand())
            {
                command.CommandText = "dbo.GetAllImpactedComponents";
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 600;

                IDbDataParameter param1 = command.CreateParameter();
                param1.ParameterName = "@eventID";
                param1.Value = eventID;

                command.Parameters.Add(param1);

                IDataReader rdr = command.ExecuteReader();
                table.Load(rdr);
            }

            returnTableContent = "<tr>";
            foreach (DataColumn column in table.Columns)
            {
                returnTableContent += "<th>" + column.ColumnName + "</th>";
            }
            returnTableContent += "</tr>";

            foreach (DataRow row in table.Rows)
            {
                returnTableContent += "<tr>";
                foreach (DataColumn column in table.Columns)
                {
                    returnTableContent += "<td>" + row[column] + "</td>";
                }
                returnTableContent += "</tr>";
            }

            returnTable.Value = returnTableContent;

            return returnTable;
        }
    }
}

#endregion
