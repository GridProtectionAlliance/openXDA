//******************************************************************************************************
//  DataQualitySummary.tsx - Gbtc
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
//  08/02/2018 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

import * as React from 'react';
import * as ReactDOM from 'react-dom';
import createHistory from "history/createBrowserHistory"
import * as queryString from "query-string";
import * as moment from 'moment';
import * as _ from "lodash";
import SpectralDataDisplayService from "./../TS/Services/SpectralDataDisplay";
import MeterInput from './MeterInput';
import 'react-datetime/css/react-datetime.css';
import * as DateTime from "react-datetime";
import * as Plot from 'react-plotly.js';


declare var phases: Array<{ID: number, Name: string }>;
declare var meters: Array<{ ID: number, Name: string }>;

export class SpectralDataDisplay extends React.Component{
    history: object;
    spectralDataDisplayService: SpectralDataDisplayService;
    resizeId: any;
    props: {};
    state: { date: string, meterID: string, type: string, data: Array<any>, level: string, phase: string, center: string, span: string};
    constructor(props) {
        super(props);

        this.history = createHistory();
        this.spectralDataDisplayService = new SpectralDataDisplayService();

        var query = queryString.parse(this.history['location'].search);

        this.state = {
            meterID: (query['meterID'] != undefined ? query['meterID'] : meters[0].ID),
            date: (query['date'] != undefined ? query['date'] : moment().format('YYYY-MM-DD')),
            level: (query['level'] != undefined ? query['level'] : "Average"),
            type: (query['type'] != undefined ? query['type'] : "Harmonic"),
            phase: (query['phase'] != undefined ? query['phase'] : phases[0].ID),
            center: (query['center'] != undefined ? query['center'] : 50),
            span: (query['span'] != undefined ? query['span'] : 50),
            data: [{ type: 'surface' , x:[11,12,13], y: ["a", 'b', 'c'], z : [[0,0,0], [0,0,0], [0,0,0]]}]
        }

        this.history['listen']((location, action) => {
            var query = queryString.parse(this.history['location'].search);
            var state = _.clone(this.state);
            delete state.data;

            if (this.compareObjects(query, state)) return;

            this.setState({
                meterID: (query['meterID'] != undefined ? query['meterID'] : meters[0].ID),
                date: (query['date'] != undefined ? query['date'] : moment().format('YYYY-MM-DD')),
                level: (query['level'] != undefined ? query['level'] : "Average"),
                type: (query['type'] != undefined ? query['type'] : "Harmonic"),
                phase: (query['phase'] != undefined ? query['phase'] : phases[0].ID),
                center: (query['center'] != undefined ? query['center'] : 50),
                span: (query['span'] != undefined ? query['span'] : 50),
            },this.getData);
        });


    }

    componentDidMount(): void {
        this.updateUrl();
    }

    render() {
        var height = window.innerHeight - 60;
        var phaseOptions = phases.map(x => <option key={x.ID} value={x.ID}>{x.Name}</option>);
        var meterOptions = meters.map(x => <option key={x.ID} value={x.ID}>{x.Name}</option>);
        var centerTimeout = null;
        var spanTimeout = null;

        return(
            <div className = "screen" style = {{ height: height, width: window.innerWidth, position: 'absolute', top: '60px' }} >
            <div className="vertical-menu">
                    <div className="form-group">
                        <label>Date: </label>
                        <DateTime
                            closeOnSelect={true}
                            isValidDate={(date) => { return date.isBefore(moment()) }}
                            value={moment(this.state.date)}
                            timeFormat={false}
                            dateFormat="MM/DD/YYYY"
                            onChange={(value) => this.setState({ date: (value as moment.Moment).format("YYYY-MM-DD") }, this.updateUrl)} />
                    </div>

                    <div className="form-group">
                        <label>Meter: </label>
                        <select
                            className='form-control'
                            onChange={(obj) => this.setState({meterID: obj.target.value }) }
                            defaultValue={this.state.meterID}>{meterOptions}</select>
                    </div>

                    <div className="form-group">
                        <label>Phase: </label>
                        <select
                            onChange={(obj) => {
                                this.setState({ phase: obj.target.value }, this.updateUrl);
                            }}
                            className="form-control"
                            defaultValue={this.state.phase}>{phaseOptions}</select>
                    </div>


                    <div className="form-group">
                        <label>Data Type: </label>
                        <select
                            onChange={(obj) => {
                                this.setState({ type: obj.target.value }, this.updateUrl);
                            }}
                            className="form-control"
                            defaultValue={this.state.type}
                            >
                            <option value="Harmonic">10 min Harmonic</option>
                            <option value="Interharmonic">10 min Interharmonic</option>
                            <option value="HighResolution">1 min High Resolution</option>
                            <option value="LowResolution">1 min Low Resolution</option>

                        </select>
                    </div>

                    <div className="form-group">
                        <label>Data Selection: </label>
                        <select
                            onChange={(obj) => {
                                this.setState({ level: obj.target.value }, this.updateUrl);
                            }}
                            className="form-control"
                            defaultValue={this.state.level}
                        >
                            <option value="Maximum">Maximum</option>
                            <option value="Average">Average</option>
                        </select>
                    </div>

                    <div className="form-group">
                        <div className="slidecontainer">
                            <p>Color Scale Center:</p>
                            <input type="range" min="1" max="100" className="slider" defaultValue={this.state.center} onChange={(obj) => {
                                clearTimeout(centerTimeout);
                                obj.persist();
                                centerTimeout = setTimeout(() => this.setState({ center: obj.target.value }, this.updateUrl), 500);
                            }}/>

                            <p>Color Scale Span:</p>
                            <input type="range" min="1" max="100" className="slider" defaultValue={this.state.span} onChange={(obj) => {
                                clearTimeout(spanTimeout);
                                obj.persist();
                                spanTimeout = setTimeout(() => this.setState({ span: obj.target.value }, this.updateUrl), 500);
                            }}/>
                        </div>
                    </div>

                    <div className="form-group">
                        <div style={{ float: 'left' }} ref={'loader'} hidden>
                            <div style={{ border: '5px solid #f3f3f3', WebkitAnimation: 'spin 1s linear infinite', animation: 'spin 1s linear infinite', borderTop: '5px solid #555', borderRadius: '50%', width: '25px', height: '25px' }}></div>
                            <span>Loading...</span>
                        </div>
                    </div>



                </div>

                <div className="waveform-viewer" style={{ width: window.innerWidth - 250, height: height, float: 'right' }}>
                    <Plot
                        data={this.state.data}
                        layout={{
                            width: window.innerWidth - 250, height: height, scene: {
                                xaxis: { title: 'Bin' },
                                yaxis: { title: 'Time' },
                                zaxis: { title: 'Value' }
                            }
                        }}
                    />
                </div>
            </div>
        );
    }


    getData(): void {
        $(this.refs.loader).show();
        this.spectralDataDisplayService.getData(this.state.meterID, this.state.date, this.state.type, this.state.level, this.state.phase).done(data => {
            if (data.length < 2) {
                this.setState({ data: [{ type: 'surface', x: [11, 12, 13], y: ["a", 'b', 'c'], z: [[0, 0, 0], [0, 0, 0], [0, 0, 0]] }] }, () => {
                    $(this.refs.loader).hide();
                });
                return;
            } 
            var x = data[0].filter((a,i) => i != 0);
            var y = data.filter((a, i) => i != 0).map(a => a[0]);
            var z = data.filter((a,i) => i != 0).map((a,i) => a.filter((b,index) => index != 0));
            var dmin = Math.min(...z.map(a => Math.min(...a)));
            var dmax = Math.max(...z.map(a => Math.max(...a)));
            var span = (dmax - dmin) * parseInt(this.state.span) / 50 / 2;
            var oldMean = (dmax - dmin) / 2;
            var cmin = oldMean - span;
            var cmax = oldMean + span;
            var colorScale = [[0, 'blue'], [parseInt(this.state.center)/100, 'yellow'], [1, 'red']];
            this.setState({ data: [{ type: 'surface', x: x, y: y, z: z, cmin: cmin, cmax: cmax, colorscale: colorScale }] }, () => {
                $(this.refs.loader).hide();
             });
        });
    }

    updateUrl(): void {
        var state = _.clone(this.state);
        delete state.data;

        this.history['push']('SpectralDataDisplay.cshtml?' + queryString.stringify(state, { encode: false }));
        this.getData();
    }

    compareObjects(object1: JSON, object2: JSON): boolean {
        var one = {};
        var two = {};
        _.each(Object.keys(object1).sort(), (d) => {
            one[d] = object1[d];
        });
        _.each(Object.keys(object2).sort(), (d) => {
            two[d] = object2[d];
        });

        return JSON.stringify(one) === JSON.stringify(two);

    }


}

ReactDOM.render(<SpectralDataDisplay />, document.getElementById('bodyContainer'));