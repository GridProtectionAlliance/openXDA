﻿//******************************************************************************************************
//  EmailDataSource.cs - Gbtc
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
//  11/5/2021 - Samuel Robinson
//       Generated original version of source code.
//
//******************************************************************************************************

using System.Runtime.Serialization;
using GSF.Data.Model;

namespace openXDA.Model
{
    [KnownType(typeof(ScheduledEmailDataSource))]
    [KnownType(typeof(TriggeredEmailDataSource))]
    public abstract class EmailDataSource
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        public string Name { get; set; }

        public string AssemblyName { get; set; }

        public string TypeName { get; set; }

        public string ConfigUI { get; set; }
    }

    public class TriggeredEmailDataSource : EmailDataSource { }
    public class ScheduledEmailDataSource : EmailDataSource { }
}
