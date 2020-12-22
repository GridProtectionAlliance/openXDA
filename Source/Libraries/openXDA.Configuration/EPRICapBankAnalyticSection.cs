//******************************************************************************************************
//  EPRICapBankAnalyticSection.cs - Gbtc
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
//  08/04/2020 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

using System.ComponentModel;
using System.Configuration;

namespace openXDA.Configuration
{
    public class EPRICapBankAnalyticSection
    {
        public const string CategoryName = "EPRICapBankAnalytic";

        [Setting]
        [DefaultValue(false)]
        public bool Enabled { get; set; }

        [Setting]
        [DefaultValue("./CapBankAnalysis/Data/")]
        public string DataFileLocation { get; set; }

        [Setting]
        [DefaultValue("./CapBankAnalysis/Parameter/")]
        public string ParameterFileLocation { get; set; }

        [Setting]
        [DefaultValue("./CapBankAnalysis/Results/")]
        public string ResultFileLocation { get; set; }

        [Setting]
        [DefaultValue(500.0D)]
        public double VThreshhold { get; set; }

        [Setting]
        [DefaultValue(4.0D)]
        public double IThreshhold { get; set; }

        [Setting]
        [DefaultValue(10.0D)]
        public double THDLimit { get; set; }

        [Setting]
        [DefaultValue(1.0D)]
        public double Toffset { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool EvalPreInsertion { get; set; }

        [Setting]
        [DefaultValue("openXDA.CapSwitchAnalysis.dll")]
        public string AnalysisRoutineAssembly { get; set; }

        [Setting]
        [DefaultValue("openXDA.CapSwitchAnalysis.Analyzer.RunAnalytic")]
        public string AnalysisRoutineMethod { get; set; }

        [Setting]
        [DefaultValue("")]
        public string Analyzer { get; set; }

        [Setting]
        [DefaultValue(1200000)]
        public int Delay { get; set; }
    }
}
