//******************************************************************************************************
//  ScheduledDataSourceDefinition.cs - Gbtc
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
using log4net;
using openXDA.Model;

namespace FaultData.DataWriters.Emails
{
    public class ScheduledDataSourceDefinition
    {
        public ScheduledEmailType EmailType { get; set; }
        public ScheduledEmailDataSourceEmailType Mapping { get; set; }
        public ScheduledEmailDataSource DataSource { get; set; }

        public DataSourceResponse CreateAndProcess(ScheduledDataSourceFactory factory, DateTime timeOccurred)
        {
            DataSourceResponse response = new DataSourceResponse() { Model = DataSource };

            try
            {
                ScheduledDataSource dataSource = factory.CreateDataSource(this);
                response.Created = true;
                response.Data = dataSource.Process(timeOccurred);
                response.Success = true;
            }
            catch (Exception ex)
            {
                Log.Debug(ex.Message, ex);
                response.Exception = ex;
            }

            return response;
        }

        private static readonly ILog Log = LogManager.GetLogger(typeof(ScheduledDataSourceDefinition));
    }
}
