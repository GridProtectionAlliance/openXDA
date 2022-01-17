﻿//******************************************************************************************************
//  global.d.ts - Gbtc
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
//  01/12/2022 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

export namespace PeriodicDataDisplay {
    type MeasurementCharateristics = { MeasurementType: string, MeasurementTypeID: number, MeasurementCharacteristic: string, MeasurementCharacteristicID: number, HarmonicGroup: number }
    interface LegendRow { color: string, data: Points, enabled: boolean }
    interface Legend { [key: string]: LegendRow }

    type Point = [number, number];
    type Points = Point[];
    interface ReturnData {
        [key: string]: Points
    }
}

export namespace TrendingcDataDisplay {
    interface Measurement { ID: number, MeterID: number, MeterName: string, MeasurementName: string, Maximum: boolean, MaxColor: string, Average: boolean, AvgColor: string, Minimum: boolean, MinColor: string, Data?: Points, Axis: number }

    type MeasurementCharateristics = { MeasurementType: string, MeasurementTypeID: number, MeasurementCharacteristic: string, MeasurementCharacteristicID: number, HarmonicGroup: number }
    interface LegendRow { color: string, data: [number, number][], enabled: boolean }
    interface Legend { [key: string]: LegendRow }

    type Point = {Time: number, Minimum: number, Average: number, Maximum: number};
    type Points = Point[];
    type Type = 'Minimum' | 'Average' | 'Maximum';
    interface ReturnData {
        [key: string]: Points
    }

    interface FlotAxis {
        position: 'left' | 'right',
        color: 'black',
        axisLabel: string,
        axisLabelUseCanvas: true,
        show: true,
        min?: number,
        max?: number
    }
}