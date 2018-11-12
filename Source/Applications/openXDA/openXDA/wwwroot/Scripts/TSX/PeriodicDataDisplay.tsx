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
    props: {};
    state: { meterID: number, startDate: string, endDate: string, type: string, detailedReport: boolean, measurements: Array<Measurement>, width: number, data: any, numMeasurements: number, measurementsReturned: number, fromStepChangeWebReport: boolean};
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
            detailedReport: (query['detailedReport'] != undefined ? query['detailedReport'] == "true" : false),
            fromStepChangeWebReport: (query['fromStepChangeWebReport'] != undefined ? query['fromStepChangeWebReport'] == "true" : false),
            measurements: [],
            width: window.innerWidth - 475,
            data: null,
            numMeasurements: 0,
            measurementsReturned: 0
        }

    }

    getData() {
        $(this.refs.loader).show();

        this.periodicDataDisplayService.getMeasurementCharacteristics(this.state.fromStepChangeWebReport, this.state.meterID).done(data => {
            this.setState({ data: data, numMeasurements: data.length });
            this.createMeasurements(data);
        });
    }

    returnedMeasurement() {
        if (this.state.numMeasurements == this.state.measurementsReturned + 1) $(this.refs.loader).hide();
        this.setState({ measurementsReturned: ++this.state.measurementsReturned })

    }
    createMeasurements(data) {
        if (data == null || data.length == 0) {
            $(this.refs.loader).hide();
            return;
        };

        $(this.refs.loader).show();

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
                returnedMeasurement={this.returnedMeasurement.bind(this)}
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
        delete state.numMeasurements;
        delete state.measurementsReturned;
        delete state.width;
        this.history['push']('PeriodicDataDisplay.cshtml?' + queryString.stringify(state, { encode: false }))
    }

    render() {
        var height = window.innerHeight -  60;

        return (
        <div>
            <div className="screen" style={{ height: height, width: window.innerWidth }}>
                <div className="vertical-menu">
                    <div className="form-group">
                        <label>Meter: </label>
                        <MeterInput value={this.state.meterID} onChange={(obj) => this.setState(obj)} />
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
                        <label>Detailed Report: <input type="checkbox" value={this.state.detailedReport.toString()} defaultChecked={this.state.detailedReport} onChange={(e) => {
                            this.setState({ detailedReport: !this.state.detailedReport })
                        }} /></label>
                    </div>

                    <div className="form-group">
                        <div style={{ float: 'left' }} ref={'loader'} hidden>
                            <div style={{ border: '5px solid #f3f3f3', WebkitAnimation: 'spin 1s linear infinite', animation: 'spin 1s linear infinite', borderTop: '5px solid #555', borderRadius: '50%', width: '25px', height: '25px' }}></div>
                            <span>Loading...</span>
                        </div>

                        <button className='btn btn-primary' style={{ float: 'right' }} onClick={() => { this.updateUrl(); this.getData(); }}>Apply</button>
                    </div>


                </div>
                <div className="waveform-viewer" style={{ width: window.innerWidth }}>
                    <div className="list-group" style={{ maxHeight:height, overflowY: 'auto' }}>
                            {this.state.measurements}
                    </div>
                </div>
            </div>
        </div>
        );
    }
}

ReactDOM.render(<PeriodicDataDisplay />, document.getElementById('bodyContainer'));