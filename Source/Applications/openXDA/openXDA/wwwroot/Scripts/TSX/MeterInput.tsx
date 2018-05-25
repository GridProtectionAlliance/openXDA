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
import PeriodicDataDisplay1Service from './../TS/Services/PeriodicDataDisplay1';
import createHistory from "history/createBrowserHistory"
import * as queryString from "query-string";
import * as moment from 'moment';
import * as _ from "lodash";

export default class MeterInput extends React.Component<any, any>{
    periodicDataDisplay1Service: PeriodicDataDisplay1Service;
    constructor(props) {
        super(props);
        this.state = {
            options: []
        }

        this.periodicDataDisplay1Service = new PeriodicDataDisplay1Service();
    }

    componentDidMount() {
        this.periodicDataDisplay1Service.getMeters().done(data => {
            var options = data.map(d => <option value={d.ID}>{d.AssetKey}</option>)
            this.setState({ options: options });
        });
    }

    render() {
        return (
            <select>{this.state.options}</select>
        );
    }

}