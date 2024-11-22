//******************************************************************************************************
//  EditionChecker.cs - Gbtc
//
//  Copyright © 2023, Grid Protection Alliance.  All Rights Reserved.
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
//  10/07/2024 - Gabriel Santos
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using GSF.Data;
using GSF.Data.Model;
using openXDA.Model;

namespace openXDA.Configuration
{
    /// <summary>
    /// Enum to hold different editions of XDA
    /// </summary>
    public enum Edition
    {
        Base = 0,
        Enterprise = 1
    }

    /// <summary>
    /// Helper class that checks the edition of XDA
    /// </summary>
    public static class EditionChecker
    {
        /// <summary>
        /// Current XDA Edition
        /// </summary>
        private static Edition CheckedEdition { get; set; }
        // ToDo: Think about replacing this with a hash function instead
        private static Guid MagicGuid = new Guid("aa644f0c-a82b-4cf1-ba6e-be3a1b05eb6a");

        static EditionChecker()
        {
            UpdateEdition();
        }

        /// <summary>
        /// Checks to see if current edition is equal to or higher than supplied
        /// </summary>
        /// <returns> response as a <see cref="Edition"/></returns>
        public static bool CheckEdition(Edition editionLevel)
        {
            return CheckedEdition.CompareTo(editionLevel) >= 0;
        }

        /// <summary>
        /// Updates the current edition based on the value in the database
        /// </summary>
        public static void UpdateEdition()
        {
            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                Guid value;
                try
                {
                    value = new Guid(new TableOperations<Setting>(connection).QueryRecordWhere($"Name = 'System.EditionKey'")?.Value);
                }
                catch
                {
                    value = new Guid("00000000-0000-0000-0000-000000000000");
                }
                if (value == MagicGuid) CheckedEdition = Edition.Enterprise;
                else CheckedEdition = Edition.Base;
            }
        }

        /// <summary>
        /// Returns the current edition
        /// </summary>
        /// <returns>response as a <see cref="Edition"/></returns>
        public static Edition GetEdition()
        {
            return CheckedEdition;
        }

    }
}
