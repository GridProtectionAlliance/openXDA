//******************************************************************************************************
//  ByMeter.tsx - Gbtc
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
//  08/22/2019 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

import * as React from 'react';
import * as ReactDOM from 'react-dom';

declare var homePath: string;
declare var controllerViewPath: string;

export default class ByMeter extends React.Component<{}, {}, {}>{
    constructor(props, context) {
        super(props, context);
    }

    render() {
        var windowHeight = window.innerHeight;

        return (
            <div style={{ width: '100%', height: '100%' }}>
                <div style={{ width: '100%', height: 'calc( 100% - 210px)' }}>
                By Meter
                    <div style={{ width: '50%', height: '100%', maxHeight: '100%', position: 'relative', float: 'left', overflowY: 'hidden' }}>
                        <div style={{ width: 'calc(100% - 120px)', padding: 10, float: 'left' }}>
                        </div>
                        <div style={{ width: 120, float: 'right', padding: 10 }}>
                        </div>
                    </div>
                    <div style={{ width: '50%', height: '100%', maxHeight: '100%', position: 'relative', float: 'right', overflowY: 'scroll' }}>
                    </div>

                </div>
            </div>
        )
    }
}

