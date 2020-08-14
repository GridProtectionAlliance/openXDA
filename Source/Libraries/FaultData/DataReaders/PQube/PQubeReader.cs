//******************************************************************************************************
//  PQubeReader.cs - Gbtc
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
//  02/17/2020 - Stephen Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GSF.PQDIF.Logical;

namespace FaultData.DataReaders.PQube
{
    public class PQubeReader
    {
        public static readonly Guid PSL = Guid.Parse("4e354a9b-2ad0-463f-9c08-52a8ff9d5149");
        public static readonly Guid PQube = Guid.Parse("06afbbc3-35a9-4840-96f9-3b64381bde51");

        public static List<string> GetTriggers(ContainerRecord containerRecord, List<ObservationRecord> observationRecords)
        {
            return observationRecords
                .Where(observationRecord => observationRecord.DataSource.VendorID == PSL)
                .Where(observationRecord => observationRecord.DataSource.EquipmentID == PQube)
                .Select(observationRecord => GetTrigger(containerRecord, observationRecord))
                .Distinct()
                .ToList();
        }

        private static string GetTrigger(ContainerRecord containerRecord, ObservationRecord observationRecord)
        {
            if (containerRecord.Subject.StartsWith("EVENT:", StringComparison.OrdinalIgnoreCase))
                return containerRecord.Subject.Substring(7);

            string observationName = observationRecord.Name;
            string pattern = "^[^a-zA-Z]*";
            return Regex.Replace(observationName, pattern, "").Replace('_', ' ');
        }
    }
}
