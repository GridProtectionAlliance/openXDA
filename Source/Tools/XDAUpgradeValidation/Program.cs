//******************************************************************************************************
//  Program.cs - Gbtc
//
//  Copyright © 2016, Grid Protection Alliance.  All Rights Reserved.
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
//  10/25/2016 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using GSF.IO;

namespace XDAUpgradeValidation
{
    class Program
    {
        static void Main(string[] args)
        {
            ValidateDependentAssemblies();
        }

        static void ValidateDependentAssemblies()
        {
            string configFilePath = FilePath.GetAbsolutePath("openXDA.exe.config");
            string defaultConfigFilePath = $"{configFilePath}.default";
            File.Copy(configFilePath, $"{configFilePath}.prev", true);

            XNamespace ns = "urn:schemas-microsoft-com:asm.v1";
            XName assemblyBindingSection = ns + "assemblyBinding";
            XDocument defaultDoc = XDocument.Load(defaultConfigFilePath);
            IEnumerable<XElement> defaultBindings = defaultDoc.Descendants(assemblyBindingSection);

            XDocument doc = XDocument.Load(configFilePath);
            XElement root = doc.Root;

            if (!root.Elements("runtime").Any())
                root.Add(new XElement("runtime"));

            root.Descendants(assemblyBindingSection).Remove();
            root.Element("runtime").Add(defaultBindings);
            doc.Save(configFilePath);
        }
    }
}
