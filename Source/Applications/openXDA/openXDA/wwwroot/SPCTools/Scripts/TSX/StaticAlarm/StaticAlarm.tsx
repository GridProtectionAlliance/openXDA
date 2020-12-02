//******************************************************************************************************
//  StaticAlarm.tsx - Gbtc
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
import GeneralSettings from './GenSettings';
import * as ReactDOM from 'react-dom';
import SelectHistoricData from './SelectHistory';
import {selectStatus, next, back, SelectTab, selectErrors} from './StaticWizzardSlice'
import { useSelector, useDispatch } from 'react-redux';
import StaticSetPoint from './SetPoint';
import TestGroup from './Test';
import { SPCTools, StaticWizzard } from '../global';
import { error } from 'jquery';

declare var homePath: string;
declare var apiHomePath: string;

declare var userIsAdmin: boolean;

// To-Do
// # Fix Tesing Controller to run actual test (1 day)
// # Add Logic to Create Alarms at the end (2days)

const StaticAlarmHome: React.FunctionComponent = (props: {}) => {
    const loading = useSelector(selectStatus);
    const tab = useSelector(SelectTab);
    const dispatch = useDispatch();
    const errors = useSelector(selectErrors);

  
    React.useLayoutEffect(() => {
        ($('[data-toggle="tooltip"]') as any).tooltip({html: true});

        return () => {
            ($('[data-toggle="tooltip"]') as any).tooltip('dispose')
        }
    })

    

    return (
        <div style={{ width: '100%', height: '100%' }}>
            <div className="row" style={{ margin: 0 }}>
                <nav style={{ width: '100%' }}>
                    <ol className="breadcrumb">
                        <li className={"breadcrumb-item" + (tab == 'general' ? " active" : '')}>General settings</li>
                        <li className={"breadcrumb-item" + (tab == 'selectData' ? " active" : '')}>Select Historical Data</li>
                        <li className={"breadcrumb-item" + (tab == 'setpoint' ? " active" : '')}>Create Setpoint</li>
                        <li className={"breadcrumb-item" + (tab == 'test' ? " active" : '')}>Test Alarm</li>
                    </ol>
                </nav>
            </div>
            <div className="row" style={{ marginLeft: 0, marginRight: 0, marginTop: 0, marginBottom: '15px', height: "calc(100% - 132px)" }}>
                {loading == 'loading' ?
                    <div className="text-center" style={{width: '100%', margin: 'auto'}}>
                        <div className="spinner-border" role="status"></div>
                    </div> :
                    <>
                        {tab == 'general' ? <GeneralSettings /> : null}
                        {tab == 'selectData' ? <SelectHistoricData /> : null}
                        {tab == 'setpoint' ? <StaticSetPoint /> : null}
                        {tab == 'test' ? <TestGroup /> : null}
                    </>
                }
            </div>
            <div className="row" style={{ margin: 0 }}>
                <div className="col" style={{width: '100%'}}>
                    <div className="btn-group mr-2 float-right" role="group">
                        <button type="button" className={"btn btn-success" + (errors.some(item => (item.complete == 'required')) ? ' disabled' : '')}
                            onClick={() => { dispatch(next()); }} data-toggle="tooltip" data-placement="top" title={ReactDOMServer.renderToString(<>{errors.map(item => <Requirements {...item} />)}</>)}>
                            {loading == 'loading' ? <span className="spinner-border spinner-border-sm"></span> : (tab == "test" ? "Save" : "Continue")}
                        </button> 
                    </div>
                    <div className="btn-group mr-2 float-right" role="group">
                        <button type="button" className="btn btn-danger" disabled={tab == 'general'} onClick={() => dispatch(back())}>
                            {loading == 'loading' ? <span className="spinner-border spinner-border-sm"></span> : "Back"}
                        </button>
                    </div>
                </div>
            </div>
        </div>      
    );
}

const Requirements = (props: StaticWizzard.IRequirement) => {
    return <div>
        {props.complete == 'complete' ?
            <svg width="1em" height="1em" viewBox="0 0 16 16" className="bi bi-check-circle-fill" >
                <path fill-rule="evenodd" d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zm-3.97-3.03a.75.75 0 0 0-1.08.022L7.477 9.417 5.384 7.323a.75.75 0 0 0-1.06 1.06L6.97 11.03a.75.75 0 0 0 1.079-.02l3.992-4.99a.75.75 0 0 0-.01-1.05z" />
            </svg> :
            (props.complete == 'required' ?
                <svg width="1em" height="1em" viewBox="0 0 16 16" className="bi bi-exclamation-circle-fill">
                    <path fill-rule="evenodd" d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zM8 4a.905.905 0 0 0-.9.995l.35 3.507a.552.552 0 0 0 1.1 0l.35-3.507A.905.905 0 0 0 8 4zm.002 6a1 1 0 1 0 0 2 1 1 0 0 0 0-2z" />
                </svg>
                :
                <svg width="1.0625em" height="1em" viewBox="0 0 17 16" className="bi bi-exclamation-triangle">
                    <path fill-rule="evenodd" d="M7.938 2.016a.146.146 0 0 0-.054.057L1.027 13.74a.176.176 0 0 0-.002.183c.016.03.037.05.054.06.015.01.034.017.066.017h13.713a.12.12 0 0 0 .066-.017.163.163 0 0 0 .055-.06.176.176 0 0 0-.003-.183L8.12 2.073a.146.146 0 0 0-.054-.057A.13.13 0 0 0 8.002 2a.13.13 0 0 0-.064.016zm1.044-.45a1.13 1.13 0 0 0-1.96 0L.165 13.233c-.457.778.091 1.767.98 1.767h13.713c.889 0 1.438-.99.98-1.767L8.982 1.566z" />
                    <path d="M7.002 12a1 1 0 1 1 2 0 1 1 0 0 1-2 0zM7.1 5.995a.905.905 0 1 1 1.8 0l-.35 3.507a.552.552 0 0 1-1.1 0L7.1 5.995z" />
                </svg>)
        }
        <p>{props.text}</p> </div>
}

export default StaticAlarmHome