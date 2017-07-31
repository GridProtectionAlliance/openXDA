//******************************************************************************************************
//  Meter.cs - Gbtc
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
//  07/31/2017 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using GSF.ComponentModel.DataAnnotations;
using GSF.Data.Model;

namespace XDAAlarmCreationApp.Model
{
    public class Meter
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        [StringLength(50)]
        public string AssetKey { get; set; }

        [Label("Location")]
        public int MeterLocationID { get; set; }

        [Searchable]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Alias { get; set; }

        [StringLength(12)]
        public string ShortName { get; set; }

        [Searchable]
        [StringLength(200)]
        public string Make { get; set; }

        [Searchable]
        [StringLength(200)]
        public string Model { get; set; }

        [Label("Meter Type")]
        public int MeterTypeID { get; set; }

        [StringLength(200)]
        public string TimeZone { get; set; }

        public string Description { get; set; }
    }

    public class MeterDetail : Meter
    {
        [Searchable]
        public string Location { get; set; }
        [Searchable]
        public string MeterType { get; set; }

        public string TimeZoneLabel
        {
            get
            {
                try
                {
                    if (TimeZone != "UTC")
                        return TimeZoneInfo.FindSystemTimeZoneById(TimeZone).ToString();
                }
                catch
                {
                    // Do not fail if the time zone cannot be found --
                    // instead, fall through to the logic below to
                    // find the label for UTC
                }

                return TimeZoneInfo.GetSystemTimeZones()
                    .Where(info => info.Id == "UTC")
                    .DefaultIfEmpty(TimeZoneInfo.Utc)
                    .First()
                    .ToString();
            }
        }
    }
}
