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
            const string runtimeSection =
                @"<runtime>" +
                @"  <assemblyBinding xmlns=""urn:schemas-microsoft-com:asm.v1"">" +
                @"    <dependentAssembly>" +
                @"      <assemblyIdentity name=""Microsoft.Owin"" publicKeyToken=""31bf3856ad364e35"" culture=""neutral"" />" +
                @"      <bindingRedirect oldVersion=""0.0.0.0-2.1.0.0"" newVersion=""2.1.0.0"" />" +
                @"    </dependentAssembly>" +
                @"    <dependentAssembly>" +
                @"      <assemblyIdentity name=""Microsoft.Owin.Security"" publicKeyToken=""31bf3856ad364e35"" culture=""neutral"" />" +
                @"      <bindingRedirect oldVersion=""0.0.0.0-2.1.0.0"" newVersion=""2.1.0.0"" />" +
                @"    </dependentAssembly>" +
                @"    <dependentAssembly>" +
                @"      <assemblyIdentity name=""System.Net.Http.Formatting"" publicKeyToken=""31bf3856ad364e35"" culture=""neutral"" />" +
                @"      <bindingRedirect oldVersion=""0.0.0.0-5.2.3.0"" newVersion=""5.2.3.0"" />" +
                @"    </dependentAssembly>" +
                @"    <dependentAssembly>" +
                @"      <assemblyIdentity name=""System.Net.Http"" publicKeyToken=""b03f5f7f11d50a3a"" culture=""neutral"" />" +
                @"      <bindingRedirect oldVersion=""0.0.0.0-4.0.0.0"" newVersion=""4.0.0.0"" />" +
                @"    </dependentAssembly>" +
                @"    <dependentAssembly>" +
                @"      <assemblyIdentity name=""Newtonsoft.Json"" culture=""neutral"" publicKeyToken=""30ad4fe6b2a6aeed"" />" +
                @"      <bindingRedirect oldVersion=""0.0.0.0-6.0.0.0"" newVersion=""6.0.0.0"" />" +
                @"    </dependentAssembly>" +
                @"    <dependentAssembly>" +
                @"      <assemblyIdentity name=""System.Web.Optimization"" publicKeyToken=""31bf3856ad364e35"" />" +
                @"      <bindingRedirect oldVersion=""1.0.0.0-1.1.0.0"" newVersion=""1.1.0.0"" />" +
                @"    </dependentAssembly>" +
                @"    <dependentAssembly>" +
                @"      <assemblyIdentity name=""WebGrease"" publicKeyToken=""31bf3856ad364e35"" />" +
                @"      <bindingRedirect oldVersion=""0.0.0.0-1.6.5135.21930"" newVersion=""1.6.5135.21930"" />" +
                @"    </dependentAssembly>" +
                @"    <dependentAssembly>" +
                @"      <assemblyIdentity name=""System.Web.Helpers"" publicKeyToken=""31bf3856ad364e35"" />" +
                @"      <bindingRedirect oldVersion=""1.0.0.0-3.0.0.0"" newVersion=""3.0.0.0"" />" +
                @"    </dependentAssembly>" +
                @"    <dependentAssembly>" +
                @"      <assemblyIdentity name=""System.Web.Mvc"" publicKeyToken=""31bf3856ad364e35"" />" +
                @"      <bindingRedirect oldVersion=""1.0.0.0-5.2.3.0"" newVersion=""5.2.3.0"" />" +
                @"    </dependentAssembly>" +
                @"    <dependentAssembly>" +
                @"      <assemblyIdentity name=""System.Web.WebPages"" publicKeyToken=""31bf3856ad364e35"" />" +
                @"      <bindingRedirect oldVersion=""1.0.0.0-3.0.0.0"" newVersion=""3.0.0.0"" />" +
                @"    </dependentAssembly>" +
                @"    <dependentAssembly>" +
                @"      <assemblyIdentity name=""Antlr3.Runtime"" publicKeyToken=""eb42632606e9261f"" culture=""neutral"" />" +
                @"      <bindingRedirect oldVersion=""0.0.0.0-3.5.0.2"" newVersion=""3.5.0.2"" />" +
                @"    </dependentAssembly>" +
                @"  </assemblyBinding>" +
                @"</runtime>";

            string configFilePath = FilePath.GetAbsolutePath("openXDA.exe.config");
            XDocument doc = XDocument.Load(configFilePath);
            XElement runtime = XElement.Parse(runtimeSection);

            if (!doc.Descendants("runtime").Any())
            {
                doc.Element("configuration").Add(runtime);
                doc.Save(configFilePath);
                return;
            }

            XElement assemblyBinding = runtime.Elements().Single();

            if (!doc.Descendants(assemblyBinding.Name).Any())
            {
                doc.Descendants("runtime").First().Add(assemblyBinding);
                doc.Save(configFilePath);
            }
        }
    }
}
