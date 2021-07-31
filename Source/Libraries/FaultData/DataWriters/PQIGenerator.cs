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

using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using GSF.Data;
using openXDA.Configuration;
using openXDA.PQI;

namespace FaultData.DataWriters
{
    /// <summary>
    /// Class containing logic to query the PQI database 
    /// </summary>
    public class PQIGenerator
    {
        #region [ Static ]

        public static XElement GetPqiInformation(AdoDataConnection connection, PQISection pqiSettings, XElement element)
        {
            string queryType = ((string)element.Attribute("type") ?? "").ToLower();

            return queryType == "customerequipment"
                ? GetCustomerEquipmentAffected(connection, pqiSettings, element)
                : element;
        }

        private static XElement GetCustomerEquipmentAffected(AdoDataConnection connection, PQISection pqiSettings, XElement element)
        {
            string[] returnFields = ((string)element.Attribute("returnFields")).Split(',') ?? new string[0];
            string[] returnFieldNames = ((string)element.Attribute("returnFieldNames")).Split(',') ?? returnFields;

            string ToFieldName(string field)
            {
                int index = Array.IndexOf(returnFields, field);
                return (index >= 0) ? returnFieldNames[index] : null;
            }

            string GetValue(Equipment equipment, string field)
            {
                switch (field)
                {
                    case nameof(equipment.Facility): return equipment.Facility;
                    case nameof(equipment.Area): return equipment.Area;
                    case nameof(equipment.SectionTitle): return equipment.SectionTitle;
                    case nameof(equipment.SectionRank): return $"{equipment.SectionRank}";
                    case nameof(equipment.ComponentModel): return equipment.ComponentModel;
                    case nameof(equipment.Manufacturer): return equipment.Manufacturer;
                    case nameof(equipment.Series): return equipment.Series;
                    case nameof(equipment.ComponentType): return equipment.ComponentType;
                    default: return null;
                }
            }

            XElement returnTable = new XElement("table");
            PQIWSClient pqiwsClient = CreatePQIWSClient(pqiSettings);

            if (pqiwsClient is null)
            {
                returnTable.Value = "Connection to PQI Web Service not configured.";
                return returnTable;
            }

            AdoDataConnection ConnectionFactory() =>
                new AdoDataConnection(connection.Connection, connection.AdapterType, false);

            PQIWSQueryHelper pqiwsQueryHelper = new PQIWSQueryHelper(ConnectionFactory, pqiwsClient);
            string eventsStrings = (string)element.Attribute("eventID") ?? "-1";
            int[] events = Array.ConvertAll(eventsStrings.Split(','), Convert.ToInt32);
            Task<List<Equipment>> task = pqiwsQueryHelper.GetAllImpactedEquipmentAsync(events);
            List<Equipment> impactedEquipment = task.GetAwaiter().GetResult();

            // Create header row
            XElement headerRow = new XElement("tr");

            foreach (string field in returnFields)
            {
                string fieldName = ToFieldName(field);
                if (fieldName is null) continue;
                XElement header = new XElement("th", fieldName);
                headerRow.Add(header);
            }

            returnTable.Add(headerRow);

            // Create body rows
            foreach (Equipment equipment in impactedEquipment)
            {
                XElement bodyRow = new XElement("tr");

                foreach (string field in returnFields)
                {
                    string fieldName = ToFieldName(field);
                    if (fieldName is null) continue;
                    string value = GetValue(equipment, field);
                    XElement cell = new XElement("td", value);
                    bodyRow.Add(cell);
                }

                returnTable.Add(bodyRow);
            }

            return returnTable;
        }

        private static PQIWSClient CreatePQIWSClient(PQISection pqiSettings)
        {
            if (string.IsNullOrEmpty(pqiSettings.BaseURL))
                return null;

            if (string.IsNullOrEmpty(pqiSettings.PingURL))
                return null;

            if (string.IsNullOrEmpty(pqiSettings.ClientID))
                return null;

            if (string.IsNullOrEmpty(pqiSettings.ClientSecret))
                return null;

            if (string.IsNullOrEmpty(pqiSettings.Username))
                return null;

            if (string.IsNullOrEmpty(pqiSettings.Password))
                return null;

            string FetchAccessToken()
            {
                NetworkCredential clientCredential = new NetworkCredential(pqiSettings.ClientID, pqiSettings.ClientSecret);
                NetworkCredential userCredential = new NetworkCredential(pqiSettings.Username, pqiSettings.Password);
                PingClient pingClient = new PingClient(pqiSettings.PingURL);
                Task exchangeTask = pingClient.ExchangeAsync(clientCredential, userCredential);
                exchangeTask.GetAwaiter().GetResult();
                return pingClient.AccessToken;
            }

            return new PQIWSClient(pqiSettings.BaseURL, FetchAccessToken);
        }
    }
}

#endregion
