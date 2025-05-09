﻿//******************************************************************************************************
//  ScheduledDataSource.cs - Gbtc
//
//  Copyright © 2024, Grid Protection Alliance.  All Rights Reserved.
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
//  05/15/2024 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Xml.Linq;
using openXDA.Model;

namespace FaultData.DataWriters.Emails
{
    public class ScheduledDataSource : IScheduledDataSource
    {
        public string Name { get; }
        private IScheduledDataSource UnderlyingDataSource { get; }

        public ScheduledDataSource(string name, IScheduledDataSource dataSource)
        {
            Name = name;
            UnderlyingDataSource = dataSource;
        }

        public void Configure(Action<object> configurator) =>
            UnderlyingDataSource.Configure(configurator);

        public XElement Process(DateTime timeOccurred) =>
            UnderlyingDataSource.Process(timeOccurred);
    }
}
