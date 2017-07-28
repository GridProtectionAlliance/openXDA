//******************************************************************************************************
//  LineSegment.cs - Gbtc
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
    public class LineSegment
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        [Label("Result ID")]
        public int ResultID { get; set; }

        [Label("Start Node")]
        public int FromNodeID { get; set; }

        [Label("End Node")]
        public int ToNodeID { get; set; }
  
        public string Data { get; set; }


    }

    [TableName("LineSegmentView")]
    public class LineSegmentView : LineSegment
    {
        [Searchable]
        public string FromNode { get; set; }
        public string ToNode { get; set; }
    }
}
