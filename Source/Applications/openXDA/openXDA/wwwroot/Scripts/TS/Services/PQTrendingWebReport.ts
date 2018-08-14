//******************************************************************************************************
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

export default class PQTrendingWebReportService {
    response: any;
    getData(date: string, stat: string, sortField: string, ascending: boolean) {
        if (this.response != null) this.response.abort();

        this.response = $.ajax({
            type: "GET",
            url: `${window.location.origin}/api/PQTrendingWebReport/GetData`+
                `?date=${moment(date).format('YYYY-MM-DD')}` +
                `&stat=${stat}` +
                `&sortField=${sortField}` +
                `&ascending=${ascending}`,
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            cache: true,
            async: true
        });

        return this.response;

    }

    getChart(date: string, stat: string, meterId: number, measurementId: number) {
        if (this.response != null) this.response.abort();

        this.response = $.ajax({
            type: "GET",
            url: `${window.location.origin}/api/PQTrendingWebReport/GetChart` +
                `?date=${moment(date).format('YYYY-MM-DD')}` +
                `&stat=${stat}` +
                `&measurementID=${measurementId}` +
                `&meterID=${meterId}`,
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            cache: true,
            async: true
        });

        return this.response;

    }

}