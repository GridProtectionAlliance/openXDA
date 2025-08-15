//******************************************************************************************************
//  TrendChannel.cs - Gbtc
//
//  Copyright © 2020, Grid Protection Alliance.  All Rights Reserved.
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
//  07/31/2025 - G. Santos
//       Generated original version of source code.
//
//******************************************************************************************************

using System.Data;
using GSF.Data;
using GSF.Data.Model;
using GSF.Reflection;

namespace SEBrowser.Model
{
    [TableName("Channel"),
    SettingsCategory("systemSettings"),
    CustomView(@"
        SELECT
	        DISTINCT Channel.ID,
	        Channel.Name,
	        Channel.Description,
            Asset.ID as AssetID,
	        Asset.AssetKey,
	        Asset.AssetName,
            Meter.ID as MeterID,
	        Meter.AssetKey AS MeterKey,
	        Meter.Name AS MeterName,
            Meter.ShortName AS MeterShortName,
	        Phase.Name AS Phase,
	        ChannelGroup.Name AS ChannelGroup,
	        ChannelGroupType.DisplayName AS ChannelGroupType,
	        ChannelGroupType.Unit
        FROM 
	        Channel LEFT JOIN
	        Phase ON Channel.PhaseID = Phase.ID LEFT JOIN
	        Asset ON Asset.ID = Channel.AssetID LEFT JOIN
	        Meter ON Meter.ID = Channel.MeterID LEFT JOIN
	        ChannelGroupType ON Channel.MeasurementCharacteristicID = ChannelGroupType.MeasurementCharacteristicID AND Channel.MeasurementTypeID = ChannelGroupType.MeasurementTypeID LEFT JOIN
	        ChannelGroup ON ChannelGroup.ID = ChannelGroupType.ChannelGroupID
    "), AllowSearch]
    public class TrendChannel
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        [DefaultSortOrder]
        public string Name { get; set; }
        public string Description { get; set; }
        public int AssetID { get; set; }
        public string AssetKey { get; set; }
        public string AssetName { get; set; }
        public int MeterID { get; set; }
        public string MeterKey { get; set; }
        public string MeterName { get; set; }
        public string MeterShortName { get; set; }
        public string Phase { get; set; }
        public string ChannelGroup { get; set; }
        public string ChannelGroupType { get; set; }
        public string Unit { get; set; }

        [NonRecordField]
        public DataTable Series
        {
            get
            {
                return m_Series ?? getSeries();
            }
            set
            {
                m_Series = value;
            }
        }
        private DataTable m_Series = null;

        private DataTable getSeries()
        {
            if (!typeof(TrendChannel).TryGetAttribute(out SettingsCategoryAttribute attribute)) return new DataTable();
            if (!typeof(DetailedSeries).TryGetAttribute(out CustomViewAttribute view)) return new DataTable();
            using (AdoDataConnection connection = new AdoDataConnection(attribute.SettingsCategory))
            {
                string sql = $"{view.CustomView} WHERE ChannelID = {{0}}";
                return connection.RetrieveData(sql, this.ID);
            }
        }
    }
}