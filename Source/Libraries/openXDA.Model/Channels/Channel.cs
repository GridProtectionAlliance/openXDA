//******************************************************************************************************
//  Channel.cs - Gbtc
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
//  08/29/2017 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Transactions;
using GSF.Data;
using GSF.Data.Model;
using Newtonsoft.Json;

namespace openXDA.Model
{
    public class ChannelKey : IEquatable<ChannelKey>
    {
        #region [ Members ]

        // Fields
        private readonly Tuple<int, int, string, string, string, string> m_tuple;

        #endregion

        #region [ Constructors ]

        public ChannelKey(int assetID, int harmonicGroup, string name, string measurementType, string measurementCharacteristic, string phase)
        {
            m_tuple = Tuple.Create(assetID, harmonicGroup, name, measurementType, measurementCharacteristic, phase);
        }

        public ChannelKey(Channel channel)
            : this(channel.AssetID, channel.HarmonicGroup, channel.Name, channel.MeasurementType.Name, channel.MeasurementCharacteristic.Name, channel.Phase.Name)
        {
        }

        #endregion

        #region [ Properties ]

        public int LineID
        {
            get
            {
                return m_tuple.Item1;
            }
        }

        public int HarmonicGroup
        {
            get
            {
                return m_tuple.Item2;
            }
        }

        public string Name
        {
            get
            {
                return m_tuple.Item3;
            }
        }

        public string MeasurementType
        {
            get
            {
                return m_tuple.Item4;
            }
        }

        public string MeasurementCharacteristic
        {
            get
            {
                return m_tuple.Item5;
            }
        }

        public string Phase
        {
            get
            {
                return m_tuple.Item6;
            }
        }

        #endregion

        #region [ Methods ]

        public override int GetHashCode()
        {
            return m_tuple.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ChannelKey);
        }

        public bool Equals(ChannelKey other)
        {
            if ((object)other == null)
                return false;

            return m_tuple.Equals(other.m_tuple);
        }

        #endregion
    }

    [TableName("Channel")]
    public class ChannelBase
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        public int MeterID { get; set; }

        public int AssetID { get; set; }

        public int MeasurementTypeID { get; set; }

        public int MeasurementCharacteristicID { get; set; }

        public int PhaseID { get; set; }

        [StringLength(200)]
        [Searchable]
        public string Name { get; set; }

        public double Adder { get; set; }

        [DefaultValue(1.0D)]
        public double Multiplier { get; set; } = 1.0D;

        public double SamplesPerHour { get; set; }

        public double? PerUnitValue { get; set; }

        public int HarmonicGroup { get; set; }

        public string Description { get; set; }

        public bool Enabled { get; set; }
    }

    public class Channel : ChannelBase
    {
        #region [ Members ]

        // Fields
        private MeasurementType m_measurementType;
        private MeasurementCharacteristic m_measurementCharacteristic;
        private Phase m_phase;
        private Meter m_meter;
        private Asset m_asset;
        private List<Series> m_series;

        #endregion

        #region [ Properties ]

        [JsonIgnore]
        [NonRecordField]
        public MeasurementType MeasurementType
        {
            get
            {
                return m_measurementType ?? (m_measurementType = QueryMeasurementType());
            }
            set
            {
                m_measurementType = value;
            }
        }

        [JsonIgnore]
        [NonRecordField]
        public MeasurementCharacteristic MeasurementCharacteristic
        {
            get
            {
                return m_measurementCharacteristic ?? (m_measurementCharacteristic = QueryMeasurementCharacteristic());
            }
            set
            {
                m_measurementCharacteristic = value;
            }
        }

        [JsonIgnore]
        [NonRecordField]
        public Phase Phase
        {
            get
            {
                return m_phase ?? (m_phase = QueryPhase());
            }
            set
            {
                m_phase = value;
            }
        }

        [JsonIgnore]
        [NonRecordField]
        public Meter Meter
        {
            get
            {
                return m_meter ?? (m_meter = QueryMeter());
            }
            set
            {
                m_meter = value;
            }
        }

        [JsonIgnore]
        [NonRecordField]
        public Asset Asset
        {
            get
            {
                return m_asset ?? (m_asset = QueryAsset());
            }
            set
            {
                m_asset = value;
            }
        }

        [JsonIgnore]
        [NonRecordField]
        public List<Series> Series
        {
            get
            {
                return m_series ?? (m_series = QuerySeries());
            }
            set
            {
                m_series = value;
            }
        }

        [JsonIgnore]
        [NonRecordField]
        public Func<AdoDataConnection> ConnectionFactory
        {
            get
            {
                return LazyContext.ConnectionFactory;
            }
            set
            {
                LazyContext.ConnectionFactory = value;
            }
        }

        [JsonIgnore]
        [NonRecordField]
        internal LazyContext LazyContext { get; set; } = new LazyContext();

        #endregion

        #region [ Methods ]

        public MeasurementType GetMeasurementType(AdoDataConnection connection)
        {
            if ((object)connection == null)
                return null;

            TableOperations<MeasurementType> measurementTypeTable = new TableOperations<MeasurementType>(connection);
            return measurementTypeTable.QueryRecordWhere("ID = {0}", MeasurementTypeID);
        }

        public MeasurementCharacteristic GetMeasurementCharacteristic(AdoDataConnection connection)
        {
            if ((object)connection == null)
                return null;

            TableOperations<MeasurementCharacteristic> measurementCharacteristicTable = new TableOperations<MeasurementCharacteristic>(connection);
            return measurementCharacteristicTable.QueryRecordWhere("ID = {0}", MeasurementCharacteristicID);
        }

        public Phase GetPhase(AdoDataConnection connection)
        {
            if ((object)connection == null)
                return null;

            TableOperations<Phase> phaseTable = new TableOperations<Phase>(connection);
            return phaseTable.QueryRecordWhere("ID = {0}", PhaseID);
        }

        public Meter GetMeter(AdoDataConnection connection)
        {
            if ((object)connection == null)
                return null;

            TableOperations<Meter> meterTable = new TableOperations<Meter>(connection);
            return meterTable.QueryRecordWhere("ID = {0}", MeterID);
        }

        public Asset GetAsset(AdoDataConnection connection)
        {
            if ((object)connection == null)
                return null;

            TableOperations<Asset> assetTable = new TableOperations<Asset>(connection);
            return assetTable.QueryRecordWhere("ID = {0}", AssetID);
        }

        public IEnumerable<Series> GetSeries(AdoDataConnection connection)
        {
            if ((object)connection == null)
                return null;

            TableOperations<Series> seriesTable = new TableOperations<Series>(connection);
            return seriesTable.QueryRecordsWhere("ChannelID = {0}", ID);
        }

        private MeasurementType QueryMeasurementType()
        {
            MeasurementType measurementType;

            using (AdoDataConnection connection = ConnectionFactory?.Invoke())
            {
                measurementType = GetMeasurementType(connection);
            }

            return LazyContext.GetMeasurementType(measurementType);
        }

        private MeasurementCharacteristic QueryMeasurementCharacteristic()
        {
            MeasurementCharacteristic measurementCharacteristic;

            using (AdoDataConnection connection = ConnectionFactory?.Invoke())
            {
                measurementCharacteristic = GetMeasurementCharacteristic(connection);
            }

            return LazyContext.GetMeasurementCharacteristic(measurementCharacteristic);
        }

        private Phase QueryPhase()
        {
            Phase phase;

            using (AdoDataConnection connection = ConnectionFactory?.Invoke())
            {
                phase = GetPhase(connection);
            }

            return LazyContext.GetPhase(phase);
        }

        private Meter QueryMeter()
        {
            Meter meter;

            using (AdoDataConnection connection = ConnectionFactory?.Invoke())
            {
                meter = GetMeter(connection);
            }

            if ((object)meter != null)
                meter.LazyContext = LazyContext;

            return LazyContext.GetMeter(meter);
        }

        private Asset QueryAsset()
        {
            Asset asset;

            using (AdoDataConnection connection = ConnectionFactory?.Invoke())
            {
                asset = GetAsset(connection);
            }

            if ((object)asset != null)
                asset.LazyContext = LazyContext;

            return LazyContext.GetAsset(asset);
        }

        private List<Series> QuerySeries()
        {
            List<Series> seriesList;

            using (AdoDataConnection connection = ConnectionFactory?.Invoke())
            {
                seriesList = GetSeries(connection)?
                    .Select(LazyContext.GetSeries)
                    .ToList();
            }

            if ((object)seriesList != null)
            {
                foreach (Series series in seriesList)
                {
                    series.Channel = this;
                    series.LazyContext = LazyContext;
                }
            }

            return seriesList;
        }

        #endregion
    }

    [TableName("ChannelDetail")]
    public class ChannelDetail : Channel
    {
        public string MeterName { get; set; }

        public string AssetKey { get; set; }

        public string AssetName { get; set; }

        [Searchable]
        public new string MeasurementType { get; set; }

        [Searchable]
        public new string MeasurementCharacteristic { get; set; }

        [Searchable]
        public new string Phase { get; set; }

        public string Mapping { get; set; }

        public int SeriesTypeID { get; set; }

        public string SeriesType { get; set; }
    }

    public class ChannelInfo
    {
        [PrimaryKey(true)]
        public int ChannelID { get; set; }

        public string ChannelName { get; set; }

        public string ChannelDescription { get; set; }

        public string MeasurementType { get; set; }

        public string MeasurementCharacteristic { get; set; }

        public string Phase { get; set; }

        public string SeriesType { get; set; }

        public string Orientation { get; set; }

        public string Phasing { get; set; }
    }

    public static partial class TableOperationsExtensions
    {
        public static DashSettings GetOrAdd(this TableOperations<DashSettings> table, string name, string value, bool enabled = true)
        {
            TransactionScopeOption required = TransactionScopeOption.Required;

            TransactionOptions transactionOptions = new TransactionOptions()
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = TransactionManager.MaximumTimeout
            };

            DashSettings dashSettings;

            using (TransactionScope transactionScope = new TransactionScope(required, transactionOptions))
            {
                if (value.Contains(","))
                    dashSettings = table.QueryRecordWhere("Name = {0} AND SUBSTRING(Value, 0, CHARINDEX(',', Value)) = {1}", name, value.Split(',').First());
                else
                    dashSettings = table.QueryRecordWhere("Name = {0} AND Value = {1}", name, value);

                if ((object)dashSettings == null)
                {
                    dashSettings = new DashSettings();
                    dashSettings.Name = name;
                    dashSettings.Value = value;
                    dashSettings.Enabled = enabled;

                    table.AddNewRecord(dashSettings);

                    dashSettings.ID = table.Connection.ExecuteScalar<int>("SELECT @@IDENTITY");
                }

                transactionScope.Complete();
            }

            return dashSettings;
        }

        public static UserDashSettings GetOrAdd(this TableOperations<UserDashSettings> table, string name, Guid user, string value, bool enabled = true)
        {
            TransactionScopeOption required = TransactionScopeOption.Required;

            TransactionOptions transactionOptions = new TransactionOptions()
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = TransactionManager.MaximumTimeout
            };

            UserDashSettings dashSettings;

            using (TransactionScope transactionScope = new TransactionScope(required, transactionOptions))
            {
                if (value.Contains(","))
                    dashSettings = table.QueryRecordWhere("Name = {0} AND SUBSTRING(Value, 0, CHARINDEX(',', Value)) = {1} AND UserAccountID = {2}", name, value.Split(',').First(), user);
                else
                    dashSettings = table.QueryRecordWhere("Name = {0} AND Value = {1} AND UserAccountID = {2}", name, value, user);

                if ((object)dashSettings == null)
                {
                    dashSettings = new UserDashSettings();
                    dashSettings.Name = name;
                    dashSettings.Value = value;
                    dashSettings.Enabled = enabled;
                    dashSettings.UserAccountID = user;

                    table.AddNewRecord(dashSettings);

                    dashSettings.ID = table.Connection.ExecuteScalar<int>("SELECT @@IDENTITY");
                }

                transactionScope.Complete();
            }

            return dashSettings;
        }

    }

}