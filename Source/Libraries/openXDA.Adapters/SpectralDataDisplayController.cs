//******************************************************************************************************
//  SpectralDataDisplayController.cs - Gbtc
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
//  05/25/2018 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Runtime.Caching;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using GSF.Data;
using GSF.Data.Model;
using GSF.Web;
using openHistorian.XDALink;
using openXDA.Model;
using TrendingDataPoint = openHistorian.XDALink.TrendingDataPoint;

namespace openXDA.Adapters
{
    public class SpectralDataDisplayController : ApiController
    {
        #region [ Constructors ]

        public SpectralDataDisplayController(Func<AdoDataConnection> connectionFactory) =>
            ConnectionFactory = connectionFactory;

        #endregion

        #region [ Properties ]

        private Func<AdoDataConnection> ConnectionFactory { get; }

        #endregion

        #region [ Methods ]

        [HttpGet]
        public Task<string[][]> GetData(CancellationToken cancellationToken)
        {
            Dictionary<string, string> query = Request.QueryParameters();
            int meterID = int.Parse(query["meterID"]);
            string level = query["level"];
            string type = query["type"];
            int phaseID = int.Parse(query["phase"]);
            DateTime startTime = DateTime.ParseExact(query["date"] + "T00:00:00", "yyyy-MM-ddTHH:mm:ss", CultureInfo.CurrentCulture);
            DateTime endTime = DateTime.ParseExact(query["date"] + "T23:59:59", "yyyy-MM-ddTHH:mm:ss", CultureInfo.CurrentCulture);

            return Task.Factory.StartNew(() =>
            {
                using (AdoDataConnection connection = ConnectionFactory())
                {
                    string target = "SpectralDataDisplay" + meterID.ToString() + startTime.ToString("yyyyMMdd") + level + type + phaseID.ToString();
                    List<List<string>> data = (List<List<string>>)s_memoryCache.Get(target);
                    if (data == null)
                    {
                        Dictionary<DateTime, Dictionary<string, double>> dict = new Dictionary<DateTime, Dictionary<string, double>>();
                        IEnumerable<Channel> channels = (new TableOperations<Channel>(connection)).QueryRecords(GetRecordRestriction(meterID, phaseID, type));
                        IEnumerable<string> headers = channels.Select(x => GetBucket(type, x));
                        data = new List<List<string>>();

                        data.Add((new List<string>() { "TimeStamp" }).Concat(headers).ToList());

                        string historianServer = connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Server'") ?? "127.0.0.1";
                        string historianInstance = connection.ExecuteScalar<string>("SELECT Value FROM Setting WHERE Name = 'Historian.Instance'") ?? "XDA";
                        using (Historian historian = new Historian(historianServer, historianInstance))
                        {
                            IEnumerable<IGrouping<DateTime, TrendingDataPoint>> points = historian.Read(channels.Select(x => x.ID).ToArray(), startTime, endTime).Where(x => x.SeriesID.ToString() == level).GroupBy(x => x.Timestamp);

                            foreach (var grouping in points)
                            {
                                dict.Add(grouping.Key, new Dictionary<string, double>());
                                foreach (TrendingDataPoint trendingDataPoint in grouping)
                                {
                                    dict[grouping.Key].Add(GetBucket(type, channels.First(x => x.ID == trendingDataPoint.ChannelID)), trendingDataPoint.Value);
                                }
                            }

                            foreach (var pair in dict)
                            {
                                List<string> row = new List<string>() { pair.Key.ToString() };
                                foreach (string header in headers)
                                {
                                    row.Add(pair.Value[header].ToString());
                                }

                                data.Add(row);
                            }
                        }

                        s_memoryCache.Add(target, data, new CacheItemPolicy { SlidingExpiration = TimeSpan.FromMinutes(5.0D) });

                    }
                    return data.Select(x => x.ToArray()).ToArray();
                }
            }, cancellationToken);

        }

        private string GetBucket(string type, Channel channel)
        {
            switch (type)
            {
                case "Interharmonic": return channel.HarmonicGroup.ToString();
                case "HighResolution": return channel.Description.Split('-')[1].Split('k')[0];
                case "LowResolution": return (double.Parse(channel.Name.Split(' ')[2]) / 1000).ToString();
                default: return channel.HarmonicGroup.ToString();
            }
        }

        private RecordRestriction GetRecordRestriction(int meterID, int phaseID, string type)
        {
            RecordRestriction recordRestriction = new RecordRestriction("MeasurementTypeID = (SELECT ID FROM MeasurementType WHERE Name = 'Voltage') AND MeterID = {0} AND PhaseID = {1}", meterID, phaseID);
            RecordRestriction recordRestriction2 = null;

            switch (type)
            {
                case "Interharmonic": recordRestriction2 = new RecordRestriction("Name LIKE '%Interharmonic%'"); break;
                case "HighResolution": recordRestriction2 = new RecordRestriction("Name LIKE '%khz'"); break;
                case "LowResolution": recordRestriction2 = new RecordRestriction("Name LIKE '%Bin%'"); break;
                default: recordRestriction2 = new RecordRestriction("Name LIKE '% Harmonic%' AND HarmonicGroup != 1"); break;
            }

            return recordRestriction + recordRestriction2;
        }

        #endregion

        #region [ Static ]

        private static MemoryCache s_memoryCache = new MemoryCache("SpectralDataDisplay");

        #endregion
    }
}
