//******************************************************************************************************
//  TaskProcessorSection.cs - Gbtc
//
//  Copyright © 2021, Grid Protection Alliance.  All Rights Reserved.
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
//  02/24/2021 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.ComponentModel;
using System.Configuration;

namespace openXDA.Configuration
{
    public class TaskProcessorSection
    {
        public const string CategoryName = "TaskProcessor";

        /// <summary>
        /// Gets or sets the query used to filter
        /// meters when polling for analysis tasks.
        /// </summary>
        [Setting]
        [DefaultValue("SELECT ID FROM Meter")]
        public string MeterFilterQuery { get; set; }

        /// <summary>
        /// Gets or sets the number of threads used
        /// for processing meter data concurrently.
        /// </summary>
        /// <remarks>
        /// Values less than or equal to zero will be set to the number of logical processors.
        /// </remarks>
        [Setting]
        [DefaultValue(0)]
        public int ProcessingThreadCount
        {
            get
            {
                return _ProcessingThreadCount;
            }
            set
            {
                _ProcessingThreadCount = value;

                if (_ProcessingThreadCount <= 0)
                    _ProcessingThreadCount = Environment.ProcessorCount;
            }
        }

        private int _ProcessingThreadCount { get; set; }
    }
}
