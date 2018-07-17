//******************************************************************************************************
//  PeriodicDataDisplay1.tsx - Gbtc
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

import * as React from 'react';
import * as ReactDOM from 'react-dom';
import PeriodicDataDisplayService from './../TS/Services/PeriodicDataDisplay';
import createHistory from "history/createBrowserHistory"
import * as queryString from "query-string";
import * as moment from 'moment';
import * as _ from "lodash";
import MeterInput from './MeterInput';
import DatetimeRangePicker from 'react-datetime-range-picker';
import Measurement from './Measurement';

export class PeriodicDataDisplay extends React.Component<any, any>{
    history: object;
    periodicDataDisplayService: PeriodicDataDisplayService;
    resizeId: any;

    constructor(props) {
        super(props);

        this.history = createHistory();
        this.periodicDataDisplayService = new PeriodicDataDisplayService();

        var query = queryString.parse(this.history['location'].search);

        this.state = {
            meterID: (query['meterID'] != undefined ? query['meterID'] : 0),
            startDate: (query['startDate'] != undefined ? query['startDate'] : moment().subtract(7, 'day').format('YYYY-MM-DD')),
            endDate: (query['endDate'] != undefined ? query['endDate'] : moment().format('YYYY-MM-DD')),
            type: (query['type'] != undefined ? query['type'] : "Average"),
            detailedReport: (query['detailedReport'] != undefined ? query['detailedReport'] == "true" : true),
            print: (query['print'] != undefined ? query['print'] == "true" : false),
            measurements: [],
            width: window.innerWidth - 475,
            data: null
        }

    }

    getData() {
        this.periodicDataDisplayService.getMeasurementCharacteristics().done(data => {
            this.setState({ data: data });
            this.createMeasurements(data);
        });
    }

    createMeasurements(data) {
        if (data == null || data.length == 0) return;

        var list = data.map(d =>
            <Measurement
                meterID={this.state.meterID}
                startDate={this.state.startDate}
                endDate={this.state.endDate}
                key={d.MeasurementType + d.MeasurementCharacteristic}
                data={d}
                type={this.state.type}
                height={300}
                stateSetter={(obj) => this.setState(obj, this.updateUrl)}
                detailedReport={this.state.detailedReport}
                width={this.state.width}
            />);
        this.setState({ measurements: list }, () => this.updateUrl());
    }

    componentDidMount() {
        window.addEventListener("resize", this.handleScreenSizeChange.bind(this));
        if (this.state.meterID != 0) this.getData();
    }   

    componentWillUnmount() {
        $(window).off('resize');
    }

    handleScreenSizeChange() {
        clearTimeout(this.resizeId);
        this.resizeId = setTimeout(() => {
            this.setState({ width: window.innerWidth - 475 }, () => { this.createMeasurements(this.state.data)});
        }, 100);
    }

    updateUrl() {
        var state = _.clone(this.state);
        delete state.measurements;
        delete state.data;
        this.history['push']('PeriodicDataDisplay.cshtml?' + queryString.stringify(state, { encode: false }))
    }

    render() {
        var height = window.innerHeight - (this.state.print ? 0 : 60);

        return (
        <div>
                <div className="screen" style={{ height: height, width: window.innerWidth }}>
                    {(!this.state.print ?
                        <div className="vertical-menu">
                            <div className="form-group">
                                <label>Meter: </label>
                                <MeterInput value={this.state.meterID} onChange={(obj) => this.setState({ meterID: obj })} />
                            </div>
                            <div className="form-group">
                                <label>Time Range: </label>
                                <DatetimeRangePicker
                                    startDate={new Date(this.state.startDate)}
                                    endDate={new Date(this.state.endDate)}
                                    onChange={(obj) => { this.setState({ startDate: moment(obj.start).format('YYYY-MM-DD'), endDate: moment(obj.end).format('YYYY-MM-DD') }) }}
                                    inputProps={{ style: { width: '100px', margin: '5px' }, className: 'form-control' }}
                                    className='form'
                                    timeFormat={false}
                                />
                            </div>
                            <div className="form-group">
                                <label>Data Type: </label>
                                <select onChange={(obj) => this.setState({ type: obj.target.value })} className="form-control" defaultValue={this.state.type}>
                                    <option value="Average">Average</option>
                                    <option value="Maximum">Maximum</option>
                                    <option value="Minimum">Minimum</option>
                                </select>
                            </div>
                            <div className="form-group">
                                <label>Detailed Report: <input type="checkbox" value={this.state.detailedReport} defaultChecked={this.state.detailedReport} onChange={(e) => {
                                    this.setState({ detailedReport: !this.state.detailedReport })
                                }} /></label>
                            </div>

                            <div className="form-group">
                                <button className='btn btn-primary' style={{ float: 'right' }} onClick={() => { this.updateUrl(); this.getData(); }}>Apply</button>
                            </div>


                        </div>
                : null)}
                <div className="waveform-viewer" style={{ width: window.innerWidth }}>
                    <div className="list-group" style={{ maxHeight:height, overflowY: (this.state.print? 'visible' :'auto') }}>
                            {this.state.measurements}
                    </div>
                </div>
            </div>
        </div>
        );
    }
}

ReactDOM.render(<PeriodicDataDisplay />, document.getElementById('bodyContainer'));