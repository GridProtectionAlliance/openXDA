//******************************************************************************************************
//  DateTimeRangePicker.tsx - Gbtc
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
//  07/23/2018 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

import * as React from 'react';
import * as ReactDOM from 'react-dom';
import * as _ from "lodash";
import * as DateTime from "react-datetime";
import * as moment from 'moment';
import 'react-datetime/css/react-datetime.css';

export default class DateTimeRangePicker extends React.Component<any, any>{
    props: { startDate: string, endDate: string, stateSetter: Function }
    state: { startDate: moment.Moment, endDate: moment.Moment }
    stateSetterId: any;
    constructor(props) {
        super(props);
        this.state = {
            startDate: moment(this.props.startDate),
            endDate: moment(this.props.endDate)
        };
    }
    render() {
        return (
            <div className="container" style={{width: 'inherit'}}>
                <div className="row">
                    <div className="form-group">
                        <DateTime
                            isValidDate={(date) => { return date.isBefore(this.state.endDate) }}
                            value={this.state.startDate}
                            timeFormat="HH:mm"
                            onChange={(value) => this.setState({ startDate: value }, () => this.stateSetter())} />
                    </div>
                </div>
                <div className="row">
                    <div className="form-group">
                        <DateTime
                            isValidDate={(date) => { return date.isAfter(this.state.startDate) }}
                            value={this.state.endDate}
                            timeFormat="HH:mm"
                            onChange={(value) => this.setState({ endDate: value }, () => this.stateSetter())} />
                    </div>
                </div>
            </div>
        );
    }

    componentWillReceiveProps(nextProps, nextContent) {
        if (nextProps.startDate != this.state.startDate.format('YYYY-MM-DDTHH:mm') || nextProps.endDate != this.state.endDate.format('YYYY-MM-DDTHH:mm'))
            this.setState({ startDate: moment(this.props.startDate), endDate: moment(this.props.endDate)});
    }

    stateSetter() {
        clearTimeout(this.stateSetterId);
        this.stateSetterId = setTimeout(() => {
            this.props.stateSetter({ startDate: this.state.startDate.format('YYYY-MM-DDTHH:mm'), endDate: this.state.endDate.format('YYYY-MM-DDTHH:mm') });
        }, 500);
    }
}


