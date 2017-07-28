//******************************************************************************************************
//  IndvidualResult.cs - Gbtc
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
//  05/24/2017 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************
using GSF.Data.Model;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using GSF.ComponentModel;
using GSF.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace EPRIDriveConfigurationApp.Model
{
    public class IndividualResult
    {       
        [PrimaryKey(true)]
        public int ID { get; set; }
        public int ResultID { get; set; }
	    public int LineSegmentID { get; set; }
	    public float PrimaryOverVoltageDistributedSmallDER  { get; set; } 
        public float PrimaryVoltageDeviationDistributedSmallDER  { get; set; } 
        public float RegulatorDeviationDistributedSmallDER  { get; set; } 
        public float SecondaryOverVoltageDistributedSmallDER  { get; set; } 
        public float SecondaryVoltageDeviationDistributedSmallDER  { get; set; } 
        public float PrimaryOverVoltageDistributedLargeDER  { get; set; } 
        public float PrimaryVoltageDeviationDistributedLargeDER  { get; set; } 
        public float RegulatorDeviationDistributedLargeDER  { get; set; } 
        public float SecondaryOverVoltageDistributedLargeDER  { get; set; } 
        public float SecondaryVoltageDeviationDistributedLargeDER  { get; set; } 
        public float PrimaryOverVoltageCentralizedLargeDER  { get; set; } 
        public float PrimaryVoltageDeviationCentralizedLargeDER  { get; set; } 
        public float RegulatorDeviationCentralizedLargeDER  { get; set; } 
        public float SecondaryOverVoltageCentralizedLargeDER  { get; set; } 
        public float SecondaryVoltageDeviationCentralizedLargeDER  { get; set; } 
        public float PrimaryOverVoltageDistributedSmallDERlatdiv  { get; set; } 
        public float PrimaryVoltageDeviationDistributedSmallDERlatdiv  { get; set; } 
        public float PrimaryOverVoltageDistributedLargeDERlatdiv  { get; set; } 
        public float PrimaryVoltageDeviationDistributedLargeDERlatdiv  { get; set; } 
        public float PrimaryUnderVoltageDistributedSmallDER  { get; set; } 
        public float PrimaryUnderVoltageDistributedSmallDERlatdiv  { get; set; } 
        public float PrimaryUnderVoltageCentralizedLargeDER  { get; set; } 
        public float PrimaryUnderVoltageDistributedLargeDER  { get; set; } 
        public float PrimaryUnderVoltageDistributedLargeDERlatdiv  { get; set; } 
        public float PrimaryUnderVoltageGenDistributedSmallDER  { get; set; } 
        public float PrimaryUnderVoltageGenDistributedSmallDERlatdiv  { get; set; } 
        public float PrimaryUnderVoltageGenCentralizedLargeDER  { get; set; } 
        public float PrimaryUnderVoltageGenDistributedLargeDER  { get; set; } 
        public float PrimaryUnderVoltageGenDistributedLargeDERlatdiv  { get; set; } 
        public float ElementFaultDistributedSmallDER  { get; set; } 
        public float BreakerReachDistributedSmallDER  { get; set; } 
        public float SympatheticTripDistributedSmallDER  { get; set; } 
        public float FuseCoordinationDistributedSmallDER  { get; set; } 
        public float ElementFaultLargeDER  { get; set; } 
        public float BreakerReachLargeDER  { get; set; } 
        public float SympatheticTripLargeDER  { get; set; } 
        public float FuseCoordinationLargeDER  { get; set; } 
        public float SympatheticTripDistributedSmallDERlatdiv  { get; set; } 
        public float ElementFaultCentralizedLargeDER  { get; set; } 
        public float DistributedSmallDERReversepowerflow  { get; set; } 
        public float DistributedLargeDERReversepowerflow  { get; set; } 
        public float CentralizedLargeDERReversepowerflow  { get; set; } 
        public float DistributedSmallDERThermalforCharging  { get; set; } 
        public float DistributedLargeDERThermalforCharging  { get; set; } 
        public float CentralizedLargeDERThermalforCharging  { get; set; } 
        public float DistributedSmallDERThermalforDischarging  { get; set; } 
        public float DistributedLargeDERThermalforDischarging  { get; set; } 
        public float CentralizedLargeDERThermalforDischarging  { get; set; } 
        public float AvailableDemandLoading  { get; set; } 
        public float AvailablePVloading  { get; set; } 
        public float MinReversePowerFlow  { get; set; } 
        public float DistributedSmallDERUnintentionalIslanding  { get; set; } 
        public float DistributedLargeDERUnintentionalIslanding  { get; set; } 
        public float CentralizedLargeDERUnintentionalIslanding  { get; set; } 
    }
}
