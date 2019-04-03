//******************************************************************************************************
//  EMCBReader.cs - Gbtc
//
//  Copyright © 2018, Grid Protection Alliance.  All Rights Reserved.
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
//  10/15/2018 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.IO;
using System.Text.RegularExpressions;
using FaultData.DataReaders;
using FaultData.DataSets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace XDA.EMCB
{
    public class EMCBReader : IDataReader
    {
        #region [ Members ]

        // Nested Types
        private class EMCBPath
        {
            public string Type { get; }
            public string EMCBID { get; }
            public string PushID { get; }

            public EMCBPath(string filePath)
            {
                const string TypeGroup = nameof(Type);
                const string EMCBIDGroup = nameof(EMCBID);
                const string PushIDGroup = nameof(PushID);

                string typePattern = ToGroupPattern(TypeGroup);
                string emcbIDPattern = ToGroupPattern(EMCBIDGroup);
                string pushIDPattern = ToGroupPattern(PushIDGroup);
                string pathPattern = $"//emcb/{typePattern}/{emcbIDPattern}/{pushIDPattern}";
                Match pathMatch = Regex.Match(filePath, pathPattern);

                if (!pathMatch.Success)
                    throw new ArgumentException("Invalid EMCB file path.", nameof(filePath));

                Type = pathMatch.Groups[TypeGroup].Value;
                EMCBID = pathMatch.Groups[EMCBIDGroup].Value;
                PushID = pathMatch.Groups[PushIDGroup].Value;
            }

            private string ToGroupPattern(string groupName)
            {
                return $"(?<{groupName}>[^/]*)";
            }
        }

        // Fields
        private bool m_disposed;

        #endregion

        #region [ Constructors ]

        #endregion

        #region [ Properties ]

        public MeterDataSet MeterDataSet { get; } = new MeterDataSet();

        private EMCBPath FilePath { get; set; }
        private dynamic FileData { get; set; }

        #endregion

        #region [ Methods ]

        public bool CanParse(string filePath, DateTime fileCreationTime)
        {
            FilePath = new EMCBPath(filePath);

            try
            {
                using (TextReader fileReader = File.OpenText(filePath))
                using (JsonReader reader = new JsonTextReader(fileReader))
                {
                    FileData = JObject.Load(reader);
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public void Parse(string filePath)
        {

        }

        #endregion
    }
}
