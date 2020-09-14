//******************************************************************************************************
//  AssetConnectionType.cs - Gbtc
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
//  12/13/2019 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.ComponentModel.DataAnnotations;
using Gemstone.Data;
using Gemstone.Data.Model;
using Newtonsoft.Json;

namespace OpenXDA.Model
{
    [TableName("AssetRelationshipType")]
    public class AssetConnectionType
    {
        #region [ Members ]
        #endregion

        #region [ Properties ]

        [PrimaryKey(true)]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        [Searchable]
        public string Name { get; set; }

        [Required]
        [StringLength(250)]
        [Searchable]
        public string Description { get; set; }

        public bool BiDirectional { get; set; }

        public string JumpConnection { get; set; }

        public string PassThrough { get; set; }
        
        // If Jump Connection is False the measurment is obnly applicaple to this Asset
        // If PassThrough is False the measurment can only be handed across this connection but no any further
        // {0} gets replaced with the MeasurementTypeID.
        // Bidirectional is not relevant at this point.

        #endregion

        #region [ Methods ]


        #endregion
    }

}
