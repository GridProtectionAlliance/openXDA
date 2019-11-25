//******************************************************************************************************
//  LightningDataOperation.cs - Gbtc
//
//  Copyright © 2019, Grid Protection Alliance.  All Rights Reserved.
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
//  11/19/2019 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using FaultData.DataAnalysis;
using FaultData.DataResources;
using FaultData.DataSets;
using GSF.Data;
using GSF.Data.Model;
using openXDA.Model;

namespace FaultData.DataOperations
{
    public class LightningDataOperation : DataOperationBase<MeterDataSet>
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
                    List<ILightningStrike> lightningStrikes = kvp.Value;

                    TableOperations<Event> eventTable = new TableOperations<Event>(connection);
                    Event evt = eventTable.GetEvent(meterDataSet.FileGroup, dataGroup);

                    if (evt == null)
                        continue;

                    foreach (ILightningStrike lightningStrike in lightningStrikes)
                    {
                        const string Query =
                            "INSERT INTO LightningStrike " +
                            "( " +
                            "    EventID, " +
                            "    Service, " +
                            "    UTCTime, " +
                            "    DisplayTime, " +
                            "    Amplitude, " +
                            "    Latitude, " +
                            "    Longitude " +
                            ") " +
                            "VALUES({0}, {1}, {2}, {3}, {4}, {5}, {6})";

                        object[] parameters =
                        {
                            evt.ID,
                            lightningStrike.Service,
                            ToDateTime2(connection, lightningStrike.UTCTime),
                            lightningStrike.DisplayTime,
                            lightningStrike.Amplitude,
                            lightningStrike.Latitude,
                            lightningStrike.Longitude
                        };

                        connection.ExecuteNonQuery(Query, parameters);
                    }
                }
            }
        }

        private IDbDataParameter ToDateTime2(AdoDataConnection connection, DateTime dateTime)
        {
            using (IDbCommand command = connection.Connection.CreateCommand())
            {
                IDbDataParameter parameter = command.CreateParameter();
                parameter.DbType = DbType.DateTime2;
                parameter.Value = dateTime;
                return parameter;
            }
        }
    }
}
