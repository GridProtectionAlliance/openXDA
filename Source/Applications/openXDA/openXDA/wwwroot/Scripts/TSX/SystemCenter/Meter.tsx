//******************************************************************************************************
//  Meter.tsx - Gbtc
//
//  Copyright © 2019, Grid Protection Alliance.  All Rights Reserved.
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
//  08/27/2019 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

import * as React from 'react';
import * as _ from 'lodash';

declare var homePath: string;

export default class Meter extends React.Component<{ meterId: number}, { }, {}>{
    jqueryHandle: JQuery.jqXHR;
    constructor(props, context) {
        super(props, context);

        this.state = {
        }
    }

    getMeters(): JQuery.jqXHR {
        if (this.jqueryHandle !== undefined)
            this.jqueryHandle.abort();

        this.jqueryHandle = $.ajax({
            type: "GET",
            url: `${homePath}api/SystemCenter/Meters`,
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            cache: true,
            async: true
        });

        return this.jqueryHandle;
    }

    componentDidMount() {
        //this.getMeters().done(data => this.setState({ data: data }));
    }

    render() {
        var windowHeight = window.innerHeight;

        return (
            <div style={{ width: '100%', height: '100%' }}>
                {this.props.meterId}
            </div>
        )
    }
}

