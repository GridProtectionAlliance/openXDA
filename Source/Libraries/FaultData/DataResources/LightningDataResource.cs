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
using GSF;
using GSF.Collections;
using GSF.Configuration;
using log4net;
using openXDA.Model;

namespace FaultData.DataResources
{
    namespace Vaisala
    {
        public interface IExtendedLightningData
        {
            int PeakCurrent { get; }
            int FlashMultiplicity { get; }
            int ParticipatingSensors { get; }
            int DegreesOfFreedom { get; }
            double EllipseAngle { get; }
            double SemiMajorAxisLength { get; }
            double SemiMinorAxisLength { get; }
            double ChiSquared { get; }
            double Risetime { get; }
            double PeakToZeroTime { get; }
            double MaximumRateOfRise { get; }
            bool CloudIndicator { get; }
            bool AngleIndicator { get; }
            bool SignalIndicator { get; }
            bool TimingIndicator { get; }
        }
    }

    public interface ILightningDataProvider
    {
        IEnumerable<ILightningStrike> GetLightningStrikes(string lineKey, DateTime start, DateTime end);
    }

    public class LightningDataSection
    {
        public const string CategoryName = "Lightning";

        [Setting]
        [DefaultValue("")]
        public string DataProviders { get; set; }

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
        double Latitude { get; }
        double Longitude { get; }
        double Amplitude { get; }
        T GetExtendedData<T>() where T : class;
    }

    public class LightningDataResource : DataResourceBase<MeterDataSet>
    {
        public LightningDataResource()
        {
            LightningDataSettings = new LightningDataSection();
            LightningStrikeLookup = new Dictionary<DataGroup, List<ILightningStrike>>();
        }

        [Category]
        [SettingName(LightningDataSection.CategoryName)]
        public LightningDataSection LightningDataSettings { get; }

        public Dictionary<DataGroup, List<ILightningStrike>> LightningStrikeLookup { get; }

        public override void Initialize(MeterDataSet meterDataSet)
        {
            CycleDataResource cycleDataResource = meterDataSet.GetResource<CycleDataResource>();
            IEnumerable<ILightningDataProvider> dataProviders = GetDataProviders();

            foreach (ILightningDataProvider dataProvider in dataProviders)
                meterDataSet.Configure(dataProvider);

            foreach (DataGroup dataGroup in cycleDataResource.DataGroups)
            {
                try
                {
                    string lineKey = dataGroup.Asset.AssetKey;
                    double timeWindow = LightningDataSettings.DataProviderTimeWindow;
                    DateTime start = dataGroup.StartTime.AddSeconds(-timeWindow);
                    DateTime end = dataGroup.EndTime.AddSeconds(timeWindow);

                    Func<ILightningStrike, T> ToFunc<T>(Func<ILightningStrike, T> func) => func;

                    var getKey = ToFunc(strike => new
                    {
                        strike.Service,
                        strike.Latitude,
                        strike.Longitude,
                        strike.UTCTime
                    });

                    List<ILightningStrike> lightningStrikes = dataProviders
                        .SelectMany(provider => provider.GetLightningStrikes(lineKey, start, end))
                        .DistinctBy(getKey)
                        .ToList();

                    LightningStrikeLookup.Add(dataGroup, lightningStrikes);
                }
                catch (Exception ex)
                {
                    string message = $"Unable to load lightning strikes for data group: {ex.Message}";
                    Exception wrapper = new Exception(message, ex);
                    Log.Error(message, wrapper);
                }
            }
        }

        private List<ILightningDataProvider> GetDataProviders()
        {
            if (string.IsNullOrEmpty(LightningDataSettings.DataProviders))
                return new List<ILightningDataProvider> { GetDataProvider(LightningDataSettings) };

            ConnectionStringParser<SettingAttribute, CategoryAttribute> connectionStringParser = new ConnectionStringParser<SettingAttribute, CategoryAttribute>();

            LightningDataSection ConfigureLightningProvider(string connectionString)
            {
                LightningDataSection lightningDataSection = new LightningDataSection();
                connectionStringParser.ParseConnectionString(connectionString, lightningDataSection);
                return lightningDataSection;
            }

            return LightningDataSettings.DataProviders
                .ParseKeyValuePairs()
                .Select(kvp => kvp.Value)
                .Select(ConfigureLightningProvider)
                .Select(GetDataProvider)
                .Where(provider => !(provider is null))
                .ToList();
        }

        private ILightningDataProvider GetDataProvider(LightningDataSection lightningDataSettings)
        {
            string dataProviderAssemblyPath = lightningDataSettings.DataProviderAssembly;
            string dataProviderTypeName = lightningDataSettings.DataProviderType;

            if (string.IsNullOrEmpty(dataProviderAssemblyPath) || string.IsNullOrEmpty(dataProviderTypeName))
                return null;

            Assembly dataProviderAssembly = Assembly.LoadFrom(dataProviderAssemblyPath);
            Type dataProviderType = dataProviderAssembly.GetType(dataProviderTypeName);
            return (ILightningDataProvider)Activator.CreateInstance(dataProviderType);
        }

        private static readonly ILog Log = LogManager.GetLogger(typeof(LightningDataResource));
    }
}
