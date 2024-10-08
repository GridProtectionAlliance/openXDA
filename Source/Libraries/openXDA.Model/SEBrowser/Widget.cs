﻿//******************************************************************************************************
//  Widget.cs - Gbtc
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
//  08/10/2020 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************
using GSF;
using GSF.Data.Model;
using GSF.Web;
using GSF.Web.Model;
using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace SEBrowser.Model
{
    /// <summary>
    /// Defines a widget used in SEBrowser
    /// </summary>
    [TableName("SEBrowser.Widget"), UseEscapedName]
    [PostRoles("Administrator")]
    [DeleteRoles("Administrator")]
    [PatchRoles("Administrator")]
    [AllowSearch]
    public class Widget
    {
        #region [ Properties ]

        [PrimaryKey(true)]
        public int ID { get; set; }

        public string Name { get; set; }

        public string Setting { get; set; }

        public string Type { get; set; }

        #endregion
    }

    [ViewOnly()]
    [TableName("SEbrowser.WidgetView"), UseEscapedName, AllowSearch]
    [PostRoles("Administrator")]
    [DeleteRoles("Administrator")]
    [PatchRoles("Administrator")]
    public class WidgetView : Widget
    {
        [ParentKey(typeof (WidgetCategory))]
        public int CategoryID { get; set; }
    }

}