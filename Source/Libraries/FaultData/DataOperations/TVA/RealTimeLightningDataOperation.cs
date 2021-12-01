//******************************************************************************************************
//  RealTimeLightningDataOperation.cs - Gbtc
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
//  11/29/2021 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Data;
using FaultData.DataAnalysis;
using FaultData.DataResources;
using FaultData.DataResources.Vaisala;
using FaultData.DataSets;
using GSF.Data;
using GSF.Data.Model;
using openXDA.Model;

namespace FaultData.DataOperations.TVA
{
    public class RealTimeLightningDataOperation : DataOperationBase<MeterDataSet>
    {
        public override void Execute(MeterDataSet meterDataSet)
        {
            LightningDataResource lightningDataResource = meterDataSet.GetResource<LightningDataResource>();

            if (lightningDataResource.LightningStrikeLookup.Count == 0)
                return;

            using (AdoDataConnection connection = meterDataSet.CreateDbConnection())
            {
                foreach (var kvp in lightningDataResource.LightningStrikeLookup)
                {
                    DataGroup dataGroup = kvp.Key;
                    TableOperations<Event> eventTable = new TableOperations<Event>(connection);
                    Event evt = eventTable.GetEvent(meterDataSet.FileGroup, dataGroup);
                    if (evt == null)
                        continue;

                    foreach (ILightningStrike strike in kvp.Value)
                    {
                        IExtendedLightningData extendedLightningData = strike.GetExtendedData<IExtendedLightningData>();
                        if (extendedLightningData is null)
                            continue;

                        int? strikeID = GetID(connection, evt.ID, strike);
                        if (strikeID is null)
                            continue;

                        const string QueryFormat =
                            "INSERT INTO VaisalaExtendedLightningData " +
                            "( " +
                            "    LightningStrikeID, " +
                            "    PeakCurrent, " +
                            "    FlashMultiplicity, " +
                            "    ParticipatingSensors, " +
                            "    DegreesOfFreedom, " +
                            "    EllipseAngle, " +
                            "    SemiMajorAxisLength, " +
                            "    SemiMinorAxisLength, " +
                            "    ChiSquared, " +
                            "    Risetime, " +
                            "    PeakToZeroTime, " +
                            "    MaximumRateOfRise, " +
                            "    CloudIndicator, " +
                            "    AngleIndicator, " +
                            "    SignalIndicator, " +
                            "    TimingIndicator " +
                            ") " +
                            "VALUES({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15})";

                        object[] parameters =
                        {
                            strikeID,
                            extendedLightningData.PeakCurrent,
                            extendedLightningData.FlashMultiplicity,
                            extendedLightningData.ParticipatingSensors,
                            extendedLightningData.DegreesOfFreedom,
                            extendedLightningData.EllipseAngle,
                            extendedLightningData.SemiMajorAxisLength,
                            extendedLightningData.SemiMinorAxisLength,
                            extendedLightningData.ChiSquared,
                            extendedLightningData.Risetime,
                            extendedLightningData.PeakToZeroTime,
                            extendedLightningData.MaximumRateOfRise,
                            extendedLightningData.CloudIndicator,
                            extendedLightningData.AngleIndicator,
                            extendedLightningData.SignalIndicator,
                            extendedLightningData.TimingIndicator
                        };

                        connection.ExecuteNonQuery(QueryFormat, parameters);
                    }
                }
            }
        }

        private int? GetID(AdoDataConnection connection, int eventID, ILightningStrike strike)
        {
            IDbDataParameter ToDateTime2(DateTime dateTime)
            {
                using (IDbCommand command = connection.Connection.CreateCommand())
                {
                    IDbDataParameter parameter = command.CreateParameter();
                    parameter.DbType = DbType.DateTime2;
                    parameter.Value = dateTime;
                    return parameter;
                }
            }

            const string QueryFormat =
                "SELECT ID " +
                "FROM LightningStrike " +
                "WHERE " +
                "    EventID = {0} AND " +
                "    Service = {1} AND " +
                "    UTCTime = {2} AND " +
                "    Latitude = {3} AND " +
                "    Longitude = {4}";

            string service = strike.Service;
            object time = ToDateTime2(strike.UTCTime);
            double latitude = strike.Latitude;
            double longitude = strike.Longitude;
            return connection.ExecuteScalar<int?>(QueryFormat, eventID, service, time, latitude, longitude);
        }
    }
}
