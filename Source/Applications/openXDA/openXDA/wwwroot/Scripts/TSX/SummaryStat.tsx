//******************************************************************************************************
//  SummaryStat.tsx - Gbtc
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
//  06/11/2018 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

import * as React from 'react';
import * as _ from "lodash";
import * as stats from 'stats-lite';
import { PeriodicDataDisplay } from './global'

const SummaryStat = (props: {data: PeriodicDataDisplay.Points}) =>{
    if (props.data == null) return null;
    if (props.data.length == 0) return null;

    var avg = stats.mean(props.data.map(d => d[1]));
    var median = stats.median(props.data.map(d => d[1]));
    var variance = stats.variance(props.data.map(d => d[1]));
    var stdev = stats.stdev(props.data.map(d => d[1]));
    var sum = stats.sum(props.data.map(d => d[1]));
    var min = _.min(props.data.map(d => d[1]));
    var max = _.max(props.data.map(d => d[1]));

    return (
        <div>
            <table className="table">
                <thead>
                    <tr>
                        <td>Stat</td>
                        <td>Value</td>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>Average</td>
                        <td>{format(avg) }</td>
                    </tr>
                    <tr>
                        <td>Median</td>
                        <td>{format(median) }</td>
                    </tr>
                    <tr>
                        <td>Variance</td>
                        <td>{format(variance)}</td>
                    </tr>
                    <tr>
                        <td>Std Dev</td>
                        <td>{format(stdev) }</td>
                    </tr>
                    <tr>
                        <td>Maximum</td>
                        <td>{format(max) }</td>
                    </tr>
                    <tr>
                        <td>Minimum</td>
                        <td>{format(min) }</td>
                    </tr>
                    <tr>
                        <td>Sum</td>
                        <td>{format(sum) }</td>
                    </tr>
                </tbody>

            </table>
        </div>
    );

    function format(val:number ) {
        if (val > 1000000 || val < -1000000)
            return ((val / 1000000) | 0) + "M";
        else if (val > 1000 || val < -1000)
            return ((val / 1000) | 0) + "K";
        else if (val < 0.01)
            return val.toFixed(4);
        else if (val == undefined)
            return val;
        else
            return val.toFixed(2);
    }

}

export default SummaryStat;