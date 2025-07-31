//******************************************************************************************************
//  ValueListGroup.cs - Gbtc
//
//  Copyright © 2018, Grid Protection Alliance.  All Rights Reserved.
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
//  09/10/2018 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using GSF.ComponentModel.DataAnnotations;
using GSF.Data;
using GSF.Data.Model;
using GSF.Web.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Http;

namespace SystemCenter.Model
{

    [PostRoles("Administrator, Transmission SME")]
    [PatchRoles("Administrator, Transmission SME")]
    [GetRoles("Administrator, Transmission SME")]
    [TableName("ValueListGroup"), UseEscapedName, PrimaryLabel("Name"), AllowSearch]
    public class ValueListGroup
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        [StringLength(200)]
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class RestrictedValueList
    {
        public string Name { get; set; }
        public string CountSQL { get; set; }
        public string UpdateSQL { get; set; }

        /// <summary>
        /// Default items in a <see cref="RestrictedValueList"/>.
        /// Objects may be either <see langword="string"/> or <see cref="Tuple"/> of two <see langword="string"/>,
        /// where the <see cref="Tuple"/> represents &lt;<see cref="ValueList.Value"/>, <see cref="ValueList.AltValue"/>&gt;
        /// </summary>
        public object[] DefaultItems { get; set; }

        public static List<RestrictedValueList> List = new List<RestrictedValueList>(){
            new RestrictedValueList() {
                Name = "TimeZones",
                CountSQL = @"SELECT COUNT(ID) FROM 
                        Meter
                        WHERE [TimeZone] = {0}",
                UpdateSQL = @"UPDATE Meter
                            SET [TimeZone] = {0} 
                        WHERE
                            [TimeZone] = {1}",
                DefaultItems = new string[] {"UTC"}
            },
             new RestrictedValueList() {
                Name = "Make",
                CountSQL = @"SELECT COUNT(ID) FROM 
                        Meter
                        WHERE [Make] = {0}",
                UpdateSQL = @"UPDATE 
                        Meter
                        SET [Make] = {0} 
                        WHERE
                        [Make] = {1}",
                DefaultItems = new string[] {"GPA"}
            },
             new RestrictedValueList() {
                Name = "Model",
                CountSQL = @"SELECT COUNT(ID) FROM 
                        Meter
                        WHERE [Model] = {0}",
                UpdateSQL = @"UPDATE 
                        Meter
                        SET [Model] = {0} 
                        WHERE
                        [Model] = {1}",
                DefaultItems = new string[] {"PQMeter"}
            },
             new RestrictedValueList() {
                Name = "Unit",
                CountSQL = @"SELECT COUNT(ID) FROM  
                        ChannelGroupType
                        WHERE
                        [Unit] = {0}",
                UpdateSQL = @"UPDATE 
                        ChannelGroupType
                        SET [Unit] = {0} 
                        WHERE
                        [Unit] = {1}",
                DefaultItems = new string[] {"Unknown"}
            },
             new RestrictedValueList() {
                Name = "Category",
                CountSQL = @"SELECT COUNT(ID) FROM  
                        LocationDrawing
                        WHERE
                        [Category] = {0}",
                UpdateSQL = @"UPDATE 
                        LocationDrawing
                        SET [Category] = {0} 
                        WHERE
                        [Category] = {1}",
                DefaultItems = new string[] {"Oneline"}
            },
             new RestrictedValueList() {
                Name = "SpareChannel",
                DefaultItems = new string[] {"Spare Channel"}
            },
             new RestrictedValueList() {
                Name = "TrendLabelDefaults",
                DefaultItems = new Tuple<string, string>[] {
                    new Tuple<string, string>("Channel.MeterName", "Meter Name"),
                    new Tuple<string, string>("Channel.AssetKey", "Asset Key"),
                    new Tuple<string, string>("Channel.Name", "Channel Label"),
                    new Tuple<string, string>("Channel.ChannelGroup", "Channel Group Name"),
                    new Tuple<string, string>("Channel.MinMaxAvg_Special", "Channel Series")
                }
            },
             new RestrictedValueList() {
                Name = "TrendLabelOptions",
                DefaultItems = new Tuple<string, string>[] {
                    new Tuple<string, string>("Channel.MeterName", "Meter Name"),
                    new Tuple<string, string>("Channel.MeterShortName", "Meter Short Name"),
                    new Tuple<string, string>("Channel.MeterKey", "Meter Asset Key"),
                    new Tuple<string, string>("Channel.AssetName", "Asset Name"),
                    new Tuple<string, string>("Channel.AssetKey", "Asset Key"),
                    new Tuple<string, string>("Channel.Phase", "Phase Name"),
                    new Tuple<string, string>("Channel.Name", "Channel Label"),
                    new Tuple<string, string>("Channel.Description", "Channel Description"),
                    new Tuple<string, string>("Channel.ChannelGroup", "Channel Group Name"),
                    new Tuple<string, string>("Channel.ChannelGroupType", "Channel Group Type"),
                    new Tuple<string, string>("Series.SeriesType", "Channel Series"),
                    new Tuple<string, string>("Channel.Unit", "Unit")
                }
            }
        };
    }

    public class ValueListGroupController<T> : ModelController<T, ValueListGroup> 
        where T : ValueListGroup, new()
    {
        public override IHttpActionResult Patch([FromBody] ValueListGroup newRecord)
        {
            if (!PatchAuthCheck())
            {
                return Unauthorized();
            }

            // Check if Value changed
            bool changeVal = false;
            ValueListGroup oldRecord;

            using (AdoDataConnection connection = new AdoDataConnection(Connection))
            {
                oldRecord = new TableOperations<ValueListGroup>(connection).QueryRecordWhere("ID = {0}", newRecord.ID);
                changeVal = !(newRecord.Name == oldRecord.Name);
            }

            if (changeVal)
            {
                using (AdoDataConnection connection = new AdoDataConnection(Connection))
                {
                    // Wrapping is needed here, since C# tries to use the wrong method signature otherwise
                    object[] parameters = new object[] { newRecord.Name, oldRecord.Name };
                    // Update Additional Fields
                    connection.ExecuteScalar(@"UPDATE 
                        AdditionalField
                        SET [Type] = {0} 
                        WHERE
                        [Type] = {1}", parameters);
                }
            }
            return base.Patch(newRecord);

        }

        public override IHttpActionResult Delete(ValueListGroup record)
        {
            if (!DeleteAuthCheck())
            {
                return Unauthorized();
            }

            using (AdoDataConnection connection = new AdoDataConnection(Connection))
            {
                // Wrapping is needed here, since C# tries to use the wrong method signature otherwise
                object[] parameters = new object[] { record.Name };
                // Update Additional Fields
                connection.ExecuteScalar(@"UPDATE 
                    AdditionalField
                    SET [Type] = 'string' 
                    WHERE
                    [Type] = {0}", parameters);
            }
            return base.Delete(record);
        }
    }
}