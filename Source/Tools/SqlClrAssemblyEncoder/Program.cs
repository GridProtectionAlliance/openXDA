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
//  02/23/2016 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using GSF.Console;
using GSF.IO;

namespace SqlClrAssemblyEncoder
{
    class Program
    {
        static void Main()
        {
            string[] args;

            XElement includeElement;
            XDocument wxiDoc;
            string outFile;

            string flag;
            string name;
            string hex;

            Directory.SetCurrentDirectory(FilePath.GetAbsolutePath(""));

            args = Arguments.ToArgs(Environment.CommandLine);
            includeElement = new XElement("Include");
            wxiDoc = new XDocument(includeElement);
            outFile = "SqlClr.wxi";
            flag = null;

            foreach (string arg in args.Skip(1))
            {
                if (flag == "-o")
                {
                    flag = "--";
                    outFile = arg;
                    continue;
                }

                if ((object)flag == null)
                {
                    if (arg == "-o" || arg == "--")
                    {
                        flag = arg;
                        continue;
                    }
                }

                name = FilePath.GetFileName(arg);
                hex = "0x" + BitConverter.ToString(File.ReadAllBytes(arg)).Replace("-", "");
                includeElement.Add(new XElement("Property", new XAttribute("Id", name), new XAttribute("Value", hex)));
            }

            if (flag == "-o")
            {
                name = FilePath.GetFileName(flag);
                hex = "0x" + BitConverter.ToString(File.ReadAllBytes(flag)).Replace("-", "");
                includeElement.Add(new XElement("Property", new XAttribute("Id", name), new XAttribute("Value", hex)));
            }

            wxiDoc.Save(outFile);
        }
    }
}
