//******************************************************************************************************
//  DetailedLocation.cs - Gbtc
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
//  10/22/2021 - C. Lackner
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
            l.ID,
            l.LocationKey,
            l.Name,
            l.Alias,
            l.ShortName,
            l.Latitude,
            l.Longitude
            COUNT(DISTINCT m.ID) as Meters,
            COUNT(DISTINCT al.AssetID) as Assets
    FROM 
        Location as l LEFT JOIN
        Meter as m ON l.ID = m.LocationID LEFT JOIN
        AssetLocation as al ON l.ID = al.LocationID LEFT JOIN
        Asset as a ON al.AssetID = a.ID
    GROUP BY
        l.ID,
        l.LocationKey,
        l.Name,
        l.Alias,
        l.ShortName,
        l.Latitude,
        l.Longitude
    "), AllowSearch]
    [AdditionalFieldSearch("ParentTable = 'Location'", @"
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
    public class DetailedLocation
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        [DefaultSortOrder]
        public string LocationKey { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string ShortName { get; set; }
        public double Longitude {get; set;}
        public double Latitude {get; set;}
        public string Meters { get; set; }
        public string Assets { get; set; } 
    }
}