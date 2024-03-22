//******************************************************************************************************
//  OpenXDAController.cs - Gbtc
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
//  03/04/2020 - Billy Ernest
//       Generated original version of source code.
//  04/11/2022 - G. Santos
//       Copied portion of source code from SEBrowser.
//
//******************************************************************************************************

using GSF.Data.Model;

namespace openXDA.Model
{
    [CustomView(@"SELECT
                    ID, Name, Color,
                    REPLACE(REPLACE(RIGHT(Area.STAsText(), len(Area.STAsText()) - charindex('(', Area.STAsText())),')',''),'(','') AS Area
                    FROM StandardMagDurCurve")]
    [AllowSearch]
    [PostRoles("Administrator")]
    [DeleteRoles("Administrator")]
    [PatchRoles("Administrator")]
    public class StandardMagDurCurve
    {
        [PrimaryKey]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Area { get; set; }
        public string Color { get; set; }
    }
}