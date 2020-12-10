//******************************************************************************************************
//  DynamicAlarm.tsx - Gbtc
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
import GeneralSettings from './GeneralSettings';

import {selectStatus, next, back, selectTab, selectErrors, SaveWizard, selectWizardEror} from './DynamicWizzardSlice'
import { useSelector, useDispatch } from 'react-redux';

import ReactTooltip from 'react-tooltip';
import { setPage } from '../store/GeneralSettingsSlice';
import { Requirements } from '../CommonComponents/Requirments';
import SelectStatisticsData from './SelectStatisticsData';
import SetPointCreator from './SetPointCreator';
import { FunctionHelp } from '../CommonComponents/FunctionHelp';
import WizardTest from './WizardTest';

declare var homePath: string;
declare var apiHomePath: string;

declare var userIsAdmin: boolean;

const DynamicAlarmHome: React.FunctionComponent = (props: {}) => {
    const status = useSelector(selectStatus);
    const tab = useSelector(selectTab);
    const dispatch = useDispatch();
    const errors = useSelector(selectErrors);
    const wizardError = useSelector(selectWizardEror);

    function Continue() {
        
        if (!errors.some(item => (item.complete == 'required'))) {

            if (tab == 'test') {
                dispatch(SaveWizard());
                return;
            }
            
            dispatch(next());
        }
    
    }

    React.useEffect(() => { if (tab == 'done') dispatch(setPage("Home")); }, [tab])

    return (
        <div style={{ width: '100%', height: '100%' }}>
            <div className="row" style={{ margin: 0 }}>
                <nav style={{ width: '100%' }}>
                    <ol className="breadcrumb">
                        <li className={"breadcrumb-item" + (tab == 'general' ? " active" : '')}>General settings</li>
                        <li className={"breadcrumb-item" + (tab == 'selectData' ? " active" : '')}>Select Historical Data</li>
                        <li className={"breadcrumb-item" + (tab == 'setpoint' ? " active" : '')}>Create Setpoints</li>
                        <li className={"breadcrumb-item" + (tab == 'test' ? " active" : '')}>Test Alarm</li>
                    </ol>
                </nav>
            </div>
            <div className="row" style={{ marginLeft: 0, marginRight: 0, marginTop: 0, marginBottom: '15px', height: "calc(100% - 132px)", overflowY: 'scroll' }}>
                {status == 'loading' ?
                    <div className="text-center" style={{ width: '100%', margin: 'auto' }}>
                        <div className="spinner-border" role="status"></div>
                    </div> : null}
                {status == 'error' || status == 'unitiated' ?
                    <div className="text-center" style={{ width: '100%', margin: 'auto' }}>
                        <div className="alert alert-danger" role="alert">
                            <h4 className="alert-heading">An Error Occured</h4>
                            <hr/>
                            <p>{status == 'error'? wizardError: 'Wizard Failed to start.'}</p>
                            
                        </div>
                    </div>: null}
                {status == 'idle' ?
                    <>
                        {tab == 'general' ? <GeneralSettings /> : null}
                        {tab == 'selectData' ? <SelectStatisticsData /> : null}
                        {tab == 'setpoint' ? <SetPointCreator /> : null}
                        {tab == 'test' ? <WizardTest /> : null}
                    </> : null
                }
                
            </div>
            <div className="row" style={{ margin: 0 }}>
                <div className="col" style={{width: '100%'}}>
                    <div className="btn-group mr-2 float-right" role="group">
                        <button type="button" className={"btn btn-success" + (errors.some(item => (item.complete == 'required')) ? ' disabled' : '')}
                            onClick={() => Continue() } data-tip data-for="registerTip">
                            {status != 'idle' ? <span className="spinner-border spinner-border-sm"></span> : (tab == "test" ? "Save" : "Continue")}
                        </button> 
                    </div>
                    <div className="btn-group mr-2 float-right" role="group">
                        <button type="button" className="btn btn-danger" disabled={tab == 'general'} onClick={() => dispatch(back())}>
                            {status != 'idle' ? <span className="spinner-border spinner-border-sm"></span> : "Back"}
                        </button>
                    </div>
                </div>
            </div>
            {tab != 'test'?
                <ReactTooltip id={'registerTip'} place={'top'} effect="solid" getContent={() => {
                    return (<div style={{ width: '600px' }}> {errors.map((item, index) => <Requirements {...item} key={index} />)} </div>)
                }}>
                </ReactTooltip>
                : null}
            {tab == 'setpoint' ? <FunctionHelp type={'static'} /> : null}
        </div>      
    );
}



export default DynamicAlarmHome