﻿//******************************************************************************************************
//  GrafanaController.cs - Gbtc
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
//  03/14/2017 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************


using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using GrafanaAdapters;
using GSF;
using GSF.Data;

namespace openXDA.Adapters
{
    /// <summary>
    /// Represents a REST based API for a simple JSON based Grafana data source.
    /// </summary>
    public class GrafanaController : ApiController
    {
        #region [ Members ]

        // Nested Types
        //[EnableCors(origins: "*", headers: "*", methods: "*")]
        private class XDADataSource : GrafanaDataSourceBase, IDisposable
        {
            #region [ Constructors ]

            public XDADataSource(AdoDataConnection connection)
            {
                Connection = connection;
                BaseTicks = UnixTimeTag.BaseTicks.Value;
                InstanceName = "XDA";

                Metadata = new DataSet();
                DataTable table = connection.RetrieveData("SELECT * FROM ActiveMeasurements").Copy();
                table.TableName = "ActiveMeasurements";
                Metadata.Tables.Add(table);
            }

            public void Dispose()
            {
                if (IsDisposed)
                    return;

                try
                {
                    Metadata.Dispose();
                    Connection.Dispose();
                }
                finally
                {
                    IsDisposed = true;
                }
            }

            #endregion

            #region [ Properties ]

            private AdoDataConnection Connection { get; }
            private long BaseTicks { get; }
            private bool IsDisposed { get; set; }

            #endregion

            #region [ Methods ]

            /// <summary>
            /// Search data source for a target.
            /// </summary>
            /// <param name="request">Search target.</param>
            public override Task<string[]> Search(Target request, CancellationToken cancellationToken)
            {
                // TODO: Make Grafana data source metric query more interactive, adding drop-downs and/or query builders
                // For now, just return a truncated list of tag names
                string target = (request.target == "select metric" ? "" : request.target);
                return Task.Factory.StartNew(() =>
                {
                    return Metadata.Tables["ActiveMeasurements"].Select($"ID LIKE '{InstanceName}:%' AND PointTag LIKE '%{target}%'").Take(MaximumSearchTargetsPerRequest).Select(row => $"{row["PointTag"]}").ToArray();
                });
            }

            protected override IEnumerable<DataSourceValue> QueryDataSourceValues(DateTime startTime, DateTime stopTime, string interval, bool decimate, Dictionary<ulong, string> targetMap)
            {
                foreach (KeyValuePair<ulong, string> kvp in targetMap)
                {
                    DataTable data;
                    switch (kvp.Value.Split('_')[kvp.Value.Split('_').Length - 1])
                    {
                        case "Event": // GlobalEventCount
                            data = Connection.RetrieveData("SELECT Count(*) as Count, Cast(StartTime as Date) as Date FROM Event WHERE StartTime >= {0} AND EndTime <= {1} AND MeterID Like {2} Group by Cast(StartTime as Date)", startTime, stopTime, int.Parse(kvp.Value.Split('_').First()));

                            foreach (DataRow row in data.Rows)
                            {
                                yield return new DataSourceValue
                                {
                                    Target = kvp.Value,
                                    Time = (row.ConvertField<DateTime>("Date").Date.Ticks - BaseTicks) / (double)Ticks.PerMillisecond,
                                    Value = row.ConvertField<int>("Count")
                                };
                            }

                            break;
                        case "Fault":
                            data = Connection.RetrieveData("SELECT Count(*) as Count, Cast(Inception as Date) as Date FROM FaultSummary WHERE Inception Between {0} AND {1} AND Algorithm LIKE 'Simple' AND EventID IN (Select ID FROM Event WHERE MeterID = {2}) Group by Cast(Inception as Date)", startTime, stopTime, int.Parse(kvp.Value.Split('_').First()));

                            foreach (DataRow row in data.Rows)
                            {
                                yield return new DataSourceValue
                                {
                                    Target = kvp.Value,
                                    Time = (row.ConvertField<DateTime>("Date").Date.Ticks - BaseTicks) / (double)Ticks.PerMillisecond,
                                    Value = row.ConvertField<int>("Count")
                                };
                            }

                            break;
                        case "Alarm":
                            data = Connection.RetrieveData("SELECT Count(*) as Count, Cast(Date as Date) as Date FROM ChannelAlarmSummary WHERE Date Between {0} AND {1} AND ChannelID IN (Select ID FROM Channel WHERE MeterID = {2}) Group by Cast(Date as Date)", startTime, stopTime, int.Parse(kvp.Value.Split('_').First()));

                            foreach (DataRow row in data.Rows)
                            {
                                yield return new DataSourceValue
                                {
                                    Target = kvp.Value,
                                    Time = (row.ConvertField<DateTime>("Date").Date.Ticks - BaseTicks) / (double)Ticks.PerMillisecond,
                                    Value = row.ConvertField<int>("Count")
                                };
                            }


                            break;
                        case "Trending":

                            break;

                        case "GlobalEvent":
                            var numAlpha = new Regex("(?<Numeric>[0-9]*)(?<Alpha>[a-zA-Z]*)");
                            var match = numAlpha.Match(interval);
                            using (IDbCommand sc = Connection.Connection.CreateCommand())
                            {
                                sc.CommandText = "dbo.GetGlobalEventsForGrafana";
                                sc.CommandType = CommandType.StoredProcedure;
                                IDbDataParameter param1 = sc.CreateParameter();
                                param1.ParameterName = "@startDate";
                                param1.Value = startTime;
                                IDbDataParameter param2 = sc.CreateParameter();
                                param2.ParameterName = "@endDate";
                                param2.Value = stopTime;
                                IDbDataParameter param3 = sc.CreateParameter();
                                param3.ParameterName = "@step";
                                param3.Value = match.Groups["Numeric"].Value;
                                IDbDataParameter param4 = sc.CreateParameter();
                                param4.ParameterName = "@stepUnit";
                                param4.Value = match.Groups["Alpha"].Value;
                                IDbDataParameter param5 = sc.CreateParameter();
                                param5.ParameterName = "@table";
                                param5.Value = "EventCount";

                                sc.Parameters.Add(param1);
                                sc.Parameters.Add(param2);
                                sc.Parameters.Add(param3);
                                sc.Parameters.Add(param4);
                                sc.Parameters.Add(param5);

                                using (IDataReader rdr = sc.ExecuteReader())
                                {
                                    while (rdr.Read())
                                    {
                                        yield return new DataSourceValue()
                                        {
                                            Target = targetMap.First().Value,
                                            Time = (DateTime.Parse(rdr["Date"].ToString()).Ticks - BaseTicks) / (double)Ticks.PerMillisecond,
                                            Value = int.Parse(rdr["Count"].ToString())
                                        };
                                    }
                                }
                            }
                            break;
                    }
                }
            }

            #endregion
        }

        #endregion

        #region [ Constructors ]

        public GrafanaController(Func<AdoDataConnection> connectionFactory) =>
            ConnectionFactory = connectionFactory;

        #endregion

        #region [ Properties ]

        private Func<AdoDataConnection> ConnectionFactory { get; }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Validates that openHistorian Grafana data source is responding as expected.
        /// </summary>
        [HttpGet]
        public HttpResponseMessage Index()
        {
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        /// <summary>
        /// Queries openHistorian as a Grafana data source.
        /// </summary>
        /// <param name="request">Query request.</param>
        /// <param name="cancellationToken">Propagates notification from client that operations should be canceled.</param>
        [HttpPost]
        public Task<List<TimeSeriesValues>> Query(QueryRequest request, CancellationToken cancellationToken)
        {
            using (AdoDataConnection connection = ConnectionFactory())
            using (XDADataSource dataSource = new XDADataSource(connection))
            {
                return dataSource.Query(request, cancellationToken)
                    ?? Task.FromResult(new List<TimeSeriesValues>());
            }
        }

        /// <summary>
        /// Search openHistorian for a target.
        /// </summary>
        /// <param name="request">Search target.</param>
        [HttpPost]
        public Task<string[]> Search(Target request)
        {
            using (AdoDataConnection connection = ConnectionFactory())
            using (XDADataSource dataSource = new XDADataSource(connection))
            {
                return dataSource.Search(request, CancellationToken.None)
                    ?? Task.FromResult(new string[0]);
            }
        }

        /// <summary>
        /// Queries openHistorian for annotations in a time-range (e.g., Alarms).
        /// </summary>
        /// <param name="request">Annotation request.</param>
        /// <param name="cancellationToken">Propagates notification from client that operations should be canceled.</param>
        [HttpPost]
        public Task<List<AnnotationResponse>> Annotations(AnnotationRequest request, CancellationToken cancellationToken)
        {
            using (AdoDataConnection connection = ConnectionFactory())
            using (XDADataSource dataSource = new XDADataSource(connection))
            {
                return dataSource.Annotations(request, cancellationToken)
                    ?? Task.FromResult(new List<AnnotationResponse>());
            }
        }

        #endregion
    }
}
