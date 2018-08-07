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
import * as ReactDOM from 'react-dom';
import * as moment from 'moment';
import * as _ from "lodash";
import * as stats from 'stats-lite';

export default class SummaryStat extends React.Component<any, any>{
    props: { data: { data: { data: any }, key: string } }

    constructor(props) {
        super(props);
    }

    render() {
        if (this.props.data == null) return null;

        var avg = stats.mean(this.props.data.data.data.map(d => d[1]));
        var median = stats.median(this.props.data.data.data.map(d => d[1]));
        var variance = stats.variance(this.props.data.data.data.map(d => d[1]));
        var stdev = stats.stdev(this.props.data.data.data.map(d => d[1]));
        var sum = stats.sum(this.props.data.data.data.map(d => d[1]));
        var min = _.min(this.props.data.data.data.map(d => d[1]));
        var max = _.max(this.props.data.data.data.map(d => d[1]));

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
                            <td>{ this.format(avg) }</td>
                        </tr>
                        <tr>
                            <td>Median</td>
                            <td>{this.format(median) }</td>
                        </tr>
                        <tr>
                            <td>Std Dev</td>
                            <td>{this.format(stdev) }</td>
                        </tr>
                        <tr>
                            <td>Maximum</td>
                            <td>{this.format(max) }</td>
                        </tr>
                        <tr>
                            <td>Minimum</td>
                            <td>{this.format(min) }</td>
                        </tr>
                        <tr>
                            <td>Sum</td>
                            <td>{this.format(sum) }</td>
                        </tr>
                    </tbody>

                </table>
            </div>
        );
    }

    format(val) {
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