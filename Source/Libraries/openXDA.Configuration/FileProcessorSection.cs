//******************************************************************************************************
//  FileProcessorSection.cs - Gbtc
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
//  01/16/2021 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.ComponentModel;
using System.Configuration;

namespace openXDA.Configuration
{
    public class FileProcessorSection
    {
        public const string CategoryName = "FileProcessor";

        /// <summary>
        /// Gets or sets the pattern used to parse file paths in
        /// order to identify the meter that the file came from.
        /// </summary>
        [Setting]
        [DefaultValue(@"(?<AssetKey>[^\\]+)\\[^\\]+$")]
        public string FilePattern { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of hours prior to the current system time
        /// before the file creation time indicates that the data should not be processed.
        /// </summary>
        [Setting]
        [DefaultValue(0.0D)]
        public double MaxFileCreationTimeOffset { get; set; }

        /// <summary>
        /// Gets or sets the maximum file size, in MB,
        /// of the files processed by openXDA.
        /// </summary>
        [Setting]
        [DefaultValue(30.0D)]
        public double MaxFileSize { get; set; }

        /// <summary>
        /// Gets or sets the number of threads used
        /// for processing file data concurrently.
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
