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
import {  TrendingcDataDisplay } from './global'
import { RandomColor } from '@gpa-gemstone/helper-functions';

const TrendingDataDisplay = () => {
    let trendingDataDisplayService = new TrendingDataDisplayService();
    const resizeId = React.useRef(null);
    const loader = React.useRef(null);

    let history = createHistory();

    let query = queryString.parse(history['location'].search);

    const [measurements, setMeasurements] = React.useState<TrendingcDataDisplay.Measurement[]>(sessionStorage.getItem('TrendingDataDisplay-measurements') == null ? [] : JSON.parse(sessionStorage.getItem('TrendingDataDisplay-measurements')));
    const [width, setWidth] = React.useState<number>(window.innerWidth - 475);
    const [startDate, setStartDate] = React.useState<string>(query['startDate'] != undefined ? query['startDate'] : moment().subtract(7, 'day').format('YYYY-MM-DDTHH:mm'));
    const [endDate, setEndDate] = React.useState<string>(query['endDate'] != undefined ? query['endDate'] : moment().format('YYYY-MM-DDTHH:mm'));
    const [axes, setAxes] = React.useState<TrendingcDataDisplay.FlotAxis[]>(sessionStorage.getItem('TrendingDataDisplay-axes') == null ? [{ axisLabel: 'Default', color: 'black', position: 'left' }] : JSON.parse(sessionStorage.getItem('TrendingDataDisplay-axes')));

    React.useEffect(() => {
        window.addEventListener("resize", handleScreenSizeChange.bind(this));
        //if (this.state.measurementID != 0) getData();

        history['listen']((location, action) => {
            let query = queryString.parse(history['location'].search);
            setStartDate(query['startDate'] != undefined ? query['startDate'] : moment().subtract(7, 'day').format('YYYY-MM-DDTHH:mm'));
            setEndDate(query['endDate'] != undefined ? query['endDate'] : moment().format('YYYY-MM-DDTHH:mm'));
        });

        return () => $(window).off('resize');
    }, []);

    React.useEffect(() => {
        if (measurements.length == 0) return;
        getData();
    }, [measurements.length, startDate, endDate]);

    React.useEffect(() => {
        history['push']('TrendingDataDisplay.cshtml?' + queryString.stringify({startDate, endDate}, { encode: false }))
    }, [startDate,endDate]);

    React.useEffect(() => {
        sessionStorage.setItem('TrendingDataDisplay-measurements', JSON.stringify(measurements.map(ms => ({ ID: ms.ID, MeterID: ms.MeterID,MeterName: ms.MeterName,MeasurementName: ms.MeasurementName,Average: ms.Average,AvgColor: ms.AvgColor,Maximum: ms.Maximum, MaxColor: ms.MaxColor, Minimum: ms.Minimum, MinColor: ms.MinColor, Axis: ms.Axis}))))
    }, [measurements]);

    React.useEffect(() => {
        sessionStorage.setItem('TrendingDataDisplay-axes', JSON.stringify(axes))
    }, [axes]);

    function getData() {
        $(loader.current).show();
        trendingDataDisplayService.getPostData(measurements, startDate, endDate, width).done(data => {
            let meas =[ ...measurements ];
            for (let key of Object.keys(data)) {
                let i = meas.findIndex(x => x.ID.toString() === key);
                meas[i].Data = data[key];
            }
            setMeasurements(meas)

            $(loader.current).hide()
        });
    }


    function handleScreenSizeChange() {
        clearTimeout(this.resizeId);
        this.resizeId = setTimeout(() => {
        }, 500);
    }

    let height = window.innerHeight - $('#navbar').height();
    let menuWidth = 250;
    let sideWidth = 400;
    let top = $('#navbar').height() - 30;
    return (
        <div>
            <div className="screen" style={{ height: height, width: window.innerWidth, position: 'relative', top: top }}>
                <div className="vertical-menu" style={{maxHeight: height, overflowY: 'auto' }}>
                    <div className="form-group">
                        <label>Time Range: </label>
                        <DateTimeRangePicker startDate={startDate} endDate={endDate} stateSetter={(obj) => {
                            if(startDate != obj.startDate)
                                setStartDate(obj.startDate);
                            if (endDate != obj.endDate)
                                setEndDate(obj.endDate);
                        }} />
                    </div>
                    <div className="form-group" style={{height: 50}}>
                        <div style={{ float: 'left' }} ref={loader} hidden>
                            <div style={{ border: '5px solid #f3f3f3', WebkitAnimation: 'spin 1s linear infinite', animation: 'spin 1s linear infinite', borderTop: '5px solid #555', borderRadius: '50%', width: '25px', height: '25px' }}></div>
                            <span>Loading...</span>
                        </div>
                    </div>


                    <div className="form-group">
                        <div className="panel-group">
                            <div className="panel panel-default">
                                <div className="panel-heading" style={{ position: 'relative', height: 60}}>
                                    <h4 className="panel-title" style={{ position: 'absolute', left: 15, width: '60%' }}>
                                        <a data-toggle="collapse" href="#MeasurementCollapse">Measurements:</a>
                                    </h4>
                                    <AddMeasurement Add={(msnt) => setMeasurements(measurements.concat(msnt))} />
                                </div>
                                <div id='MeasurementCollapse' className='panel-collapse'>
                                    <ul className='list-group'>
                                        {measurements.map((ms, i) => <MeasurementRow key={i} Measurement={ms} Measurements={measurements} Axes={axes} Index={i} SetMeasurements={setMeasurements} />)}
                                    </ul>
                                </div>
                            </div>

                        </div>
                    </div>


                    <div className="form-group">
                        <div className="panel-group">
                            <div className="panel panel-default">
                                <div className="panel-heading" style={{ position: 'relative', height: 60 }}>
                                    <h4 className="panel-title" style={{ position: 'absolute', left: 15, width: '60%' }}>
                                        <a data-toggle="collapse" href="#axesCollapse">Axes:</a>
                                    </h4>
                                    <AddAxis Add={(axis) => setAxes(axes.concat(axis))} />
                                </div>
                                <div id='axesCollapse' className='panel-collapse'>
                                    <ul className='list-group'>
                                        {axes.map((axis, i) => <AxisRow key={i} Axes={axes} Index={i} SetAxes={setAxes}/>)}
                                    </ul>
                                </div>
                            </div>

                        </div>
                    </div>

                </div>
                <div className="waveform-viewer" style={{ width: window.innerWidth - menuWidth, height: height, float: 'left', left: 0 }}>
                    <TrendingChart startDate={startDate} endDate={endDate} data={measurements} axes={axes} stateSetter={(object) => {
                        setStartDate(object.startDate);
                        setEndDate(object.endDate);
                    }} />
                </div>
            </div>
        </div>
    );
}

const AddMeasurement = (props: { Add: (msnt:TrendingcDataDisplay.Measurement) => void}) => {
    const [measurement, setMeasurement] = React.useState<TrendingcDataDisplay.Measurement>({ ID: 0, MeterID: 0, MeterName: '', MeasurementName: '', Maximum: true, MaxColor: RandomColor(), Average: true, AvgColor: RandomColor(), Minimum: true, MinColor: RandomColor(), Axis: 1 });

    return (
        <>
            <button type="button" style={{ position: 'absolute', right: 15}} className="btn btn-info" data-toggle="modal" data-target="#measurementModal" onClick={() => {
                setMeasurement({ ID: 0, MeterID: 0, MeterName: '', MeasurementName: '', Maximum: true, MaxColor: RandomColor(), Average: true, AvgColor: RandomColor(), Minimum: true, MinColor: RandomColor(), Axis: 1 });
            }}>Add</button>
            <div id="measurementModal" className="modal fade" role="dialog">
                <div className="modal-dialog">
                    <div className="modal-content">
                        <div className="modal-header">
                            <button type="button" className="close" data-dismiss="modal">&times;</button>
                            <h4 className="modal-title">Add Measurement</h4>
                        </div>
                        <div className="modal-body">
                            <div className="form-group">
                                <label>Meter: </label>
                                <MeterInput value={measurement.MeterID} onChange={(obj) => setMeasurement({ ...measurement, MeterID: obj.meterID, MeterName: obj.meterName })} />
                            </div>

                            <div className="form-group">
                                <label>Measurement: </label>
                                <MeasurementInput meterID={measurement.MeterID} value={measurement.ID} onChange={(obj) => setMeasurement({ ...measurement, ID: obj.measurementID, MeasurementName: obj.measurementName })} />
                            </div>

                            <div className="row">
                                <div className="col-lg-6">
                                    <div className="checkbox">
                                        <label><input type="checkbox" checked={measurement.Maximum} value={measurement.Maximum.toString()} onChange={() => setMeasurement({ ...measurement, Maximum: !measurement.Maximum }) }/> Maximum</label>
                                    </div>
                                </div>
                                <div className="col-lg-6">
                                    <input type="color" style={{textAlign:'left'}} className="form-control" value={measurement.MaxColor} onChange={(evt) => setMeasurement({ ...measurement, MaxColor: evt.target.value })} />
                                </div>

                            </div>
                            <div className="row">
                                <div className="col-lg-6">
                                    <div className="checkbox">
                                        <label><input type="checkbox" checked={measurement.Average} value={measurement.Average.toString()} onChange={() => setMeasurement({ ...measurement, Average: !measurement.Average })} /> Average</label>
                                    </div>
                                </div>
                                <div className="col-lg-6">
                                    <input type="color" style={{ textAlign: 'left' }} className="form-control" value={measurement.AvgColor} onChange={(evt) => setMeasurement({ ...measurement, AvgColor: evt.target.value })} />
                                </div>

                            </div>
                            <div className="row">
                                <div className="col-lg-6">
                                    <div className="checkbox">
                                        <label><input type="checkbox" checked={measurement.Minimum} value={measurement.Minimum.toString()} onChange={() => setMeasurement({ ...measurement, Minimum: !measurement.Minimum })} /> Minimum</label>
                                    </div>
                                </div>
                                <div className="col-lg-6">
                                    <input type="color" style={{ textAlign: 'left' }} className="form-control" value={measurement.MinColor} onChange={(evt) => setMeasurement({ ...measurement, MinColor: evt.target.value })} />
                                </div>

                            </div>

                        </div>
                        <div className="modal-footer">
                            <button type="button" className="btn btn-primary" data-dismiss="modal" onClick={() => props.Add(measurement) }>Add</button>
                            <button type="button" className="btn btn-default" data-dismiss="modal">Cancel</button>
                        </div>
                    </div>
                </div>
            </div>

        </>
    );
}

const MeasurementRow = (props: { Measurement: TrendingcDataDisplay.Measurement, Measurements: TrendingcDataDisplay.Measurement[], Index: number, SetMeasurements: (measurements: TrendingcDataDisplay.Measurement[]) => void, Axes: TrendingcDataDisplay.FlotAxis[]}) => {
    return (
        <li className='list-group-item' key={props.Measurement.ID}>
            <div>{props.Measurement.MeterName}</div><button type="button" style={{ position: 'relative', top: -20 }} className="close" onClick={() => {
                let meas = [...props.Measurements];
                meas.splice(props.Index, 1);
                props.SetMeasurements(meas)
            }}>&times;</button>
            <div>{props.Measurement.MeasurementName}</div>
            <div>
                <select className='form-control' value={props.Measurement.Axis} onChange={(evt) => {
                    let meas = [...props.Measurements];
                    meas[props.Index].Axis = parseInt(evt.target.value);
                    props.SetMeasurements(meas)
                }}>
                    {props.Axes.map((a, ix) => <option key={ix} value={ix + 1}>{a.axisLabel}</option>)}
                </select>

            </div>

            <div className='row'>
                <div className='col-lg-4'>
                    <div className="">
                        <div className="checkbox">
                            <label><input type="checkbox" checked={props.Measurement.Maximum} value={props.Measurement.Maximum.toString()} onChange={() => {
                                let meas = [...props.Measurements];
                                meas[props.Index].Maximum = !meas[props.Index].Maximum;
                                props.SetMeasurements(meas)
                            }} /> Max</label>
                        </div>
                    </div>
                    <div className="">
                        <input type="color" className="form-control" value={props.Measurement.MaxColor} onChange={(evt) => {
                            let meas = [...props.Measurements];
                            meas[props.Index].MaxColor = evt.target.value;
                            props.SetMeasurements(meas)
                        }} />
                    </div>
                </div>
                <div className='col-lg-4'>
                    <div className="">
                        <div className="checkbox">
                            <label><input type="checkbox" checked={props.Measurement.Average} value={props.Measurement.Average.toString()} onChange={() => {
                                let meas = [...props.Measurements];
                                meas[props.Index].Average = !meas[props.Index].Average;
                                props.SetMeasurements(meas)
                            }} /> Avg</label>
                        </div>
                    </div>
                    <div className="">
                        <input type="color" className="form-control" value={props.Measurement.AvgColor} onChange={(evt) => {
                            let meas = [...props.Measurements];
                            meas[props.Index].AvgColor = evt.target.value;
                            props.SetMeasurements(meas)
                        }} />
                    </div>
                </div>
                <div className='col-lg-4'>
                    <div className="">
                        <div className="checkbox">
                            <label><input type="checkbox" checked={props.Measurement.Minimum} value={props.Measurement.Minimum.toString()} onChange={() => {
                                let meas = [...props.Measurements];
                                meas[props.Index].Minimum = !meas[props.Index].Minimum;
                                props.SetMeasurements(meas)
                            }} /> Min</label>
                        </div>
                    </div>
                    <div className="">
                        <input type="color" className="form-control" value={props.Measurement.MinColor} onChange={(evt) => {
                            let meas = [...props.Measurements];
                            meas[props.Index].MinColor = evt.target.value;
                            props.SetMeasurements(meas)
                        }} />
                    </div>
                </div>

            </div>

        </li>

    );
}

const AddAxis = (props: { Add: (axis: TrendingcDataDisplay.FlotAxis) => void }) => {
    const [axis, setAxis] = React.useState<TrendingcDataDisplay.FlotAxis>({ position: 'left', color: 'black', axisLabel: '', axisLabelUseCanvas: true, show: true });

    return (
        <>
            <button type="button" style={{ position: 'absolute', right: 15 }}  className="btn btn-info" data-toggle="modal" data-target="#axisModal" onClick={() => {
                setAxis({ position: 'left', color: 'black', axisLabel: '', axisLabelUseCanvas: true, show: true });
            }}>Add</button>
            <div id="axisModal" className="modal fade" role="dialog">
                <div className="modal-dialog">
                    <div className="modal-content">
                        <div className="modal-header">
                            <button type="button" className="close" data-dismiss="modal">&times;</button>
                            <h4 className="modal-title">Add Axis</h4>
                        </div>
                        <div className="modal-body">
                            <div className="form-group">
                                <label>Label: </label>
                                <input type="text" className="form-control" value={axis.axisLabel} onChange={(evt) => setAxis({ ...axis, axisLabel: evt.target.value })} />
                            </div>

                            <div className="form-group">
                                <label>Position: </label>
                                <select className='form-control' value={axis.position} onChange={(evt) => setAxis({ ...axis, position: evt.target.value as 'left' | 'right'})}>
                                    <option value='left'>left</option>
                                    <option value='right'>right</option>
                                </select>
                            </div>

                        </div>
                        <div className="modal-footer">
                            <button type="button" className="btn btn-primary" data-dismiss="modal" onClick={() => props.Add(axis)}>Add</button>
                            <button type="button" className="btn btn-default" data-dismiss="modal">Cancel</button>
                        </div>
                    </div>
                </div>
            </div>

        </>
    );
}

const AxisRow = (props: { Index: number, Axes: TrendingcDataDisplay.FlotAxis[], SetAxes: ( axes: TrendingcDataDisplay.FlotAxis[]) => void}) => {
    return (
        <li className='list-group-item' key={props.Index}>
            <div>{props.Axes[props.Index].axisLabel}</div><button type="button" style={{ position: 'relative', top: -20 }} className="close" onClick={() => {
                let a = [...props.Axes];
                a.splice(props.Index, 1);
                props.SetAxes(a)
            }}>&times;</button>
            <div>
                <select className='form-control' value={props.Axes[props.Index].position} onChange={(evt) => {
                    let a = [...props.Axes];
                    a[props.Index].position = evt.target.value as 'left' | 'right';
                    props.SetAxes(a)
                }}>
                    <option value='left'>left</option>
                    <option value='right'>right</option>
                </select>
            </div>
            <div className='row'>
                <div className='col-lg-6'>
                    <div className='form-group'>
                        <label>Min</label>
                        <input className='form-control' type="number" value={props.Axes[props.Index]?.min ?? ''} onChange={(evt) => {
                            let axes = [...props.Axes];
                            if (evt.target.value == '')
                                delete axes[props.Index].min;
                            else
                                axes[props.Index].min = parseFloat(evt.target.value);
                            props.SetAxes(axes)
                            }} /> 
                    </div>
                </div>
                <div className='col-lg-6'>
                    <div className='form-group'>
                        <label>Max</label>
                        <input className='form-control' type="number" value={props.Axes[props.Index]?.max ?? ''} onChange={(evt) => {
                            let axes = [...props.Axes];
                            if (evt.target.value == '')
                                delete axes[props.Index].max;
                            else
                                axes[props.Index].max = parseFloat(evt.target.value);
                            props.SetAxes(axes)
                        }} />
                    </div>
                </div>

            </div>
            <button className='btn btn-info' onClick={() => {
                let axes = [...props.Axes];
                delete axes[props.Index].max;
                delete axes[props.Index].min;

                props.SetAxes(axes)

            }} >Use Default</button>

        </li>

);
}

ReactDOM.render(<TrendingDataDisplay />, document.getElementById('bodyContainer'));