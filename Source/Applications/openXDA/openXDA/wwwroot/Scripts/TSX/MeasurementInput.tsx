﻿//******************************************************************************************************
//  MeasurementInput.tsx - Gbtc
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
import TrendingDataDisplayService from './../TS/Services/TrendingDataDisplay';
import * as _ from "lodash";

export default class MeasurementInput extends React.Component<{ value: number, meterID: number, onChange: Function }, { options: any[] }>{
    trendingDataDisplayService: TrendingDataDisplayService;
    constructor(props) {
        super(props);
        this.state = {
            options: []
        }

        this.trendingDataDisplayService = new TrendingDataDisplayService();
    }

    componentWillReceiveProps(nextProps) {
        if(this.props.meterID != nextProps.meterID)
            this.getData(nextProps.meterID);
    }

    componentDidMount() {
        this.getData(this.props.meterID);
    }

    getData(meterID) {
        this.trendingDataDisplayService.getMeasurements(meterID).done(data => {
            if (data.length == 0) return;

            var value = (this.props.value ? this.props.value : data[0].ID)
            var options = data.map(d => <option key={d.ID} value={d.ID}>{d.Name}</option>);
            this.setState({ options });
        });

    }

    render() {
        return (<select className='form-control' onChange={(e) => { this.props.onChange({ measurementID: parseInt(e.target.value), measurementName: e.target.selectedOptions[0].text }); }} value={this.props.value}>
            <option value='0'></option>
            {this.state.options}
        </select>);
    }

}