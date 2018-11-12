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
import PQTrendingWebReportService from "./../TS/Services/PQTrendingWebReport";
import BigTable from "./BigTable";
import Select from "./Select";
import 'react-datetime/css/react-datetime.css';
import * as DateTime from "react-datetime";
import { Modal } from 'react-bootstrap';
import { Button } from 'react-bootstrap';
import './../flot/jquery.flot.min.js';
import './../flot/jquery.flot.time.min.js';

declare var pqMeasurements: JSON;

export class PQTrendingWebReport extends React.Component<any, any>{
    history: object;
    pqTrendingWebReportService: PQTrendingWebReportService;
    resizeId: any;
    props: {};
    state: { date: string, stat: string, data: Array<any>, sortField: string, ascending: boolean, listModalData: any };
    cols: Array<object>;
    options: object;
    constructor(props) {
        super(props);

        this.history = createHistory();
        this.pqTrendingWebReportService = new PQTrendingWebReportService();

        var query = queryString.parse(this.history['location'].search);

        this.state = {
            date: (query['date'] != undefined ? query['date'] : moment().format('YYYY-MM-DD')),
            stat: (query['stat'] != undefined ? query['stat'] : "Avg"),
            data: [],
            sortField: (query['sortField'] != undefined ? query['sortField'] : "Name"),
            ascending: (query['ascending'] != undefined ? query['ascending'] == "true" : true),
            listModalData: false
        }

        this.history['listen']((location, action) => {
            var query = queryString.parse(this.history['location'].search);
            this.setState({
                date: (query['date'] != undefined ? query['date'] : moment().format('YYYY-MM-DD')),
                stat: (query['stat'] != undefined ? query['stat'] : "Avg"),
                sortField: (query['sortField'] != undefined ? query['sortField'] : "Name"),
                ascending: (query['ascending'] != undefined ? query['ascending'] == "true" : true)
            },this.getData);
        });

        this.cols = [];
        this.cols.push({ key: "Name", label: "Meter", headerStyle: { minWidth: '150px' }, rowStyle: { minWidth: '150px' } });
        _.each(pqMeasurements, (measurement, index) => {
            this.cols.push({ key: measurement.Name, label: measurement.Name, headerStyle: { minWidth: '200px' }, rowStyle: { minWidth: '200px'} });
        });

        this.options = {
            legend: { show: false },
            xaxis: {
                mode: "time",
                reserveSpace: false
            },
            yaxis: {
                position: 'left',
                tickLength: 30
            }
        }

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
                        <label>Statistic: </label>
                        <Select value={this.state.stat} options={["Max", "CP99", "CP95", "Avg", "CP05", "CP01", "Min"]} onChange={(obj) => this.setState({ stat: obj }, this.updateUrl)} />
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
                        theadStyle={{ position: 'absolute', overflowY: 'scroll', maxHeight: '50px'}}
                        tbodyStyle={{ top: '50px', position: 'absolute', overflowY: 'scroll', maxHeight: height-60}}
                        cols={this.cols}
                        data={this.state.data}
                        sortField={this.state.sortField}
                        ascending={this.state.ascending}
                        onClick={this.handleTableClick.bind(this)}
                        onSort={this.handleTableSort.bind(this)}
                    />
                </div>
                <div ref={"listModal"} className="static-modal" style={{display: 'none'}}>
                    <Modal.Dialog>
                        <Modal.Header>
                            <button type="button" className="close" onClick={() => { $(this.refs.listModal).hide() }}>&times;</button>
                            <Modal.Title>{this.state.listModalData.Name}</Modal.Title>
                        </Modal.Header>

                        <div className="modal-body" style={{ overflowY: 'auto', maxHeight: height/2}}>
                            {this.state.listModalData.Table}
                        </div>

                        <Modal.Footer>
                            <Button onClick={() => { $(this.refs.listModal).hide() }}>Close</Button>
                        </Modal.Footer>
                    </Modal.Dialog>
                </div>
                <div ref={"graphModal"} className="static-modal" style={{ display: 'none' }}>
                    <Modal.Dialog>
                        <Modal.Header>
                            <button type="button" className="close" onClick={() => { $(this.refs.graphModal).hide() }}>&times;</button>
                            <Modal.Title><span ref={"graphTitle"}></span></Modal.Title>
                        </Modal.Header>

                        <div className="modal-body">
                            <div ref={'graph'} style={{ height: '250px', width: '500px' }}></div>
                        </div>

                        <Modal.Footer>
                            <Button onClick={() => { $(this.refs.graphModal).hide() }}>Close</Button>
                        </Modal.Footer>
                    </Modal.Dialog>
                </div>
            </div>
        );
    }

    handleTableClick(data) {
        if (data.col == "Name") {
            var rows = Object.keys(data.row).map(key => <tr key={key}><td>{key}</td><td>{data.row[key]}</td></tr>)
            var listModalData = {
                Name: data.data,
                Table: <table className="table" style={{ maxHeight: '350px', overflowY: 'auto' }}><tbody>{rows}</tbody></table>
            }
            this.setState({ listModalData: listModalData }, () => $(this.refs.listModal).show())

        }
        else {
            this.pqTrendingWebReportService.getChart(this.state.date, this.state.stat,data.row.Name,data.col).done(chartData => {
                ($ as any).plot($(this.refs.graph), [chartData.map(d => [moment(d.Date), d.Value])], this.options);
                $(this.refs.graphTitle).text(data.row.Name + ' - ' + data.col);
                $(this.refs.graphModal).show()
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
        this.setState({ data: null });
        this.pqTrendingWebReportService.getData(this.state.date, this.state.stat, this.state.sortField, this.state.ascending).done(data => {
            this.setState({ data: data }, () => {
                $(this.refs.loader).hide();
             });
        });
    }

    updateUrl() {
        var state = _.clone(this.state);
        delete state.data;
        delete state.listModalData;

        this.history['push']('PQTrendingWebReport.cshtml?' + queryString.stringify(state, { encode: false }));
    }


}


ReactDOM.render(<PQTrendingWebReport />, document.getElementById('bodyContainer'));