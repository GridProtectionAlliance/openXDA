//******************************************************************************************************
//  DynamicWizzardSlice.tsx - Gbtc
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
//  12/04/2020 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************
import { createSlice, createAsyncThunk, PayloadAction, createSelector, Selector } from '@reduxjs/toolkit';
import { SPCTools, StaticWizzard, openXDA, Redux, DynamicWizzard } from '../global';
import _ from 'lodash';
import { FetchAffectedChannels, SelectAffectedChannelCount } from '../store/WizardAffectedChannelSlice';
import { SelectSelectedPhases } from '../store/WizardPhaseOptionSlice';
import { SelectSelectedVoltages } from '../store/WizardVoltageOptionSlice';

declare var homePath: string;
declare var apiHomePath: string;
declare var userIsAdmin: boolean;

// Thunk For Reseting Wizrd
export const ResetWizzard = createAsyncThunk('DynamicWizzard/ResetWizzard', (_, { dispatch, getState }) => {
    dispatch(DynamicWizzardSlice.actions.reset((getState() as Redux.StoreState).GeneralSettings));
    dispatch(FetchAffectedChannels());
})


export const DynamicWizzardSlice = createSlice({
    name: 'DynamicWizzard',
    initialState: {
        Step: 'general',
        AlarmGroup: {},
        Status: 'unitiated',
        SelectedMeter: [],
        SelectedMeterSort: 'Name',
        SelectedMeterASC: true,

        MeasurmentTypeID: 0,
        SeriesTypeID: 0,
        AlarmDayGroupID: 0,
        StatisticsRange: { start: '', end: '' },
        StatisticsFilter: { FilterLower: false, FilterUpper: false, FilterZero: true, LowerLimit: 0, UpperLimit: 0 },
        StatisticsChannelIDs: [],
        
    } as DynamicWizzard.IState,
    reducers: {
        reset: (state, action: PayloadAction<SPCTools.ISettingsState>) => {

            let dt = new Date();

            state.Step = 'general'
            state.AlarmGroup = { AlarmTypeID: 1, Formula: "", ID: -1, Name: "" }
            state.Status = 'idle'
            state.SelectedMeter = []
            state.MeasurmentTypeID = 1
            state.SeriesTypeID = 1
            state.AlarmDayGroupID = 1
            state.StatisticsRange = {
                start: `${dt.getFullYear() - 1}-${(dt.getMonth() + 1).toString().padStart(2, '0')}-${dt.getDate().toString().padStart(2, '0')}`,
                end: `${dt.getFullYear()}-${(dt.getMonth() + 1).toString().padStart(2, '0')}-${dt.getDate().toString().padStart(2, '0')}`
            }
            state.StatisticsFilter = { FilterLower: false, FilterUpper: false, FilterZero: true, LowerLimit: 0, UpperLimit: 0 }
            state.StatisticsChannelIDs = []

        },
        next: (state) => {
            if (state.Step == 'general')
                state.Step = 'selectData'
            else if (state.Step == 'selectData')
                state.Step = 'setpoint'
            else if (state.Step == 'setpoint')
                state.Step = 'test'
        },
        back: (state) => {
            if (state.Step == 'selectData')
                state.Step = 'general'
            else if (state.Step == 'setpoint')
                state.Step = 'selectData'
            else if (state.Step == 'test')
                state.Step = 'setpoint'

        },
        updateAlarmGroup: (state, action: PayloadAction<SPCTools.IAlarmGroup>) => {
            state.AlarmGroup = action.payload;
        },
        sortSelectedMeters: (state, action: PayloadAction<{ field: keyof openXDA.IMeter, asc: boolean }>) => {
            // Implement later since nor neccesarry
        },
        addMeter: (state, action: PayloadAction<openXDA.IMeter[]>) => {
            state.SelectedMeter.push(...action.payload);
            state.SelectedMeter = _.uniqBy(state.SelectedMeter, m => m.ID);
        },
        removeMeter: (state, action: PayloadAction<openXDA.IMeter[]>) => {
            state.SelectedMeter = state.SelectedMeter.filter(m => action.payload.findIndex(d => d.ID == m.ID) == -1);
        },
        updateMeasurmentTypeID: (state, action: PayloadAction<number>) => {
            state.MeasurmentTypeID = action.payload
        },
        updateSeriesTypeID: (state, action: PayloadAction<number>) => {
            state.SeriesTypeID = action.payload
        },
        updateAlarmDayGroupID: (state, action: PayloadAction<number>) => {
            state.AlarmDayGroupID = action.payload
        },
        updateStatisticsRange: (state, action: PayloadAction<SPCTools.IDateRange>) => {
            state.StatisticsRange = action.payload
        },
        updateStatisticsFilter: (state, action: PayloadAction<SPCTools.IDataFilter>) => {
            state.StatisticsFilter = action.payload
        },
        updateStatisticChannels: (state, action: PayloadAction<openXDA.IChannel[]>) => {
            state.StatisticsChannelIDs = action.payload.map(ch => ch.ID)
        }
    },
    
     extraReducers: (builder) => {


    }

});

export const {
    next, back,
    updateAlarmGroup,
    sortSelectedMeters,
    addMeter, removeMeter,
    updateMeasurmentTypeID,
    updateSeriesTypeID, updateAlarmDayGroupID,
    updateStatisticsRange, updateStatisticsFilter,
    updateStatisticChannels

} = DynamicWizzardSlice.actions

export default DynamicWizzardSlice.reducer;

// Selectors
export const selectTab = (state: Redux.StoreState) => state.DynamicWizzard.Step;
export const selectStatus = (state: Redux.StoreState) => state.DynamicWizzard.Status;

export const selectSelectedMeter = (state: Redux.StoreState) => state.DynamicWizzard.SelectedMeter;
export const selectSelectedMeterASC = (state: Redux.StoreState) => state.DynamicWizzard.SelectedMeterASC;
export const selectSelectedMeterSort = (state: Redux.StoreState) => state.DynamicWizzard.SelectedMeterSort;

export const selectMeasurmentTypeID = (state: Redux.StoreState) => state.DynamicWizzard.MeasurmentTypeID;
export const selectSeriesTypeID = (state: Redux.StoreState) => state.DynamicWizzard.SeriesTypeID;
export const selectAlarmGroup = (state: Redux.StoreState) => state.DynamicWizzard.AlarmGroup;
export const selectAlarmDayGroupID = (state: Redux.StoreState) => state.DynamicWizzard.AlarmDayGroupID;

export const SelectStatisticsrange = (state: Redux.StoreState) => state.DynamicWizzard.StatisticsRange;
export const SelectStatisticsFilter = (state: Redux.StoreState) => state.DynamicWizzard.StatisticsFilter;


export const selectErrors = createSelector(
    ((state: Redux.StoreState) => (state.DynamicWizzard.Step)),
    SelectAffectedChannelCount,
    (state: Redux.StoreState) => (state.DynamicWizzard.AlarmGroup.Name != undefined && state.DynamicWizzard.AlarmGroup.Name.length > 0),
    (state: Redux.StoreState) => (state.DynamicWizzard.SelectedMeter.length > 0),
    (state: Redux.StoreState) => SelectSelectedPhases(state).some(i => i),
    (state: Redux.StoreState) => SelectSelectedVoltages(state).some(i => i),
    SelectStatisticsrange,
    (state: Redux.StoreState) => state.DynamicWizzard.StatisticsChannelIDs.length,
//    (state: Redux.StoreState) => (state.StaticWizzard.StatChannels.length == state.StaticWizzard.SelectedChannelCount),
//    (state: Redux.StoreState) => (state.StaticWizzard.SetPointEvaluation == undefined ? false : state.StaticWizzard.SetPointEvaluation.Valid) ,
//    (state: Redux.StoreState) => (state.StaticWizzard.SetPointEvaluation == undefined ? false : state.StaticWizzard.SetPointEvaluation.IsScalar),
//    (state: Redux.StoreState) => (state.StaticWizzard.AlarmFactors),
    (step, channelCount, name, meterCount, phaseCount, voltageCount, statisticsRange, statisticsChannelCount) => {
        let result = [] as StaticWizzard.IRequirement[];
        if (step == 'general') {
            result.push({ text: "A Name is required", complete: (name ? 'complete' : 'required') });
            result.push({ text: "At least 1 Meter needs to be selected", complete: (meterCount ? 'complete' : 'required') });
            result.push({ text: "At least 1 Phase needs to be selected", complete: (phaseCount ? 'complete' : 'required') });
            result.push({ text: "At least 1 Base Voltage needs to be selected", complete: (voltageCount ? 'complete' : 'required') });
            result.push({ text: "The Selection needs to result in at least 1 Channel", complete: (channelCount ? 'complete' : 'required') });
        }
        else if (step == 'selectData') {
            result.push({ text: "A valid start date has to be selected", complete: (statisticsRange.start != "" ? 'complete' : 'required') });
            result.push({ text: "A valid end date has to be selected", complete: (statisticsRange.end != "" ? 'complete' : 'required') })
            result.push({ text: "At least 1 Channel needs to be selected", complete: (statisticsChannelCount > 0 ? 'complete' : 'required') })
            if (!(statisticsChannelCount == channelCount))
                result.push({ text: "A single threshhold will be required for all Channels. Not all Channels are used as historic datasource", complete: 'warning' })
        }
        else if (step == 'setpoint') {
            //result.push({ text: "A valid setpoint Expression is Required", complete: (setPoint ? 'complete' : 'required') });
            //if (!fullHistory)
            //    result.push({ text: "A single scalar setpoint is required for all Channels", complete: (scalarSetpoint ? 'complete' : 'required') })
            //else if (!scalarSetpoint)
            //    result.push({ text: "The setpoint expression will result in different threshold for each Channel", complete: 'warning' })
            //if (alarmFactors.length > 0)
            //    result.push({ text: "No level can not be applied at the original SetPoint", complete: (!alarmFactors.some(item => item.Value == 1.0) ? 'complete' : 'required') })
        }
        return result;

    });


// Async Functions



function getChannelCount(state: StaticWizzard.IState): JQuery.jqXHR<number> {
    

    let handle = $.ajax({
        type: "POST",
        url: `${apiHomePath}api/SPCTools/StaticAlarmCreation/AffectedChannels`,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        data: JSON.stringify({
            MeterID: state.SelectedMeterID, MeasurmentTypeID: state.SelectedMeasurmentTypeID,
            BaseVoltage: state.AvailableVoltages.filter((p, i) => state.SelectedVoltages[i]), PhaseID: state.AvailablePhases.filter((p, i) => state.SelectedPhases[i]).map(p => p.ID)
        }),
        cache: false,
        async: true
    });

    return handle;
}

