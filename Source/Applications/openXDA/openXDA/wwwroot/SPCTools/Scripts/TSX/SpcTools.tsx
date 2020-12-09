//******************************************************************************************************
//  SPCTools.tsx - Gbtc
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
import NavBar from './NavBar';
import { SPCTools } from './global';
import AlarmGroupHome from './AlarmGroup/AlarmGroup';
import ChannelOverview from './ChannelOverview/ChannelOverview';
import MeterOverview from './MeterOverview/MeterOverview';

import StaticAlarmHome from './StaticAlarm/StaticAlarm';
import { Provider, useSelector, useDispatch } from 'react-redux';
import store from './store/store';
import { fetchAlarmTypes, fetchMeasurmentTypes, fetchSeverities } from './store/GeneralSettingsSlice';
import { selectPage, setPage } from './store/GeneralSettingsSlice';
import { ResetWizzard as ResetStatic} from './StaticAlarm/StaticWizzardSlice';
import { ResetWizzard as RestDynamic } from './DynamicAlarm/DynamicWizzardSlice';
import { FetchAlarmDay } from './store/AlarmDaySlice';
import { FetchAlarmDayGroup } from './store/AlarmDayGroupSlice';
import DynamicAlarmHome from './DynamicAlarm/DynamicAlarm';
import { FetchMeasurmentTypes } from './store/MeasurmentTypeSlice';
import { FetchSeriesType } from './store/SeriesTypeSlice';
import { FetchAlarmType } from './store/AlarmTypeSlice';
import { FetchSeverities } from './store/SeveritySlice';


declare var homePath: string;
declare var userIsAdmin: boolean;


const SPCTools: React.FunctionComponent = (props: {}) => {
    const page = useSelector(selectPage);
    const dispatch = useDispatch();

    function switchPage(pg: SPCTools.Page) {
        if (pg == 'Static') dispatch(ResetStatic());
        if (pg == 'Dynamic') dispatch(RestDynamic());
        dispatch(setPage(pg))

    }
   
    return (
        <>
            <NavBar page={page} pageSetter={(pg) => switchPage(pg)} />
            <div className="container theme-showcase" role="main" id="bodyContainer">
                <div className="screen" style={{ height: (window.innerHeight - 66), width: '100%', position: 'absolute', top: '66px', left: '0px'}}>
                    <React.Suspense fallback={<div>Loading...</div>}>
                        {(page == 'Home' ? <AlarmGroupHome /> : null)}
                        {(page == 'Static' ? <StaticAlarmHome /> : null)}
                        {(page == 'Dynamic' ? <DynamicAlarmHome /> : null)}
                        {(page == 'Meter' ? <MeterOverview /> : null)}
                        {(page == 'Channel' ? <ChannelOverview/> : null)}                               
                    </React.Suspense>
                </div>
            </div>
        </>
       
    );
}

//Load General Options
store.dispatch(fetchAlarmTypes());
store.dispatch(fetchMeasurmentTypes());
store.dispatch(fetchSeverities());

store.dispatch(FetchAlarmDay());
store.dispatch(FetchAlarmDayGroup());
store.dispatch(FetchMeasurmentTypes());
store.dispatch(FetchAlarmType());
store.dispatch(FetchSeriesType());
store.dispatch(FetchSeverities());

ReactDOM.render(<Provider store={store}><SPCTools/></Provider>, document.getElementById('body'));
