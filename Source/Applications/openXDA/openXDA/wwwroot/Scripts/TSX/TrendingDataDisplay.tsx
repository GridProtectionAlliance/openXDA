//******************************************************************************************************
//  TrendingDataDisplay.tsx - Gbtc
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
//  07/19/2018 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

import * as React from 'react';
import * as ReactDOM from 'react-dom';
import TrendingDataDisplayService from './../TS/Services/TrendingDataDisplay';
import createHistory from "history/createBrowserHistory"
import * as queryString from "query-string";
import * as moment from 'moment';
import * as _ from "lodash";
import MeterInput from './MeterInput';
import MeasurementInput from './MeasurementInput';
import TrendingChart from './TrendingChart';
import DateTimeRangePicker from './DateTimeRangePicker';
import { relative } from 'path';
import DistributionPlot from './DistributionPlot';
import SummaryStat from './SummaryStat';

declare interface state { meterID: number, startDate: string, endDate: string, width: number, data: JSON, measurementID: number, type: Array<string>, distributionData: any };

class TrendingDataDisplay extends React.Component<any, any>{
    history: object;
    trendingDataDisplayService: TrendingDataDisplayService;
    resizeId: any;
    updateUrlId: any;
    state: state
    constructor(props) {
        super(props);

        this.history = createHistory();
        this.trendingDataDisplayService = new TrendingDataDisplayService();

        var query = queryString.parse(this.history['location'].search);

        this.state = {
            meterID: (query['meterID'] != undefined ? query['meterID'] : 0),
            startDate: (query['startDate'] != undefined ? query['startDate'] : moment().subtract(7, 'day').format('YYYY-MM-DDTHH:mm')),
            endDate: (query['endDate'] != undefined ? query['endDate'] : moment().format('YYYY-MM-DDTHH:mm')),
            measurementID: (query['measurementID'] != undefined ? query['measurementID'] : 0),
            width: window.innerWidth - 475,
            type: ["Minimum", "Maximum", "Average"],
            data: null,
            distributionData: null
        }

        this.history['listen']((location, action) => {
            var query = queryString.parse(this.history['location'].search);
            this.setState({
                meterID: (query['meterID'] != undefined ? query['meterID'] : 0),
                startDate: (query['startDate'] != undefined ? query['startDate'] : moment().subtract(7, 'day').format('YYYY-MM-DDTHH:mm')),
                endDate: (query['endDate'] != undefined ? query['endDate'] : moment().format('YYYY-MM-DDTHH:mm')),
                measurementID: (query['measurementID'] != undefined ? query['measurementID'] : 0),
                width: window.innerWidth - 475
            }, () => this.getData());
        });


    }

    stateSetter(object) {
        this.setState(object, () => {
            this.updateUrl();
        });
    }

    getData() {
        $(this.refs.loader).show();
        this.trendingDataDisplayService.getData(this.state.measurementID, this.state.startDate, this.state.endDate, this.state.width).done(data => {
            this.setState({ data: data, distributionData: { key: '', data: {data: data['Average']}} }, () => $(this.refs.loader).hide() );
        });
    }

    componentDidMount() {
        window.addEventListener("resize", this.handleScreenSizeChange.bind(this));
        if (this.state.measurementID != 0) this.getData();
    }   

    componentWillUnmount() {
        $(window).off('resize');
    }

    handleScreenSizeChange() {
        clearTimeout(this.resizeId);
        this.resizeId = setTimeout(() => {
            this.updateUrl();
        }, 500);
    }

    updateUrl() {
        clearTimeout(this.updateUrlId);
        this.updateUrlId = setTimeout(() => {
            var state = _.clone(this.state) as state;
            delete state.data;
            delete state.type;
            delete state.width;
            delete state.distributionData;

            this.history['push']('TrendingDataDisplay.cshtml?' + queryString.stringify(state, { encode: false }))
        }, 500);

    }

    render() {
        var height = window.innerHeight - $('#navbar').height();
        var menuWidth = 250;
        var sideWidth = 400;
        var top = $('#navbar').height() - 30;
        return (
        <div>
            <div className="screen" style={{ height: height, width: window.innerWidth, position: 'relative', top: top }}>
                <div className="vertical-menu">
                    <div className="form-group">
                        <label>Time Range: </label>
                    <DateTimeRangePicker startDate={this.state.startDate} endDate={this.state.endDate} stateSetter={(obj) => {
                        this.setState(obj, () => this.updateUrl());
                    }} />
                    </div>
                    <div className="form-group">
                        <label>Data Type: </label>
                    <select onChange={(obj) => this.setState({ type: $(obj.currentTarget).val() })} className="form-control" style={{overflowY: "hidden"}} defaultValue={this.state.type} multiple>
                            <option value="Average">Average</option>
                            <option value="Maximum">Maximum</option>
                            <option value="Minimum">Minimum</option>
                        </select>
                    </div>

                    <div className="form-group">
                        <label>Meter: </label>
                            <MeterInput value={this.state.meterID} onChange={(obj) => this.setState({ meterID: obj })} />
                    </div>

                    <div className="form-group">
                        <label>Measurement: </label>
                        <MeasurementInput meterID={this.state.meterID} value={this.state.measurementID} onChange={(obj) => this.setState({ measurementID: obj }, this.updateUrl)} />
                    </div>


                    <div className="form-group">
                        <div style={{ float: 'left' }} ref={'loader'} hidden>
                            <div style={{border: '5px solid #f3f3f3', WebkitAnimation: 'spin 1s linear infinite', animation: 'spin 1s linear infinite', borderTop: '5px solid #555', borderRadius: '50%', width: '25px', height: '25px'}}></div>
                            <span>Loading...</span>
                        </div>
                    </div>


                </div>
                <div className="waveform-viewer" style={{ width: window.innerWidth - menuWidth - sideWidth, height: height, float: 'left', left: sideWidth }}>
                    <TrendingChart startDate={this.state.startDate} endDate={this.state.endDate} data={this.state.data} type={this.state.type} stateSetter={(object) => this.stateSetter(object)}/>
                </div>
                <div style={{width: sideWidth, height: 'inherit', position: 'relative', float: 'right'}}>
                    <h4>Statistics</h4>
                    <div style={{ width: 'inherit', height: 'calc(50% - 100px)', padding: '5px', marginTop: 50, marginBottom: 50 }}>
                        <DistributionPlot data={this.state.distributionData} bins={40} />
                    </div>
                    <div style={{ width: 'inherit', height: '50%', padding: '5px' }}>
                        <SummaryStat data={this.state.distributionData} />
                    </div>
                </div>
            </div>
        </div>
        );
    }
}

ReactDOM.render(<TrendingDataDisplay />, document.getElementById('bodyContainer'));