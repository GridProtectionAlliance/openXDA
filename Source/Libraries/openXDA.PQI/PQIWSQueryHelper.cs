//******************************************************************************************************
//  PQIWSQueryHelper.cs - Gbtc
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
//  07/29/2021 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GSF.Data;

namespace openXDA.PQI
{
    public class PQIWSQueryHelper
    {
        private class EquipmentComparer : IEqualityComparer<Equipment>
        {
            public bool Equals(Equipment x, Equipment y)
            {
                if (x is null && y is null)
                    return true;

                if (x is null || y is null)
                    return false;

                object xKey = GetKey(x);
                object yKey = GetKey(y);
                return Equals(xKey, yKey);
            }

            public int GetHashCode(Equipment equipment)
            {
                object key = GetKey(equipment);
                return key.GetHashCode();
            }

            private object GetKey(Equipment equipment)
            {
                return new
                {
                    equipment.Facility,
                    equipment.Area,
                    equipment.SectionTitle,
                    equipment.SectionRank,
                    equipment.ComponentModel,
                    equipment.Manufacturer,
                    equipment.Series,
                    equipment.ComponentType
                };
            }

            public static EquipmentComparer Instance { get; }
                = new EquipmentComparer();
        }

        private const string FacilityDisturbanceQueryFormat =
            "SELECT " +
            "    MeterFacility.FacilityID, " +
            "    Disturbance.PerUnitMagnitude, " +
            "    Disturbance.DurationSeconds " +
            "FROM " +
            "    Disturbance JOIN " +
            "    Event ON Disturbance.EventID = Event.ID JOIN " +
            "    MeterFacility ON MeterFacility.MeterID = Event.MeterID " +
            "WHERE Disturbance.EventID = {0}";

        public PQIWSQueryHelper(Func<AdoDataConnection> connectionFactory, PQIWSClient pqiwsClient)
        {
            ConnectionFactory = connectionFactory;
            PQIWSClient = pqiwsClient;
        }

        private Func<AdoDataConnection> ConnectionFactory { get; }
        private PQIWSClient PQIWSClient { get; }

        public async Task<bool> HasImpactedComponentsAsync(int eventID, CancellationToken cancellationToken = default)
        {
            async Task<bool> IsImpactedAsync(DataRow facilityDisturbance)
            {
                int facilityID = facilityDisturbance.ConvertField<int>("FacilityID");
                double magnitude = facilityDisturbance.ConvertField<double>("PerUnitMagnitude");
                double duration = facilityDisturbance.ConvertField<double>("DurationSeconds");
                return await PQIWSClient.IsImpactedAsync(facilityID, magnitude, duration, cancellationToken);
            }

            async Task<bool> AnyImpactedAsync(IEnumerable<Task<bool>> isImpactedTasks)
            {
                foreach (Task<bool> task in isImpactedTasks)
                {
                    if (await task)
                        return true;
                }

                return false;
            }

            using (AdoDataConnection connection = ConnectionFactory())
            using (DataTable table = connection.RetrieveData(FacilityDisturbanceQueryFormat, eventID))
            {
                var tasks = table
                    .AsEnumerable()
                    .Select(IsImpactedAsync);

                return await AnyImpactedAsync(tasks);
            }
        }

        public async Task<List<Equipment>> GetAllImpactedEquipmentAsync(int eventID, CancellationToken cancellationToken = default)
        {
            async Task<List<Equipment>> GetImpactedEquipmentAsync(DataRow facilityDisturbance)
            {
                int facilityID = facilityDisturbance.ConvertField<int>("FacilityID");
                double magnitude = facilityDisturbance.ConvertField<double>("PerUnitMagnitude");
                double duration = facilityDisturbance.ConvertField<double>("DurationSeconds");
                return await PQIWSClient.GetImpactedEquipmentAsync(facilityID, magnitude, duration, cancellationToken);
            }

            using (AdoDataConnection connection = ConnectionFactory())
            using (DataTable table = connection.RetrieveData(FacilityDisturbanceQueryFormat, eventID))
            {
                var tasks = table
                    .AsEnumerable()
                    .Select(GetImpactedEquipmentAsync);

                List<Equipment>[] equipmentLists = await Task.WhenAll(tasks);

                return equipmentLists
                    .SelectMany(list => list)
                    .Distinct(EquipmentComparer.Instance)
                    .ToList();
            }
        }
    }
}
