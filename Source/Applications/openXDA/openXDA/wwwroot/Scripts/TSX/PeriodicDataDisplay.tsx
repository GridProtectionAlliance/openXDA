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
import { createBrowserHistory as createHistory, History} from "history"
import * as queryString from "query-string";
import * as moment from 'moment';
import * as _ from "lodash";
import MeterInput from './MeterInput';
import DatetimeRangePicker from 'react-datetime-range-picker';
import Measurement from './Measurement';
import { PeriodicDataDisplay as PDD, PeriodicDataDisplay} from './global'

type XDATrendDataPointField = 'Minimum' | 'Maximum' | 'Average';

const PeriodicDataDisplay = () => {
    let history = createHistory();
    let periodicDataDisplayService = new PeriodicDataDisplayService();
    const query = queryString.parse(history['location'].search);
    const [meterID, setMeterID] = React.useState<number>((query['meterID'] != undefined ? query['meterID'] : 0));
    const [startDate, setStartDate] = React.useState<string>((query['startDate'] != undefined ? query['startDate'] : moment().subtract(7, 'day').format('YYYY-MM-DD')));
    const [endDate, setEndDate] = React.useState<string>((query['endDate'] != undefined ? query['endDate'] : moment().format('YYYY-MM-DD')));
    const [type, setType] = React.useState<XDATrendDataPointField>((query['type'] != undefined ? query['type'] : "Average"));
    const [detailedReport, setDetailedReport] = React.useState<boolean>(query['detailedReport'] != undefined ? query['detailedReport'] == "true" : false);
    const [width, setWidth] = React.useState<number>(window.innerWidth - 475);
    const [data, setData] = React.useState<PeriodicDataDisplay.MeasurementCharateristics[]>([]);
    const [measurementsReturned, setMeasurementsReturned] = React.useState<number>(0);
    let resizeID = React.useRef(0);
    let loader = React.useRef(null);

    React.useEffect(() => {
        window.addEventListener("resize", handleScreenSizeChange.bind(this));
        if (meterID != 0) getData();

    }, [meterID, startDate, endDate, type, detailedReport]);

    React.useEffect(() => {
        window.addEventListener("resize", handleScreenSizeChange.bind(this));
        return () => $(window).off('resize');
    }, []);


    function getData() {
        $(loader).show();

        periodicDataDisplayService.getMeasurementCharacteristics(meterID).done(data => {
            setData(data);
        });
    }

    function returnedMeasurement() {
        if (data.length == measurementsReturned + 1) $(loader.current).hide();
        setMeasurementsReturned(measurementsReturned + 1);

    }

    function handleScreenSizeChange() {
        clearTimeout(resizeID.current);
        resizeID.current = setTimeout(() => {
            setWidth(window.innerWidth - 475)
        }, 100);
    }

    function updateUrl() {
        history['push']('PeriodicDataDisplay.cshtml?' + queryString.stringify({meterID, startDate, endDate, type, detailedReport}, { encode: false }))
    }

    let height = window.innerHeight - 60;

    return (
        <div>
            <div className="screen" style={{ height: height, width: window.innerWidth }}>
                <div className="vertical-menu">
                    <div className="form-group">
                        <label>Meter: </label>
                        <MeterInput value={meterID} onChange={(obj) => setMeterID(parseInt(obj.meterID.toString()))} />
                    </div>
                    <div className="form-group">
                        <label>Time Range: </label>
                        <DatetimeRangePicker
                            startDate={new Date(startDate)}
                            endDate={new Date(endDate)}
                            onChange={(obj) => {
                                setStartDate(moment(obj.start).format('YYYY-MM-DD'));
                                setEndDate(moment(obj.end).format('YYYY-MM-DD'));
                            }}
                            inputProps={{ style: { width: '100px', margin: '5px' }, className: 'form-control' }}
                            className='form'
                            timeFormat={false}
                        />
                    </div>
                    <div className="form-group">
                        <label>Data Type: </label>
                        <select onChange={(obj) => setType(obj.target.value as XDATrendDataPointField)} className="form-control" value={type}>
                            <option value="Average">Average</option>
                            <option value="Maximum">Maximum</option>
                            <option value="Minimum">Minimum</option>
                        </select>
                    </div>
                    <div className="form-group">
                        <label>Detailed Report: <input type="checkbox" value={detailedReport.toString()} defaultChecked={detailedReport} onChange={(e) => setDetailedReport(!detailedReport) } /></label>
                    </div>

                    <div className="form-group">
                        <div style={{ float: 'left' }} ref={loader} hidden>
                            <div style={{ border: '5px solid #f3f3f3', WebkitAnimation: 'spin 1s linear infinite', animation: 'spin 1s linear infinite', borderTop: '5px solid #555', borderRadius: '50%', width: '25px', height: '25px' }}></div>
                            <span>Loading...</span>
                        </div>

                        <button className='btn btn-primary' style={{ float: 'right' }} onClick={() => { updateUrl(); getData(); }}>Apply</button>
                    </div>


                </div>
                <div className="waveform-viewer" style={{ width: window.innerWidth }}>
                    <div className="list-group" style={{ maxHeight: height, overflowY: 'auto' }}>
                        {data.map(d =>
                            <Measurement
                                meterID={meterID}
                                startDate={startDate}
                                endDate={endDate}
                                key={d.MeasurementType + d.MeasurementCharacteristic + d.HarmonicGroup}
                                data={d}
                                type={type}
                                height={300}
                                detailedReport={detailedReport}
                                width={width}
                                returnedMeasurement={returnedMeasurement.bind(this)}
                            />)}
                    </div>
                </div>
            </div>
        </div>
    );
}


ReactDOM.render(<PeriodicDataDisplay />, document.getElementById('bodyContainer'));