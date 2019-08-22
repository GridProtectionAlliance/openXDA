//******************************************************************************************************
//  SystemCenter.tsx - Gbtc
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
import { BrowserRouter as Router, Route, NavLink } from 'react-router-dom';
import ByMeter from './ByMeter';
import ByLocation from './ByLocation';

declare var homePath: string;
declare var controllerViewPath: string;

class SystemCenter extends React.Component<{}, {}, {}>{
    constructor(props, context) {
        super(props, context);
    }

    render() {
        var windowHeight = window.innerHeight;

        return (
            <Router>
                <div style={{ position: 'absolute', width: '100%', height: '100%', overflow: 'hidden' }}>
                    <div style={{ width: 300, height: 'inherit', backgroundColor: '#eeeeee', position: 'relative', float: 'left' }}>
                        <a href="https://www.gridprotectionalliance.org"><img style={{ width: 280, margin: 10 }} src={"../Images/2-Line - 500.png"} /></a>
                        <div style={{ width: '100%', marginTop: 5, textAlign: 'center' }}><h3>System Center</h3></div>
                        <div style={{ width: '100%', height: '100%', marginTop: 30 }}>
                            <div className="nav flex-column nav-pills" id="v-pills-tab" role="tablist" aria-orientation="vertical" style={{ height: 'calc(100% - 240px)' }}>
                                <NavLink activeClassName='nav-link active' className="nav-link" isActive={(match, location) => location.pathname + location.search == controllerViewPath || location.pathname + location.search == controllerViewPath + "?name=Meter"} to={controllerViewPath + "?name=Meter" }>By Meter</NavLink>
                                <NavLink activeClassName='nav-link active' className="nav-link" isActive={(match, location) => location.pathname + location.search == controllerViewPath + "?name=MeterLocation" } to={ controllerViewPath + "?name=MeterLocation"}>By Location</NavLink>
                            </div>
                            <div style={{ width: '100%', textAlign: 'center' }}>

                                <span>Version 1.0</span>
                                <br />
                                <span></span>
                            </div>
                        </div>
                    </div>
                    <div style={{ width: 'calc(100% - 300px)', height: 'inherit', position: 'relative', float: 'right' }}>
                        <Route children={({ match, ...rest }) => {
                            if (rest.location.pathname + rest.location.search == controllerViewPath || rest.location.pathname + rest.location.search == controllerViewPath + "?name=Meter")
                                return <ByMeter />
                            else
                                return null;
                        }} />
                        <Route children={({ match, ...rest }) => {
                            if (rest.location.pathname + rest.location.search == controllerViewPath + "?name=MeterLocation")
                                return <ByLocation />
                            else
                                return null;
                        }} />
                    </div>
                </div>
            </Router>
        )
    }
}

ReactDOM.render(<SystemCenter />, document.getElementById('window'));
