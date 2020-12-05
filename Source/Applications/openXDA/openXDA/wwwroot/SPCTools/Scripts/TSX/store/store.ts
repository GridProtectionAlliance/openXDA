//******************************************************************************************************
//  store.tsx - Gbtc
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
//  10/29/2020 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

import { configureStore } from '@reduxjs/toolkit';
import StaticWizzardReducer from '../StaticAlarm/StaticWizzardSlice';
import GeneralSettingsReducer from './GeneralSettingsSlice';
import HistoryTestReducer from './HistoryTestSlice';
import ChannelOverviewReducer from './ChannelOverviewSlice';
import ChannelAlarmGroupReducer from './ChannelAlarmGroupSlice';
import AlarmDayReducer from './AlarmDaySlice';
import AlarmDayGroupReducer from './AlarmDayGroupSlice';
import DynamicWizzardReducer from '../DynamicAlarm/DynamicWizzardSlice';
import MeasurementTypeReducer from './MeasurmentTypeSlice';
import AlarmTypeReducer from './AlarmTypeSlice';
import WizardPhaseOptionReducer from './WizardPhaseOptionSlice';
import WizardVoltageOptionReducer from './WizardVoltageOptionSlice';
import SeriesTypeReducer from './SeriesTypeSlice';
import MeterReducer from './MeterSlice';
import MeterAlarmGroupReducer from './MeterAlarmGroupSlice';
import AlarmGroupViewReducer from './AlarmGroupViewSlice';
import WizardAffectedChannelReducer from './WizardAffectedChannelSlice';

export default configureStore({
    reducer: {
        StaticWizzard: StaticWizzardReducer,
        GeneralSettings: GeneralSettingsReducer,
        HistoryTest: HistoryTestReducer,
        ChannelOverview: ChannelOverviewReducer,
        ChannelAlarmGroup: ChannelAlarmGroupReducer,
        AlarmDay: AlarmDayReducer,
        AlarmDayGroup: AlarmDayGroupReducer,
        DynamicWizzard: DynamicWizzardReducer,
        MeasurmentType: MeasurementTypeReducer,
        AlarmType: AlarmTypeReducer,
        WizardPhaseOption: WizardPhaseOptionReducer,
        WizardVoltageOption: WizardVoltageOptionReducer,
        SeriesType: SeriesTypeReducer,
        Meter: MeterReducer,
        MeterAlarmGroup: MeterAlarmGroupReducer,
        AlarmGroupView: AlarmGroupViewReducer,
        WizardAffectedChannel: WizardAffectedChannelReducer,
    }
});