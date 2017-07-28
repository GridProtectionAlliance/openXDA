//******************************************************************************************************
//  Setting.cs - Gbtc
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
//  05/04/2017 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using GSF.Data.Model;
using System.ComponentModel.DataAnnotations;

namespace XDAAlarmCreationApp.Model
{
    public class Setting
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        [StringLength(300)]
        public string Name { get; set; }

        [StringLength(300)]
        [Searchable]
        public string Label { get; set; }
    }

    public class LineCharacteristicsToDisplay
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        [StringLength(400)]
        public string Name { get; set; }

        [StringLength(500)]
        [Searchable]
        public string Label { get; set; }
        public int FileTypeID { get; set; }
        public bool Display { get; set; }
    }

    public class ResultsToDisplay
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        [StringLength(400)]
        public string Name { get; set; }
        [StringLength(500)]
        [Searchable]
        public string Label { get; set; }
        public int FileTypeID { get; set; }
        public bool Display { get; set; }
    }

    public class ColorGradients
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        [StringLength(400)]
        [Required]
        public string Name { get; set; }

        [StringLength(500)]
        [Searchable]
        [Required]
        [RegularExpression(@"^(((-?(?:\d+(?:\.\d*)?|(\.\d+)))\-(-?(?:\d+(?:\.\d*)?|(\.\d+))))|([><=]-?(?:\d+(?:\.\d*)?|(\.\d+))))$", ErrorMessage = "Invalid Range: Format as =x.xx, x.xx-x.xx, <x.xx, or >x.xx. Enter negative signs directly infront of leading x with no extra space.")]
        public string RangeValue { get; set; }
        [Required]
        [RegularExpression(@"^([0-9a-fA-F]{6})$", ErrorMessage = "Enter a 6 digit Hex number")]
        public string HexColor { get; set; }
        [Required]
        public int LoadOrder { get; set; }

    }


}
