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

import { selectStatus, next, back, selectTab, selectErrors, SaveWizard, selectWizardEror, ResetWizzard } from './DynamicWizzardSlice'
import { useSelector, useDispatch } from 'react-redux';

import { ToolTip, ProgressBar, ServerErrorIcon, LoadingIcon } from '@gpa-gemstone/react-interactive';
import { Requirements } from '../CommonComponents/Requirments';
import SelectStatisticsData from './SelectStatisticsData';
import SetPointCreator from './SetPointCreator';
import { FunctionHelp } from '../CommonComponents/FunctionHelp';
import WizardTest from './WizardTest';

declare let homePath: string;
declare let apiHomePath: string;

declare let userIsAdmin: boolean;

interface IProps { complete: () => void }

const WizardHome = (props: IProps) => {
    const status = useSelector(selectStatus);
    const tab = useSelector(selectTab);
    const dispatch = useDispatch();
    const errors = useSelector(selectErrors);
    const wizardError = useSelector(selectWizardEror);

    const [hover, setHover] = React.useState<boolean>(false);


    // Define Step Numbers
    const generalStep = 'general';
    const selectStep = 'selectData';
    const setpointStep = 'setpoint';
    const testStep = 'test';

    function Continue() {

        if (!errors.some(item => (item.complete == 'required'))) {

            if (tab == 'test') {
                dispatch(SaveWizard());
                return;
            }

            dispatch(next());
        }

    }

    React.useEffect(() => {
        if (tab == 'done') {
            props.complete();
            dispatch(ResetWizzard('static'));
        }
    }, [tab])
    return (
        <div className="container-fluid d-flex h-100 flex-column" style={{ paddingBottom: "15px" }}>
            <div className="row" style={{ height: '40px', marginTop: '25px', marginRight: '25px', marginLeft: '25px', marginBottom: '25px' }}>
                <ProgressBar width={'100%'} height={20}
                    steps={[
                        { id: generalStep, long: 'General Settings', short: 'General' },
                        { id: selectStep, long: 'Select Historical Data', short: 'Select Data' },
                        { id: setpointStep, long: 'Create Setpoints', short: 'Setpoint' },
                        { id: testStep, long: 'Test Alarm', short: 'Test' },
                    ]}
                    activeStep={tab}
                />
            </div>
            <div className="row" style={{ overflow: 'hidden', flex:1}}>
                {status == 'loading' ? <div style={{ height: '40px', width: '40px', margin: 'auto' }}>
                    <LoadingIcon Show={true} Size={40} />
                </div> : null}
                {status == 'error' || status == 'unitiated' ?
                    <ServerErrorIcon Show={true} Size={50} Label={'Wizard Failed to start.'} />
                    : null}
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
                <div className="col" style={{ width: '100%' }}>
                    <div className="btn-group mr-2 " role="group">
                        <button type="button" className="btn btn-danger" disabled={tab == 'general'} onClick={() => dispatch(back())}>
                            {status != 'idle' ? <LoadingIcon Show={true} Size={40} /> : "Back"}
                        </button>
                    </div>
                </div>
                <div className="btn-group mr-2 " role="group">
                    <button type="button" className={"btn btn-success" + (errors.some(item => (item.complete == 'required')) ? ' disabled' : '')}
                        onClick={() => Continue()} data-tooltip='registerTip' onMouseEnter={() => setHover(true)} onMouseLeave={() => setHover(false)}>
                        {status != 'idle' ? <LoadingIcon Show={true} Size={40} /> : (tab == "test" ? "Save" : "Continue")}
                    </button>
                </div>
            </div>
            {tab != 'test' ?
                <ToolTip Show={hover && errors.length > 0} Position={'left'} Theme={'dark'} Target={"registerTip"}>
                    {errors.map((item, index) => <Requirements {...item} key={index} />)}
                </ToolTip>
                : null}
            {tab == 'setpoint' ? <FunctionHelp /> : null}
        </div>
    );
}



export default WizardHome