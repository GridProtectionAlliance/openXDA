//******************************************************************************************************
//  Analyzer.cs - Gbtc
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
//  09/08/2020 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Reflection;
using GSF;
using MathWorks.MATLAB.NET.Arrays;
using TCSAM;

namespace openXDA.CapSwitchAnalysis
{
    public class Analyzer
    {
        [Setting]
        [DefaultValue("")]
        public string DependentAssemblies
        {
            get
            {
                return DependentAssemblyLookup.JoinKeyValuePairs();
            }
            set
            {
                DependentAssemblyLookup = value.ParseKeyValuePairs();
            }
        }

        private Dictionary<string, string> DependentAssemblyLookup { get; set; }

        public void RunAnalytic(double[,] kFactors, string inputParameterFile)
        {
            try
            {
                AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
                InvokeTCSAM(kFactors, inputParameterFile);
            }
            finally
            {
                AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;
            }
        }

        private void InvokeTCSAM(double[,] kFactors, string inputParameterFile)
        {
            capsw theObject = new capsw();
            MWNumericArray mwArray = new MWNumericArray(kFactors);
            theObject.fTCSAM_Analytics(inputParameterFile, mwArray, 0.0D, 0.0D);
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            AssemblyName assemblyName = new AssemblyName(args.Name);

            if (DependentAssemblyLookup.TryGetValue(assemblyName.Name, out string path))
                return Assembly.LoadFile(path);

            return null;
        }
    }
}
