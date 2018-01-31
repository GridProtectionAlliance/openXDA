﻿//******************************************************************************************************
//  PQMarkAggregate.cs - Gbtc
//
//  Copyright © 2017, Grid Protection Alliance.  All Rights Reserved.
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
//  08/29/2017 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using GSF.Data.Model;

namespace openXDA.Model
{
    public class PQMarkAggregate
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

	    public int MeterID { get; set;}

	    public int Year { get; set;}

	    public int Month { get; set;}

	    public int ITIC { get; set;}

	    public int SEMI { get; set;}

	    public int SARFI90 { get; set;}

	    public int SARFI70 { get; set;}

	    public int SARFI50 { get; set;}

	    public int SARFI10 { get; set;}

	    public string THDJson { get; set; }
    }
}
