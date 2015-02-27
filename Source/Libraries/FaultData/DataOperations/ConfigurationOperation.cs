//******************************************************************************************************
//  ConfigurationOperation.cs - Gbtc
//
//  Copyright © 2014, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the Eclipse Public License -v 1.0 (the "License"); you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/eclipse-1.0.php
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  07/21/2014 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FaultData.DataAnalysis;
using FaultData.Database;
using FaultData.DataSets;
using log4net;
using SeriesKey = System.Tuple<int, string, string, string, string, string>;

namespace FaultData.DataOperations
{
    public class ConfigurationOperation : DataOperationBase<MeterDataSet>
    {
        #region [ Members ]

        // Fields
        private string m_filePattern;
        private MeterInfoDataContext m_meterInfo;
		
        #endregion

        #region [ Properties ]

        public string FilePattern
        {
            get
            {
                return m_filePattern;
            }
            set
            {
                m_filePattern = value;
            }
        }

        #endregion

        #region [ Methods ]

        public override void Prepare(DbAdapterContainer dbAdapterContainer)
        {
            m_meterInfo = dbAdapterContainer.MeterInfoAdapter;
        }

        public override void Execute(MeterDataSet meterDataSet)
        {
            Meter meter;
            MeterFileGroup meterFileGroup;
            string meterKey;

            Dictionary<SeriesKey, Series> seriesLookup;
            Series seriesInfo;

            Log.Info("Executing operation to locate meter in database...");

            // Try to parse the name of the meter from the file path
            if (!string.IsNullOrEmpty(FilePattern) && TryParseFilePath(meterDataSet.FilePath, out meterKey))
                meterDataSet.Meter.AssetKey = meterKey;

            // Search the database for a meter definition that matches the parsed meter
            meter = m_meterInfo.Meters.SingleOrDefault(m => m.AssetKey == meterDataSet.Meter.AssetKey);

            if ((object)meter != null)
            {
                Log.Info(string.Format("Found meter {0} in database.", meter.Name));

                // Match the parsed series with the ones associated with the meter in the database
                seriesLookup = meter.Channels
                    .SelectMany(channel => channel.Series)
                    .Where(series => string.IsNullOrEmpty(series.SourceIndexes))
                    .ToDictionary(GetKey);

                foreach (DataSeries dataSeries in meterDataSet.DataSeries)
                {
                    if ((object)dataSeries.SeriesInfo == null)
                        continue;

                    if (seriesLookup.TryGetValue(GetKey(dataSeries.SeriesInfo), out seriesInfo))
                        dataSeries.SeriesInfo = seriesInfo;
                }

                // Create data series for series which
                // are combinations of the parsed series
                foreach (Series series in meter.Channels.SelectMany(channel => channel.Series))
                {
                    if (!string.IsNullOrEmpty(series.SourceIndexes))
                        AddCalculatedDataSeries(meterDataSet, series);
                }

                // There may be some placeholder DataSeries objects with no data so that indexes
                // would be correct for calculating data series--now that we are finished
                // calculating data series, these need to be removed
                for (int i = meterDataSet.DataSeries.Count - 1; i >= 0; i--)
                {
                    if ((object)meterDataSet.DataSeries[i].SeriesInfo == null)
                        meterDataSet.DataSeries.RemoveAt(i);
                }

                // Replace the parsed meter with
                // the one from the database
                meterDataSet.Meter = meter;
            }
            else
            {
                Log.Info(string.Format("No existing meter found matching meter with name {0}.", meterDataSet.Meter.Name));

                // If configuration cannot be modified and existing configuration cannot be found for this meter,
                // throw an exception to indicate the operation could not be executed
                throw new InvalidOperationException("Cannot process meter - configuration does not exist");
            }

            // Remove data series that were not defined in the configuration
            // since configuration information cannot be added for it
            RemoveUndefinedDataSeries(meterDataSet);

            meterFileGroup = new MeterFileGroup();
            meterFileGroup.Meter = meterDataSet.Meter;
            meterFileGroup.FileGroupID = meterDataSet.FileGroup.ID;

            // Submit changes to the meter configuration so
            // that they may be entered into the database
            m_meterInfo.SubmitChanges();
        }

        public override void Load(DbAdapterContainer dbAdapterContainer)
        {
        }

        private bool TryParseFilePath(string fileName, out string meterKey)
        {
            Match match = Regex.Match(fileName, FilePattern);
            Group meterKeyGroup;

            if (match.Success)
            {
                meterKeyGroup = match.Groups["AssetKey"];

                if ((object)meterKeyGroup != null)
                {
                    meterKey = meterKeyGroup.Value;
                    return true;
                }
            }

            meterKey = null;
            return false;
        }

        private void AddCalculatedDataSeries(MeterDataSet meterDataSet, Series series)
        {
            int sourceIndex = 0;

            List<Tuple<char, int>> sourceIndexes = series.SourceIndexes.Split(',')
                .Where(str => int.TryParse(str, out sourceIndex))
                .Select(str => Tuple.Create(str[0], sourceIndex))
                .ToList();

            DataSeries dataSeries = sourceIndexes
                .Select(tuple => (tuple.Item1 != '-') ? meterDataSet.DataSeries[tuple.Item2].Copy() : meterDataSet.DataSeries[Math.Abs(tuple.Item2)].Negate())
                .Aggregate((series1, series2) => series1.Add(series2));

            dataSeries.SeriesInfo = series;

            if (!meterDataSet.DataSeries.Contains(dataSeries))
                meterDataSet.DataSeries.Add(dataSeries);
        }

        private void RemoveUndefinedDataSeries(MeterDataSet meterDataSet)
        {
            for (int i = meterDataSet.DataSeries.Count - 1; i >= 0; i--)
            {
                if ((object)meterDataSet.DataSeries[i].SeriesInfo.Channel.Line == null)
                    meterDataSet.DataSeries.RemoveAt(i);
            }
        }

        private SeriesKey GetKey(Series series)
        {
            Channel channel = series.Channel;

            return Tuple.Create(
                channel.HarmonicGroup,
                channel.Name,
                channel.MeasurementType.Name,
                channel.MeasurementCharacteristic.Name,
                channel.Phase.Name,
                series.SeriesType.Name);
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(ConfigurationOperation));

        #endregion
    }
}
