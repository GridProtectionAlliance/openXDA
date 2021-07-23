﻿//******************************************************************************************************
//  ExternalOpenXDAField.cs - Gbtc
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
//  04/13/2020 - Christoph Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

using GSF.Data.Model;

namespace SystemCenter.Model
{
    /// <summary>
    /// Model that contains data obtained from external Databases and should be saved directly in the Model (e.g. VoltageKV, AssetKey...) 
    /// </summary>
    [UseEscapedName, TableName("ExternalOpenXDAField")]
    public class ExternalOpenXDAField
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        public string ParentTable { get; set; }
        public string FieldName { get; set; }
        public string ExternalDB { get; set; }
        public string ExternalDBTable { get; set; }
        public string ExternalDBTableKey { get; set; }

    }


}