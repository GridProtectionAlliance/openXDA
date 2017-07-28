//******************************************************************************************************
//  NodePoint.cs - Gbtc
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
//  05/23/2017 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using GSF.ComponentModel.DataAnnotations;
using GSF.Data.Model;
using System.ComponentModel.DataAnnotations;

namespace XDAAlarmCreationApp.Model
{
    public class NodePoint
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        [Searchable]
        [StringLength(100)]
        public string Name { get; set; }

        [Label("Result ID")]
        public int ResultID { get; set; }

        [Label("X Coord")]
        public int XCoordinate { get; set; }

        [Label("Y Coors")]
        public int YCoordinate { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

    }
}
