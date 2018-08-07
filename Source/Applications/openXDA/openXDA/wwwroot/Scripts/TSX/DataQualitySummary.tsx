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
import DataQualitySummaryService from "./../TS/Services/DataQualitySummary";
import Table from "./Table";
import MeterInput from './MeterInput';
import 'react-datetime/css/react-datetime.css';
import * as DateTime from "react-datetime";

export class DataQualitySummary extends React.Component<any, any>{
    history: object;
    dataQualitySummaryService: DataQualitySummaryService;
    resizeId: any;
    props: {};
    state: {date: string, meterID: number, level: string, data: Array<any>, sortField: string, ascending: boolean};
    constructor(props) {
        super(props);

        this.history = createHistory();
        this.dataQualitySummaryService = new DataQualitySummaryService();

        var query = queryString.parse(this.history['location'].search);

        this.state = {
            meterID: (query['meterID'] != undefined ? query['meterID'] : 0),
            date: (query['date'] != undefined ? query['date'] : moment().format('YYYY-MM-DD')),
            level: (query['level'] != undefined ? query['level'] : "Meter"),
            data: [],
            sortField: (query['sortField'] != undefined ? query['sortField'] : "Name"),
            ascending: (query['ascending'] != undefined ? query['ascending'] == "true" : true)
        }

        this.history['listen']((location, action) => {
            var query = queryString.parse(this.history['location'].search);
            this.setState({
                meterID: (query['meterID'] != undefined ? query['meterID'] : 0),
                date: (query['date'] != undefined ? query['date'] : moment().format('YYYY-MM-DD')),
                level: (query['level'] != undefined ? query['level'] : "Meter"),
                sortField: (query['sortField'] != undefined ? query['sortField'] : "Name"),
                ascending: (query['ascending'] != undefined ? query['ascending'] == "true" : true)
            },this.getData);
        });


    }

    componentDidMount() {
        if (this.state.level == "Meter")
            this.updateUrl();
    }

    render() {
        var height = window.innerHeight - 60;

        return (
            <div className="screen" style={{ height: height, width: window.innerWidth, position: 'absolute', top: '60px' }}>
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
                        <label>Data Level: </label>
                        <select
                            onChange={(obj) => {
                                this.setState({ level: obj.target.value }, this.updateUrl);
                            }}
                            className="form-control"
                            defaultValue={this.state.level}>
                            <option value="Meter">Meter</option>
                            <option value="Channel">Channel</option>
                        </select>
                    </div>
                    {( this.state.level == "Channel" ?
                        <div className="form-group">
                            <label>Meter: </label>
                            <MeterInput value={this.state.meterID} onChange={(obj) => this.setState({ meterID: obj }, this.updateUrl)} />
                        </div> : null
                    )}
                    <div className="form-group">
                        <div style={{ float: 'left' }} ref={'loader'} hidden>
                            <div style={{ border: '5px solid #f3f3f3', WebkitAnimation: 'spin 1s linear infinite', animation: 'spin 1s linear infinite', borderTop: '5px solid #555', borderRadius: '50%', width: '25px', height: '25px' }}></div>
                            <span>Loading...</span>
                        </div>

                        {//<button className='btn btn-primary' style={{ float: 'right' }} onClick={this.updateUrl}>Apply</button>
                        }
                    </div>


                </div>

                <div className="waveform-viewer" style={{ width: window.innerWidth }}>
                    <div className="list-group" style={{ maxHeight: height, overflowY: 'auto' }}>
                        <Table
                            cols={
                                [{ key: "Name", label: (this.state.level == "Meter" ? "Meter Name" : "Channel Name"), headerStyle: {width: '20%'} },
                                 { key: "Expected", label: "Expected", headerStyle: { width: '20%' } },
                                 { key: "Missing", label: "Missing", headerStyle: { width: '20%' } },
                                 { key: "Latched", label: "Latched", headerStyle: { width: '20%' } },
                                 { key: "Unreasonable", label: "Unreasonable", headerStyle: { width: '20%' } }]}
                            data={this.state.data}
                            sortField={this.state.sortField}
                            ascending={this.state.ascending}
                            onClick={this.handleTableClick.bind(this)}
                            onSort={this.handleTableSort.bind(this)}
                        />
                    </div>
                </div>
            </div>
        );
    }

    handleTableClick(data) {
        if (this.state.level == "Channel")
            window.open(`../OpenSTE.cshtml?ChannelID=${data.row.ID}&date=${this.state.date}`);
        else {
            this.setState({
                level: "Channel",
                meterID: data.row.ID,
                ascending: (data.col == "Name"),
                sortField: data.col
            }, this.updateUrl);
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

        this.dataQualitySummaryService.getData(this.state.meterID,this.state.date, this.state.level, this.state.sortField, this.state.ascending).done(data => {
            this.setState({ data: data }, () => {
                $(this.refs.loader).hide();
             });
        });
    }

    updateUrl() {
        var state = _.clone(this.state);
        delete state.data;

        this.history['push']('DataQualitySummary.cshtml?' + queryString.stringify(state, { encode: false }));
        this.getData();
    }


}

ReactDOM.render(<DataQualitySummary />, document.getElementById('bodyContainer'));