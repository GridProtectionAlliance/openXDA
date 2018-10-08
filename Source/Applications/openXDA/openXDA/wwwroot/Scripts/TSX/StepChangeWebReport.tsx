//******************************************************************************************************
//  StepChangeWebReport.tsx - Gbtc
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
import StepChangeWebReportService from "./../TS/Services/StepChangeWebReport";
import BigTable from "./BigTable";
import Select from "./Select";
import 'react-datetime/css/react-datetime.css';
import * as DateTime from "react-datetime";

declare var stepChangeMeasurements: JSON;

export class StepChangeWebReport extends React.Component<any, any>{
    history: object;
    stepChangeWebReportService: StepChangeWebReportService;
    resizeId: any;
    props: {};
    state: { startDate: string, endDate: string, data: Array<any>, sortField: string, ascending: boolean };
    cols: Array<object>;
    options: object;
    constructor(props) {
        super(props);

        this.history = createHistory();
        this.stepChangeWebReportService = new StepChangeWebReportService();

        var query = queryString.parse(this.history['location'].search);

        this.state = {
            startDate: (query['startDate'] != undefined ? query['startDate'] : moment().format('YYYY-MM-DD')),
            endDate: (query['endDate'] != undefined ? query['endDate'] : moment().format('YYYY-MM-DD')),
            data: [],
            sortField: (query['sortField'] != undefined ? query['sortField'] : "Name"),
            ascending: (query['ascending'] != undefined ? query['ascending'] == "true" : true)
        }

        this.history['listen']((location, action) => {
            var query = queryString.parse(this.history['location'].search);
            this.setState({
                startDate: (query['startDate'] != undefined ? query['startDate'] : moment().format('YYYY-MM-DD')),
                endDate: (query['endDate'] != undefined ? query['endDate'] : moment().format('YYYY-MM-DD')),
                sortField: (query['sortField'] != undefined ? query['sortField'] : "Name"),
                ascending: (query['ascending'] != undefined ? query['ascending'] == "true" : true)
            },this.getData);
        });

        this.cols = [];
        this.cols.push({ key: "Name", label: "Meter", headerStyle: { minWidth: '150px' }, rowStyle: { minWidth: '150px' } });
        _.each(stepChangeMeasurements, (measurement, index) => {
            this.cols.push({
                key: measurement.Name,
                label: measurement.Name,
                headerStyle: { minWidth: '200px' },
                rowStyle: { minWidth: '200px', textAlign: 'center' },
                content: (value, style) => {
                    if (value == 0)
                        return value;
                    else if (value < 10)
                        style.backgroundColor = 'yellow';
                    else                    
                        style.backgroundColor = 'red';

                    return value;
                }
            });
        });
    }

    componentDidMount() {
        this.updateUrl();
    }

    render() {
        var height = window.innerHeight - 60;

        return (
            <div className="screen" style={{ height: height, width: window.innerWidth, position: 'absolute', top: '60px' }}>
                <div className="vertical-menu">
                    <div className="form-group">
                        <label>Start Date: </label>
                        <DateTime
                            closeOnSelect={true}
                            isValidDate={(date) => { return date.isBefore(moment()) }}
                            value={moment(this.state.startDate)}
                            timeFormat={false}
                            dateFormat="MM/DD/YYYY"
                            onChange={(value) => this.setState({ startDate: (value as moment.Moment).format("YYYY-MM-DD") }, this.updateUrl)} />
                    </div>
                    <div className="form-group">
                        <label>End Date: </label>
                        <DateTime
                            closeOnSelect={true}
                            isValidDate={(date) => { return date.isBefore(moment()) }}
                            value={moment(this.state.endDate)}
                            timeFormat={false}
                            dateFormat="MM/DD/YYYY"
                            onChange={(value) => this.setState({ endDate: (value as moment.Moment).format("YYYY-MM-DD") }, this.updateUrl)} />
                    </div>

                    <div className="form-group">
                        <div style={{ float: 'left' }} ref={'loader'} hidden>
                            <div style={{ border: '5px solid #f3f3f3', WebkitAnimation: 'spin 1s linear infinite', animation: 'spin 1s linear infinite', borderTop: '5px solid #555', borderRadius: '50%', width: '25px', height: '25px' }}></div>
                            <span>Loading...</span>
                        </div>
                    </div>


                </div>

                <div className="waveform-viewer" style={{ width: window.innerWidth - 250, float: 'right',  maxHeight: height, overflowY: 'auto' }}>
                    <BigTable
                        theadStyle={{ position: 'absolute', overflowY: 'scroll', maxHeight: '50px' }}
                        tbodyStyle={{ top: '50px', position: 'absolute', overflowY: 'scroll', maxHeight: height - 60 }}
                        cols={this.cols}
                        data={this.state.data}
                        sortField={this.state.sortField}
                        ascending={this.state.ascending}
                        onClick={this.handleTableClick.bind(this)}
                        onSort={this.handleTableSort.bind(this)}
                    />
                </div>
            </div>
        );
    }

    handleTableClick(data) {
        if (data.col == "Name") {
            window.open(`PeriodicDataDisplay.cshtml?meterID=${data.row.MeterID}&startDate=${moment(this.state.startDate).startOf("day").format('YYYY-MM-DDTHH:mm')}&endDate=${moment(this.state.endDate).endOf("day").format('YYYY-MM-DDTHH:mm')}&fromStepChangeWebReport=true`);
        }
        else {
            this.stepChangeWebReportService.getChannel(data.row.MeterID, data.col).done(channel => {
                if (channel != null)
                    window.open(`TrendingDataDisplay.cshtml?meterID=${data.row.MeterID}&measurementID=${channel.ID}&startDate=${moment(this.state.startDate).startOf("day").format('YYYY-MM-DDTHH:mm')}&endDate=${moment(this.state.endDate).endOf("day").format('YYYY-MM-DDTHH:mm')}`);
            });
        }
    }

    handleTableSort(data) {
        var ascending = data.ascending;
        if (data.col == this.state.sortField)
            ascending = !data.ascending

        this.setState({
            ascending: ascending,
            sortField: data.col
        }, this.updateUrl);

    }

    getData() {
        $(this.refs.loader).show();

        this.stepChangeWebReportService.getData(this.state.startDate, this.state.endDate, this.state.sortField, this.state.ascending).done(data => {
            this.setState({ data: data }, () => {
                $(this.refs.loader).hide();
             });
        });
    }

    updateUrl() {
        var state = _.clone(this.state);
        delete state.data;

        this.history['push']('StepChangeWebReport.cshtml?' + queryString.stringify(state, { encode: false }));
    }


}


ReactDOM.render(<StepChangeWebReport />, document.getElementById('bodyContainer'));