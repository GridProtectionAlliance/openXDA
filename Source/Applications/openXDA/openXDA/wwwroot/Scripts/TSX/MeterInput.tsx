//******************************************************************************************************
//  MeterInput.tsx - Gbtc
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

declare var meters: Array<{ ID: number, Name: string }>;

export default class MeterInput extends React.Component<any, any>{
    periodicDataDisplayService: PeriodicDataDisplayService;
    props: { value: number, onChange: Function };
    state: { select: JSX.Element }
    constructor(props) {
        super(props);

        if (meters == undefined) {
            this.state = {
                select: null
            }
        }
        else {
            this.state = {
                select: this.getSelect(meters)
            }
        }

        this.periodicDataDisplayService = new PeriodicDataDisplayService();
    }

    componentDidMount() {
        if (meters != undefined) return;

        this.periodicDataDisplayService.getMeters().done(data => {
            var select = this.getSelect(data);
            this.setState({ select: select });
        });
    }

    render() {
        return this.state.select;
    }

    getSelect(data) {
        if (data.length == 0) return <select className='form-control'></select>;

        var value = (this.props.value ? this.props.value : data[0].ID)
        var options = data.map(d => <option key={d.ID} value={d.ID}>{d.Name}</option>);
        this.props.onChange({ meterID: value });
        return <select className='form-control' onChange={(e) => { this.props.onChange({ meterID: e.target.value, measurementID: null }); }} defaultValue={value}>{options}</select>;
    }

}