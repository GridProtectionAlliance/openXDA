//******************************************************************************************************
//  LightningDataResource.cs - Gbtc
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
//  04/02/2019 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Reflection;
using FaultData.DataAnalysis;
using FaultData.DataSets;
using GSF.Configuration;
using openXDA.Model;

namespace FaultData.DataResources
{
    public interface ILightningDataProvider
    {
        IEnumerable<ILightningStrike> GetLightningStrikes(string lineKey, DateTime start, DateTime end);
    }

    public class LightningDataSettings
    {
        public const string CategoryName = "Lightning";

        [Setting]
        [DefaultValue("")]
        public string DataProviderAssembly { get; set; }

        [Setting]
        [DefaultValue("")]
        public string DataProviderType { get; set; }

        [Setting]
        [DefaultValue(2.0D)]
        public double DataProviderTimeWindow { get; set; }
    }

    public interface ILightningStrike
    {
        string Service { get; }
        DateTime UTCTime { get; }
        string DisplayTime { get; }
        double Amplitude { get; }
        double Latitude { get; }
        double Longitude { get; }
    }

    public class LightningDataResource : DataResourceBase<MeterDataSet>
    {
        public LightningDataResource()
        {
            LightningDataSettings = new LightningDataSettings();
            LightningStrikeLookup = new Dictionary<DataGroup, List<ILightningStrike>>();
        }

        [Category]
        [SettingName(LightningDataSettings.CategoryName)]
        public LightningDataSettings LightningDataSettings { get; }

        public Dictionary<DataGroup, List<ILightningStrike>> LightningStrikeLookup { get; }

        public override void Initialize(MeterDataSet meterDataSet)
        {
            CycleDataResource cycleDataResource = meterDataSet.GetResource<CycleDataResource>();
            ILightningDataProvider dataProvider = GetDataProvider();

            if (dataProvider == null)
                return;

            ConnectionStringParser<SettingAttribute, CategoryAttribute> connectionStringParser = new ConnectionStringParser<SettingAttribute, CategoryAttribute>();
            connectionStringParser.ParseConnectionString(meterDataSet.ConnectionString, dataProvider);

            foreach (DataGroup dataGroup in cycleDataResource.DataGroups)
            {
                string lineKey = dataGroup.Asset.AssetKey;
                DateTime start = dataGroup.StartTime.AddSeconds(-LightningDataSettings.DataProviderTimeWindow);
                DateTime end = dataGroup.EndTime.AddSeconds(LightningDataSettings.DataProviderTimeWindow);

                List<ILightningStrike> lightningStrikes = dataProvider
                    .GetLightningStrikes(lineKey, start, end)
                    .ToList();

                LightningStrikeLookup.Add(dataGroup, lightningStrikes);
            }
        }

        private ILightningDataProvider GetDataProvider()
        {
            string dataProviderAssemblyPath = LightningDataSettings.DataProviderAssembly;
            string dataProviderTypeName = LightningDataSettings.DataProviderType;

            if (string.IsNullOrEmpty(dataProviderAssemblyPath) || string.IsNullOrEmpty(dataProviderTypeName))
                return null;

            Assembly dataProviderAssembly = Assembly.LoadFrom(LightningDataSettings.DataProviderAssembly);
            Type dataProviderType = dataProviderAssembly.GetType(LightningDataSettings.DataProviderType);
            return (ILightningDataProvider)Activator.CreateInstance(dataProviderType);
        }
    }
}
