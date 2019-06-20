//******************************************************************************************************
//  INFDataSet.cs - Gbtc
//
//  Copyright © 2019, Grid Protection Alliance.  All Rights Reserved.
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
//  06/13/2019 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System.Collections.Generic;
using System.Linq;
using GSF.Interop;

namespace FaultData.DataSets
{
    public class INFSection
    {
        public string Name { get; }
        private Dictionary<string, INFEntry> Entries { get; }

        public INFSection(string name, Dictionary<string, INFEntry> entries)
        {
            Name = name;
            Entries = entries;
        }

        public List<INFEntry> GetEntries() =>
            Entries.Values.ToList();

        public INFEntry GetEntry(string key) =>
            Entries.TryGetValue(key, out INFEntry entry)
                ? entry
                : null;
    }

    public class INFEntry
    {
        public INFSection Section { get; }
        public string Key { get; }
        public string Value { get; }

        public INFEntry(INFSection section, string key, string value)
        {
            Section = section;
            Key = key;
            Value = value;
        }
    }

    public class INFDataSet
    {
        private Dictionary<string, INFSection> Sections { get; }

        public INFDataSet(IniFile infFile)
        {
            Sections = new Dictionary<string, INFSection>();

            foreach (string sectionName in infFile.GetSectionNames())
            {
                Dictionary<string, INFEntry> entries = new Dictionary<string, INFEntry>();
                INFSection section = new INFSection(sectionName, entries);

                foreach (string key in infFile.GetSectionKeys(sectionName))
                {
                    string value = infFile.GetKeyValue(sectionName, key);
                    INFEntry entry = new INFEntry(section, key, value);
                    entries.Add(key, entry);
                }

                Sections.Add(sectionName, section);
            }
        }

        public List<INFSection> GetSections() =>
            Sections.Values.ToList();

        public INFSection GetSection(string name) =>
            Sections.TryGetValue(name, out INFSection section)
                ? section
                : null;

        public INFEntry GetEntry(INFSection section, string key) =>
            section?.GetEntry(key);

        public INFEntry GetEntry(string section, string key) =>
            GetEntry(GetSection(section), key);

        public string GetValue(INFSection section, string key) =>
            GetEntry(section, key)?.Value;

        public string GetValue(string section, string key) =>
            GetValue(GetSection(section), key);
    }
}
