//******************************************************************************************************
//  IDataReader.cs - Gbtc
//
//  Copyright © 2014, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the Eclipse Public License -v 1.0 (the "License"); you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/eclipse-1.0.php
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  08/01/2014 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System.IO;
using FaultData.DataSets;
using openXDA.Model;

namespace FaultData.DataReaders
{
    public interface IDataReader
    {
        /// <summary>
        /// Determines whether the file can be loaded at this time.
        /// </summary>
        /// <param name="fileList">The list of files to be loaded from disk.</param>
        /// <returns>True if the file can be loaded; false otherwise.</returns>
        bool IsReadyForLoad(FileInfo[] fileList);

        /// <summary>
        /// Identifies the primary data file from the given file group.
        /// </summary>
        /// <param name="fileGroup">The group of files to be parsed.</param>
        /// <returns>The primary data file from the file group.</returns>
        DataFile GetPrimaryDataFile(FileGroup fileGroup);

        /// <summary>
        /// Parses the file into a meter data set per meter contained in the file.
        /// </summary>
        /// <param name="fileGroup">The group of files to be parsed.</param>
        /// <returns>The data set containing data parsed from the file.</returns>
        MeterDataSet Parse(FileGroup fileGroup);
    }
}
