//******************************************************************************************************
//  DetailedMeter.cs - Gbtc
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
//  08/01/2020 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

using GSF.Data.Model;

namespace SystemCenter.Model
{
     [TableName("Meter"), 
     CustomView(@"
    SELECT
        DISTINCT
        Meter.ID,
        Meter.AssetKey,
        Meter.Name,
        Meter.Make,
        Meter.Model,
        Location.Name as Location,
        COUNT(DISTINCT MeterAsset.AssetID)  as MappedAssets
    FROM 
        Meter LEFT JOIN
        Location ON Meter.LocationID = Location.ID LEFT JOIN
        MeterAsset ON Meter.ID = MeterAsset.MeterID LEFT JOIN 
        Asset ON MeterAsset.AssetID = Asset.ID LEFT JOIN
        Note ON Note.NoteTypeID = (SELECT ID FROM NoteType WHERE Name = 'Meter') AND Note.ReferenceTableID = Meter.ID
    GROUP BY
        Meter.ID,
        Meter.AssetKey,
        Meter.Name,
        Meter.Make,
        Meter.Model,
        Location.Name
    "), AllowSearch]
    [AdditionalFieldSearch("ParentTable='Meter'", @"
    (SELECT
	    AdditionalFieldValue.ID,
	    AdditionalField.FieldName,
	    AdditionalFieldValue.Value,
        AdditionalFieldValue.ParentTableID, 
        AdditionalField.ParentTable
    FROM
	    AdditionalField JOIN
	    AdditionalFieldValue ON AdditionalField.ID = AdditionalFieldValue.AdditionalFieldID)
    ", "ParentTableID", "Value", "FieldName")]
    public class DetailedMeter
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        [DefaultSortOrder]
        public string AssetKey { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int MappedAssets { get; set; } 
        public string Make { get; set; }
        public string Model { get; set; } 
    }
}