//******************************************************************************************************
//  EventOperation.cs - Gbtc
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
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using FaultData.DataAnalysis;
using FaultData.Database;
using FaultData.Database.MeterDataTableAdapters;

namespace FaultData.DataOperations
{
    public class EventOperation : IDataOperation
    {
        #region [ Members ]

        // Fields
        private string m_connectionString;
        private Dictionary<DataClassification, int> m_eventTypeLookup;

        #endregion

        #region [ Constructors ]

        public EventOperation(string connectionString)
        {
            m_connectionString = connectionString;
        }

        #endregion

        #region [ Methods ]

        public void Execute(MeterDataSet meterDataSet)
        {
            List<DataGroup> dataGroups = meterDataSet.GetResource<DataGroupsResource>().DataGroups;
            LoadEventTypes(dataGroups);
            LoadEvents(meterDataSet, dataGroups);
        }

        private void LoadEventTypes(List<DataGroup> dataGroups)
        {
            MeterData.EventTypeDataTable eventTypeTable = new MeterData.EventTypeDataTable();
            List<string> eventTypeNames;

            if ((object)m_eventTypeLookup == null)
                m_eventTypeLookup = GetEventTypeLookup();

            eventTypeNames = dataGroups
                .Select(dataGroup => dataGroup.Classification)
                .Distinct()
                .Where(classification => classification != DataClassification.Trend)
                .Where(classification => classification != DataClassification.Unknown)
                .Where(classification => !m_eventTypeLookup.ContainsKey(classification))
                .Select(classification => classification.ToString())
                .ToList();

            if (eventTypeNames.Count > 0)
            {
                foreach (string eventTypeName in eventTypeNames)
                    eventTypeTable.AddEventTypeRow(eventTypeName, eventTypeName);

                using (SqlConnection connection = new SqlConnection(m_connectionString))
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    connection.Open();
                    bulkCopy.BulkCopyTimeout = 0;

                    bulkCopy.DestinationTableName = eventTypeTable.TableName;
                    bulkCopy.WriteToServer(eventTypeTable);
                }

                m_eventTypeLookup = GetEventTypeLookup();
            }
        }

        private void LoadEvents(MeterDataSet meterDataSet, List<DataGroup> dataGroups)
        {
            MeterData.EventDataTable eventTable = new MeterData.EventDataTable();
            MeterData.EventRow eventRow;
            int eventTypeID;

            IEnumerable<DataGroup> eventGroups = dataGroups
                .Where(dataGroup => dataGroup.Classification != DataClassification.Trend)
                .Where(dataGroup => dataGroup.Classification != DataClassification.Unknown);

            foreach (DataGroup eventGroup in eventGroups)
            {
                if (!m_eventTypeLookup.TryGetValue(eventGroup.Classification, out eventTypeID))
                    continue;

                eventRow = eventTable.NewEventRow();
                eventRow.FileGroupID = meterDataSet.FileGroup.ID;
                eventRow.MeterID = meterDataSet.Meter.ID;
                eventRow.LineID = eventGroup.Line.ID;
                eventRow.EventTypeID = eventTypeID;
                eventRow.Name = string.Empty;
                eventRow.Data = eventGroup.ToData();
                eventRow.StartTime = eventGroup.StartTime;
                eventRow.EndTime = eventGroup.EndTime;
                eventRow.Magnitude = 0.0D;
                eventRow.Duration = (eventGroup.EndTime - eventGroup.StartTime).TotalSeconds;
                eventTable.AddEventRow(eventRow);
            }

            using (SqlConnection connection = new SqlConnection(m_connectionString))
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
            {
                connection.Open();
                bulkCopy.BulkCopyTimeout = 0;
                bulkCopy.DestinationTableName = eventTable.TableName;
                bulkCopy.WriteToServer(eventTable);
            }
        }

        private Dictionary<DataClassification, int> GetEventTypeLookup()
        {
            MeterData.EventTypeDataTable eventTypeTable = new MeterData.EventTypeDataTable();
            DataClassification eventClassification = default(DataClassification);

            using (EventTypeTableAdapter eventTypeAdapter = new EventTypeTableAdapter())
            {
                eventTypeAdapter.Connection.ConnectionString = m_connectionString;
                eventTypeAdapter.Fill(eventTypeTable);

                return eventTypeTable
                    .Where(row => Enum.TryParse(row.Name, out eventClassification))
                    .Select(row => Tuple.Create(eventClassification, row.ID))
                    .ToDictionary(tuple => tuple.Item1, tuple => tuple.Item2);
            }
        }

        #endregion
    }
}
