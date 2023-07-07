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
using GSF.Web.Model;
using System.Collections.Generic;
using System.Data;
using System.Linq;

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
            l.Longitude,
            l.Description,
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
        l.Longitude,
        l.Description
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
        public string Description { get; set; }
        public int Meters { get; set; }
        public int Assets { get; set; } 
    }
    public class DetailedLocationController<T> : ModelController<T> where T : DetailedLocation, new()
    {
        protected override DataTable GetSearchResults(PostData postData, int? page)
        {
            List<Search> searches = postData.Searches.ToList();
            searches = searches.Select((s) =>
            {
                if (s.FieldName == "Meter")
                    return new Search() { 
                        Type = "query",
                        Operator = ">",
                        isPivotColumn = false,
                        SearchText = "0",
                        FieldName = $"(SELECT Count(*) FROM Meter WHERE Meter.AssetKey {Transform(s)} AND Meter.LocationID = FullTbl.ID)",
                    };
                if (s.FieldName == "Asset")
                    return new Search()
                    {
                        Type = "query",
                        Operator = ">",
                        isPivotColumn = false,
                        SearchText = "0",
                        FieldName = $"(SELECT Count(*) FROM Asset LEFT JOIN AssetLocation ON AssetLocation.AssetID = Asset.ID WHERE Asset.AssetKey {Transform(s)} AND AssetLocation.LocationID = FullTbl.ID)",
                    };
                return s;

            }).ToList();
            postData.Searches = searches;

            return base.GetSearchResults(postData, page);
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