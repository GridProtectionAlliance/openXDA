//******************************************************************************************************
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
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using GrafanaAdapters;
using System.Threading;
using System.Web.Http.Cors;
using GSF;
using GSF.Web.Model;

namespace JSONApi
{
    /// <summary>
    /// Represents a REST based API for a simple JSON based Grafana data source.
    /// </summary>
    
    
    public class GrafanaController : ApiController
    {
        #region [ Members ]

        // Nested Types
        //[EnableCors(origins: "*", headers: "*", methods: "*")]
        private class openXDADataSource : GrafanaDataSourceBase
        {
            private readonly ulong m_baseTicks = (ulong)UnixTimeTag.BaseTicks.Value;

            protected override IEnumerable<DataSourceValue> QueryDataSourceValues(DateTime startTime, DateTime stopTime, bool decimate, Dictionary<ulong, string> targetMap)
            {
                using (DataContext dataContext = new DataContext("systemSettings"))
                {
                    

                }
                return null;
            }

            public override Task<string[]> Search(Target request)
            {
                // TODO: Make Grafana data source metric query more interactive, adding drop-downs and/or query builders
                // For now, just return a truncated list of tag names
                return Task.Factory.StartNew(() => { return new []{ "Event Counts", "Fault Counts", "Alarm Counts"}; });
            }

        }

        // Fields
        private openXDADataSource m_dataSource;

        #endregion

        #region [ Properties ]

        private openXDADataSource DataSource
        {
            get
            {
                if ((object)m_dataSource == null)
                {
                    string uriPath = Request.RequestUri.PathAndQuery;
                    string instanceName;

                    if (uriPath.StartsWith(DefaultApiPath, StringComparison.OrdinalIgnoreCase))
                    {
                        // No instance provided in URL, use default instance name
                        //instanceName = TrendValueAPI.DefaultInstanceName;
                    }
                    else
                    {
                        string[] pathElements = uriPath.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);

                        if (pathElements.Length > 2)
                            instanceName = pathElements[1].Trim();
                        else
                            throw new InvalidOperationException($"Unexpected API URL route destination encountered: {Request.RequestUri}");
                    }

                    //if (!string.IsNullOrWhiteSpace(instanceName))
                    //{
                    //    //LocalOutputAdapter adapterInstance = GetAdapterInstance(instanceName);

                        //if ((object)adapterInstance != null)
                        //{
                            m_dataSource = new openXDADataSource()
                            {
                                //InstanceName = instanceName,
                                //Metadata = adapterInstance.DataSource
                            };
                        //}
                    //}
                }

                return m_dataSource;
            }
        }

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
            return DataSource?.Query(request, cancellationToken) ?? Task.FromResult(new List<TimeSeriesValues>());
            //return null;
        }

        /// <summary>
        /// Search openHistorian for a target.
        /// </summary>
        /// <param name="request">Search target.</param>
        [HttpPost]
        public Task<string[]> Search(Target request)
        {
            return DataSource?.Search(request) ?? Task.FromResult(new string[0]);
        }

        /// <summary>
        /// Queries openHistorian for annotations in a time-range (e.g., Alarms).
        /// </summary>
        /// <param name="request">Annotation request.</param>
        /// <param name="cancellationToken">Propagates notification from client that operations should be canceled.</param>
        [HttpPost]
        public Task<List<AnnotationResponse>> Annotations(AnnotationRequest request, CancellationToken cancellationToken)
        {
            return DataSource?.Annotations(request, cancellationToken) ?? Task.FromResult(new List<AnnotationResponse>());
        }

        #endregion


        #region [ Static ]

        // Static Fields
        private static readonly string DefaultApiPath = "/api/Grafana";

        // Static Methods

        //private static LocalOutputAdapter GetAdapterInstance(string instanceName)
        //{
        //    if (!string.IsNullOrWhiteSpace(instanceName))
        //    {
        //        LocalOutputAdapter adapterInstance;

        //        if (LocalOutputAdapter.Instances.TryGetValue(instanceName, out adapterInstance))
        //            return adapterInstance;
        //    }

        //    return null;
        //}

        #endregion

    }
}
