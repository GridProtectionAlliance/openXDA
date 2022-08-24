﻿//******************************************************************************************************
//  SentEmail.cs - Gbtc
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
//  06/23/2017 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using GSF.Data.Model;

namespace openXDA.Model
{
    public class SentEmail
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        [ParentKey(typeof(EmailType))]
        public int EmailTypeID { get; set; }

        public DateTime TimeSent { get; set; }

        public string ToLine { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }
    }
}
