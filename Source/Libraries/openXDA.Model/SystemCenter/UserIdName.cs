//******************************************************************************************************
//  AdditionalField.cs - Gbtc
//
//  Copyright © 2019, Grid Protection Alliance.  All Rights Reserved.
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
//  05/10/2022 - Gabriel Santos
//       Generated original version of source code.
//
//******************************************************************************************************

using GSF.Data.Model;
using System.ComponentModel.DataAnnotations;

namespace SystemCenter.Model
{
    /// <summary>
    /// View to just grab ID and name, to protect other sensitive portions of the DB
    /// </summary>
    [AllowSearch]
    [CustomView(@"
    SELECT
        ID,
        Name
    FROM
        UserAccount")]
    public class UserIdName
    {

        [PrimaryKey(true)]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }
    }
}