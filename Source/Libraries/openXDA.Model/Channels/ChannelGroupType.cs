//******************************************************************************************************
//  ChannelGroupType.cs - Gbtc
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
//  10/12/2020 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using GSF.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openXDA.Model
{
    [SettingsCategory("systemSettings")]
    [AllowSearch]
    [PostRoles("Administrator, Transmission SME")]
    [PatchRoles("Administrator, Transmission SME")]
    [DeleteRoles("Administrator, Transmission SME")]
    public class ChannelGroupType
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        [ParentKey(typeof(ChannelGroup))]
        public int ChannelGroupID { get; set; }
        public int MeasurementTypeID { get; set; }
        public int MeasurementCharacteristicID { get; set; }

        [StringLength(20)]
        [DefaultSortOrder]
        public string DisplayName { get; set; }
        public string Unit { get; set; }
    }

    [CustomView(@"
        SELECT
	        ChannelGroupType.*,
	        ChannelGroup.Name as ChannelGroup,
	        MeasurementType.Name as MeasurementType,
	        MeasurementCharacteristic.Name as MeasurementCharacteristic,
	        ChannelGroupType.DisplayName
        FROM
	        ChannelGroupType JOIN 
	        ChannelGroup ON ChannelGroupType.ChannelGroupID = ChannelGroup.ID JOIN 
	        MeasurementType ON ChannelGroupType.MeasurementTypeID = MeasurementType.ID JOIN
	        MeasurementCharacteristic ON ChannelGroupType.MeasurementCharacteristicID = MeasurementCharacteristic.ID
    ")]
    [SettingsCategory("systemSettings")]
    [AllowSearch]
    [PostRoles("Administrator, Transmission SME")]
    [PatchRoles("Administrator, Transmission SME")]
    [DeleteRoles("Administrator, Transmission SME")]
    public class ChannelGroupDetails : ChannelGroupType
    {
        public string ChannelGroup { get; set; }
        public string MeasurementType { get; set; }
        public string MeasurementCharacteristic { get; set; }
    }
}
