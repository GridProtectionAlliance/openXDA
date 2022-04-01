﻿//******************************************************************************************************
//  DistributionPlot.tsx - Gbtc
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
import * as ReactDOM from 'react-dom';
import * as moment from 'moment';
import * as _ from "lodash";
import './../flot/jquery.flot.min.js';
import * as stats from 'stats-lite';
import { PeriodicDataDisplay } from './global'

interface Props{
    data: PeriodicDataDisplay.Points, label: string , bins: number
}

const DistributionPlot = (props: Props) => {
    const ref = React.useRef(null);

    React.useEffect(() => {
        if (props.data == null) return;
        else if (props.data.length == 0) return;

        let stat = stats.histogram(props.data.map(d => d[1]), props.bins)
        let totalCount = stat.values.reduce((t, x) => { return t + x });
        let options = {
            series: {
                bars: {
                    show: true,
                    fill: true,
                    lineWidth: 0,
                    fillColor: '#517ec6',
                    barWidth: stat.binWidth
                }
            },
            canvas: true,
            legend: { show: false },
            grid: {
                show: true,
                autoHighlight: false,
                borderWidth: { top: 0, right: 0, bottom: 1, left: 0 },
                labelMargin: 0,
                axisMargin: 0,
                minBorderMargin: 0
            },
            yaxis: { tickLength: 0 },
            xaxis: {
                tickLength: 0,
                reserveSpace: false,
                ticks: 4,
                min: stat.binLimits[0],
                max: stat.binLimits[1],
                tickFormatter: function (val, axis) {
                    if (axis.delta > 1000000 && (val > 1000000 || val < -1000000))
                        return ((val / 1000000) | 0) + "M";
                    else if (axis.delta > 1000 && (val > 1000 || val < -1000))
                        return ((val / 1000) | 0) + "K";
                    else
                        return val.toFixed(axis.tickDecimals);
                }
            },

        }

        let data = stat.values.map((d, i) => {
            var bin = stat.binLimits[0] + i * stat.binWidth;
            return [bin, (d / totalCount) * 100]
        });

        ($ as any).plot($(ref.current), [{ label: props.label, data: data }], options);

    }, [props.data, props.label, props.bins]);

    return (
        <div style={{ height: '100%', margin: '10px'}}>
            <label style={{ textAlign: 'center' }}>{props.label}</label>
            <div ref={ref} style={{ height: 'calc(100% - 20px)', width: 'inherit' }}></div>
        </div>
    );

}

export default DistributionPlot;