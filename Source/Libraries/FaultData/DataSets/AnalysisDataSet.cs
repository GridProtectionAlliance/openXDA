//******************************************************************************************************
//  AnalysisDataSet.cs - Gbtc
//
//  Copyright © 2022, Grid Protection Alliance.  All Rights Reserved.
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
//  07/08/2022 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

namespace FaultData.DataSets
{
    public class AnalysisDataSet
    {
        public string EventType { get; set; }
        public double FaultLocation { get; set; } = double.NaN;
        public double VA { get; set; } = double.NaN;
        public double VB { get; set; } = double.NaN;
        public double VC { get; set; } = double.NaN;
        public double IA { get; set; } = double.NaN;
        public double IB { get; set; } = double.NaN;
        public double IC { get; set; } = double.NaN;
        public double IG { get; set; } = double.NaN;
        public double IN { get; set; } = double.NaN;
        public double INeg3 { get; set; } = double.NaN;
    }
}
