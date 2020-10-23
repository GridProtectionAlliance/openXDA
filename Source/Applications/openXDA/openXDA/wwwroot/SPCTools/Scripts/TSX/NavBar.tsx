//******************************************************************************************************
//  NavBar.tsx - Gbtc
//
//  Copyright © 2020, Grid Protection Alliance.  All Rights Reserved.
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
//  10/20/2020 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { SPCTools } from './global';

declare var homePath: string;
declare var userIsAdmin: boolean;

interface IProps { page: SPCTools.Page, pageSetter: (page: SPCTools.Page) => void }

const NavBar = (props: IProps ) => {
    
    return (
        <nav className="navbar navbar-expand-lg navbar-dark bg-dark">
            <a className="navbar-brand" href="#">
                <img className="pull-left" style={{ padding: "0px", height: '40px' }} alt="PQ SPC Limits Configurator " src="/Images/SPCTools.png" />
            </a>

            <div className="navbar-collapse collapse">
                <ul className="navbar-nav mr-auto">
                    <li className={'nav-item' + (props.page == 'Home' ? ' active' : '')} > <a className='nav-link' onClick={() => props.pageSetter('Home')}>Home</a></li>
                    <li className={"nav-item dropdown" + (props.page == 'Dynamic' || props.page == 'Static' ? ' active' : '')}>
                        <a className="nav-link dropdown-toggle" data-toggle="dropdown" href="#">New Alarm Group <span className="caret"></span> </a>
                        <div className="dropdown-menu">
                            <a className="dropdown-item" onClick={() => props.pageSetter('Static')}>New Static Alarm Group</a>
                            <a className="dropdown-item" onClick={() => props.pageSetter('Dynamic')}>New Dynamic Alarm Group</a>
                        </div>
                    </li>
                    <li className={'nav-item' + (props.page == 'Meter' ? ' active' : '')}> <a className='nav-link' onClick={() => props.pageSetter('Meter')}>Meter Overview</a></li>
                    <li className={'nav-item' + (props.page == 'Channel' ? ' active' : '')}> <a className='nav-link' onClick={() => props.pageSetter('Channel')}>Channel Overview</a></li>
                       
                </ul>
                <p className="nav navbar-text navbar-right">
                    <button id="logoutButton" type="button" className="btn btn-sm btn-info">Log Out</button>
                    <a href="https://www.gridprotectionalliance.org/" target="_blank" style={{ paddingLeft: '10px' }}>
                        <img alt="Grid Protection Alliance" src="/Images/gpa-smalllock.png"/>
                    </a>
                </p>
            </div>
        </nav>
    );
}

export default NavBar;