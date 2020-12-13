//******************************************************************************************************
//  SPCTools/WizardController.cs - Gbtc
//
//  Copyright © 2018, Grid Protection Alliance.  All Rights Reserved.
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
//  10/23/2020 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

using GSF.Data;
using GSF.Data.Model;
using Newtonsoft.Json.Linq;
using openXDA.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using HIDS;
using openXDA.HIDS;
using openXDA.HIDS.APIExtensions;
using Newtonsoft.Json;

namespace SPCTools
{
    
    [RoutePrefix("api/SPCTools/Wizard")]
    public class WizardController : ApiController
    {
        #region [internal Classes]

        /// <summary>
        /// Request for Saving a full AlarmGroup
        /// </summary>
        public class SaveRequest
        {
            public AlarmGroup AlarmGroup { get; set; }
            public List<AlarmValue> AlarmValues { get; set; }
            public List<AlarmFactor> AlarmFactors { get; set; }
            public List<int> ChannelIDs { get; set; }
            public List<int> StatisticChannelsID { get; set; }
            public DateTime StatisticsStart { get; set; }
            public DateTime StatisticsEnd { get; set; }
            public DataFilter StatisticsFilter { get; set; }
            public int SeriesTypeID { get; set; }

        }

        public class LoadResponse
        {
            public AlarmGroup AlarmGroup { get; set; }
            public int SeriesTypeID { get; set; }
            public int MeasurementTypeID { get; set; }
            public int AlarmDayGroupID { get; set; }
            public List<MeterDetail> SelectedMeter { get; set; }

        }
        #endregion

        #region [Properties]

        protected virtual string Connection { get; } = "systemSettings";
        protected virtual string SaveRoles { get; } = "Administrator";

        private static DateTime s_epoch = new DateTime(1970, 1, 1);

        #endregion


        #region [HTTPRequests]

        /// <summary>
        /// Saves a new or existing AlarmGroup with all associated Parts
        /// </summary>
        [HttpPost, Route("Save")]
        public IHttpActionResult SaveAlarmGroup([FromBody] SaveRequest request)
        {
            if ((SaveRoles != string.Empty && !User.IsInRole(SaveRoles)))
                return Unauthorized();

            try
            {
                using (AdoDataConnection connection = new AdoDataConnection(Connection))
                {
                    bool isNew = false;
                    if (request.AlarmGroup.ID == -1)
                        isNew = true;

                    // AlarmGroup
                    int alarmgroupID = request.AlarmGroup.ID;
                    if (isNew)
                    {
                        new TableOperations<AlarmGroup>(connection).AddNewRecord(request.AlarmGroup);
                        alarmgroupID = connection.ExecuteScalar<int>("SELECT @@IDENTITY");
                    }
                    else
                        new TableOperations<AlarmGroup>(connection).UpdateRecord(request.AlarmGroup);

                    // AlarmFactor (attaches to AlarmGroup)
                    if (request.AlarmFactors.Count > 0)
                    {
                        TableOperations<AlarmFactor> alarmFactorTbl = new TableOperations<AlarmFactor>(connection);
                        if (!isNew)
                            connection.ExecuteNonQuery($"DELETE AlarmFactor WHERE ID NOT IN ({string.Join(",", request.AlarmFactors.Select(item => item.ID))}) AND AlarmGroupID = {alarmgroupID}");
                        request.AlarmFactors.ForEach(item =>
                        {
                            item.AlarmGroupID = alarmgroupID;
                            if (item.ID == -1)
                                alarmFactorTbl.AddNewRecord(item);
                            else
                                alarmFactorTbl.UpdateRecord(item);
                        });
                    }
                    else if (!isNew)
                        connection.ExecuteNonQuery($"DELETE AlarmFactor WHERE AlarmGroupID = {alarmgroupID}");

                    // Alarm (attaches to Channel and AlarmGroup)
                    TableOperations<Alarm> alarmTbl = new TableOperations<Alarm>(connection);
                    List<Tuple<int, int>> updateAlarmID = new List<Tuple<int, int>>();

                    request.ChannelIDs.ForEach(item =>
                    {
                        Alarm alarm = null;
                        int seriesID = connection.ExecuteScalar<int>($"SELECT Top 1 ID FROM Series WHERE ChannelID = {item} AND SeriesTypeID = {request.SeriesTypeID}");
                        int alarmID = 0;
                        if (!isNew)
                        {
                            alarm = alarmTbl.QueryRecordWhere("AlarmGroupID = {0} AND ChannelID = {1}", alarmgroupID, seriesID);
                            alarmID = alarm.ID;

                        }
                        if (isNew || alarm == null)
                        {
                            alarm = new Alarm()
                            {
                                AlarmGroupID = alarmgroupID,
                                Manual = false,
                                SeriesID = seriesID
                            };
                            alarmTbl.AddNewRecord(alarm);
                            alarmID = connection.ExecuteScalar<int>("SELECT @@IDENTITY");
                        }
                        if (!alarm.Manual)
                            updateAlarmID.Add(new Tuple<int, int>(alarmID, item));

                    });

                    // AlarmValue (attaches to Alarm)
                    TableOperations<AlarmValue> alarmValueTbl = new TableOperations<AlarmValue>(connection);
                    Dictionary<int, List<Point>> data = LoadChannel(request.StatisticChannelsID, request.StatisticsStart, request.StatisticsEnd);

                    request.AlarmValues.ForEach(value =>
                    {
                        Token token = new Token(value.Formula, data, request.StatisticChannelsID, request.StatisticsFilter, GetTimeFilter(value));

                        updateAlarmID.ForEach(alarmID =>
                        {
                            if (!isNew)
                            {
                                connection.ExecuteNonQuery($"DELETE AlarmValue WHERE AlarmID = {alarmID} AND StartHour = {value.StartHour} AND AlarmDayID {(value.AlarmdayID == null ? "IS NULL" : ("= " + value.AlarmdayID.ToString()))}");
                            }
                            AlarmValue alarmValue = new AlarmValue()
                            {
                                StartHour = value.StartHour,
                                EndHour = value.EndHour,
                                AlarmdayID = value.AlarmdayID,
                                AlarmID = alarmID.Item1,
                                Formula = value.Formula,
                                Value = (token.isScalar ? token.Scalar : token.Slice[request.StatisticChannelsID.FindIndex(item => item == alarmID.Item2)])
                            };

                            alarmValueTbl.AddNewRecord(alarmValue);

                        });


                    });


                }

                return Ok(true);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }


        }


        /// <summary>
        /// Loads The selected Voltages for an existing AlarmGroup
        /// </summary>
        [HttpGet, Route("LoadVoltages/{id}")]
        public IHttpActionResult LoadVoltages(int id)
        {
            if ((SaveRoles != string.Empty && !User.IsInRole(SaveRoles)))
                return Unauthorized();

            try
            {
                using (AdoDataConnection connection = new AdoDataConnection(Connection))
                {
                    string voltageSQL = $@"SELECT DISTINCT Asset.VoltageKV 
                        FROM 
                            Alarm LEFT JOIN Series ON Series.ID = Alarm.SeriesID LEFT JOIN
                            Channel ON CHannel.ID = Series.ChannelID LEFT JOIN
                            Asset ON Asset.ID = Channel.AssetID 
                        WHERE Alarm.AlarmGroupID = {id} AND Alarm.Manual = 0";

                    DataTable tbl = connection.RetrieveData(voltageSQL);



                return Ok(JsonConvert.SerializeObject(tbl));
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Loads The selected Phases for an existing AlarmGroup
        /// </summary>
        [HttpGet, Route("LoadPhases/{id}")]
        public IHttpActionResult LoadPhases(int id)
        {
            if ((SaveRoles != string.Empty && !User.IsInRole(SaveRoles)))
                return Unauthorized();

            try
            {
                using (AdoDataConnection connection = new AdoDataConnection(Connection))
                {
                    string phaseSQL = $@"SELECT * FROM Phase 
                        WHERE ID IN 
                            (SELECT Channel.PhaseID 
                                FROM Alarm LEFT JOIN 
                                Series on series.ID = Alarm.SeriesID 
                                LEFT JOIN Channel ON CHannel.ID = Series.ChannelID
                            WHERE Alarm.AlarmGroupID = 1 AND Alarm.Manual = 0)";

                    DataTable tbl = connection.RetrieveData(phaseSQL);

                    return Ok(JsonConvert.SerializeObject(tbl));
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // <summary>
        /// Loads an existing AlarmGroup with all associated Parts
        /// </summary>
        [HttpGet, Route("Load/{id}")]
        public IHttpActionResult LoadAlarmgroup(int id)
        {
            if ((SaveRoles != string.Empty && !User.IsInRole(SaveRoles)))
                return Unauthorized();

            try
            {
                using (AdoDataConnection connection = new AdoDataConnection(Connection))
                {
                    AlarmGroup alarmGroup = (new TableOperations<AlarmGroup>(connection)).LoadRecord(id);
                    if (alarmGroup == null)
                        return InternalServerError();

                    int measurmentTypeID = connection.ExecuteScalar<int>($@"SELECT TOP 1 ChannelGroupType.ID
                        FROM 
                            Alarm LEFT JOIN Series ON Series.ID = Alarm.SeriesID LEFT JOIN
                            Channel ON Channel.ID = Series.ChannelID LEFT JOIN
                            ChannelGroupType ON Channel.MeasurementCharacteristicID = ChannelGroupType.MeasurementCharacteristicID AND
                                Channel.MeasurementTypeID = ChannelGroupType.MeasurementTypeID 
                        WHERE  AlarmGroupID = {id} AND Manual = 0");

                    int seriesTypeID = connection.ExecuteScalar<int>($@"SELECT TOP 1 Series.SeriesTypeID
                        FROM 
                            Alarm LEFT JOIN Series ON Series.ID = Alarm.SeriesID
                        WHERE AlarmGroupID = {id} AND Manual = 0");

                    int alarmDayGroupID = connection.ExecuteScalar<int>($@"SELECT TOP 1 ALarmDayGroup.ID FROM AlarmValue LEFT JOIN 
                        Alarm ON AlarmValue.AlarmID = Alarm.ID LEFT JOIN
                        ALarmDayGroupAlarmDay ON ALarmDayGroupAlarmDay.AlarmDayID = AlarmValue.AlarmDayID OR
                            (ALarmDayGroupAlarmDay.AlarmDayID IS NULL AND  AlarmValue.AlarmDayID IS NULL)  LEFT JOIN 
                        ALarmDayGroup ON ALarmDayGroup.ID = ALarmDayGroupAlarmDay.AlarmDayGroupID 
                        WHERE Alarm.AlarmGroupID = {id} AND Alarm.Manual = 0
                        GROUP BY ALarmDayGroup.ID, ALarmDayGroup.Description
                        ORDER BY COUNT(AlarmValue.ID) DESC
                        ");

                    string meterSQl = $@"ID IN (SELECT MeterID FROM ALARM LEFT JOIN
                         Series ON Series.ID = Alarm.SeriesID LEFT JOIN
                        Channel ON Series.ChannelID = Channel.ID WHERE Alarm.AlarmGroupID = {id}
                        )";
                    LoadResponse result = new LoadResponse()
                    {
                        AlarmGroup = alarmGroup,
                        MeasurementTypeID = measurmentTypeID,
                        SeriesTypeID = seriesTypeID,
                        AlarmDayGroupID = alarmDayGroupID,
                        SelectedMeter = (new TableOperations<MeterDetail>(connection).QueryRecordsWhere(meterSQl).ToList())
                    };


                    return Ok(JsonConvert.SerializeObject(result));
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        #endregion

        #region [ HelperFunction ]

        private Dictionary<int, List<Point>> LoadChannel(List<int> channelID, DateTime start, DateTime end)
        {

            HIDSSettings settings = new HIDSSettings();
            settings.Load();

            Dictionary<int, List<Point>> result = new Dictionary<int, List<Point>>();

            string cachTarget = start.Subtract(s_epoch).TotalMilliseconds + "-" + end.Subtract(s_epoch).TotalMilliseconds + "-";
            List<string> dataToGet = channelID.Select(item => item.ToString("x8")).ToList();

            List<Point> data;
            using (API hids = new API())
            {
                hids.Configure(settings);
                data = hids.ReadPointsAsync(dataToGet, start, end).ToListAsync().Result;
            }


            return channelID.ToDictionary(item => item, item => data.Where(pt => pt.Tag == item.ToString("x8")).ToList());


        }

        private Func<DateTime, bool> GetTimeFilter(AlarmValue alarmValue)
        {
            AlarmDay day;
            using (AdoDataConnection connection = new AdoDataConnection(Connection))
            {
                day = new TableOperations<AlarmDay>(connection).QueryRecordWhere("ID = {0}", alarmValue.AlarmdayID);
            }
            if (day == null)
                return (DateTime input) => { return (input.Hour >= alarmValue.StartHour && input.Hour < alarmValue.EndHour); };
            if (day.Name == "WeekEnd")
                return (DateTime input) => { return (input.Hour >= alarmValue.StartHour && input.Hour < alarmValue.EndHour && (input.DayOfWeek == DayOfWeek.Sunday || input.DayOfWeek == DayOfWeek.Saturday)); };
            if (day.Name == "WeekDay")
                return (DateTime input) => { return (input.Hour >= alarmValue.StartHour && input.Hour < alarmValue.EndHour && !(input.DayOfWeek == DayOfWeek.Sunday || input.DayOfWeek == DayOfWeek.Saturday)); };

            return (DateTime input) => { return (input.Hour >= alarmValue.StartHour && input.Hour < alarmValue.EndHour); };
        }

        #endregion
    }

}
