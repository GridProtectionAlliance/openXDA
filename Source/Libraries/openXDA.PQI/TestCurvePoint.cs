//******************************************************************************************************
//  TestCurvePoint.cs - Gbtc
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
//  08/09/2022 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

using System;

namespace openXDA.PQI
{
   
    public class TestCurvePoint
    {
        /// <summary>
        /// Path to query this test curve point
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Path to query the test curve this point belongs to
        /// </summary>
        public string TestCurve { get; set; }

        /// <summary>
        /// X-value (duration) on a mag/dur chart
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Y-value(magnitude) on a mag/dur chart
        /// </summary>
        public double Y { get; set; }

       
    }
}
