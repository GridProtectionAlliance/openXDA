//******************************************************************************************************
//  RealTimeLightningDataProvider.cs - Gbtc
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
//  11/12/2021 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using FaultData.DataResources;
using FaultData.DataResources.Vaisala;
using GSF.Configuration;
using GSF.Data;
using GSF.Geo;
using openXDA.Configuration;

namespace FaultData.DataOperations.TVA
{
    public class RealTimeLightningDataProviderSettings
    {
        public const string CategoryName = "RTLightning";

        private const string DefaultOracleDataProviderString =
            "AssemblyName={Oracle.DataAccess, Version=2.112.2.0, Culture=neutral, PublicKeyToken=89b483f429c47342}; " +
            "ConnectionType=Oracle.DataAccess.Client.OracleConnection; " +
            "AdapterType=Oracle.DataAccess.Client.OracleDataAdapter";

        [Setting]
        [DefaultValue("")]
        public string RTLightningDatabaseConnectionString { get; set; }

        [Setting]
        [DefaultValue("yyyy-MM-dd HH:mm:ss.fffffff")]
        public string DisplayTimeFormat { get; set; }

        [Setting]
        [DefaultValue("")]
        public string MaximoConnectionString { get; set; }

        [Setting]
        [DefaultValue(DefaultOracleDataProviderString)]
        public string MaximoDataProviderString { get; set; }

        [Setting]
        [DefaultValue(0.5D)]
        public double Proximity { get; set; }
    }

    public class RealTimeLightningDataProvider : ILightningDataProvider
    {
        private class LightningStrike : ILightningStrike, IExtendedLightningData
        {
            public string Service => "Vaisala Real-Time Feed";

            public DateTime UTCTime { get; set; }
            public string DisplayTime { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public double Amplitude { get; set; }
            public int PeakCurrent { get; set; }
            public int FlashMultiplicity { get; set; }
            public int ParticipatingSensors { get; set; }
            public int DegreesOfFreedom { get; set; }
            public double EllipseAngle { get; set; }
            public double SemiMajorAxisLength { get; set; }
            public double SemiMinorAxisLength { get; set; }
            public double ChiSquared { get; set; }
            public double Risetime { get; set; }
            public double PeakToZeroTime { get; set; }
            public double MaximumRateOfRise { get; set; }
            public bool CloudIndicator { get; set; }
            public bool AngleIndicator { get; set; }
            public bool SignalIndicator { get; set; }
            public bool TimingIndicator { get; set; }

            public T GetExtendedData<T>() where T : class
            {
                object extendedData = this;
                if (extendedData is T)
                    return (T)extendedData;
                return default(T);
            }
        }

        [Category]
        [SettingName(SystemSection.CategoryName)]
        public SystemSection SystemSettings { get; }

        [Category]
        [SettingName(RealTimeLightningDataProviderSettings.CategoryName)]
        public RealTimeLightningDataProviderSettings LightningDataSettings { get; }

        private TimeZoneInfo XDATimeZone =>
            SystemSettings.XDATimeZoneInfo;

        public IEnumerable<ILightningStrike> GetLightningStrikes(string lineKey, DateTime start, DateTime end)
        {
            GeoCoordinate GetLocation(ILightningStrike strike) =>
                new GeoCoordinate(strike.Latitude, strike.Longitude);

            double ToMiles(double km) => km / 1.609344;

            using (AdoDataConnection lightningDbConnection = CreateLightningDbConnection())
            using (AdoDataConnection maximoConnection = CreateMaximoDbConnection())
            {
                CoordinateReferenceSystem crs = new EPSG3857();
                List<GeoCoordinate> structureLocations = QueryStructureLocations(maximoConnection, lineKey).ToList();

                foreach (ILightningStrike strike in QueryStrikes(lightningDbConnection, start, end))
                {
                    GeoCoordinate strikeLocation = GetLocation(strike);
                    double distanceThreshold = LightningDataSettings.Proximity;

                    bool isNearLine = structureLocations
                        .Select(location => crs.Distance(location, strikeLocation))
                        .Select(km => ToMiles(km))
                        .Any(miles => miles <= distanceThreshold);

                    if (isNearLine)
                        yield return strike;
                }
            }
        }

        private IEnumerable<ILightningStrike> QueryStrikes(AdoDataConnection connection, DateTime start, DateTime end)
        {
            const string QueryFormat =
                "SELECT " +
                "    StrikeTime UTCTime, " +
                "    Latitude, " +
                "    PeakCurrent Amplitude, " +
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
                "FROM RTLightningStrike " +
                "WHERE StrikeTime BETWEEN {0} AND {1}";

            DateTime utcStart = TimeZoneInfo.ConvertTimeToUtc(start, XDATimeZone);
            DateTime utcEnd = TimeZoneInfo.ConvertTimeToUtc(end, XDATimeZone);
            object utcStart2 = ToDateTime2(connection, utcStart);
            object utcEnd2 = ToDateTime2(connection, utcEnd);

            using (DataTable table = connection.RetrieveData(QueryFormat, utcStart2, utcEnd2))
            {
                foreach (DataRow row in table.Rows)
                {
                    LightningStrike strike = new LightningStrike();
                    strike.UTCTime = row.ConvertField<DateTime>(nameof(strike.UTCTime));
                    strike.Latitude = row.ConvertField<double>(nameof(strike.Latitude));
                    strike.Longitude = row.ConvertField<double>(nameof(strike.Longitude));
                    strike.Amplitude = row.ConvertField<double>(nameof(strike.Amplitude));
                    strike.PeakCurrent = row.ConvertField<int>(nameof(strike.PeakCurrent));
                    strike.FlashMultiplicity = row.ConvertField<int>(nameof(strike.FlashMultiplicity));
                    strike.ParticipatingSensors = row.ConvertField<int>(nameof(strike.ParticipatingSensors));
                    strike.DegreesOfFreedom = row.ConvertField<int>(nameof(strike.DegreesOfFreedom));
                    strike.EllipseAngle = row.ConvertField<double>(nameof(strike.EllipseAngle));
                    strike.SemiMajorAxisLength = row.ConvertField<double>(nameof(strike.SemiMajorAxisLength));
                    strike.SemiMinorAxisLength = row.ConvertField<double>(nameof(strike.SemiMinorAxisLength));
                    strike.ChiSquared = row.ConvertField<double>(nameof(strike.ChiSquared));
                    strike.Risetime = row.ConvertField<double>(nameof(strike.Risetime));
                    strike.PeakToZeroTime = row.ConvertField<double>(nameof(strike.PeakToZeroTime));
                    strike.MaximumRateOfRise = row.ConvertField<double>(nameof(strike.MaximumRateOfRise));
                    strike.CloudIndicator = row.ConvertField<bool>(nameof(strike.CloudIndicator));
                    strike.AngleIndicator = row.ConvertField<bool>(nameof(strike.AngleIndicator));
                    strike.SignalIndicator = row.ConvertField<bool>(nameof(strike.SignalIndicator));
                    strike.TimingIndicator = row.ConvertField<bool>(nameof(strike.TimingIndicator));

                    DateTime xdaTime = TimeZoneInfo.ConvertTimeFromUtc(strike.UTCTime, XDATimeZone);
                    strike.DisplayTime = xdaTime.ToString(LightningDataSettings.DisplayTimeFormat);
                    yield return strike;
                }
            }
        }

        private IEnumerable<GeoCoordinate> QueryStructureLocations(AdoDataConnection connection, string lineKey)
        {
            const string QueryFormat =
                @"SELECT" +
                @"    Location.TVA_LATITUDE ""Latitude""," +
                @"    Location.TVA_LONGITUDE ""Longitude""" +
                @"FROM EAMDM.EAM_OD_LOCATION_MV Location" +
                @"WHERE" +
                @"    Location.SITE_ID = 'PSO' AND" +
                @"    Location.STATUS_CD = 'ACTIVE' AND" +
                @"    Location.STATION_LINE_CD = {0} AND" +
                @"    Location.CLASSSTRUCTURE_ID = '1062'";

            using (DataTable table = connection.RetrieveData(QueryFormat, lineKey))
            {
                foreach (DataRow row in table.Rows)
                {
                    double latitude = row.ConvertField<double>("Latitude");
                    double longitude = row.ConvertField<double>("Longitude");
                    yield return new GeoCoordinate(latitude, longitude);
                }
            }
        }

        private AdoDataConnection CreateLightningDbConnection()
        {
            string connectionString = LightningDataSettings.RTLightningDatabaseConnectionString;
            Type connectionType = typeof(SqlConnection);
            Type adapterType = typeof(SqlDataAdapter);
            return new AdoDataConnection(connectionString, connectionType, adapterType);
        }

        private AdoDataConnection CreateMaximoDbConnection()
        {
            string connectionString = LightningDataSettings.MaximoConnectionString;
            string dataProviderString = LightningDataSettings.MaximoDataProviderString;
            return new AdoDataConnection(connectionString, dataProviderString);
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
