//******************************************************************************************************
//  LineChart.tsx - Gbtc
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
//  06/07/2018 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

import * as React from 'react';
import * as moment from 'moment';
import * as _ from "lodash";
import './../flot/jquery.flot.min.js';
import './../flot/jquery.flot.time.min.js';
import { PeriodicDataDisplay } from './global'

const LineChart = (props: {legend: PeriodicDataDisplay.Legend, startDate: string, endDate: string,  width: number}) => {

    const graph = React.useRef(null);

    let options = {
        canvas: true,
        legend: { show: false },
        crosshair: { mode: "x" },
        selection: { mode: "x" },
        grid: {
            autoHighlight: false,
            clickable: true,
            hoverable: true,
            markings: []
        },
        xaxis: {
            mode: "time",
            tickLength: 10,
            reserveSpace: false,
            ticks: function (axis) {
                var ticks = [],
                    start = floorInBase(axis.min, axis.delta),
                    i = 0,
                    v = Number.NaN,
                    prev;

                do {
                    prev = v;
                    v = start + i * axis.delta;
                    ticks.push(v);
                    ++i;
                } while (v < axis.max && v != prev);
                return ticks;
            },
            tickFormatter: function (value, axis) {
                return moment(value).utc().format("MM/DD");
            }
        },
        yaxis: {
            labelWidth: 50,
            panRange: false,
            //ticks: 1,
            tickLength: 10,
            tickFormatter: function (val, axis) {
                if (axis.delta > 1000000 && (val > 1000000 || val < -1000000))
                    return ((val / 1000000) | 0) + "M";
                else if (axis.delta > 1000 && (val > 1000 || val < -1000))
                    return ((val / 1000) | 0) + "K";
                else
                    return val.toFixed(axis.tickDecimals);
            }
        }
    }

    React.useEffect(() =>{
       createDataRows();
    },[props.legend]);

    function createDataRows() {
        $(graph.current).children().remove();

        var startString = props.startDate;
        var endString = props.endDate;
        var newVessel = [];
        if (props.legend != undefined && Object.keys(props.legend).length > 0) {
            $.each(Object.keys(props.legend), (i, key) => {
                if (props.legend[key].enabled)
                    newVessel.push({ label: key, data: props.legend[key].data, color: props.legend[key].color })
            });
        }

        newVessel.push([[getMillisecondTime(startString), null], [getMillisecondTime(endString), null]]);
        ($ as any).plot($(graph.current), newVessel, options);
    }



    // round to nearby lower multiple of base
    function floorInBase(n, base) {
        return base * Math.floor(n / base);
    }

    function getMillisecondTime(date) {
        var milliseconds = moment.utc(date).valueOf();
        return milliseconds;
    }

    function getDateString(float) {
        var date = moment.utc(float).format('YYYY-MM-DDTHH:mm:ss');
        return date;
    }

    return <div ref={graph} style={{ height: 'inherit', width: props.width }}></div>;


}

export default LineChart;