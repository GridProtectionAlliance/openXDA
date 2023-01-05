//******************************************************************************************************
//  DetailedAsset.cs - Gbtc
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
using GSF.Web.Model;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SystemCenter.Model
{
     [TableName("Asset"), 
     CustomView(@"
    SELECT
        DISTINCT
            Asset.ID,
            Asset.AssetKey,
            Asset.AssetName,
            Asset.VoltageKV,
            AssetType.Name as AssetType,
            COUNT(DISTINCT Meter.ID) as Meters,
            COUNT(DISTINCT Location.ID) as Locations
    FROM 
        Asset Join
        AssetType ON Asset.AssetTypeID = AssetType.ID LEFT JOIN
        MeterAsset ON MeterAsset.AssetID = Asset.ID LEFT JOIN
        Meter ON MeterAsset.MeterID = Meter.ID LEFT JOIN
        AssetLocation ON AssetLocation.AssetID = Asset.ID LEFT JOIN
        Location ON AssetLocation.LocationID = Location.ID
    GROUP BY
        Asset.ID,
        Asset.AssetKey,
        Asset.AssetName,
        Asset.VoltageKV,
        AssetType.Name
    "), AllowSearch]
    [AdditionalFieldSearch("ParentTable = 'Line' OR  ParentTable = 'Transformer' OR  ParentTable = 'Breaker'  OR  ParentTable = 'CapBank'  OR  ParentTable = 'Bus'", @"
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
    public class DetailedAsset
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        [DefaultSortOrder]
        public string AssetKey { get; set; }
        public string AssetName { get; set; }
        public double VoltageKV { get; set; }
        public string AssetType { get; set; } 
        public int Meters { get; set; }
        public int Locations { get; set; } 
    }

    public class DetailedAssetController<T> : ModelController<T> where T : DetailedAsset, new()
    {
        protected override DataTable GetSearchResults(PostData postData)
        {
            List<Search> searches = postData.Searches.ToList();
            searches = searches.Select((s) =>
            {
                if (s.FieldName == "Meter")
                    return new Search()
                    {
                        Type = "query",
                        Operator = ">",
                        isPivotColumn = false,
                        SearchText = "0",
                        FieldName = $"(SELECT Count(*) FROM Meter LEFT JOIN MeterAsset ON Meter.ID = MeterAsset.MeterID WHERE Meter.AssetKey {Transform(s)} AND MeterAsset.AssetID = FullTbl.ID)",
                    };
                if (s.FieldName == "Location")
                    return new Search()
                    {
                        Type = "query",
                        Operator = ">",
                        isPivotColumn = false,
                        SearchText = "0",
                        FieldName = $"(SELECT Count(*) FROM Location LEFT JOIN AssetLocation ON AssetLocation.LocationID = Location.ID WHERE Location.LocationKey {Transform(s)} AND AssetLocation.AssetID = FullTbl.ID)",
                    };
                return s;

            }).ToList();
            postData.Searches = searches;

            return base.GetSearchResults(postData);
        }

        private string Transform(Search search)
        {
            if (search.SearchText == string.Empty) search.SearchText = "%";
            else search.SearchText = search.SearchText.Replace("*", "%");
            search.SearchText = $"'{search.SearchText}'";

            string escape = "ESCAPE '$'";
            if (search.Operator != "LIKE")
                escape = "";
            return $"{search.Operator} {search.SearchText} {escape}";
        }
    }
}