//******************************************************************************************************
//  SPCTools/AlarmGroupController.cs - Gbtc
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

using GSF;
using GSF.Data;
using GSF.Data.Model;
using GSF.Identity;
using GSF.Web;
using GSF.Web.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using openHistorian.XDALink;
using openXDA.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace openXDA.Controllers
{
    [RoutePrefix("api/SPCTools/StaticAlarmCreation")]
    public class StaticAlarmCreationController : ApiController
    {
        #region [internal Classes]

        /// <summary>
        /// Filter for Data when Loading or when applying Setpoints
        /// </summary>
        public class DataFilter
        {
            public bool FilterZero { get; set; }
            public bool FilterUpper { get; set; }
            public double UpperLimit { get; set; }
            public bool FilterLower { get; set; }
            public double LowerLimit { get; set; }
        }

        /// <summary>
        /// Request for Parsing a setpoint 
        /// </summary>
        public class TokenizerRequest
        {
            public string Value { get; set; }
            public DataFilter DataFilter { get; set; }
            public List<int> Channels { get; set; }
            public string StartDate { get; set; }
            public string EndDate { get; set; }
        }

        /// <summary>
        /// Response with Parsed Setpoint
        /// </summary>
        public class TokenizerResponse
        {
            public bool Valid { get; set; }
            public string Message { get; set; }
            public bool IsScalar { get; set; }
            public List<double> Value { get; set; }
        }

        /// <summary>
        /// Requset for saving AlarmGroup
        /// </summary>
        public class SaveRequest
        {
            public AlarmGroup AlarmGroup { get; set; }
            public TokenizerRequest TokenizerRequest { get; set; }
            public string IntervallDataType { get; set; }
            public List<int> ChannelID { get; set; }
            public List<AlarmFactor> AlarmFactor { get; set; }

            public int SeverityID { get; set; }
        }

    #endregion

    #region [Properties]

    protected virtual string Connection { get; } = "systemSettings";
        protected virtual string GetRoles { get; } = "Viewer,Administrator";

        #endregion

        #region [HTTPRequests]

        /// <summary>
        /// Gets the Available Phases for the given <see cref="Meter"/> and <see cref="ChannelGroupType"/>
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("AvailablePhases")]
        public IHttpActionResult GetAvailablePhases([FromBody] JObject postedData)
        {
            if ((GetRoles != string.Empty && !User.IsInRole(GetRoles)))
                return Unauthorized();

            if (postedData == null)
                return Ok();
            try
            {

                List<int> meterIds = postedData["MeterID"].ToObject<List<int>>();
                int measurmentTypeId = postedData["MeasurmentTypeID"].ToObject<int>();

                if (meterIds.Count == 0)
                    return Ok();

                string sql = $@"SELECT DISTINCT Phase.*
                            FROM Channel LEFT JOIN 
                                Meter ON Channel.MeterID = Meter.ID LEFT JOIN
                                Phase ON Channel.PhaseID = Phase.ID 
                            WHERE 
                                Meter.ID in ({string.Join(",",meterIds)}) AND
                            (
                                SELECT ID 
                                    FROM ChannelGroupType
                                    WHERE
                                        ChannelGroupType.MeasurementTypeID = Channel.MeasurementTypeID AND
                                        ChannelGroupType.MeasurementCharacteristicID = Channel.MeasurementCharacteristicID
                            ) = {measurmentTypeId} AND
                            (
                                SELECT COUNT(Series.ID) 
                                    FROM Series LEFT JOIN 
                                        SeriesType ON Series.SeriesTypeID = SeriesType.ID
                                    WHERE 
                                        (SeriesType.Name = 'Minimum' OR SeriesType.Name = 'Maximum' OR SeriesType.Name = 'Average') AND
                                        Series.ChannelID = Channel.ID
                            ) > 0";

                using (AdoDataConnection connection = new AdoDataConnection(Connection))
                {
                    DataTable table = connection.RetrieveData(sql, "");

                    return Ok(table);
                }


            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Gets the Available Base Voltages for the given <see cref="Meter"/> and <see cref="ChannelGroupType"/>
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("AvailableVoltages")]
        public IHttpActionResult GetVoltages([FromBody] JObject postedData)
        {
            if ((GetRoles != string.Empty && !User.IsInRole(GetRoles)))
                return Unauthorized();
             if (postedData == null)
                    return Ok();
                try
                {

                    List<int> meterIds = postedData["MeterID"].ToObject<List<int>>();
                    int measurmentTypeId = postedData["MeasurmentTypeID"].ToObject<int>();

                    if (meterIds.Count == 0 )
                        return Ok(new List<int>());

                    string sql = $@"SELECT DISTINCT Asset.VoltageKV
                            FROM Channel LEFT JOIN 
                                Meter ON Channel.MeterID = Meter.ID LEFT JOIN
                                Asset ON Channel.AssetID = Asset.ID 
                            WHERE 
                                Meter.ID in ({string.Join(",", meterIds)}) AND
                            (
                                SELECT ID 
                                    FROM ChannelGroupType
                                    WHERE
                                        ChannelGroupType.MeasurementTypeID = Channel.MeasurementTypeID AND
                                        ChannelGroupType.MeasurementCharacteristicID = Channel.MeasurementCharacteristicID
                            ) = {measurmentTypeId} AND
                            (
                                SELECT COUNT(Series.ID) 
                                    FROM Series LEFT JOIN 
                                        SeriesType ON Series.SeriesTypeID = SeriesType.ID
                                    WHERE 
                                        (SeriesType.Name = 'Minimum' OR SeriesType.Name = 'Maximum' OR SeriesType.Name = 'Average') AND
                                        Series.ChannelID = Channel.ID
                            ) > 0"; ;

                using (AdoDataConnection connection = new AdoDataConnection(Connection))
                {
                    DataTable table = connection.RetrieveData(sql, "");

                    return Ok(table.AsEnumerable().Select(item => item[0]));
                }


            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Gets the Number of affected Channels
        /// </summary>
        /// <returns>The number of Channels affected</returns>
        [HttpPost, Route("AffectedChannels")]
        public IHttpActionResult GetAffectedChannels([FromBody] JObject postedData)
        {
            if ((GetRoles != string.Empty && !User.IsInRole(GetRoles)))
                return Unauthorized();

            try
            {
                if (postedData == null)
                    return Ok();

                List<int> meterIds = postedData["MeterID"].ToObject<List<int>>();
                int measurmentTypeId = postedData["MeasurmentTypeID"].ToObject<int>();
                List<double> baseVoltages = postedData["BaseVoltage"].ToObject<List<double>>();
                List<int> phaseID = postedData["PhaseID"].ToObject<List<int>>();

                if (meterIds.Count == 0 || baseVoltages.Count == 0 || phaseID.Count == 0)
                    return Ok(0);

                string sql = $@"SELECT COUNT(Channel.ID) 
	                            FROM Channel LEFT JOIN 
                                    Meter ON Channel.MeterID = Meter.ID LEFT JOIN
                                    Asset ON Channel.AssetID = Asset.ID 
                                WHERE 
                                    Meter.ID in ({string.Join(",", meterIds)}) AND
                                (
                                    SELECT ID 
                                        FROM ChannelGroupType
                                        WHERE
                                            ChannelGroupType.MeasurementTypeID = Channel.MeasurementTypeID AND
                                            ChannelGroupType.MeasurementCharacteristicID = Channel.MeasurementCharacteristicID
                                ) = {measurmentTypeId} AND
                                Asset.VoltageKV in ({string.Join(",", baseVoltages)}) AND
	                            Channel.PhaseID in ({string.Join(",", phaseID)})";

                using (AdoDataConnection connection = new AdoDataConnection(Connection))
                {
                    int channelCount = connection.ExecuteScalar<int>(sql);

                    return Ok(channelCount);
                }


            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }


        }

        /// <summary>
        /// Gets the Channels included in the Defined AlarmGroup
        /// </summary>
        /// <returns>The Channnels Affected</returns>
        [HttpPost, Route("Channel")]
        public IHttpActionResult GetChannels([FromBody] JObject postedData)
        {
            if ((GetRoles != string.Empty && !User.IsInRole(GetRoles)))
                return Unauthorized();

            try
            {
                if (postedData == null)
                    return Ok();

                List<int> meterIds = postedData["MeterID"].ToObject<List<int>>();
                int measurmentTypeId = postedData["MeasurmentTypeID"].ToObject<int>();
                List<double> baseVoltages = postedData["BaseVoltage"].ToObject<List<double>>();
                List<int> phaseID = postedData["PhaseID"].ToObject<List<int>>();

                if (meterIds.Count == 0 || baseVoltages.Count == 0 || phaseID.Count == 0)
                    return Ok(new List<ChannelDetail>());

                string condition = $@"MeterID in ({string.Join(",", meterIds)}) AND 
                                    PhaseID in ({string.Join(",", phaseID)}) AND
                                    (SELECT VoltageKV FROM ASSET WHERE Asset.ID = ChannelDetail.AssetID) in ({string.Join(",", baseVoltages)}) AND
                                    (
                                        SELECT ID 
                                            FROM ChannelGroupType
                                            WHERE
                                                ChannelGroupType.MeasurementTypeID = ChannelDetail.MeasurementTypeID AND
                                                ChannelGroupType.MeasurementCharacteristicID = ChannelDetail.MeasurementCharacteristicID
                                    ) = {measurmentTypeId} AND
                                    SeriesType = 'Average'
                                    ";
                using (AdoDataConnection connection = new AdoDataConnection(Connection))
                {
                    TableOperations<ChannelDetail> channelTbl = new TableOperations<ChannelDetail>(connection);
                   
                    return Ok(channelTbl.QueryRecordsWhere(condition));
                }


            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }


        }

        /* This is removed fue to EPRI Time Constraints
        /// <summary>
        /// Gets the Channels included in the Defined AlarmGroup
        /// </summary>
        /// <returns>The Channnels Affected</returns>
        [HttpPost, Route("Channel/{meterId}")]
        public IHttpActionResult GetChannelsByMeter([FromBody] GeneralSettings postedData, string meterId)
        {
            if ((GetRoles != string.Empty && !User.IsInRole(GetRoles)))
                return Unauthorized();

            try
            {
                if (postedData == null)
                    return Ok();

                if (postedData.MeterIDs.Count == 0 || postedData.BaseVoltages.Count == 0 || postedData.PhaseIDs.Count == 0)
                    return Ok();

                string condition = $@"MeterID = {int.Parse(meterId)} AND 
                                    PhaseID in ({string.Join(",", postedData.PhaseIDs)}) AND
                                    (SELECT VoltageKV FROM ASSET WHERE Asset.ID = ChannelDetail.AssetID) in ({string.Join(",", postedData.BaseVoltages)}) AND
                                    (
                                        SELECT ID 
                                            FROM ChannelGroupType
                                            WHERE
                                                ChannelGroupType.MeasurementTypeID = ChannelDetail.MeasurementTypeID AND
                                                ChannelGroupType.MeasurementCharacteristicID = ChannelDetail.MeasurementCharacteristicID
                                    ) = {postedData.MeasurementTypeID} AND
                                    SeriesType = 'Average'
                                    ";
                using (AdoDataConnection connection = new AdoDataConnection(Connection))
                {
                    TableOperations<ChannelDetail> channelTbl = new TableOperations<ChannelDetail>(connection);

                    return Ok(channelTbl.QueryRecordsWhere(condition));
                }


            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }


        }
        */
        /// <summary>
        /// Gets the <see cref="AlarmSeverity"/>
        /// </summary>
        /// <returns>List of all available AlarmSeverity </returns>
        [HttpGet, Route("Severity")]
        public IHttpActionResult GetSeveritys()
        {
            if ((GetRoles != string.Empty && !User.IsInRole(GetRoles)))
                return Unauthorized();

            try
            {
               
                using (AdoDataConnection connection = new AdoDataConnection(Connection))
                {
                    TableOperations<AlarmSeverity> severityTbl = new TableOperations<AlarmSeverity>(connection);

                    return Ok(severityTbl.QueryRecords());
                }


            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }


        }

        /// <summary>
        /// Gets the Channel Data requested for Plots 
        /// </summary>
        /// <returns>List of all DataPoints for the Channel  </returns>
        [HttpPost, Route("GetData/{ChannelId}")]
        public IHttpActionResult getChannelData(int ChannelId, [FromBody] DataFilter postedFilter)
        {
            NameValueCollection queryParameters = Request.RequestUri.ParseQueryString();
            string startTime = queryParameters["start"];
            string endTime = queryParameters["end"];

            if ((GetRoles != string.Empty && !User.IsInRole(GetRoles)))
                return Unauthorized();

            try
            {
                DateTime start = DateTime.Parse(startTime);
                DateTime end = DateTime.Parse(endTime);

                List<double[]> data = createData(ChannelId, start, end);

                data = data.Select(pt => {
                    if (postedFilter.FilterZero && pt[1] == 0.0D)
                        return new double[] { pt[0], double.NaN };
                    if (postedFilter.FilterLower && pt[1] < postedFilter.LowerLimit)
                        return new double[] { pt[0], double.NaN };
                    if (postedFilter.FilterUpper && pt[1] > postedFilter.UpperLimit)
                        return new double[] { pt[0], double.NaN };
                    return pt;
                }).ToList();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }


        }

        // Logic For Computing Points and Parsing Functions

        /// <summary>
        /// Parses a setpoint and Returns any errors that need to be displayed
        /// </summary>
        [HttpPost, Route("ParseSetPoint")]
        public IHttpActionResult ParseSetPoint([FromBody] TokenizerRequest request)
        {
            try
            {
                Token root = new Token(request.Value, DateTime.Parse(request.StartDate), DateTime.Parse(request.EndDate), request.Channels);

                TokenizerResponse result = new TokenizerResponse()
                {
                    IsScalar = root.isScalar,
                    Valid = root.Valid,
                    Message = root.Error,
                    Value = new List<double>()
                };

                // If it is not a scalar or a slice it is an error
                if (!root.isScalar && !root.isSlice && result.Valid)
                {
                    result.Valid = false;
                    result.Message = "The Expression needs to result in a static threshhold.";
                }

                if (result.Valid)
                    result.Value = root.ComputeSlice();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        /// <summary>
        /// Parses a setpoint and Returns any errors that need to be displayed
        /// </summary>
        [HttpPost, Route("CreateAlarmgroup")]
        public IHttpActionResult SaveAlarmGroup([FromBody] SaveRequest request)
        {
            try
            {
                // Figure out Channels
                if (request == null)
                    return InternalServerError();

                using (AdoDataConnection connection = new AdoDataConnection(Connection))
                {

                    new TableOperations<AlarmGroup>(connection).AddNewOrUpdateRecord(request.AlarmGroup);

                    AlarmGroup group = request.AlarmGroup;

                    if (group.ID == -1)
                        group = new TableOperations<AlarmGroup>(connection).QueryRecordWhere("Name = {0}", group.Name);

                    // Start by Getting Setpoint
                    Token root = new Token(request.TokenizerRequest.Value, DateTime.Parse(request.TokenizerRequest.StartDate), DateTime.Parse(request.TokenizerRequest.EndDate), request.TokenizerRequest.Channels);

                    if (!root.Valid || (!root.isScalar && !root.isSlice))
                        return InternalServerError(new Exception("Setpoint Expression is not Valid"));

                    if (request.ChannelID.Count != request.TokenizerRequest.Channels.Count && root.isSlice)
                        return InternalServerError(new Exception("Unable to compute Setpoints for every Channel"));

                   
                    List<double> setPoint = root.ComputeSlice();

                    //Create alarms, Alarmvalues and AlarmFactors
                    TableOperations<Alarm> alarmTbl = new TableOperations<Alarm>(connection);
                    TableOperations<Series> seriesTbl = new TableOperations<Series>(connection);
                    TableOperations<AlarmFactor> factorTbl = new TableOperations<AlarmFactor>(connection);
                    TableOperations<AlarmValue> valueTbl = new TableOperations<AlarmValue>(connection);


                    SeriesType seriesType = new TableOperations<SeriesType>(connection).QueryRecordWhere("Name = {0}", request.IntervallDataType);
                    
                    request.ChannelID.ForEach(chID =>
                    {

                        Series series = seriesTbl.QueryRecordWhere("ChannelID = {0} AND SeriesID = {1}", chID, seriesType.ID);
                        Alarm alarm = alarmTbl.QueryRecordWhere("AlarmGroupID = {0} AND SeriesID = {1}", group.ID, series.ID);
                        if (alarm == null)
                        {
                            alarmTbl.AddNewOrUpdateRecord(new Alarm() { AlarmGroupID = group.ID, SeriesID = chID, SeverityID = request.SeverityID });
                            alarm = alarmTbl.QueryRecordWhere("AlarmGroupID = {0} AND SeriesID = {1}", group.ID, series.ID);
                        }

                        // Add Factors
                        request.AlarmFactor.ForEach(factor => {
                            factor.AlarmID = alarm.ID;
                            factorTbl.AddNewRecord(factor);
                        });

                        double threshhold = setPoint[0];
                        if (!root.isScalar)
                        {
                            int index = request.TokenizerRequest.Channels.IndexOf(chID);
                            threshhold = setPoint[index];
                        }
                        // Add Values
                        valueTbl.AddNewRecord(new AlarmValue() { AlarmID = alarm.ID, AlarmdayID = null, EndHour = null, StartHour = 0, Value = threshhold });
                    });



                }
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }
        #endregion

        #region [ HIDS Data Functions ]

        private static readonly Dictionary<int, double> m_randomData = new Dictionary<int, double>();

        public static List<double[]> createData(int channelId, DateTime start, DateTime end)
        {
            double delta = (end.Subtract(m_epoch).TotalMilliseconds - start.Subtract(m_epoch).TotalMilliseconds) / 1000.0D;
            double center;
            if (!m_randomData.TryGetValue(channelId, out center))
            {
                center = m_random.NextDouble() * 1000.0D;
                m_randomData.Add(channelId, center);
            }
                
            // For Now we just make up 1000 points and send those back
            // ToDo: Pull from hidds instead
            List<double[]> data = Enumerable.Range(0, 1000).Select(i => new double[] { start.Subtract(m_epoch).TotalMilliseconds + (double)i * delta, ((double)i > center ? 30.0d - ((double)i - center) / (1000.0D - center) : 30.0D * (double)i / center) }).ToList();

            return data;
        }

        #endregion

        private static DateTime m_epoch = new DateTime(1970, 1, 1);
        private static Random m_random = new Random();

        //This is for easy Testing of Tokenizer should be removed eventually
        [HttpGet, Route("Token/{token}")]
        public IHttpActionResult TestToken(string token)
        {
           
            try
            {

                Token token1 = new Token(token, DateTime.Now.Subtract(new TimeSpan(31, 0, 0, 0, 0)), DateTime.Now,new List<int>() { 1, 2, 3 });
                return Ok();

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }


        }
    }
}
