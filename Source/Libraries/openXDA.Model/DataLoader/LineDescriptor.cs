//******************************************************************************************************
//  LineDescriptor.cs - Gbtc
//
//  Copyright © 2026, Grid Protection Alliance.  All Rights Reserved.
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
//  04/03/2026 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

namespace openXDA.Model.DataLoader
{
    public class LineDescriptor
    {
        public string AssetKey { get; set; }
        public string AssetName { get; set; }
        public double? VoltageKV { get; set; }
        public double? Length { get; set; }
        public double? R0 { get; set; }
        public double? X0 { get; set; }
        public double? R1 { get; set; }
        public double? X1 { get; set; }
        public double? ThermalRating { get; set; }
    }
}
