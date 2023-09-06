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

        private class PQIModelComparer<T>: IEqualityComparer<T> where T: class, PQIModel
        {
            public bool Equals(T x, T y)
            {
                if (x is null && y is null)
                    return true;

                if (x is null || y is null)
                    return false;

                object xKey = GetKey(x);
                object yKey = GetKey(y);
                return Equals(xKey, yKey);
            }

            public int GetHashCode(T facilityAudit)
            {
                object key = GetKey(facilityAudit);
                return key.GetHashCode();
            }

            private object GetKey(T facilityAudit)
            {
                return new
                {
                    facilityAudit.Path
                };
            }

            public static PQIModelComparer<T> Instance { get; }
                = new PQIModelComparer<T>();
        }

        private const string FacilityDisturbanceQueryFormat =
            @"SELECT 
                Customer.PQIFacilityID,
                Disturbance.PerUnitMagnitude,
                Disturbance.DurationSeconds 
            FROM Disturbance LEFT JOIN
                Event ON Event.ID = Disturbance.EventID  LEFT JOIN
                CustomerMeter ON CustomerMeter.MeterID = Event.MeterID LEFT JOIN 
                Customer ON Customer.ID = CustomerMeter.CustomerID
            WHERE PQIFacilityID IS NOT NULL AND EventID = {0}
            UNION
            SELECT
                Customer.PQIFacilityID,
                Disturbance.PerUnitMagnitude,
                Disturbance.DurationSeconds
            FROM Disturbance LEFT JOIN
                Event ON Event.ID = Disturbance.EventID LEFT JOIN
                CustomerAsset ON CustomerAsset.AssetID = Event.AssetID LEFT JOIN
                Customer ON Customer.ID = CustomerAsset.CustomerID
            WHERE PQIFacilityID IS NOT NULL AND EventID = {0}";

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
                int facilityID = facilityDisturbance.ConvertField<int>("PQIFacilityID");
                double magnitude = facilityDisturbance.ConvertField<double>("PerUnitMagnitude");
                double duration = facilityDisturbance.ConvertField<double>("DurationSeconds");
                return await PQIWSClient.IsImpactedAsync(facilityID, magnitude, duration, cancellationToken).ConfigureAwait(false);
            }

            async Task<bool> AnyImpactedAsync(IEnumerable<Task<bool>> isImpactedTasks)
            {
                foreach (Task<bool> task in isImpactedTasks)
                {
                    if (await task.ConfigureAwait(false))
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

                return await AnyImpactedAsync(tasks).ConfigureAwait(false);
            }
        }

        public async Task<List<Equipment>> GetAllImpactedEquipmentAsync(int eventID, CancellationToken cancellationToken = default)
        {
            using (AdoDataConnection connection = ConnectionFactory())
                return await GetAllImpactedEquipmentAsync(connection, eventID, cancellationToken).ConfigureAwait(false);
        }

        public async Task<List<Equipment>> GetAllImpactedEquipmentAsync(IEnumerable<int> eventIDs, CancellationToken cancellationToken = default)
        {
            using (AdoDataConnection connection = ConnectionFactory())
            {
                Task<List<Equipment>> _GetAllImpactedEquipmentAsync(int eventID) =>
                    GetAllImpactedEquipmentAsync(connection, eventID, cancellationToken);

                var tasks = eventIDs.Select(_GetAllImpactedEquipmentAsync);
                List<Equipment>[] equipmentLists = await Task.WhenAll(tasks).ConfigureAwait(false);
                return Flatten(equipmentLists);
            }
        }

        public async Task<List<Tuple<TestCurve, List<TestCurvePoint>>>> GetAllTestCurvesAsync (IEnumerable<int> eventIDs, CancellationToken cancellationToken = default)
        {
            using (AdoDataConnection connection = ConnectionFactory())
            {
                Task<List<Tuple<TestCurve, List<TestCurvePoint>>>> _GetAllTestCurvesAsync(int eventID) =>
                    GetAllCurvesAsync(connection, eventID, cancellationToken);

                var tasks = eventIDs.Select(_GetAllTestCurvesAsync);
                List<Tuple<TestCurve, List<TestCurvePoint>>>[] curveList = await Task.WhenAll(tasks).ConfigureAwait(false);
                return curveList.SelectMany(list => list).ToList();
            }
        }

        private async Task<List<Tuple<TestCurve,List<TestCurvePoint>>>> GetAllCurvesAsync(AdoDataConnection connection, int eventID, CancellationToken cancellationToken = default)
        {
            int GetFacilityID(DataRow facilityDisturbance)
            {
                return facilityDisturbance.ConvertField<int>("PQIFacilityID");
            }

            async Task<List<FacilityAudit>> GetFacilityAudits(int facilityID)
            {
                return await PQIWSClient.GetFacilityAudits(facilityID, cancellationToken).ConfigureAwait(false);
            }

            async Task<List<PQIEquipment>> GetAuditedEquipment(FacilityAudit facilityAudit)
            {
                return await PQIWSClient.GetAuditedEquipment(facilityAudit, cancellationToken).ConfigureAwait(false);
            }

            async Task<List<AuditCurve>> GetAuditCurves(PQIEquipment equipment)
            {
                return await PQIWSClient.GetAuditCurve(equipment, cancellationToken).ConfigureAwait(false);
            }

            async Task<TestCurve> GetTestCurve(AuditCurve auditCurve)
            {
                return await PQIWSClient.GetTestCurve(auditCurve, cancellationToken).ConfigureAwait(false);
            }

            async Task<Tuple<TestCurve,List<TestCurvePoint>>> GetTestCurvePoints(TestCurve testCurve)
            {
                return new Tuple<TestCurve, List<TestCurvePoint>>(testCurve, (await PQIWSClient.GetTestCurvePoints(testCurve, cancellationToken).ConfigureAwait(false)));                
            }

            using (DataTable table = connection.RetrieveData(FacilityDisturbanceQueryFormat, eventID))
            {
                int[] facilityIDs = table
                    .AsEnumerable()
                    .Select(GetFacilityID)
                    .ToArray();

                List<FacilityAudit>[] facilityAudits = await Task.WhenAll(facilityIDs.Distinct().Select(GetFacilityAudits)).ConfigureAwait(false);
                List<PQIEquipment>[] auditedEquipment = await Task.WhenAll(Flatten(facilityAudits).Select(GetAuditedEquipment)).ConfigureAwait(false);
                List<AuditCurve>[] auditCurves = await Task.WhenAll(Flatten(auditedEquipment).Select(GetAuditCurves)).ConfigureAwait(false);
                TestCurve[] testCurves = await Task.WhenAll(Flatten(auditCurves).Select(GetTestCurve)).ConfigureAwait(false);

                return (await Task.WhenAll(testCurves.Distinct().Select(GetTestCurvePoints)).ConfigureAwait(false)).ToList();
            }
        }
        private async Task<List<Equipment>> GetAllImpactedEquipmentAsync(AdoDataConnection connection, int eventID, CancellationToken cancellationToken)
        {
            async Task<List<Equipment>> GetImpactedEquipmentAsync(DataRow facilityDisturbance)
            {
                int facilityID = facilityDisturbance.ConvertField<int>("PQIFacilityID");
                double magnitude = facilityDisturbance.ConvertField<double>("PerUnitMagnitude");
                double duration = facilityDisturbance.ConvertField<double>("DurationSeconds");
                return await PQIWSClient.GetImpactedEquipmentAsync(facilityID, magnitude, duration, cancellationToken).ConfigureAwait(false);
            }

            using (DataTable table = connection.RetrieveData(FacilityDisturbanceQueryFormat, eventID))
            {
                var tasks = table
                    .AsEnumerable()
                    .Select(GetImpactedEquipmentAsync);

                List<Equipment>[] equipmentLists = await Task.WhenAll(tasks).ConfigureAwait(false);
                return Flatten(equipmentLists);
            }
        }

        private List<Equipment> Flatten(IEnumerable<IEnumerable<Equipment>> equipmentLists) => equipmentLists
            .SelectMany(list => list)
            .Distinct(EquipmentComparer.Instance)
            .ToList();

        private List<T> Flatten<T>(IEnumerable<IEnumerable<T>> facilityAuditLists) where T: class,PQIModel => facilityAuditLists
            .SelectMany(list => list)
            .Distinct(PQIModelComparer<T>.Instance)
            .ToList();
    }
}
