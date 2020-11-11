//******************************************************************************************************
//  QualityFlags.cs - Gbtc
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
//  11/14/2020 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using GSF;

namespace openXDA.HIDS
{
    [Flags]
    public enum QualityFlags : uint
    {
        // Value flags
        Unreasonable = (uint)Bits.Bit07,
        Latched = (uint)Bits.Bit06,
        MarkedBadByUser = (uint)Bits.Bit04,
        MarkedBadByDevice = (uint)Bits.Bit03,
        MarkedBadByExternalSystem = (uint)Bits.Bit02,
        MarkedBadByDataRecovery = (uint)Bits.Bit01,
        Empty = (uint)Bits.Bit00,

        // Tuple flags
        Noncongruent = (uint)Bits.Bit15,

        // Device flags
        DeviceOutOfService = (uint)Bits.Bit23,
        DeviceMarkedBadByExternalSystem = (uint)Bits.Bit22,
        DeviceNotEnergized = (uint)Bits.Bit21
    }
}
