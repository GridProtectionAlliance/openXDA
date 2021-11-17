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
using System.Collections.Generic;
using System.Linq;
using FaultData.DataResources;
using FaultData.DataResources.Vaisala;
using FaultData.DataSets;
using GSF.Data;

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
                IEnumerable<ILightningStrike> lightningStrikes = lightningDataResource.LightningStrikeLookup.Values
                    .SelectMany(list => list);

                foreach (ILightningStrike strike in lightningStrikes)
                {
                    int? strikeID = GetID(connection, strike);
                    if (strikeID is null)
                        continue;

                    IExtendedLightningData extendedLightningData = strike.GetExtendedData<IExtendedLightningData>();
                    if (extendedLightningData is null)
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

        private int? GetID(AdoDataConnection connection, ILightningStrike strike)
        {
            const string QueryFormat =
                "SELECT ID " +
                "FROM LightningStrike " +
                "WHERE " +
                "    Service = {0} AND " +
                "    UTCTime = {1} AND " +
                "    Latitude = {2} AND " +
                "    Longitude = {3}";

            string service = strike.Service;
            DateTime time = strike.UTCTime;
            double latitude = strike.Latitude;
            double longitude = strike.Longitude;
            return connection.ExecuteScalar<int?>(QueryFormat, service, time, latitude, longitude);
        }
    }
}
