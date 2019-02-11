//******************************************************************************************************
//  Measurement.tsx - Gbtc
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
//  05/29/2018 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

import * as React from 'react';
import * as _ from "lodash";
import PeriodicDataDisplayService from './../TS/Services/PeriodicDataDisplay';
import Legend from "./Legend";
import DistributionPlot from './DistributionPlot';
import SummaryStat from './SummaryStat';
import LineChart from './LineChart';

export default class Measurement extends React.Component<any, any>{
    periodicDataDisplayService: PeriodicDataDisplayService;
    options: object;
    plot: any;
    state: {legend: object, hover: number, display: string, distributions: Array<DistributionPlot>, legendRows: object}
    constructor(props) {
        super(props);

        this.state = {
            legend: {},
            hover: null,
            display: 'block',
            distributions: [],
            legendRows: []
        }

        this.periodicDataDisplayService = new PeriodicDataDisplayService();
    }

    getColor(index) {
        switch (index % 20) {
            case 0: return "#dcc582";
            case 1: return "#cb4b4b";
            case 2: return "#afd8f8";
            case 3: return "#56a14d";
            case 4: return "#734da1";
            case 5: return "#795548";
            case 6: return "#9e9e9e";
            case 7: return "#a31c73";
            case 8: return "#3a96ca";
            case 9: return "#a00bda";
            case 10: return "#aa9b66";
            case 11: return "#54ec23";
            case 12: return "#6d3827";
            case 13: return "#7b84ae";
            case 14: return "#dceccd";
            case 15: return "#0c87fc";
            case 16: return "#575ddd";
            case 17: return "#5b4f5b";
            case 18: return "#5986e3";
            case 19: return "#cb42b3";

        }
    }

    componentWillReceiveProps(nextProps) {

        var props = _.clone(this.props) as any;
        var nextPropsClone = _.clone(nextProps);

        delete props.data;
        delete props.stateSetter;
        delete props.height;
        delete nextPropsClone.data;
        delete nextPropsClone.stateSetter;
        delete nextPropsClone.height;

        if (!(_.isEqual(props, nextPropsClone))) {
            this.getData(nextProps);

        }
    }

    componentDidMount() {
        this.getData(this.props);
    }




    getData(props) {
        props.stateSetter()
        this.periodicDataDisplayService.getData(props.meterID, props.startDate, props.endDate, window.innerWidth, props.data.MeasurementCharacteristicID, props.data.MeasurementTypeID,this.props.type).done(data => {
            this.props.returnedMeasurement();

            if (Object.keys(data).length == 0) {
                this.setState({ display: 'none' });
                return;
            }

            this.setState({ display: 'block' });

            var legend = this.createLegendRows(data);
            this.createDistributionPlots(legend);
        });

    }

    createDistributionPlots(legend) {
        var distributions = Object.keys(legend).map(key => {
            var data = { key: key, data: legend[key] };
            return (
                <li key={key} style={{ height: '270px', width: '600px', display: 'inline-block' }}>
                    <table className="table" style={{ height: '270px', width: '600px'}}>
                        <tbody>
                            <tr style={{ height: '270px', width: '600px' }}>
                                <td style={{ height: 'inherit', width: '50%' }}><DistributionPlot data={data} bins={40} /></td>
                                <td style={{ height: 'inherit', width: '50%' }}><SummaryStat data={data} /></td>
                            </tr>
                        </tbody>
                    </table>
                </li>
            );
        });

        this.setState({ distributions: distributions});

    }


    createLegendRows(data) {
        var ctrl = this;

        var legend = (this.state.legend != undefined ? this.state.legend : {});

        $.each(Object.keys(data), function (i, key) {
            if (legend[key] == undefined)
                legend[key] = { color: ctrl.getColor(i), enabled: true, data: data[key] };
            else
                legend[key].data = data[key];
        });

        this.setState({ legendRows: legend });
        return legend;
    }

    render() {
        return (
            <div id={this.props.data.ID} className="list-group-item panel-default" style={{ padding: 0, display: this.state.display }}>
                <div className="panel-heading">{this.props.data.MeasurementCharacteristic} - {this.props.data.MeasurementType}</div>
                <div className="panel-body">
                    <div style={{ height: this.props.height, float: 'left', width: '100%', marginBottom: '10px'}}>
                        <div id={'graph'} style={{ height: 'inherit', width: this.props.width, position: 'absolute' }}>
                            <LineChart legend={this.state.legendRows} startDate={this.props.startDate} endDate={this.props.endDate} width={this.props.width}/>
                        </div>
                        <div id={'legend'} className='legend' style={{ position: 'absolute', right: '0' , width: '200px', height: this.props.height - 38, marginTop: '6px', borderStyle: 'solid', borderWidth: '2px', overflowY: 'auto' }}>
                            <Legend data={this.state.legendRows} callback={() => {
                                this.setState({ legend: this.state.legend });
                            }} />
                        </div>
                    </div>
                    { (this.props.detailedReport?
                        <div style={{ width: '100%', margin: '10px' }}>
                            <label style={{ width: '100%' }}>Distributions</label>

                            <ul style={{ display: 'inline', position: 'relative', width: '100%', marginTop: '25px', paddingLeft: '0' }}>
                                {this.state.distributions}
                            </ul>
                        </div>: null) 
                    }
                </div>
                <br />
            </div>
        );
    }

}
