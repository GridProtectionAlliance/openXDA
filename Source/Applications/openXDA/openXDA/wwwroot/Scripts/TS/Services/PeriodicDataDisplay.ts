﻿//******************************************************************************************************
//  PeriodicDataDisplay1.ts - Gbtc
//
//  Copyright © 2018, Grid Protection Alliance.  All Rights Reserved.
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
//  05/25/2018 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************
import * as moment from 'moment';
import { PeriodicDataDisplay } from './../../TSX/global'

export default class PeriodicDataDisplayService {
    getData(meterID: number, startDate: string, endDate:string, pixels: number, measurementCharacteristicID:number, measurementTypeID:number,harmonicGroup: number, type) {
        return $.ajax({
            type: "GET",
            url: `${window.location.origin}/api/PeriodicDataDisplay/GetData?MeterID=${meterID}` +
                `&startDate=${moment(startDate).format('YYYY-MM-DD')}` +
                `&endDate=${moment(endDate).format('YYYY-MM-DD')}` +
                `&pixels=${pixels}` +
                `&MeasurementCharacteristicID=${measurementCharacteristicID}` + 
                `&MeasurementTypeID=${measurementTypeID}` +
                `&HarmonicGroup=${harmonicGroup}` +
                `&type=${type}`,
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            cache: true,
            async: true
        }) as JQuery.jqXHR<PeriodicDataDisplay.ReturnData>;
    }

    getMeters() {
        return $.ajax({
            type: "GET",
            url: `${window.location.origin}/api/PeriodicDataDisplay/GetMeters`,
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            cache: true,
            async: true
        });
    }

    getMeasurementCharacteristics(fromStepChangeWebReport, meterID) {
        return $.ajax({
            type: "GET",
            url: `${window.location.origin}/api/PeriodicDataDisplay/GetMeasurementCharacteristics` +
                `${(fromStepChangeWebReport ? `?MeterID=${meterID}` : ``)}`,
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            cache: true,
            async: true
        }) as JQuery.jqXHR<PeriodicDataDisplay.MeasurementCharateristics[]>;
    }


}