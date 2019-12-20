//******************************************************************************************************
//  AssetView.cs - Gbtc
//
//  Copyright © 2019, Grid Protection Alliance.  All Rights Reserved.
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
//  12/20/2019 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************


using GSF.Data;
using GSF.Data.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openXDA.Model
{
   
    public class AssetView
    {
       
        #region [ Properties ]

        [PrimaryKey(true)]
        public int ID { get; set; }

        [Required]
        public double VoltageKV { get; set; }

        [Required]
        [StringLength(50)]
        [Searchable]
        public string AssetKey { get; set; }

        public string Description { get; set; }

        public string AssetName { get; set; }

        [Required]
        public string AssetType {get; set; }
       
        #endregion

        #region [ Methods ]

        #endregion
    }
}
